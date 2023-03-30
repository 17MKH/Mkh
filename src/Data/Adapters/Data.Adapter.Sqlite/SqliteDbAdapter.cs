using System;
using System.Data;
using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Utils.Helpers;

namespace Mkh.Data.Adapter.Sqlite;

public class SqliteDbAdapter : DbAdapterAbstract
{
    public SqliteDbAdapter()
    {
        SqlMapper.AddTypeHandler(new GuidTypeHandler());
    }

    public override DbProvider Provider => DbProvider.Sqlite;

    /// <summary>
    /// 左引号
    /// </summary>
    public override char LeftQuote => '[';

    /// <summary>
    /// 右引号
    /// </summary>
    public override char RightQuote => ']';

    /// <summary>
    /// 获取最后新增ID语句
    /// </summary>
    public override string IdentitySql => "SELECT LAST_INSERT_ROWID() ID;";

    public override IDbConnection NewConnection(string connectionString)
    {
        return new SqliteConnection(connectionString);
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

        sqlBuilder.AppendFormat(" LIMIT {0} OFFSET {1};", take, skip);

        return sqlBuilder.ToString();
    }

    public override void ResolveColumn(IColumnDescriptor descriptor)
    {
        var propertyType = descriptor.PropertyInfo.PropertyType;
        var isNullable = propertyType.IsNullable();
        if (isNullable)
        {
            propertyType = Nullable.GetUnderlyingType(propertyType);
            if (propertyType == null)
                throw new Exception("Property2Column error");
        }

        if (propertyType.IsEnum)
        {
            if (!isNullable)
            {
                descriptor.DefaultValue = "0";
            }

            descriptor.TypeName = "integer";
            return;
        }

        if (propertyType.IsGuid())
        {
            descriptor.TypeName = "UNIQUEIDENTIFIER";

            return;
        }

        var typeCode = Type.GetTypeCode(propertyType);
        switch (typeCode)
        {
            case TypeCode.Boolean:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
                descriptor.TypeName = "integer";
                break;
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                descriptor.TypeName = "decimal";
                break;
            default:
                descriptor.TypeName = "text";
                break;
        }
    }

    #region ==函数映射==

    public override string FunctionMapper(string sourceName, string columnName, Type dataType = null, object[] args = null)
    {
        switch (sourceName)
        {
            case "Substring":
                return Mapper_Substring(columnName, args[0], args.Length > 1 ? args[1] : null);
            case "ToString":
                if (dataType.IsDateTime() && args[0] != null)
                {
                    return Mapper_DatetimeToString(columnName, args[0]);
                }
                return string.Empty;
            case "Replace":
                return $"REPLACE({columnName},'{args[0]}','{args[1]}')";
            case "ToLower":
                return $"LOWER({columnName})";
            case "ToUpper":
                return $"UPPER({columnName})";
            case "Length":
                return $"LENGTH({columnName})";
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

    public override Guid CreateSequentialGuid()
    {
        return GuidGenerator.Create(SequentialGuidType.SequentialAsString);
    }

    private string Mapper_Substring(string columnName, object arg0, object arg1)
    {
        if (arg1 != null)
        {
            return $"SUBSTR({columnName},{arg0.ToInt() + 1},{arg1})";
        }

        return $"SUBSTR({columnName},{arg0.ToInt() + 1})";
    }

    private string Mapper_DatetimeToString(string columnName, object arg0)
    {
        var format = arg0.ToString();
        format = format!.Replace("yyyy", "%Y")
            .Replace("yy", "%Y")
            .Replace("MM", "%m")
            .Replace("dd", "%d")
            .Replace("hh", "%H")
            .Replace("mm", "%M")
            .Replace("ss", "%S");

        return $"STRFTIME('{format}',{columnName})";
    }

    #endregion
}