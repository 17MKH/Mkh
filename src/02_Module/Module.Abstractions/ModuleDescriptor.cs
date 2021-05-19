using System.Collections.Generic;
using System.Linq;

namespace Mkh.Module.Abstractions
{
    /// <summary>
    /// 模块描述符
    /// </summary>
    public class ModuleDescriptor
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 说明介绍
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 初始化数据库脚本路径
        /// </summary>
        public string DbInitScript { get; set; }

        /// <summary>
        /// 初始化器
        /// </summary>
        public IModuleServicesConfigurator ServicesConfigurator { get; set; }

        /// <summary>
        /// 分层信息
        /// </summary>
        public ModuleLayerAssemblies LayerAssemblies { get; } = new();

        /// <summary>
        /// 枚举描述符集合
        /// </summary>
        public List<ModuleEnumDescriptor> EnumDescriptors { get; } = new();

        /// <summary>
        /// 获取指定枚举描述符信息
        /// </summary>
        /// <param name="enumName">枚举名称</param>
        /// <returns></returns>
        public ModuleEnumDescriptor GetEnum(string enumName)
        {
            return EnumDescriptors.FirstOrDefault(m => m.Name.Equals(enumName));
        }
    }
}