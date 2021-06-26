using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.Role.Dto;
using Mkh.Mod.Admin.Core.Domain.Role;
using Mkh.Utils.Map;
using Mkh.Utils.Models;

namespace Mkh.Mod.Admin.Core.Application.Role
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<IResultModel> Query()
        {
            var list = _repository.Find().ToList();

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
    }
}
