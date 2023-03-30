using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.DictGroup.Dto;
using Mkh.Mod.Admin.Core.Domain.Dict;
using Mkh.Mod.Admin.Core.Domain.DictGroup;
using Mkh.Mod.Admin.Core.Infrastructure;
using Mkh.Utils.Map;

namespace Mkh.Mod.Admin.Core.Application.DictGroup;

internal class DictGroupService : IDictGroupService
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

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<IResultModel<IList<DictGroupEntity>>> Query(DictGroupQueryDto dto)
    {
        var query = _repository.Find();
        query.WhereNotNull(dto.Name, m => m.Name.Equals(dto.Name));
        var result = await query.ToList();
        return ResultModel.Success(result);
    }

    public async Task<int> Add(DictGroupAddDto dto)
    {
        if (await _repository.Find(m => m.Code == dto.Code).ToExists())
            throw new AdminException(AdminErrorCode.DictGroupCodeExists);

        var entity = _mapper.Map<DictGroupEntity>(dto);

        await _repository.Add(entity);

        return entity.Id;
    }

    public async Task<DictGroupUpdateDto> Edit(int id)
    {
        var entity = await _repository.Get(id);

        return _mapper.Map<DictGroupUpdateDto>(entity);
    }

    public async Task Update(DictGroupUpdateDto dto)
    {
        var entity = await _repository.Get(dto.Id);

        if (await _repository.Find(m => m.Code == dto.Code && m.Id != dto.Id).ToExists())
            throw new AdminException(AdminErrorCode.DictGroupCodeExists);

        _mapper.Map(dto, entity);

        await _repository.Update(entity);
    }

    public async Task Delete(int id)
    {
        var group = await _repository.Get(id);

        if (await _dictRepository.Find(m => m.GroupCode == group.Code).ToExists())
            throw new AdminException(AdminErrorCode.DictGroupIncludeDict);

        await _repository.SoftDelete(id);
    }

    public async Task<IEnumerable<OptionResultModel>> Select()
    {
        var list = await _repository.Find().ToList();
        return list.Select(m => new OptionResultModel
        {
            Label = m.Name,
            Value = m.Code,
            Data = m.Id
        });
    }
}
