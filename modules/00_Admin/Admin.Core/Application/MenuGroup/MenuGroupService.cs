using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Query;
using Mkh.Mod.Admin.Core.Application.MenuGroup.Dto;
using Mkh.Mod.Admin.Core.Domain.Menu;
using Mkh.Mod.Admin.Core.Domain.MenuGroup;
using Mkh.Mod.Admin.Core.Domain.Role;
using Mkh.Mod.Admin.Core.Domain.RoleMenu;
using Mkh.Mod.Admin.Core.Domain.RolePermission;
using Mkh.Mod.Admin.Core.Infrastructure;
using Mkh.Utils.Map;

namespace Mkh.Mod.Admin.Core.Application.MenuGroup;

internal class MenuGroupService : IMenuGroupService
{
    private readonly IMapper _mapper;
    private readonly IMenuGroupRepository _repository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IRoleMenuRepository _roleMenuRepository;
    private readonly IRolePermissionRepository _rolePermissionRepository;

    public MenuGroupService(IMapper mapper, IMenuGroupRepository repository, IRoleRepository roleRepository, IMenuRepository menuRepository, IRoleMenuRepository roleMenuRepository, IRolePermissionRepository rolePermissionRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _roleRepository = roleRepository;
        _menuRepository = menuRepository;
        _roleMenuRepository = roleMenuRepository;
        _rolePermissionRepository = rolePermissionRepository;
    }

    public Task<PagingQueryResultModel<MenuGroupEntity>> Query(MenuGroupQueryDto dto)
    {
        var query = _repository.Find();
        query.WhereNotNull(dto.Name, m => m.Name.Equals(dto.Name));

        return query.ToPaginationResult(dto.Paging);
    }

    public async Task<int> Add(MenuGroupAddDto dto)
    {
        var entity = _mapper.Map<MenuGroupEntity>(dto);

        await _repository.Add(entity);
        return entity.Id;
    }

    public async Task<MenuGroupUpdateDto> Edit(int id)
    {
        var entity = await _repository.Get(id);

        return _mapper.Map<MenuGroupUpdateDto>(entity);
    }

    public async Task Update(MenuGroupUpdateDto dto)
    {
        var entity = await _repository.Get(dto.Id);

        _mapper.Map(dto, entity);

        await _repository.Update(entity);
    }

    [Transaction]
    public async Task Delete(int id)
    {
        //如果有角色绑定了该菜单分组，则不允许删除
        if (await _roleRepository.Find(m => m.MenuGroupId == id).ToExists())
            throw new AdminException(AdminErrorCode.MenuGroupNotAllowDelete);

        if (await _repository.Delete(id))
        {
            //删除关联菜单
            var task1 = _menuRepository.Find(m => m.GroupId == id).ToDelete();
            //删除角色绑定的该分组的菜单信息
            var task2 = _roleMenuRepository.Find(m => m.MenuGroupId == id).ToDelete();
            //删除角色绑定的该分组的权限信息
            var task3 = _rolePermissionRepository.Find(m => m.MenuGroupId == id).ToDelete();

            await task1;
            await task2;
            await task3;
        }
    }

    public async Task<IEnumerable<OptionResultModel>> Select()
    {
        var list = await _repository.Find().ToList();
        return list.Select(m => new OptionResultModel
        {
            Label = m.Name,
            Value = m.Id
        });
    }
}