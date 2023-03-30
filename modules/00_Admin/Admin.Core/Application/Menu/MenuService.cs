using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Query;
using Mkh.Mod.Admin.Core.Application.Menu.Dto;
using Mkh.Mod.Admin.Core.Domain.Menu;
using Mkh.Mod.Admin.Core.Domain.RoleButton;
using Mkh.Mod.Admin.Core.Domain.RoleMenu;
using Mkh.Mod.Admin.Core.Domain.RolePermission;
using Mkh.Mod.Admin.Core.Infrastructure;
using Mkh.Utils.Json;
using Mkh.Utils.Map;

namespace Mkh.Mod.Admin.Core.Application.Menu;

internal class MenuService : IMenuService
{
    private readonly IMapper _mapper;
    private readonly IMenuRepository _repository;
    private readonly IRoleMenuRepository _roleMenuRepository;
    private readonly IRoleButtonRepository _roleButtonRepository;
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly JsonHelper _jsonHelper;
    private readonly AdminLocalizer _localizer;

    public MenuService(IMapper mapper, IMenuRepository repository, IRoleMenuRepository roleMenuRepository, IRoleButtonRepository roleButtonRepository, IRolePermissionRepository rolePermissionRepository, JsonHelper jsonHelper, AdminLocalizer localizer)
    {
        _mapper = mapper;
        _repository = repository;
        _roleMenuRepository = roleMenuRepository;
        _roleButtonRepository = roleButtonRepository;
        _rolePermissionRepository = rolePermissionRepository;
        _jsonHelper = jsonHelper;
        _localizer = localizer;
    }

    public Task<PagingQueryResultModel<MenuEntity>> Query(MenuQueryDto dto)
    {
        var query = _repository.Find(m => m.GroupId == dto.GroupId && m.ParentId == dto.ParentId);
        query.WhereNotNull(dto.Name, m => m.LocalesConfig.Contains(dto.Name));
        query.OrderBy(m => m.Sort);

        return query.ToPaginationResult(dto.Paging);
    }

    [Transaction]
    public async Task<int> Add(MenuAddDto dto)
    {
        var menu = _mapper.Map<MenuEntity>(dto);
        menu.Level = 1;

        if (dto.Type == MenuType.Route)
        {
            if (dto.Module.IsNull())
                throw new AdminException(AdminErrorCode.MenuModuleCodeNull);

            if (dto.RouteName.IsNull())
                throw new AdminException(AdminErrorCode.MenuRouteNameNull);
        }
        else if (dto.Type == MenuType.Link)
        {
            if (dto.Url.IsNull())
                throw new AdminException(AdminErrorCode.MenuUrlNull);
        }
        else if (dto.Type == MenuType.CustomJs)
        {
            if (dto.CustomJs.IsNull())
                throw new AdminException(AdminErrorCode.MenuCustomJsNull);
        }

        if (dto.Locales != null)
        {
            menu.LocalesConfig = _jsonHelper.Serialize(dto.Locales);
        }

        if (dto.ParentId > 0)
        {
            var parent = await _repository.Get(dto.ParentId);
            if (parent == null)
            {
                throw new AdminException(AdminErrorCode.MenuParentNotExists);
            }

            menu.Level = parent.Level + 1;
        }

        await _repository.Add(menu);
        return menu.Id;
    }

    public async Task<MenuUpdateDto> Edit(int id)
    {
        var menu = await _repository.Get(id);

        var model = _mapper.Map<MenuUpdateDto>(menu);

        if (menu.LocalesConfig.NotNull())
        {
            model.Locales = menu.Locales;
        }

        return model;
    }

    public async Task Update(MenuUpdateDto dto)
    {
        var menu = await _repository.Get(dto.Id);

        _mapper.Map(dto, menu);

        if (dto.Locales != null)
        {
            menu.LocalesConfig = _jsonHelper.Serialize(dto.Locales);
        }

        await _repository.Update(menu);
    }

    [Transaction]
    public async Task Delete(int id)
    {
        if (await _repository.Delete(id))
        {
            //删除角色菜单绑定数据
            await _roleMenuRepository.Find(m => m.MenuId == id).ToDelete();
            //删除角色按钮绑定数据
            await _roleButtonRepository.Find(m => m.MenuId == id).ToDelete();
            //删除角色权限绑定数据
            await _rolePermissionRepository.Find(m => m.MenuId == id).ToDelete();
        }
    }

    public async Task<List<TreeResultModel<MenuEntity>>> GetTree(int groupId)
    {
        var root = new TreeResultModel<MenuEntity>();
        var all = await _repository.Find(m => m.GroupId == groupId).ToList();

        ResolveTree(all, root);

        return root.Children;
    }

    private void ResolveTree(IList<MenuEntity> all, TreeResultModel<MenuEntity> parent)
    {
        foreach (var menu in all.Where(m => m.ParentId == parent.Id).OrderBy(m => m.Sort))
        {
            var child = new TreeResultModel<MenuEntity>
            {
                Id = menu.Id,
                Item = menu
            };

            //只有节点菜单才有子级
            if (menu.Type == MenuType.Node)
            {
                ResolveTree(all, child);
            }

            parent.Children.Add(child);
        }
    }

    [Transaction]
    public async Task UpdateSort(IList<MenuEntity> menus)
    {
        if (!menus.Any())
            return;

        foreach (var menu in menus)
        {
            await _repository.Find(m => m.Id == menu.Id).ToUpdate(m => new MenuEntity
            {
                ParentId = menu.ParentId,
                Sort = menu.Sort
            });
        }
    }
}