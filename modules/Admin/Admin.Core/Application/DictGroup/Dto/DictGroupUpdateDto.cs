using System.ComponentModel.DataAnnotations;
using Mkh.Mod.Admin.Core.Domain.DictGroup;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Application.DictGroup.Dto;

[ObjectMap(typeof(DictGroupEntity), true)]
public class DictGroupUpdateDto : DictGroupAddDto
{
    [Required(ErrorMessage = "请选择要修改的分组")]
    [Range(1, int.MaxValue, ErrorMessage = "请选择要修改的分组")]
    public int Id { get; set; }
}