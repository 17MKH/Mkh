using System;
using System.Threading.Tasks;
using MediatR;
using Mkh.Mod.Admin.Core.Application.Roles.Dto;
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

    public async Task<Result<Guid>> Create(RoleCreateDto dto)
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

        var role = new Role(dto.Name, dto.Code);

        await _repository.InsertAsync(role);

        return result;
    }

    public async Task<Result> Update(RoleUpdateDto dto)
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

    public async Task<Result<RoleDetailsRto>> Get(Guid id)
    {
        var role = await _repository.GetAsync(id);

        return ResultBuilder.Success(new RoleDetailsRto
        {
            Id = role.Id,
            Name = role.Name,
            Code = role.Code,
        });
    }

    public async Task<Result> Delete(Guid id)
    {
        var result = await _repository.Find(m => m.Id == id).DeleteAsync();
        if (result)
        {
            _mediator.Publish()
        }

        return ResultBuilder.Condition(result);
    }
}