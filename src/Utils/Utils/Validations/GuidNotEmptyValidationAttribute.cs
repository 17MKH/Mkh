﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Mkh.Utils.Validations;

/// <summary>
/// GUID不能为空的验证
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class GuidNotEmptyValidationAttribute : ValidationAttribute
{
    public GuidNotEmptyValidationAttribute() : base("属性 {0} 不能为空") { }

    public override bool IsValid(object value)
    {
        if (value is null)
        {
            return true;
        }

        switch (value)
        {
            case Guid guid:
                return guid != Guid.Empty;
            default:
                return true;
        }
    }
}