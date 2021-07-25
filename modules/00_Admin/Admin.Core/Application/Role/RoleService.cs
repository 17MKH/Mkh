using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Mod.Admin.Core.Application.Role.Dto;
using Mkh.Mod.Admin.Core.Domain.MenuGroup;
using Mkh.Mod.Admin.Core.Domain.Role;
using Mkh.Mod.Admin.Core.Domain.RoleButton;
using Mkh.Mod.Admin.Core.Domain.RoleMenu;
using Mkh.Mod.Admin.Core.Domain.RolePermission;
using Mkh.Utils.Map;
using Mkh.Utils.Models;

namespace Mkh.Mod.Admin.Core.Application.Role
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IRoleMenuRepository _roleMenuRepository;
        private readonly IRoleButtonRepository _roleButtonRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public RoleService(IRoleRepository repository, IMapper mapper, IRoleMenuRepository roleMenuRepository, IRoleButtonRepository roleButtonRepository, IRolePermissionRepository rolePermissionRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _roleMenuRepository = roleMenuRepository;
            _roleButtonRepository = roleButtonRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public Task<IResultModel> Query()
        {
            var list = _repository.Find()
                .LeftJoin<MenuGroupEntity>(m => m.T1.MenuGroupId == m.T2.Id)
                .Select(m => new { m.T1, MenuGroupName = m.T2.Name })
                .ToList();

            return ResultModel.SuccessAsync(list);
        }

        public async Task<IResultModel> Add(RoleAddDto dto)
        {
            if (await _repository.Find(m => m.Name == dto.Name).ToExists())
            {
                return ResultModel.Failed($"角色名称({dto.Name})已存在~");
            }

            if (await _repository.Find(m => m.Code == dto.Code).ToExists())
            {
                return ResultModel.Failed($"角色编码({dto.Code})已存在~");
            }

            var role = _mapper.Map<RoleEntity>(dto);

            return ResultModel.Result(await _repository.Add(role));
        }

        public async Task<IResultModel> Edit(int id)
        {
            var role = await _repository.Get(id);
            if (role == null)
                return ResultModel.NotExists;

            return ResultModel.Success(_mapper.Map<RoleUpdateDto>(role));
        }

        public async Task<IResultModel> Update(RoleUpdateDto dto)
        {
            var role = await _repository.Get(dto.Id);
            if (role == null)
                return ResultModel.NotExists;

            if (await _repository.Find(m => m.Name == dto.Name && m.Id != dto.Id).ToExists())
            {
                return ResultModel.Failed($"角色名称({dto.Name})已存在~");
            }

            if (await _repository.Find(m => m.Code == dto.Code && m.Id != dto.Id).ToExists())
            {
                return ResultModel.Failed($"角色编码({dto.Code})已存在~");
            }

            _mapper.Map(dto, role);

            return ResultModel.Success(await _repository.Update(role));
        }

        public async Task<IResultModel> Delete(int id)
        {
            var role = await _repository.Get(id);
            if (role == null)
                return ResultModel.NotExists;

            var result = await _repository.Delete(id);

            return ResultModel.Result(result);
        }

        public async Task<IResultModel> QueryBindMenus(int id)
        {
            var role = await _repository.Get(id);
            if (role == null)
                return ResultModel.NotExists;

            var roleMenus = await _roleMenuRepository
                .Find(m => m.MenuGroupId == role.MenuGroupId && m.RoleId == id)
                .ToList();

            var roleButtons = await _roleButtonRepository
                .Find(m => m.MenuGroupId == role.MenuGroupId && m.RoleId == id)
                .ToList();

            var menus = new List<BindMenuUpdateDto>();
            foreach (var roleMenu in roleMenus)
            {
                var bindMenuUpdateDto = new BindMenuUpdateDto
                {
                    MenuId = roleMenu.MenuId,
                    MenuType = roleMenu.MenuType,
                    Buttons = roleButtons.Where(m => m.MenuId == roleMenu.MenuId).Select(m => m.ButtonCode).ToList()
                };

                menus.Add(bindMenuUpdateDto);
            }

            return ResultModel.Success(new RoleBindMenusUpdateDto
            {
                RoleId = id,
                Menus = menus
            });
        }

        [Transaction]
        public async Task<IResultModel> UpdateBindMenus(RoleBindMenusUpdateDto dto)
        {
            var role = await _repository.Get(dto.RoleId);
            if (role == null)
                return ResultModel.NotExists;

            //删除当前角色已绑定的菜单数据
            await _roleMenuRepository.Find(m => m.MenuGroupId == role.MenuGroupId && m.RoleId == role.Id).ToDelete();

            //删除当前角色已绑定的按钮数据
            await _roleButtonRepository.Find(m => m.MenuGroupId == role.MenuGroupId && m.RoleId == role.Id).ToDelete();

            //删除当前角色已绑定的权限数据
            await _rolePermissionRepository.Find(m => m.MenuGroupId == role.MenuGroupId && m.RoleId == role.Id).ToDelete();

            //添加绑定菜单数据
            if (dto.Menus.NotNullAndEmpty())
            {
                foreach (var dtoMenu in dto.Menus)
                {
                    await _roleMenuRepository.Add(new RoleMenuEntity
                    {
                        MenuGroupId = role.MenuGroupId,
                        RoleId = role.Id,
                        MenuId = dtoMenu.MenuId,
                        MenuType = dtoMenu.MenuType
                    });

                    var tasks = new List<Task>();

                    //添加绑定按钮数据
                    if (dtoMenu.Buttons.NotNullAndEmpty())
                    {
                        foreach (var dtoButton in dtoMenu.Buttons)
                        {
                            tasks.Add(_roleButtonRepository.Add(new RoleButtonEntity
                            {
                                MenuGroupId = role.MenuGroupId,
                                MenuId = dtoMenu.MenuId,
                                RoleId = role.Id,
                                ButtonCode = dtoButton.ToLower()
                            }));
                        }
                    }

                    //添加绑定权限数据
                    if (dtoMenu.Permissions.NotNullAndEmpty())
                    {
                        foreach (var dtoPermission in dtoMenu.Permissions)
                        {
                            tasks.Add(_rolePermissionRepository.Add(new RolePermissionEntity
                            {
                                MenuGroupId = role.MenuGroupId,
                                RoleId = role.Id,
                                MenuId = dtoMenu.MenuId,
                                PermissionCode = dtoPermission.ToLower()
                            }));
                        }
                    }

                    await Task.WhenAll(tasks);
                }
            }

            return ResultModel.Success();
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
}
