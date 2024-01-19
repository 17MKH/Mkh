using System;

namespace Mkh.Utils.Generators;

/// <summary>
/// 标准的GUID生成器
/// </summary>
public class NormalGuidGenerator : IGuidGenerator
{
    public Guid Create()
    {
        return Guid.NewGuid();
    }
}