using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mkh.Cache.Abstractions;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Mod.Admin.Core.Application.Role.Dto;
using Mkh.Mod.Admin.Core.Application.Role.Rto;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Mod.Admin.Core.Domain.MenuGroup;
using Mkh.Mod.Admin.Core.Domain.RoleButton;
using Mkh.Mod.Admin.Core.Domain.RoleMenu;
using Mkh.Mod.Admin.Core.Domain.RolePermission;
using Mkh.Mod.Admin.Core.Domain.Roles;
using Mkh.Mod.Admin.Core.Infrastructure;
using Mkh.Utils.Map;

namespace Mkh.Mod.Admin.Core.Application.Role;

internal class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public Task<IList<RoleDetailsRto>> QueryAll()
    {
        return _roleRepository.Find().ToListAsync<RoleDetailsRto>();
    }

    public Task<int> Add(RoleAddDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<RoleDetailsRto> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task Update(RoleUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}