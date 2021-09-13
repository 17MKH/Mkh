using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Mkh.Module.Abstractions;
using Mkh.Module.Abstractions.Options;
using Mkh.Utils.Abstracts;
using Mkh.Utils.Helpers;
using Mkh.Utils.Extensions;

namespace Mkh.Module.Core
{
    /// <summary>
    /// 模块集合的默认实现
    /// </summary>
    public class ModuleCollection : CollectionAbstract<ModuleDescriptor>, IModuleCollection
    {
        private AssemblyHelper _assemblyHelper;
        private readonly IConfiguration _configuration;

        public ModuleCollection(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public ModuleDescriptor Get(int id)
        {
            return Collection.FirstOrDefault(m => m.Id == id);
        }

        public ModuleDescriptor Get(string code)
        {
            return Collection.FirstOrDefault(m => m.Code.EqualsIgnoreCase(code));
        }

        /// <summary>
        /// 加载模块
        /// </summary>
        public void Load()
        {
            var modulesRootPath = Path.Combine(AppContext.BaseDirectory, Constants.ROOT_DIR);
            if (!Directory.Exists(modulesRootPath))
                return;

            var moduleDirs = Directory.GetDirectories(modulesRootPath);
            if (!moduleDirs.Any())
                return;

            _assemblyHelper = new AssemblyHelper();
            var optionsList = new List<ModuleOptions>();
            foreach (var dir in moduleDirs)
            {
                var code = Path.GetFileName(dir)!.Split("_")[1];
                var options = _configuration.Get<ModuleOptions>($"Mkh:Modules:{code}");
                if (options.Db != null)
                {
                    options.Code = code;
                    options.Dir = dir;
                    optionsList.Add(options);
                }
            }

            foreach (var options in optionsList.OrderBy(m => m.Sort))
            {
                LoadModule(options);
            }

            //释放资源
            _assemblyHelper = null;
        }

        /// <summary>
        /// 加载模块
        /// </summary>
        /// <param name="options"></param>
        private void LoadModule(ModuleOptions options)
        {
            var jsonFilePath = Path.Combine(options.Dir, Constants.JSON_FILE_NAME);
            if (!File.Exists(jsonFilePath))
                return;

            using var reader = new StreamReader(jsonFilePath, Encoding.UTF8);
            var jsonStr = reader.ReadToEnd();
            var moduleDescriptor = JsonSerializer.Deserialize<ModuleDescriptor>(jsonStr);
            if (moduleDescriptor == null)
                return;

            moduleDescriptor.Options = options;

            LoadLayerAssemblies(moduleDescriptor);

            LoadServicesConfigurator(moduleDescriptor);

            LoadEnums(moduleDescriptor);

            LoadDbInitFilePath(options.Dir, moduleDescriptor);

            Add(moduleDescriptor);
        }

        /// <summary>
        /// 加载模块分层信息
        /// </summary>
        /// <param name="descriptor"></param>
        private void LoadLayerAssemblies(ModuleDescriptor descriptor)
        {
            var layer = descriptor.LayerAssemblies;
            layer.Core = _assemblyHelper.LoadByNameEndString($"{Constants.PREFIX}.Mod.{descriptor.Code}.Core");
            layer.Web = _assemblyHelper.LoadByNameEndString($"{Constants.PREFIX}.Mod.{descriptor.Code}.Web");
            layer.Api = _assemblyHelper.LoadByNameEndString($"{Constants.PREFIX}.Mod.{descriptor.Code}.Api");
            layer.Client = _assemblyHelper.LoadByNameEndString($"{Constants.PREFIX}.Mod.{descriptor.Code}.Client");
        }

        /// <summary>
        /// 加载模块服务配置器
        /// </summary>
        /// <param name="descriptor"></param>
        private void LoadServicesConfigurator(ModuleDescriptor descriptor)
        {
            if (descriptor.LayerAssemblies.Core != null)
            {
                var servicesConfiguratorType = descriptor.LayerAssemblies.Core.GetTypes()
                    .FirstOrDefault(m => typeof(IModuleServicesConfigurator).IsAssignableFrom(m));

                if (servicesConfiguratorType != null)
                {
                    descriptor.ServicesConfigurator =
                        (IModuleServicesConfigurator)Activator.CreateInstance(servicesConfiguratorType);
                }
            }
        }

        /// <summary>
        /// 加载枚举信息
        /// </summary>
        /// <param name="descriptor"></param>
        private void LoadEnums(ModuleDescriptor descriptor)
        {
            var layer = descriptor.LayerAssemblies;

            if (layer.Core == null)
                return;

            var enumTypes = layer.Core.GetTypes().Where(m => m.IsEnum);
            foreach (var enumType in enumTypes)
            {
                var enumDescriptor = new ModuleEnumDescriptor
                {
                    Name = enumType.Name,
                    Type = enumType,
                    Options = Enum.GetValues(enumType).Cast<Enum>().Where(m => !m.ToString().EqualsIgnoreCase("UnKnown")).Select(x => new OptionResultModel
                    {
                        Label = x.ToDescription(),
                        Value = x
                    }).ToList()
                };

                descriptor.EnumDescriptors.Add(enumDescriptor);
            }
        }

        /// <summary>
        /// 加载数据库初始化数据文件路径
        /// </summary>
        private void LoadDbInitFilePath(string modulePath, ModuleDescriptor descriptor)
        {
            var filePath = Path.Combine(modulePath, Constants.DB_INIT_FILE_NAME);
            if (!File.Exists(filePath))
                return;

            descriptor.DbInitFilePath = filePath;
        }
    }
}
