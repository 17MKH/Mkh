using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.Authorize.Vo;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Mod.Admin.Core.Domain.AccountSkin;
using Mkh.Mod.Admin.Core.Domain.Menu;
using Mkh.Mod.Admin.Core.Domain.RoleButton;
using Mkh.Mod.Admin.Core.Domain.RoleMenu;
using Mkh.Utils.Annotations;
using Mkh.Utils.Json;
using Mkh.Utils.Map;

namespace Mkh.Mod.Admin.Core.Infrastructure.Defaults;

/// <summary>
/// 默认账户资料解析器
/// </summary>
[ScopedInject]
internal class DefaultAccountProfileResolver : IAccountProfileResolver
{
    private readonly IMapper _mapper;
    private readonly IRoleMenuRepository _roleMenuRepository;
    private readonly IRoleButtonRepository _roleButtonRepository;
    private readonly IAccountSkinRepository _accountSkinRepository;
    private readonly JsonHelper _jsonHelper;

    public DefaultAccountProfileResolver(IRoleMenuRepository roleMenuRepository, IMapper mapper, IRoleButtonRepository roleButtonRepository, IAccountSkinRepository accountSkinRepository, JsonHelper jsonHelper)
    {
        _roleMenuRepository = roleMenuRepository;
        _mapper = mapper;
        _roleButtonRepository = roleButtonRepository;
        _accountSkinRepository = accountSkinRepository;
        _jsonHelper = jsonHelper;
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
        };

        //读取账户皮肤配置信息
        var accountSkin = await _accountSkinRepository.Find(m => m.AccountId == account.Id).ToFirst();
        if (accountSkin != null)
        {
            vo.Skin = new ProfileSkinVo
            {
                Name = accountSkin.Name,
                Code = accountSkin.Code,
                Theme = accountSkin.Theme,
                Size = accountSkin.Size
            };
        }
        else
        {
            vo.Skin = new ProfileSkinVo();
        }

        var menusQuery = _roleMenuRepository.Find().InnerJoin<MenuEntity>(m => m.T1.MenuId == m.T2.Id)
            .Where(m => m.T1.RoleId == account.RoleId)
            .Select(m => new { m.T2 });

        var menus = await menusQuery.ToList<MenuEntity>();

        var buttons = await _roleButtonRepository.Find(m => m.RoleId == account.RoleId).ToList();

        var rootMenu = new ProfileMenuVo { Id = 0 };

        ResolveMenu(menus, buttons, rootMenu);

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

            if (child.LocalesConfig.NotNull())
            {
                menuVo.Locales = _jsonHelper.Deserialize<MenuLocales>(child.LocalesConfig);
            }

            menuVo.Buttons = buttons.Where(m => m.MenuId == child.Id).Select(m => m.ButtonCode.ToLower()).ToList();

            parent.Children.Add(menuVo);

            ResolveMenu(menus, buttons, menuVo);
        }
    }
}