using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Utils.Helpers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mkh.Data.Adapter.PostgreSQL;

internal sealed class PostgreSQLDbAdapter : DbAdapterAbstract
{
    public override DbProvider Provider => DbProvider.PostgreSQL;

    /// <summary>
    /// 左引号
    /// </summary>
    public override char LeftQuote => '"';

    /// <summary>
    /// 右引号
    /// </summary>
    public override char RightQuote => '"';

    /// <summary>
    /// 获取最后新增ID语句
    /// </summary>
    public override string IdentitySql => "RETURNING \"id\";";

    public override string BooleanFalseValue => "false";
    public override string BooleanTrueValue => "true";

    public override bool SqlLowerCase => true;

    public override Guid CreateSequentialGuid()
    {
        return GuidGenerator.Create(SequentialGuidType.SequentialAsString);
    }

    

    public override string GenerateFirstSql(string select, string table, string where, string sort, string groupBy = null, string having = null)
    {
        return GeneratePagingSql(select, table, where, sort, 0, 1, groupBy, having);
    }

    public override string GeneratePagingSql(string select, string table, string where, string sort, int skip, int take, string groupBy = null, string having = null)
    {
        var sqlBuilder = new StringBuilder();
        sqlBuilder.AppendFormat("SELECT {0} FROM {1}", select, table);
        if (where.NotNull())
            sqlBuilder.AppendFormat(" {0}", where);

        if (groupBy.NotNull())
            sqlBuilder.Append(groupBy);

        if (having.NotNull())
            sqlBuilder.Append(having);

        if (sort.NotNull())
            sqlBuilder.AppendFormat("{0}", sort);

        if (skip == 0)
            sqlBuilder.AppendFormat(" LIMIT {0}", take);
        else
            sqlBuilder.AppendFormat(" LIMIT {0} offset {1}", take, skip);

        return sqlBuilder.ToString();
    }

    public override IDbConnection NewConnection(string connectionString)
    {
        return new NpgsqlConnection(connectionString);
    }

    public override void ResolveColumn(IColumnDescriptor columnDescriptor)
    {
        var propertyType = columnDescriptor.PropertyInfo.PropertyType;
        var isNullable = propertyType.IsNullable();
        if (isNullable)
        {
            propertyType = Nullable.GetUnderlyingType(propertyType);
            if (propertyType == null)
                throw new Exception("Property2Column error");
        }

        //使用指定的字段类型
        var typeName = columnDescriptor.TypeName;
        if (!string.IsNullOrWhiteSpace(typeName))
        {
            return;
        }

        if (propertyType.IsEnum)
        {
            if (!isNullable)
            {
                columnDescriptor.DefaultValue = "0";
            }

            columnDescriptor.TypeName = "int2";
            return;
        }

        if (propertyType.IsGuid())
        {
            columnDescriptor.TypeName = "uuid";
            columnDescriptor.Length = 36;
            return;
        }

        var typeCode = Type.GetTypeCode(propertyType);
        switch (typeCode)
        {
            case TypeCode.Char:
                columnDescriptor.TypeName = "CHAR";
                break;
            case TypeCode.String:
                columnDescriptor.TypeName = columnDescriptor.Length < 1 ? "TEXT" : "VARCHAR";
                break;
            case TypeCode.Boolean:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "false";
                }

                columnDescriptor.TypeName = "bool";
                break;
            case TypeCode.Byte:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "0";
                }
                columnDescriptor.TypeName = "bytea";
                break;
            case TypeCode.Int16:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "0";
                }
                columnDescriptor.TypeName = "int2";
                break;
            case TypeCode.Int32:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "0";
                }
                columnDescriptor.TypeName = "int4";
                break;
            case TypeCode.Int64:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "0";
                }

                columnDescriptor.TypeName = "int8";
                break;
            case TypeCode.DateTime:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "CURRENT_TIMESTAMP";
                }
                columnDescriptor.TypeName = "timestamp";
                break;
            case TypeCode.Decimal:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "0";
                }

                columnDescriptor.TypeName = "numeric";
                break;
            case TypeCode.Double:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "0";
                }

                columnDescriptor.TypeName = "float8";
                break;
            case TypeCode.Single:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "0";
                }

                columnDescriptor.TypeName = "float4";
                break;
        }

        //主键默认值为null
        if (columnDescriptor.IsPrimaryKey)
        {
            columnDescriptor.DefaultValue = null;
        }
    }


    #region 函数映射
    public override string FunctionMapper(string sourceName, string columnName, Type dataType = null, object[] args = null)
    {
        switch (sourceName)
        {
            case "Substring":
                return Mapper_Substring(columnName, args[0], args.Length > 1 ? args[1] : null);
            case "ToString":
                if (dataType.IsDateTime() && args[0] != null)
                {
                    return $"to_char(columnName, '{args[0]}')";
                }
                return string.Empty;
            case "Replace":
                return $"REPLACE({columnName},'{args[0]}','{args[1]}')";
            case "ToLower":
                return $"LOWER({columnName})";
            case "ToUpper":
                return $"UPPER({columnName})";
            case "Length":
                return $"CHAR_LENGTH({columnName})";
            case "Count":
                return "COUNT(0)";
            case "Sum":
                return $"SUM({columnName})";
            case "Avg":
                return $"AVG({columnName})";
            case "Max":
                return $"MAX({columnName})";
            case "Min":
                return $"MIN({columnName})";
            default:
                return string.Empty;
        }
    }

    private string Mapper_Substring(string columnName, object arg0, object arg1)
    {
        if (arg1 != null)
        {
            return $"SUBSTRING({columnName},{arg0.ToInt() + 1},{arg1})";
        }

        return $"SUBSTRING({columnName},{arg0.ToInt() + 1})";
    }

    #endregion
}
