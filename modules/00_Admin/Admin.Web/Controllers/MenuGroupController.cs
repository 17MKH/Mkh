﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mkh.Auth.Abstractions.Annotations;
using Mkh.Mod.Admin.Core.Application.MenuGroup;
using Mkh.Mod.Admin.Core.Application.MenuGroup.Dto;
using Mkh.Utils.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Mkh.Mod.Admin.Web.Controllers
{
    [SwaggerTag("菜单分组")]
    public class MenuGroupController : BaseController
    {
        private readonly IMenuGroupService _service;

        public MenuGroupController(IMenuGroupService service)
        {
            _service = service;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <remarks>查询角色列表</remarks>
        [HttpGet]
        public Task<IResultModel> Query([FromQuery] MenuGroupQueryDto dto)
        {
            return _service.Query(dto);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <remarks></remarks>
        [HttpPost]
        public Task<IResultModel> Add(MenuGroupAddDto dto)
        {
            return _service.Add(dto);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<IResultModel> Edit(int id)
        {
            return _service.Edit(id);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <remarks></remarks>
        [HttpPost]
        public Task<IResultModel> Update(MenuGroupUpdateDto dto)
        {
            return _service.Update(dto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public Task<IResultModel> Delete([BindRequired] int id)
        {
            return _service.Delete(id);
        }

        /// <summary>
        /// 下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowWhenAuthenticated]
        public Task<IResultModel> Select()
        {
            return _service.Select();
        }
    }
}
