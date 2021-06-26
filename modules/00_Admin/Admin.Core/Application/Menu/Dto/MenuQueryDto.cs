using Mkh.Data.Abstractions.Query;

namespace Mkh.Mod.Admin.Core.Application.Menu.Dto
{
    public class MenuQueryDto : QueryDto
    {
        /// <summary>
        /// 父节点
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
    }
}
