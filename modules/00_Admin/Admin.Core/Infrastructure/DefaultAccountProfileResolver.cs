using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.Authorize.Vo;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Mod.Admin.Core.Domain.Menu;
using Mkh.Mod.Admin.Core.Domain.RoleButton;
using Mkh.Mod.Admin.Core.Domain.RoleMenu;
using Mkh.Utils.Map;

namespace Mkh.Mod.Admin.Core.Infrastructure
{
    /// <summary>
    /// 默认账户资料解析器
    /// </summary>
    public class DefaultAccountProfileResolver : IAccountProfileResolver
    {
        private readonly IRoleMenuRepository _roleMenuRepository;
        private readonly IRoleButtonRepository _roleButtonRepository;
        private readonly IMapper _mapper;

        public DefaultAccountProfileResolver(IRoleMenuRepository roleMenuRepository, IMapper mapper, IRoleButtonRepository roleButtonRepository)
        {
            _roleMenuRepository = roleMenuRepository;
            _mapper = mapper;
            _roleButtonRepository = roleButtonRepository;
        }

        public async Task<ProfileVo> Resolve(AccountEntity account, int platform)
        {
            var vo = new ProfileVo
            {
                AccountId = account.Id,
                Platform = platform,
                RoleId = account.RoleId,
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
            };

            var menus = _roleMenuRepository.Find().InnerJoin<MenuEntity>(m => m.T1.MenuId == m.T2.Id)
                .Where(m => m.T1.RoleId == account.RoleId)
                .Select(m => new { m.T2 })
                .ToList<MenuEntity>();

            var buttons = _roleButtonRepository.Find(m => m.RoleId == account.RoleId).ToList();

            var rootMenu = new ProfileMenuVo { Id = 0 };
            ResolveMenu(await menus, await buttons, rootMenu);

            vo.Menus = rootMenu.Children;
            return vo;
        }

        private void ResolveMenu(IList<MenuEntity> menus, IList<RoleButtonEntity> buttons, ProfileMenuVo parent)
        {
            parent.Children = new List<ProfileMenuVo>();
            var children = menus.Where(m => m.ParentId == parent.Id).ToList();
            foreach (var child in children)
            {
                var menuVo = _mapper.Map<ProfileMenuVo>(child);
                menuVo.Buttons = buttons.Where(m => m.MenuId == child.Id).Select(m => m.ButtonCode.ToLower()).ToList();

                parent.Children.Add(menuVo);

                ResolveMenu(menus, buttons, menuVo);
            }
        }
    }
}
