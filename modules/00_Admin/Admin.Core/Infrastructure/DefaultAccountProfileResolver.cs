using System.Collections.Generic;
using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.Authorize.Vo;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Mod.Admin.Core.Domain.Menu;

namespace Mkh.Mod.Admin.Core.Infrastructure
{
    /// <summary>
    /// 默认账户资料解析器
    /// </summary>
    public class DefaultAccountProfileResolver : IAccountProfileResolver
    {
        public async Task<ProfileVo> Resolve(AccountEntity account)
        {
            var vo = new ProfileVo
            {
                AccountId = account.Id,
                Avatar = account.Avatar,
                Username = account.Username,
                Name = account.Name,
                Phone = account.Phone,
                Email = account.Email,
                Skin = new ProfileSkinVo
                {
                    Code = "brief",
                    Size = "small",
                    Theme = "dark"
                },
                Menus = new List<ProfileMenuVo>
                {
                    new()
                    {
                        Id = 1,
                        Name = "权限管理",
                        Icon = "lock",
                        Level = 1,
                        Show = true,
                        Type = MenuType.Node,
                        Children = new List<ProfileMenuVo>
                        {
                            new()
                            {
                                Id = 2,
                                Name = "账户管理",
                                Icon = "user",
                                Level = 2,
                                Show = true,
                                Type = MenuType.Route,
                                RouteName = "admin_account"
                            },
                            new()
                            {
                                Id = 2,
                                Name = "角色管理",
                                Icon = "role",
                                Level = 2,
                                Show = true,
                                Type = MenuType.Route,
                                RouteName = "admin_role"
                            },
                            new()
                            {
                                Id = 3,
                                Name = "菜单管理",
                                Icon = "menu",
                                Level = 2,
                                Show = true,
                                Type = MenuType.Route,
                                RouteName = "admin_menu"
                            }
                        }
                    }
                }
            };

            return vo;
        }
    }
}
