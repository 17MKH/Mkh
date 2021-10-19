using System.Reflection;

namespace Mkh.Module.Abstractions;

/// <summary>
/// 模块分层程序集
/// </summary>
public class ModuleLayerAssemblies
{
    /// <summary>
    /// 任务层程序集
    /// </summary>
    public Assembly Core { get; set; }

    /// <summary>
    /// 管理端接口分层程序集
    /// </summary>
    public Assembly Web { get; set; }

    /// <summary>
    /// 应用端接口分层程序集
    /// </summary>
    public Assembly Api { get; set; }

    /// <summary>
    /// 客户端分层程序集
    /// </summary>
    public Assembly Client { get; set; }
}