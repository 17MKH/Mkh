using System.ComponentModel.DataAnnotations;
using Mkh.Mod.Admin.Core.Domain.Menu;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Application.Menu.Dto;

[ObjectMap(typeof(MenuEntity), true)]
public class MenuUpdateDto : MenuAddDto
{
    [Required(ErrorMessage = "Please select the menu you want to modify")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select the menu you want to modify")]
    public int Id { get; set; }
}