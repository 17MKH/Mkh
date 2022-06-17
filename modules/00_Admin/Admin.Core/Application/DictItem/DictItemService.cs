using System.Threading.Tasks;
using Mkh.Cache.Abstractions;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Query;
using Mkh.Mod.Admin.Core.Application.DictItem.Dto;
using Mkh.Mod.Admin.Core.Domain.Dict;
using Mkh.Mod.Admin.Core.Domain.DictGroup;
using Mkh.Mod.Admin.Core.Domain.DictItem;
using Mkh.Mod.Admin.Core.Infrastructure;
using Mkh.Utils.Map;

namespace Mkh.Mod.Admin.Core.Application.DictItem;

public class DictItemService : IDictItemService
{
    private readonly IMapper _mapper;
    private readonly IDictItemRepository _repository;
    private readonly IDictRepository _dictRepository;
    private readonly IDictGroupRepository _dictGroupRepository;
    private readonly ICacheProvider _cacheHandler;
    private readonly AdminCacheKeys _cacheKeys;
    private readonly AdminLocalizer _localizer;

    public DictItemService(IMapper mapper, IDictItemRepository repository, IDictRepository dictRepository, IDictGroupRepository dictGroupRepository, ICacheProvider cacheHandler, AdminCacheKeys cacheKeys, AdminLocalizer localizer)
    {
        _mapper = mapper;
        _repository = repository;
        _dictRepository = dictRepository;
        _dictGroupRepository = dictGroupRepository;
        _cacheHandler = cacheHandler;
        _cacheKeys = cacheKeys;
        _localizer = localizer;
    }

    public Task<PagingQueryResultModel<DictItemEntity>> Query(DictItemQueryDto dto)
    {
        var query = _repository.Find(m => m.ParentId == dto.ParentId);
        query.WhereNotNull(dto.GroupCode, m => m.GroupCode.Equals(dto.GroupCode));
        query.WhereNotNull(dto.DictCode, m => m.DictCode.Equals(dto.DictCode));
        query.WhereNotNull(dto.Name, m => m.Name.Contains(dto.Name));
        query.WhereNotNull(dto.Value, m => m.Value.Contains(dto.Value));

        return query.ToPaginationResult(dto.Paging);
    }

    [Transaction]
    public async Task<IResultModel> Add(DictItemAddDto dto)
    {
        if (!await _dictGroupRepository.Find(m => m.Code == dto.GroupCode).ToExists())
            return ResultModel.Failed(_localizer["当前分组不存在"]);

        if (!await _dictRepository.Find(m => m.Code == dto.DictCode).ToExists())
            return ResultModel.Failed(_localizer["字典不存在"]);

        if (await _repository.Find(m => m.GroupCode == dto.GroupCode && m.DictCode == dto.DictCode && m.Value == dto.Value).ToExists())
            return ResultModel.Failed(_localizer["当前项的值已存在"]);

        var entity = _mapper.Map<DictItemEntity>(dto);

        var result = await _repository.Add(entity);
        if (result)
        {
            await ClearCache(dto.GroupCode, dto.DictCode);
        }

        return ResultModel.Result(result);
    }

    public async Task<IResultModel> Edit(int id)
    {
        var entity = await _repository.Get(id);
        if (entity == null)
            return ResultModel.NotExists;

        var model = _mapper.Map<DictItemUpdateDto>(entity);
        return ResultModel.Success(model);
    }

    [Transaction]
    public async Task<IResultModel> Update(DictItemUpdateDto dto)
    {
        var entity = await _repository.Get(dto.Id);
        if (entity == null)
            return ResultModel.NotExists;

        if (!await _dictGroupRepository.Find(m => m.Code == dto.GroupCode).ToExists())
            return ResultModel.Failed(_localizer["当前分组不存在"]);

        if (!await _dictRepository.Find(m => m.Code == dto.DictCode).ToExists())
            return ResultModel.Failed(_localizer["字典不存在"]);

        if (await _repository.Find(m => m.GroupCode == dto.GroupCode && m.DictCode == dto.DictCode && m.Value == dto.Value && m.Id != dto.Id).ToExists())
            return ResultModel.Failed(_localizer["当前项的值已存在"]);

        _mapper.Map(dto, entity);

        var result = await _repository.Update(entity);
        if (result)
        {
            await ClearCache(dto.GroupCode, dto.DictCode);
        }
        return ResultModel.Result(result);
    }

    public async Task<IResultModel> Delete(int id)
    {
        var entity = await _repository.Get(id);
        if (entity == null)
            return ResultModel.NotExists;

        var result = await _repository.SoftDelete(id);
        if (result)
        {
            await ClearCache(entity.GroupCode, entity.DictCode);
        }
        return ResultModel.Result(result);
    }

    private async Task ClearCache(string groupCode, string dictCode)
    {
        await _cacheHandler.Remove(_cacheKeys.DictTree(groupCode, dictCode));
        await _cacheHandler.Remove(_cacheKeys.DictSelect(groupCode, dictCode));
    }
}