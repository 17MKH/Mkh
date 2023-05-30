using System.ComponentModel.DataAnnotations;
using Mkh.Mod.Admin.Core.Domain.MenuGroup;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Application.MenuGroup.Dto;

[ObjectMap(typeof(MenuGroupEntity))]
public class MenuGroupAddDto
{
    [Required(ErrorMessage = "请输入分组名称")]
    public string Name { get; set; }

    public string Remarks { get; set; }
}