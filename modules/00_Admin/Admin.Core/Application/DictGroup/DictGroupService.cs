using System.Linq;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Query;
using Mkh.Mod.Admin.Core.Application.DictGroup.Dto;
using Mkh.Mod.Admin.Core.Domain.Dict;
using Mkh.Mod.Admin.Core.Domain.DictGroup;
using Mkh.Mod.Admin.Core.Infrastructure;
using Mkh.Utils.Map;

namespace Mkh.Mod.Admin.Core.Application.DictGroup;

public class DictGroupService : IDictGroupService
{
    private readonly IMapper _mapper;
    private readonly IDictGroupRepository _repository;
    private readonly IDictRepository _dictRepository;
    private readonly AdminLocalizer _localizer;

    public DictGroupService(IMapper mapper, IDictGroupRepository repository, IDictRepository dictRepository, AdminLocalizer localizer)
    {
        _mapper = mapper;
        _repository = repository;
        _dictRepository = dictRepository;
        _localizer = localizer;
    }

    public Task<PagingQueryResultModel<DictGroupEntity>> Query(DictGroupQueryDto dto)
    {
        var query = _repository.Find();
        query.WhereNotNull(dto.Name, m => m.Name.Equals(dto.Name));
        return query.ToPaginationResult(dto.Paging);
    }

    public async Task<IResultModel> Add(DictGroupAddDto dto)
    {
        if (await _repository.Find(m => m.Code == dto.Code).ToExists())
            return ResultModel.Failed(_localizer["分组编码已存在"]);

        var entity = _mapper.Map<DictGroupEntity>(dto);

        var result = await _repository.Add(entity);

        return ResultModel.Result(result);
    }

    public async Task<IResultModel> Edit(int id)
    {
        var entity = await _repository.Get(id);
        if (entity == null)
            return ResultModel.NotExists;

        var model = _mapper.Map<DictGroupUpdateDto>(entity);
        return ResultModel.Success(model);
    }

    public async Task<IResultModel> Update(DictGroupUpdateDto dto)
    {
        var entity = await _repository.Get(dto.Id);
        if (entity == null)
            return ResultModel.NotExists;

        if (await _repository.Find(m => m.Code == dto.Code && m.Id != dto.Id).ToExists())
            return ResultModel.Failed(_localizer["分组编码已存在"]);

        _mapper.Map(dto, entity);

        var result = await _repository.Update(entity);
        return ResultModel.Result(result);
    }

    public async Task<IResultModel> Delete(int id)
    {
        var group = await _repository.Get(id);
        if (group == null)
            return ResultModel.NotExists;

        if (await _dictRepository.Find(m => m.GroupCode == group.Code).ToExists())
            return ResultModel.Failed(_localizer["该分组下面包含字典数据，请先删除字典数据后在删除分组"]);

        var result = await _repository.SoftDelete(id);
        return ResultModel.Result(result);
    }

    public async Task<IResultModel> Select()
    {
        var list = await _repository.Find().ToList();
        var options = list.Select(m => new OptionResultModel
        {
            Label = m.Name,
            Value = m.Code,
            Data = m.Id
        });

        return ResultModel.Success(options);
    }
}