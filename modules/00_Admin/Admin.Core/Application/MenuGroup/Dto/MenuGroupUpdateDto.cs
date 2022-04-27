using System.ComponentModel.DataAnnotations;
using Mkh.Mod.Admin.Core.Domain.MenuGroup;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Application.MenuGroup.Dto;

[ObjectMap(typeof(MenuGroupEntity), true)]
public class MenuGroupUpdateDto : MenuGroupAddDto
{
    [Required(ErrorMessage = "请选择要修改的分组")]
    [Range(1, int.MaxValue, ErrorMessage = "请选择要修改的分组")]
    public int Id { get; set; }
}