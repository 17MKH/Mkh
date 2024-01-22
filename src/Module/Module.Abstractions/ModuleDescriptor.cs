using System;
using System.Collections.Generic;
using Mkh.Module.Abstractions.Options;
using Mkh.Utils.Permissions;

namespace Mkh.Module.Abstractions;

/// <summary>
/// 模块描述符
/// </summary>
public class ModuleDescriptor
{
    /// <summary>
    /// 编号
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// 说明介绍
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 数据库初始化数据文件路径
    /// </summary>
    public string? DbInitFilePath { get; set; }

    /// <summary>
    /// 模块配置项
    /// </summary>
    public ModuleOptions? Options { get; set; }

    /// <summary>
    /// 初始化器
    /// </summary>
    public IModuleServicesConfigurator ServicesConfigurator { get; set; }

    /// <summary>
    /// 多语言读取器类型
    /// </summary>
    public Type LocalizerType { get; }

    /// <summary>
    /// 分层信息
    /// </summary>
    public ModuleLayerAssemblies LayerAssemblies { get; } = new();

    /// <summary>
    /// 应用服务集合
    /// </summary>
    public Dictionary<Type, Type> ApplicationServices { get; set; } = new();

    /// <summary>
    /// 权限列表
    /// </summary>
    public List<PermissionDescriptor> Permissions { get; } = new();

    public ModuleDescriptor(int id, string code, IModuleServicesConfigurator servicesConfigurator, Type localizerType)
    {
        Id = id;
        Code = code;
        ServicesConfigurator = servicesConfigurator;
        LocalizerType = localizerType;
    }
}