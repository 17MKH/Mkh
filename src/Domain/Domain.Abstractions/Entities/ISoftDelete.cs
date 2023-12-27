namespace Mkh.Domain.Abstractions.Entities;

/// <summary>
/// 软删除
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// 已删除的
    /// </summary>
    bool IsDeleted { get; set; }
}