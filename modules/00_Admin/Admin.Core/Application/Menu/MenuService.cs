using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Mod.Admin.Core.Application.Menu.Dto;
using Mkh.Mod.Admin.Core.Domain.Menu;
using Mkh.Mod.Admin.Core.Domain.RoleButton;
using Mkh.Mod.Admin.Core.Domain.RoleMenu;
using Mkh.Mod.Admin.Core.Domain.RolePermission;
using Mkh.Utils.Map;

namespace Mkh.Mod.Admin.Core.Application.Menu
{
    public class MenuService : IMenuService
    {
        private readonly IMapper _mapper;
        private readonly IMenuRepository _repository;
        private readonly IRoleMenuRepository _roleMenuRepository;
        private readonly IRoleButtonRepository _roleButtonRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public MenuService(IMapper mapper, IMenuRepository repository, IRoleMenuRepository roleMenuRepository, IRoleButtonRepository roleButtonRepository, IRolePermissionRepository rolePermissionRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _roleMenuRepository = roleMenuRepository;
            _roleButtonRepository = roleButtonRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public Task<IResultModel> Query(MenuQueryDto dto)
        {
            var query = _repository.Find(m => m.GroupId == dto.GroupId && m.ParentId == dto.ParentId);
            query.WhereNotNull(dto.Name, m => m.Name.Equals(dto.Name));
            query.OrderBy(m => m.Sort);

            return query.ToPaginationResult(dto.Paging);
        }

        [Transaction]
        public async Task<IResultModel> Add(MenuAddDto dto)
        {
            var menu = _mapper.Map<MenuEntity>(dto);
            menu.Level = 1;

            if (dto.ParentId > 0)
            {
                var parent = await _repository.Get(dto.ParentId);
                if (parent == null)
                {
                    return ResultModel.Failed("父节点菜单不存在~");
                }

                menu.Level = parent.Level + 1;
            }

            var result = await _repository.Add(menu);
            return ResultModel.Result(result);
        }

        public async Task<IResultModel> Edit(int id)
        {
            var menu = await _repository.Get(id);
            if (menu == null)
                return ResultModel.NotExists;

            var model = _mapper.Map<MenuUpdateDto>(menu);

            return ResultModel.Success(model);
        }

        public async Task<IResultModel> Update(MenuUpdateDto dto)
        {
            var menu = await _repository.Get(dto.Id);
            if (menu == null)
                return ResultModel.NotExists;

            _mapper.Map(dto, menu);

            var result = await _repository.Update(menu);

            return ResultModel.Result(result);
        }

        [Transaction]
        public async Task<IResultModel> Delete(int id)
        {
            var result = await _repository.Delete(id);
            if (result)
            {
                //删除角色菜单绑定数据
                var deleteRoleMenuTask = _roleMenuRepository.Find(m => m.MenuId == id).ToDelete();
                //删除角色按钮绑定数据
                var deleteRoleButtonTask = _roleButtonRepository.Find(m => m.MenuId == id).ToDelete();
                //删除角色权限绑定数据
                var deleteRolePermissionTask = _rolePermissionRepository.Find(m => m.MenuId == id).ToDelete();

                await deleteRoleMenuTask;
                await deleteRoleButtonTask;
                await deleteRolePermissionTask;
            }

            return ResultModel.Result(result);
        }

        public async Task<IResultModel> GetTree(int groupId)
        {
            var root = new TreeResultModel<MenuEntity>();
            var all = await _repository.Find(m => m.GroupId == groupId).ToList();

            ResolveTree(all, root);

            return ResultModel.Success(root.Children);
        }

        private void ResolveTree(IList<MenuEntity> all, TreeResultModel<MenuEntity> parent)
        {
            foreach (var menu in all.Where(m => m.ParentId == parent.Id).OrderBy(m => m.Sort))
            {
                var child = new TreeResultModel<MenuEntity>
                {
                    Id = menu.Id,
                    Label = menu.Name,
                    Item = menu
                };

                child.Path.AddRange(parent.Path);
                child.Path.Add(child.Label);

                //只有节点菜单才有子级
                if (menu.Type == MenuType.Node)
                {
                    ResolveTree(all, child);
                }

                parent.Children.Add(child);
            }
        }

        [Transaction]
        public async Task<IResultModel> UpdateSort(IList<MenuEntity> menus)
        {
            if (!menus.Any())
                return ResultModel.Success();

            var tasks = new List<Task>();
            foreach (var menu in menus)
            {
                var task = _repository.Find(m => m.Id == menu.Id).ToUpdate(m => new MenuEntity
                {
                    ParentId = menu.ParentId,
                    Sort = menu.Sort
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            return ResultModel.Success();
        }

    }
}
