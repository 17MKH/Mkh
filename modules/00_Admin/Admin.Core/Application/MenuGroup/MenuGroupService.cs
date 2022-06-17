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

public class MenuGroupService : IMenuGroupService
{
    private readonly IMapper _mapper;
    private readonly IMenuGroupRepository _repository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IRoleMenuRepository _roleMenuRepository;
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly AdminLocalizer _localizer;

    public MenuGroupService(IMapper mapper, IMenuGroupRepository repository, IRoleRepository roleRepository, IMenuRepository menuRepository, IRoleMenuRepository roleMenuRepository, IRolePermissionRepository rolePermissionRepository, AdminLocalizer localizer)
    {
        _mapper = mapper;
        _repository = repository;
        _roleRepository = roleRepository;
        _menuRepository = menuRepository;
        _roleMenuRepository = roleMenuRepository;
        _rolePermissionRepository = rolePermissionRepository;
        _localizer = localizer;
    }

    public Task<PagingQueryResultModel<MenuGroupEntity>> Query(MenuGroupQueryDto dto)
    {
        var query = _repository.Find();
        query.WhereNotNull(dto.Name, m => m.Name.Equals(dto.Name));

        return query.ToPaginationResult(dto.Paging);
    }

    public async Task<IResultModel> Add(MenuGroupAddDto dto)
    {
        var entity = _mapper.Map<MenuGroupEntity>(dto);

        var result = await _repository.Add(entity);
        return ResultModel.Result(result);
    }

    public async Task<IResultModel> Edit(int id)
    {
        var entity = await _repository.Get(id);
        if (entity == null)
            return ResultModel.NotExists;

        var model = _mapper.Map<MenuGroupUpdateDto>(entity);
        return ResultModel.Success(model);
    }

    public async Task<IResultModel> Update(MenuGroupUpdateDto dto)
    {
        var entity = await _repository.Get(dto.Id);
        if (entity == null)
            return ResultModel.NotExists;

        _mapper.Map(dto, entity);

        var result = await _repository.Update(entity);
        return ResultModel.Result(result);
    }

    [Transaction]
    public async Task<IResultModel> Delete(int id)
    {
        if (!await _repository.Exists(id))
            return ResultModel.NotExists;

        //如果有角色绑定了该菜单分组，则不允许删除
        if (await _roleRepository.Find(m => m.MenuGroupId == id).ToExists())
            return ResultModel.Failed(_localizer["有角色绑定了该菜单分组，不允许删除"]);

        var result = await _repository.Delete(id);
        if (result)
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

        return ResultModel.Result(result);
    }

    public async Task<IResultModel> Select()
    {
        var list = await _repository.Find().ToList();
        var options = list.Select(m => new OptionResultModel
        {
            Label = m.Name,
            Value = m.Id
        });

        return ResultModel.Success(options);
    }
}