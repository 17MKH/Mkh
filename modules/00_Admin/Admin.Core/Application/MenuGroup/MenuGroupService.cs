using System.Linq;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Mod.Admin.Core.Application.MenuGroup.Dto;
using Mkh.Mod.Admin.Core.Domain.MenuGroup;
using Mkh.Utils.Map;
using Mkh.Utils.Models;

namespace Mkh.Mod.Admin.Core.Application.MenuGroup
{
    public class MenuGroupService : IMenuGroupService
    {
        private readonly IMapper _mapper;
        private readonly IMenuGroupRepository _repository;

        public MenuGroupService(IMapper mapper, IMenuGroupRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public Task<IResultModel> Query(MenuGroupQueryDto dto)
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

            _mapper.Map(entity, dto);

            var result = await _repository.Update(entity);
            return ResultModel.Result(result);
        }

        [Transaction]
        public async Task<IResultModel> Delete(int id)
        {
            //如果菜单分组无数据，则将新增的分组激活状态设置为true
            if (await _repository.Find().ToCount() < 2)
            {
                return ResultModel.Failed("系统需要至少一个菜单分组");
            }

            var entity = await _repository.Get(id);
            if (entity == null)
                return ResultModel.NotExists;

            var result = await _repository.Delete(id);
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
}
