using System;
using System.ComponentModel.DataAnnotations;

namespace Mkh.Utils.Validations;

/// <summary>
/// 手机号验证
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class PhoneValidationAttribute : RegularExpressionAttribute
{
    public PhoneValidationAttribute() : base("^1(3[0-9]|4[01456879]|5[0-35-9]|6[2567]|7[0-8]|8[0-9]|9[0-35-9])\\d{8}$")
    {
    }
}