﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Mkh.Auth.Abstractions.Annotations;
using Mkh.Module.Abstractions;
using Swashbuckle.AspNetCore.Annotations;

namespace Mkh.Mod.Admin.Web.Controllers;

[SwaggerTag("通用功能")]
[AllowWhenAuthenticated]
public class CommonController : Web.ModuleController
{
    private readonly IModuleCollection _moduleCollection;

    public CommonController(IModuleCollection moduleCollection)
    {
        _moduleCollection = moduleCollection;
    }

    /// <summary>
    /// 获取枚举中选项列表
    /// </summary>
    /// <param name="moduleCode"></param>
    /// <param name="enumName"></param>
    /// <returns></returns>
    [HttpGet]
    public IResultModel EnumOptions(string moduleCode, string enumName)
    {
        var module = _moduleCollection.FirstOrDefault(m => m.Code.EqualsIgnoreCase(moduleCode));
        if (module == null)
            return ResultModel.Success(new List<OptionResultModel>());

        var enumDescriptor = module.EnumDescriptors.FirstOrDefault(m => m.Name.EqualsIgnoreCase(enumName));
        if (enumDescriptor == null)
            return ResultModel.Success(new List<OptionResultModel>());

        return ResultModel.Success(enumDescriptor.Options);
    }
}