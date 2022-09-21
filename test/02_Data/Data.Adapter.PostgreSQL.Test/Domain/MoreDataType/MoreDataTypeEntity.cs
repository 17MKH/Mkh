using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Adapter.PostgreSQL.Test.Domain.MoreDataType;


[Table("more_data_type")]
public class MoreDataTypeEntity : EntityBaseSoftDelete<Guid>
{
    [Length(32)]
    public string Name { get; set; }

    // 取消注释更改字段
    // [Precision(18,2)]
    public decimal Money1 { get; set; }

    public float Money2 { get; set; }

    public double Money3 { get; set; }

    public TestEnum EnumField { get; set; }

    public DateTime Dt { get; set; }

    [Column("dt2", Type = "timestamp without time zone")]
    public DateTime Dt2 { get; set; }

    public int Field1 { get; set; }

    public long Field2 { get; set; }

    public short Field3 { get; set; }

    public bool Field4 { get; set; }

    [Column("f_byte")]
    public byte FByte { get; set; }

    public enum TestEnum
    {
        One,
        Two,
    }
}
