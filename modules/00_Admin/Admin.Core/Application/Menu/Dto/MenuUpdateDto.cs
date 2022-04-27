using System.ComponentModel.DataAnnotations;
using Mkh.Mod.Admin.Core.Domain.Menu;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Application.Menu.Dto;

[ObjectMap(typeof(MenuEntity), true)]
public class MenuUpdateDto : MenuAddDto
{
    [Required(ErrorMessage = "请选择要修改的菜单")]
    [Range(1, int.MaxValue, ErrorMessage = "请选择要修改的菜单")]
    public int Id { get; set; }
}