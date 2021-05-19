using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Abstractions.Annotations;
using Mkh.Utils.Web;

namespace Mkh.Auth.Core
{
    public class PermissionResolver : IPermissionResolver
    {
        private readonly ControllerResolver _controllerResolver;
        private readonly List<PermissionDescriptor> _descriptors = new();

        public PermissionResolver(ControllerResolver controllerResolver)
        {
            _controllerResolver = controllerResolver;

            Load();
        }

        /// <summary>
        /// 加载权限信息
        /// </summary>
        private void Load()
        {
            var controllers = _controllerResolver.Controllers;
            foreach (var controller in controllers)
            {
                if (!controller.Actions.Any())
                    continue;

                foreach (var action in controller.Actions)
                {
                    var permission = new PermissionDescriptor
                    {
                        Action = action.Name,
                        Controller = controller.Name,
                        Description = action.Description,
                        ModuleCode = controller.Area,
                        HttpMethod = HttpMethod.Unknown
                    };

                    //请求方式
                    var httpMethodAttr = action.MethodInfo.CustomAttributes.FirstOrDefault(m => typeof(HttpMethodAttribute).IsAssignableFrom(m.AttributeType));
                    if (httpMethodAttr != null)
                    {
                        var httpMethodName = httpMethodAttr.AttributeType.Name.Replace("Http", "").Replace("Attribute", "");
                        permission.HttpMethod = (HttpMethod)Enum.Parse(typeof(HttpMethod), httpMethodName);
                    }

                    #region ==权限模式==

                    var allowAnonymousAttribute = action.MethodInfo.CustomAttributes.FirstOrDefault(m => typeof(AllowAnonymousAttribute) == m.AttributeType);
                    if (allowAnonymousAttribute != null)
                    {
                        permission.Mode = PermissionMode.Anonymous;
                    }
                    else
                    {
                        var allowLoginAttribute = action.MethodInfo.CustomAttributes.FirstOrDefault(m => typeof(AllowLoginAttribute) == m.AttributeType);
                        if (allowLoginAttribute != null)
                        {
                            permission.Mode = PermissionMode.Login;
                        }
                        else
                        {

                        }
                    }

                    #endregion

                    _descriptors.Add(permission);
                }
            }
        }

        public List<PermissionDescriptor> GetPermissions()
        {
            return _descriptors;
        }
    }
}