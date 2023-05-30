using System;

namespace Mkh.Data.Abstractions.Descriptors;

/// <summary>
/// 仓储描述符
/// </summary>
public interface IRepositoryDescriptor
{
    /// <summary>
    /// 实体类型
    /// </summary>
    Type EntityType { get; }

    /// <summary>
    /// 仓储接口类型
    /// </summary>
    Type InterfaceType { get; }

    /// <summary>
    /// 仓储实现类型
    /// </summary>
    Type ImplementType { get; }
}