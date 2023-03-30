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

internal class DictItemService : IDictItemService
{
    private readonly IMapper _mapper;
    private readonly IDictItemRepository _repository;
    private readonly IDictRepository _dictRepository;
    private readonly IDictGroupRepository _dictGroupRepository;
    private readonly ICacheProvider _cacheHandler;
    private readonly AdminCacheKeys _cacheKeys;

    public DictItemService(IMapper mapper, IDictItemRepository repository, IDictRepository dictRepository, IDictGroupRepository dictGroupRepository, ICacheProvider cacheHandler, AdminCacheKeys cacheKeys)
    {
        _mapper = mapper;
        _repository = repository;
        _dictRepository = dictRepository;
        _dictGroupRepository = dictGroupRepository;
        _cacheHandler = cacheHandler;
        _cacheKeys = cacheKeys;
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
    public async Task<int> Add(DictItemAddDto dto)
    {
        if (!await _dictGroupRepository.Find(m => m.Code == dto.GroupCode).ToExists())
            throw new AdminException(AdminErrorCode.DictGroupNotExists);

        if (!await _dictRepository.Find(m => m.Code == dto.DictCode).ToExists())
            throw new AdminException(AdminErrorCode.DictNotExists);

        if (await _repository.Find(m => m.GroupCode == dto.GroupCode && m.DictCode == dto.DictCode && m.Value == dto.Value).ToExists())
            throw new AdminException(AdminErrorCode.DictItemValueExists);

        var entity = _mapper.Map<DictItemEntity>(dto);

        await _repository.Add(entity);

        await ClearCache(dto.GroupCode, dto.DictCode);

        return entity.Id;
    }

    public async Task<DictItemUpdateDto> Edit(int id)
    {
        var entity = await _repository.Get(id);

        return _mapper.Map<DictItemUpdateDto>(entity);
    }

    [Transaction]
    public async Task Update(DictItemUpdateDto dto)
    {
        var entity = await _repository.Get(dto.Id);

        if (!await _dictGroupRepository.Find(m => m.Code == dto.GroupCode).ToExists())
            throw new AdminException(AdminErrorCode.DictGroupNotExists);

        if (!await _dictRepository.Find(m => m.Code == dto.DictCode).ToExists())
            throw new AdminException(AdminErrorCode.DictNotExists);

        if (await _repository.Find(m => m.GroupCode == dto.GroupCode && m.DictCode == dto.DictCode && m.Value == dto.Value && m.Id != dto.Id).ToExists())
            throw new AdminException(AdminErrorCode.DictItemValueExists);

        _mapper.Map(dto, entity);

        await _repository.Update(entity);

        await ClearCache(dto.GroupCode, dto.DictCode);
    }

    public async Task Delete(int id)
    {
        var entity = await _repository.Get(id);

        if (await _repository.SoftDelete(id))
        {
            await ClearCache(entity.GroupCode, entity.DictCode);
        }
    }

    private async Task ClearCache(string groupCode, string dictCode)
    {
        await _cacheHandler.Remove(_cacheKeys.DictTree(groupCode, dictCode));
        await _cacheHandler.Remove(_cacheKeys.DictSelect(groupCode, dictCode));
    }
}