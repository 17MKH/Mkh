using System.ComponentModel.DataAnnotations;

namespace Mkh.Mod.Admin.Core.Application.Menu.Dto
{
    public class MenuUpdateDto : MenuAddDto
    {
        [Required(ErrorMessage = "请选择要修改的角色")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择要修改的角色")]
        public int Id { get; set; }
    }
}
