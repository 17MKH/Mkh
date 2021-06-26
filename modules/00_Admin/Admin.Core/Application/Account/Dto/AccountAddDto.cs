using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mkh.Mod.Admin.Core.Application.Account.Dto
{
    /// <summary>
    /// 账户新增模型
    /// </summary>
    public class AccountAddDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "请输入用户名")]
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 姓名或名称
        /// </summary>
        [Required(ErrorMessage = "请输入姓名或名称")]
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 绑定的角色编号列表
        /// </summary>
        public List<int> Roles { get; set; }
    }
}
