using System;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Utils.Helpers;

namespace Mkh.Data.Adapter.SqlServer;

public class SqlServerDbAdapter : DbAdapterAbstract
{
    public bool IsHighVersion
    {
        get
        {
            var version = Options.Version.ToInt();
            return version == 0 || version >= 2012;
        }
    }

    public override DbProvider Provider => DbProvider.SqlServer;

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
    public override string IdentitySql => "SELECT SCOPE_IDENTITY() ID;";

    public override IDbConnection NewConnection(string connectionString)
    {
        return new SqlConnection(connectionString);
    }

    public override string GenerateFirstSql(string @select, string table, string @where, string sort, string groupBy = null,
        string having = null)
    {
        return GeneratePagingSql(select, table, where, sort, 0, 1, groupBy, having);
    }

    public override string GeneratePagingSql(string select, string table, string where, string sort, int skip, int take, string groupBy = null, string having = null)
    {
        if (sort.IsNull())
        {
            if (groupBy.IsNull())
                sort = " ORDER BY [Id] ASC";
            else
            {
                throw new ArgumentException("SqlServer分组分页查询需要指定排序规则");
            }
        }

        var sqlBuilder = new StringBuilder();
        if (IsHighVersion)
        {
            #region ==2012+版本==

            sqlBuilder.AppendFormat("SELECT {0} FROM {1}", select, table);

            if (where.NotNull())
                sqlBuilder.AppendFormat(" {0}", where);

            if (groupBy.NotNull())
                sqlBuilder.Append(groupBy);

            if (having.NotNull())
                sqlBuilder.Append(having);

            sqlBuilder.AppendFormat("{0} OFFSET {1} ROW FETCH NEXT {2} ROW ONLY", sort, skip, take);

            #endregion
        }
        else
        {
            #region ==2012以下版本==

            sqlBuilder.AppendFormat("SELECT * FROM (SELECT ROW_NUMBER() OVER({0}) AS RowNum,{1} FROM {2}", sort, select, table);
            if (!string.IsNullOrWhiteSpace(where))
                sqlBuilder.AppendFormat(" WHERE {0}", where);

            sqlBuilder.AppendFormat(") AS T WHERE T.RowNum BETWEEN {0} AND {1}", skip + 1, skip + take);

            #endregion
        }

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
            columnDescriptor.TypeName = "UNIQUEIDENTIFIER";

            return;
        }

        var typeCode = Type.GetTypeCode(propertyType);
        switch (typeCode)
        {
            case TypeCode.Char:
            case TypeCode.String:
                columnDescriptor.TypeName = "NVARCHAR";
                break;
            case TypeCode.Boolean:
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "0";
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
                    columnDescriptor.DefaultValue = "GETDATE()";
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
                return $"LEN({columnName})";
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
        return GuidGenerator.Create(SequentialGuidType.SequentialAtEnd);
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
        if (IsHighVersion)
        {
            return $"FORMAT({columnName},'{arg0}')";
        }

        throw new ArgumentException("2012以下版本SqlServer不支持日期格式化功能");
    }

    #endregion
}