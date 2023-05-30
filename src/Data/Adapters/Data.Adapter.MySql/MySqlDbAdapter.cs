using System;
using System.Data;
using System.Text;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Utils.Helpers;
using MySqlConnector;

namespace Mkh.Data.Adapter.MySql;

public class MySqlDbAdapter : DbAdapterAbstract
{
    public override DbProvider Provider => DbProvider.MySql;

    /// <summary>
    /// 左引号
    /// </summary>
    public override char LeftQuote => '`';

    /// <summary>
    /// 右引号
    /// </summary>
    public override char RightQuote => '`';

    /// <summary>
    /// 获取最后新增ID语句
    /// </summary>
    public override string IdentitySql => "SELECT LAST_INSERT_ID() ID;";

    public override IDbConnection NewConnection(string connectionString)
    {
        return new MySqlConnection(connectionString);
    }

    public override string GenerateFirstSql(string @select, string table, string @where, string sort, string groupBy = null,
        string having = null)
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
            sqlBuilder.AppendFormat(" LIMIT {0},{1}", skip, take);

        return sqlBuilder.ToString();
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

        if (propertyType.IsEnum)
        {
            if (!isNullable)
            {
                columnDescriptor.DefaultValue = "0";
            }

            columnDescriptor.TypeName = "SMALLINT";
            return;
        }

        if (propertyType.IsGuid())
        {
            columnDescriptor.TypeName = "CHAR";
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
                    columnDescriptor.DefaultValue = "b'0'";
                }

                columnDescriptor.TypeName = "BIT";
                break;
            case TypeCode.Byte:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "0";
                }
                columnDescriptor.TypeName = "TINYINT";
                break;
            case TypeCode.Int16:
            case TypeCode.Int32:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "0";
                }

                columnDescriptor.TypeName = "INT";
                break;
            case TypeCode.Int64:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "0";
                }

                columnDescriptor.TypeName = "BIGINT";
                break;
            case TypeCode.DateTime:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "CURRENT_TIMESTAMP";
                }
                columnDescriptor.TypeName = "DATETIME";
                break;
            case TypeCode.Decimal:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "0";
                }

                columnDescriptor.TypeName = "DECIMAL";
                break;
            case TypeCode.Double:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "0";
                }

                columnDescriptor.TypeName = "DOUBLE";
                break;
            case TypeCode.Single:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "0";
                }

                columnDescriptor.TypeName = "FLOAT";
                break;
        }

        //主键默认值为null
        if (columnDescriptor.IsPrimaryKey)
        {
            columnDescriptor.DefaultValue = null;
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
            .Replace("yy", "%y")
            .Replace("MM", "%m")
            .Replace("dd", "%d")
            .Replace("hh", "%k")
            .Replace("mm", "%i")
            .Replace("ss", "%s");

        return $"DATE_FORMAT({columnName},'{format}')";
    }

    #endregion
}