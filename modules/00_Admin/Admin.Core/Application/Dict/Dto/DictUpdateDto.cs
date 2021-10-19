using System.ComponentModel.DataAnnotations;
using Mkh.Mod.Admin.Core.Domain.Dict;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Application.Dict.Dto;

[ObjectMap(typeof(DictEntity), true)]
public class DictUpdateDto : DictAddDto
{
    [Required(ErrorMessage = "请选择要修改的字典")]
    [Range(1, int.MaxValue, ErrorMessage = "请选择要修改的字典")]
    public int Id { get; set; }
}