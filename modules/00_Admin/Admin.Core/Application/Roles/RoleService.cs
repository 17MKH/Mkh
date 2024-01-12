using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Mkh.Mod.Admin.Core.Application.Roles.Dto;
using Mkh.Mod.Admin.Core.Application.Roles.Event;
using Mkh.Mod.Admin.Core.Application.Roles.Rto;
using Mkh.Mod.Admin.Core.Domain.Roles;
using Mkh.Mod.Admin.Core.Infrastructure;

namespace Mkh.Mod.Admin.Core.Application.Roles;

internal class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;
    private readonly IMediator _mediator;

    public RoleService(IRoleRepository roleRepository, IMediator mediator)
    {
        _repository = roleRepository;
        _mediator = mediator;
    }

    public async Task<Result<Guid>> CreateAsync(RoleCreateDto dto)
    {
        var result = ResultBuilder.Success<Guid>();

        if (await _repository.ExistsAsync(m => m.Name.Equals(dto.Name)))
        {
            return result.Failed(AdminErrorCode.RoleNameExists);
        }

        if (await _repository.ExistsAsync(m => m.Code.Equals(dto.Code)))
        {
            return result.Failed(AdminErrorCode.RoleCodeExists);
        }

        var role = new Role(dto.Name, dto.Code)
        {
            Remarks = dto.Remarks
        };

        await _repository.InsertAsync(role);

        return result;
    }

    public async Task<Result> UpdateAsync(RoleUpdateDto dto)
    {
        var role = await _repository.GetAsync(dto.Id);

        var result = ResultBuilder.Success();

        if (await _repository.ExistsAsync(m => m.Id != dto.Id && m.Name.Equals(dto.Name)))
        {
            return result.Failed(AdminErrorCode.RoleNameExists);
        }

        if (await _repository.ExistsAsync(m => m.Id != dto.Id && m.Code.Equals(dto.Code)))
        {
            return result.Failed(AdminErrorCode.RoleCodeExists);
        }

        role.Name = dto.Name;
        role.Code = dto.Code;

        await _repository.UpdateAsync(role);

        return result;
    }

    public async Task<Result<RoleDetailsRto>> GetAsync(Guid id)
    {
        var role = await _repository.GetAsync(id);

        return ResultBuilder.Success(new RoleDetailsRto
        {
            Id = role.Id,
            Name = role.Name,
            Code = role.Code,
            Remarks = role.Remarks
        });
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var role = await _repository.GetAsync(id);

        await _repository.DeleteAsync(role);

        await _mediator.Publish(new RoleDeleteEvent(id, role.Name, role.Code));

        return ResultBuilder.Success();
    }

    public async Task<bool> IsExistAsync(Guid[] ids)
    {
        if (ids.IsNullOrEmpty())
            return true;

        var count = await _repository.Find(m => ids.Contains(m.Id)).CountAsync();

        return count == ids.Length;
    }
}