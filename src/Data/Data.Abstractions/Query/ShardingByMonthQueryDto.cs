using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Mkh.Data.Abstractions.Query;

/// <summary>
/// 按月分表查询对象
/// </summary>
public class ShardingByMonthQueryDto : QueryDto
{
    /// <summary>
    /// 开始日期
    /// </summary>
    [ShardingQueryValidation]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime EndDate { get; set; }
}

/// <summary>
/// 分表验证
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
internal class ShardingQueryValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var localizer = validationContext.GetService<IStringLocalizer<ShardingQueryValidationAttribute>>()!;

        if (value == null)
            return new ValidationResult(localizer["请选择开始日期"]);

        var endDateValue = validationContext.ObjectType.GetProperty(nameof(ShardingByMonthQueryDto.EndDate))!.GetValue(validationContext.ObjectInstance);
        if (endDateValue == null)
            return new ValidationResult(localizer["请选择结束日期"]);

        var beginDate = value.ToDate();
        var endDate = endDateValue.ToDate();
        if (beginDate.Year != endDate.Year || beginDate.Month != endDate.Month)
        {
            return new ValidationResult(localizer["日期不能跨月查询"]);
        }

        return ValidationResult.Success;
    }
}