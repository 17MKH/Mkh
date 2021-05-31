using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Mod.Admin.Core.Domain.MenuGroup
{
    [Table("Menu_Group")]
    public class MenuGroupEntity : EntityBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Length(100)]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Length(500)]
        [Nullable]
        public string Remarks { get; set; }
    }
}
