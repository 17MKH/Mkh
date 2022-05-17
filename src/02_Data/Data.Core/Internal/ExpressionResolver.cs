using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Extensions;
using Mkh.Data.Core.Internal.QueryStructure;

namespace Mkh.Data.Core.Internal;

/// <summary>
/// 表达式解析器
/// </summary>
public class ExpressionResolver
{
    public static string Resolve(QueryBody queryBody, LambdaExpression expression, IQueryParameters parameters)
    {
        if (expression == null)
            return string.Empty;

        var sqlBuilder = new StringBuilder();

        Resolve(queryBody, expression, expression, sqlBuilder, parameters);

        return sqlBuilder.ToString();
    }

    public static void Resolve(QueryBody queryBody, Expression expression, LambdaExpression fullLambda, StringBuilder sqlBuilder, IQueryParameters parameters)
    {
        switch (expression.NodeType)
        {
            case ExpressionType.Lambda:
                Resolve(queryBody, (expression as LambdaExpression)!.Body, fullLambda, sqlBuilder, parameters);
                break;
            case ExpressionType.Convert:
            case ExpressionType.ConvertChecked:
                Resolve(queryBody, (expression as UnaryExpression)!.Operand, fullLambda, sqlBuilder, parameters);
                break;
            case ExpressionType.Add:
            case ExpressionType.AddChecked:
            case ExpressionType.Subtract:
            case ExpressionType.SubtractChecked:
            case ExpressionType.Multiply:
            case ExpressionType.MultiplyChecked:
            case ExpressionType.Divide:
            case ExpressionType.Modulo:
            case ExpressionType.And:
            case ExpressionType.AndAlso:
            case ExpressionType.Or:
            case ExpressionType.OrElse:
            case ExpressionType.LessThan:
            case ExpressionType.LessThanOrEqual:
            case ExpressionType.GreaterThan:
            case ExpressionType.GreaterThanOrEqual:
            case ExpressionType.Equal:
            case ExpressionType.NotEqual:
            case ExpressionType.Coalesce:
            case ExpressionType.ArrayIndex:
            case ExpressionType.RightShift:
            case ExpressionType.LeftShift:
            case ExpressionType.ExclusiveOr:
                ResolveBinary(queryBody, expression as BinaryExpression, fullLambda, sqlBuilder, parameters);
                break;
            case ExpressionType.Constant:
                AppendValue(queryBody, (expression as ConstantExpression)!.Value, sqlBuilder, parameters);
                break;
            case ExpressionType.MemberAccess:
                ResolveMember(queryBody, expression as MemberExpression, fullLambda, sqlBuilder, parameters);
                break;
            case ExpressionType.Call:
                ResolveCall(queryBody, expression as MethodCallExpression, fullLambda, sqlBuilder, parameters);
                break;
            case ExpressionType.MemberInit:
                ResolveMemberInit(queryBody, expression, fullLambda, sqlBuilder, parameters);
                break;
        }
    }

    /// <summary>
    /// 附加值
    /// </summary>
    public static void AppendValue(QueryBody queryBody, object value, StringBuilder sqlBuilder, IQueryParameters parameters)
    {
        if (value == null)
        {
            var len = sqlBuilder.Length;
            if (sqlBuilder[len - 1] == ' ' && sqlBuilder[len - 2] == '>' && sqlBuilder[len - 3] == '<')
            {
                sqlBuilder.Remove(len - 3, 3);
                sqlBuilder.Append("IS NOT NULL");
                return;
            }

            if (sqlBuilder[len - 1] == ' ' && sqlBuilder[len - 2] == '=')
            {
                sqlBuilder.Remove(len - 2, 2);
                sqlBuilder.Append("IS NULL");
            }

            return;
        }

        var dbAdapter = queryBody.DbAdapter;
        if (queryBody.UseParameters)
        {
            //使用参数化
            var pName = parameters.Add(value);
            sqlBuilder.Append(dbAdapter.AppendParameter(pName));
        }
        else
        {
            var type = value.GetType();
            //不使用参数化
            if (type.IsNullable())
            {
                type = Nullable.GetUnderlyingType(type);
            }

            if (type!.IsEnum)
            {
                sqlBuilder.AppendFormat("{0}", value.ToInt());
            }
            else if (type.IsBool())
            {
                sqlBuilder.AppendFormat("{0}", value.ToBool() ? dbAdapter.BooleanTrueValue : dbAdapter.BooleanFalseValue);
            }
            else if (type.IsDateTime())
            {
                sqlBuilder.AppendFormat("'{0:yyyy-MM-dd HH:mm:ss}'", value);
            }
            else if (type.IsString() || type.IsGuid())
            {
                sqlBuilder.AppendFormat("'{0}'", value);
            }
            else
            {
                sqlBuilder.AppendFormat("{0}", value);
            }
        }
    }

    /// <summary>
    /// 获取表达式中的参数数组
    /// </summary>
    /// <param name="arguments"></param>
    /// <returns></returns>
    public static object[] Arguments2Object(ReadOnlyCollection<Expression> arguments)
    {
        object[] args = null;
        if (arguments.Any())
        {
            args = new object[arguments.Count];

            for (int i = 0; i < arguments.Count; i++)
            {
                args[i] = ((ConstantExpression)arguments[i]).Value;
            }
        }

        return args;
    }

    #region ==解析表==

    public static string ResolveFrom(QueryBody queryBody, IQueryParameters parameters)
    {
        var sqlBuilder = new StringBuilder();
        ResolveFrom(queryBody, sqlBuilder, parameters);
        return sqlBuilder.ToString();
    }

    public static void ResolveFrom(QueryBody queryBody, StringBuilder sqlBuilder, IQueryParameters parameters)
    {
        var dbAdapter = queryBody.DbAdapter;
        var first = queryBody.Joins.First();

        if (queryBody.Joins.Count < 2)
        {
            sqlBuilder.AppendFormat("{0}", dbAdapter.AppendQuote(first.TableName));

            //附加SqlServer的NOLOCK特性
            if (dbAdapter.Provider == DbProvider.SqlServer && first.NoLock)
            {
                sqlBuilder.Append(" WITH (NOLOCK)");
            }

            return;
        }

        sqlBuilder.AppendFormat("{0} AS {1}", dbAdapter.AppendQuote(first.TableName), first.Alias);
        //附加NOLOCK特性
        if (dbAdapter.Provider == DbProvider.SqlServer && first.NoLock)
        {
            sqlBuilder.Append(" WITH (NOLOCK)");
        }

        for (var i = 1; i < queryBody.Joins.Count; i++)
        {
            var join = queryBody.Joins[i];
            switch (join.Type)
            {
                case JoinType.Inner:
                    sqlBuilder.Append(" INNER");
                    break;
                case JoinType.Right:
                    sqlBuilder.Append(" RIGHT");
                    break;
                default:
                    sqlBuilder.Append(" LEFT");
                    break;
            }

            sqlBuilder.AppendFormat(" JOIN {0} AS {1}", dbAdapter.AppendQuote(join.TableName), join.Alias);
            //附加SqlServer的NOLOCK特性
            if (dbAdapter.Provider == DbProvider.SqlServer && first.NoLock)
            {
                sqlBuilder.Append(" WITH (NOLOCK)");
            }

            sqlBuilder.Append(" ON ");
            sqlBuilder.Append(Resolve(queryBody, join.On, parameters));

            if (join.Type == JoinType.Inner)
            {
                //过滤软删除
                if (queryBody.FilterDeleted && join.EntityDescriptor.IsSoftDelete)
                {
                    sqlBuilder.AppendFormat(" AND {0}.{1} = {2}", join.Alias, dbAdapter.AppendQuote(join.EntityDescriptor.GetDeletedColumnName()), dbAdapter.BooleanFalseValue);
                }

                //添加租户过滤
                if (queryBody.FilterTenant && join.EntityDescriptor.IsTenant)
                {
                    var x1 = dbAdapter.AppendQuote(DbConstants.TENANT_COLUMN_NAME);
                    var tenantId = queryBody.Repository.DbContext.AccountResolver.TenantId;
                    if (tenantId == null)
                    {
                        sqlBuilder.AppendFormat(" AND {0}.{1} IS NULL", join.Alias, x1);
                    }
                    else
                    {
                        sqlBuilder.AppendFormat(" AND {0}.{1} = '{2}'", join.Alias, x1, tenantId);
                    }
                }
            }
        }
    }

    #endregion

    #region ==解析过滤条件==

    public static string ResolveWhere(QueryBody queryBody, IQueryParameters parameters)
    {
        var sqlBuilder = new StringBuilder();
        ResolveWhere(queryBody, sqlBuilder, parameters);
        return sqlBuilder.ToString();
    }

    public static void ResolveWhere(QueryBody queryBody, StringBuilder sqlBuilder, IQueryParameters parameters)
    {
        sqlBuilder.Append("WHERE ");
        //记录下当前sqlBuilder的长度，用于解析完成后比对
        var length = sqlBuilder.Length;

        //解析where条件
        if (queryBody.Wheres.NotNullAndEmpty())
        {
            foreach (var w in queryBody.Wheres)
            {
                switch (w.Mode)
                {
                    case QueryWhereMode.Lambda:
                        Resolve(queryBody, w.Lambda.Body, w.Lambda, sqlBuilder, parameters);
                        break;
                    case QueryWhereMode.SubQuery:
                        Resolve(queryBody, w.SubQueryColumn.Body, w.SubQueryColumn, sqlBuilder, parameters);
                        var subSql = w.SubQueryable.ToListSql(parameters);
                        sqlBuilder.AppendFormat("{0} ({1})", w.SubQueryOperator, subSql);
                        break;
                    case QueryWhereMode.Sql:
                        sqlBuilder.AppendFormat("({0})", w.Sql);
                        break;
                }

                //通过比对长度判断是否有附加有效条件
                if (length != sqlBuilder.Length)
                    sqlBuilder.Append(" AND ");
            }
        }

        //解析软删除
        ResolveWhereForSoftDelete(queryBody, sqlBuilder);

        //解析租户
        ResolveWhereForTenant(queryBody, sqlBuilder);

        /*
         * 1、当没有过滤条件时，需要移除WHERE关键字，此时sqlBuilder是以"WHERE "结尾，只需删除最后面的6位即可
         * 2、当有过滤条件时，需要移除最后面的AND关键字，此时sqlBuilder是以" AND "结尾，也是只需删除最后面的5位即可
         */
        var removeLength = length == sqlBuilder.Length ? 6 : 5;
        sqlBuilder.Remove(sqlBuilder.Length - removeLength, removeLength);
    }

    /// <summary>
    /// 解析软删除过滤条件
    /// </summary>
    private static void ResolveWhereForSoftDelete(QueryBody queryBody, StringBuilder sqlBuilder)
    {
        //未开启软删除过滤
        if (!queryBody.FilterDeleted)
            return;

        var dbAdapter = queryBody.DbAdapter;

        //单表
        if (queryBody.Joins.Count < 2)
        {
            var first = queryBody.Joins.First();
            if (!first.EntityDescriptor.IsSoftDelete)
                return;

            sqlBuilder.AppendFormat("{0} = {1} AND ", dbAdapter.AppendQuote(first.EntityDescriptor.GetDeletedColumnName()), dbAdapter.BooleanFalseValue);

            return;
        }

        //多表
        foreach (var join in queryBody.Joins)
        {
            if (!join.EntityDescriptor.IsSoftDelete || join.Type == JoinType.Inner)
                return;

            sqlBuilder.AppendFormat("{0}.{1} = {2} AND ", join.Alias, dbAdapter.AppendQuote(join.EntityDescriptor.GetDeletedColumnName()), dbAdapter.BooleanFalseValue);
        }
    }

    /// <summary>
    /// 解析租户过滤条件
    /// </summary>
    private static void ResolveWhereForTenant(QueryBody queryBody, StringBuilder sqlBuilder)
    {
        //未开启过滤租户
        if (!queryBody.FilterTenant)
            return;

        var dbAdapter = queryBody.DbAdapter;
        var tenantId = queryBody.Repository.DbContext.AccountResolver.TenantId;
        //单表
        if (queryBody.Joins.Count < 2)
        {
            var first = queryBody.Joins.First();
            if (!first.EntityDescriptor.IsTenant)
                return;

            //单表
            var x0 = dbAdapter.AppendQuote(DbConstants.TENANT_COLUMN_NAME);
            if (tenantId == null)
            {
                sqlBuilder.AppendFormat("{0} IS NULL AND ", x0);
            }
            else
            {
                sqlBuilder.AppendFormat("{0} = '{1}' AND ", x0, tenantId);
            }

            return;
        }

        //多表
        foreach (var join in queryBody.Joins)
        {
            if (!join.EntityDescriptor.IsTenant || join.Type == JoinType.Inner)
                return;

            //多表时附加别名
            var x0 = join.Alias;
            var x1 = dbAdapter.AppendQuote(DbConstants.TENANT_COLUMN_NAME);
            if (tenantId == null)
            {
                sqlBuilder.AppendFormat("{0}.{1} IS NULL AND ", x0, x1);
            }
            else
            {
                sqlBuilder.AppendFormat("{0}.{1} = '{2}' AND", x0, x1, tenantId);
            }
        }
    }

    #endregion

    #region ==Private==

    /// <summary>
    /// 解析二元运算符表达式
    /// </summary>
    private static void ResolveBinary(QueryBody queryBody, BinaryExpression exp, LambdaExpression fullLambda, StringBuilder sqlBuilder, IQueryParameters parameters)
    {
        //针对简写方式的布尔类型解析m => m.Deleted
        if (exp.Left.NodeType == ExpressionType.MemberAccess && exp.Left.Type == typeof(bool) && exp.NodeType != ExpressionType.Equal && exp.NodeType != ExpressionType.NotEqual)
        {
            ResolveMember(queryBody, exp.Left as MemberExpression, fullLambda, sqlBuilder, parameters);
            sqlBuilder.Append(" = ");
            AppendValue(queryBody, queryBody.DbAdapter.BooleanTrueValue, sqlBuilder, parameters);
        }
        //针对简写方式的布尔类型解析m => !m.Deleted
        else if (exp.Left.NodeType == ExpressionType.Not)
        {
            ResolveMember(queryBody, (exp.Left as UnaryExpression)!.Operand as MemberExpression, fullLambda, sqlBuilder, parameters);
            sqlBuilder.Append(" = ");
            AppendValue(queryBody, queryBody.DbAdapter.BooleanFalseValue, sqlBuilder, parameters);
        }
        else
        {
            Resolve(queryBody, exp.Left, fullLambda, sqlBuilder, parameters);
        }

        switch (exp.NodeType)
        {
            case ExpressionType.And:
            case ExpressionType.AndAlso:
                sqlBuilder.Append(" AND ");
                break;
            case ExpressionType.GreaterThan:
                sqlBuilder.Append(" > ");
                break;
            case ExpressionType.GreaterThanOrEqual:
                sqlBuilder.Append(" >= ");
                break;
            case ExpressionType.LessThan:
                sqlBuilder.Append(" < ");
                break;
            case ExpressionType.LessThanOrEqual:
                sqlBuilder.Append(" <= ");
                break;
            case ExpressionType.Equal:
                sqlBuilder.Append(" = ");
                break;
            case ExpressionType.OrElse:
            case ExpressionType.Or:
                sqlBuilder.Append(" OR ");
                break;
            case ExpressionType.NotEqual:
                sqlBuilder.Append(" <> ");
                break;
            case ExpressionType.Add:
                sqlBuilder.Append(" + ");
                break;
            case ExpressionType.Subtract:
                sqlBuilder.Append(" - ");
                break;
            case ExpressionType.Multiply:
                sqlBuilder.Append(" * ");
                break;
            case ExpressionType.Divide:
                sqlBuilder.Append(" / ");
                break;
        }

        Resolve(queryBody, exp.Right, fullLambda, sqlBuilder, parameters);
    }

    /// <summary>
    /// 解析成员表达式
    /// </summary>
    private static void ResolveMember(QueryBody queryBody, MemberExpression exp, LambdaExpression fullLambda, StringBuilder sqlBuilder, IQueryParameters parameters)
    {
        if (exp.Expression != null)
        {
            switch (exp.Expression.NodeType)
            {
                case ExpressionType.Parameter:
                    sqlBuilder.Append(queryBody.GetColumnName(exp));
                    break;
                case ExpressionType.Constant:
                    var val = ResolveDynamicInvoke(exp);
                    AppendValue(queryBody, val, sqlBuilder, parameters);
                    break;
                case ExpressionType.MemberAccess:
                    if (IsParameter(exp))
                    {
                        var columnName = queryBody.GetColumnName(exp);
                        sqlBuilder.Append(columnName);
                    }
                    else
                    {
                        val = ResolveDynamicInvoke(exp);
                        AppendValue(queryBody, val, sqlBuilder, parameters);
                    }
                    break;
            }

            //针对简写方式的布尔类型解析 m => m.Deleted
            if (exp == fullLambda.Body && exp.NodeType == ExpressionType.MemberAccess && exp.Type == typeof(bool))
            {
                sqlBuilder.Append(" = ");
                AppendValue(queryBody, queryBody.DbAdapter.BooleanTrueValue, sqlBuilder, parameters);
            }
        }
    }

    /// <summary>
    /// 解析方法调用表达式
    /// </summary>
    private static void ResolveCall(QueryBody queryBody, MethodCallExpression exp, LambdaExpression fullLambda, StringBuilder sqlBuilder, IQueryParameters parameters)
    {
        string columnName;
        switch (exp.Method.Name)
        {
            case "Contains":
                ResolveMethodForContains(queryBody, exp, sqlBuilder, parameters);
                break;
            case "NotContains":
                ResolveMethodForNotContains(queryBody, exp, sqlBuilder);
                break;
            case "StartsWith":
                ResolveMethodForStartsWith(queryBody, exp, sqlBuilder, parameters);
                break;
            case "EndsWith":
                ResolveMethodForEndsWith(queryBody, exp, sqlBuilder, parameters);
                break;
            case "Equals":
                ResolveMethodForEquals(queryBody, exp, fullLambda, sqlBuilder, parameters);
                break;
            case "Sum":
            case "Avg":
            case "Max":
            case "Min":
                var fullExp = (exp.Arguments[0] as UnaryExpression)!.Operand as LambdaExpression;
                columnName = queryBody.GetColumnName(fullExp!.Body);
                sqlBuilder.AppendFormat(" {0}", queryBody.DbAdapter.FunctionMapper(exp.Method.Name, columnName));
                break;
            default:
                if (exp.Object != null)
                {
                    if (exp.Object is MemberExpression memberExpression && IsParameter(memberExpression))
                    {
                        columnName = queryBody.GetColumnName(exp.Object);
                        var args = Arguments2Object(exp.Arguments);
                        sqlBuilder.AppendFormat("{0}", queryBody.DbAdapter.FunctionMapper(exp.Method.Name, columnName, exp.Object.Type, args));
                    }
                    else
                    {
                        var val = ResolveDynamicInvoke(exp);
                        AppendValue(queryBody, val, sqlBuilder, parameters);
                    }
                }

                break;
        }
    }

    /// <summary>
    /// 解析成员初始化表达式
    /// </summary>
    private static void ResolveMemberInit(QueryBody queryBody, Expression exp, LambdaExpression fullLambda, StringBuilder sqlBuilder, IQueryParameters parameters)
    {
        if (exp == null || !(exp is MemberInitExpression initExp) || !initExp.Bindings.Any())
            return;

        for (var i = 0; i < initExp.Bindings.Count; i++)
        {
            if (initExp.Bindings[i] is MemberAssignment assignment)
            {
                var descriptor = queryBody.Joins.First(m => m.EntityDescriptor.EntityType == initExp.Type);
                var col = descriptor.EntityDescriptor.Columns.FirstOrDefault(m => m.PropertyInfo.Name.Equals(assignment.Member.Name));
                if (col != null)
                {
                    if (queryBody.Joins.Count < 2)
                        sqlBuilder.Append(queryBody.DbAdapter.AppendQuote(col.Name));
                    else
                        sqlBuilder.Append($"{queryBody.DbAdapter.AppendQuote(descriptor.Alias)}.{queryBody.DbAdapter.AppendQuote(col.Name)}");

                    sqlBuilder.Append(" = ");

                    Resolve(queryBody, assignment.Expression, fullLambda, sqlBuilder, parameters);

                    if (i < initExp.Bindings.Count - 1)
                        sqlBuilder.Append(",");
                }
            }
        }
    }

    /// <summary>
    /// 解析Contains方法
    /// </summary>
    private static void ResolveMethodForContains(QueryBody queryBody, MethodCallExpression exp, StringBuilder sqlBuilder, IQueryParameters parameters)
    {
        if (exp.Object is MemberExpression objExp)
        {
            #region ==字符串类型的Contains方法==

            if (objExp.Type.IsString())
            {
                sqlBuilder.Append(queryBody.GetColumnName(objExp));

                string value;
                if (exp.Arguments[0] is ConstantExpression c)
                {
                    value = c.Value!.ToString();
                }
                else
                {
                    value = ResolveDynamicInvoke(exp.Arguments[0]).ToString();
                }

                sqlBuilder.Append(" LIKE ");

                AppendValue(queryBody, $"%{value}%", sqlBuilder, parameters);
                return;
            }

            #endregion

            if (objExp.Type.IsGenericType)
            {
                var columnName = queryBody.GetColumnName(exp.Arguments[0]);
                ResolveInForGeneric(sqlBuilder, columnName, objExp, (exp.Arguments[0] as MemberExpression)!.Type);
            }

            return;
        }

        #region ==数组、泛型类型的Contains方法==

        if (exp.Arguments[1] is MemberExpression argExp1)
        {
            var columnName = queryBody.GetColumnName(argExp1);
            ResolveInForGeneric(sqlBuilder, columnName, exp.Arguments[0], argExp1.Type);
        }

        #endregion
    }

    /// <summary>
    /// 解析NotContains方法
    /// </summary>
    private static void ResolveMethodForNotContains(QueryBody queryBody, MethodCallExpression exp, StringBuilder sqlBuilder)
    {
        var argExp = exp.Arguments[1] as MemberExpression;
        var columnName = queryBody.GetColumnName(argExp);

        ResolveInForGeneric(sqlBuilder, columnName, exp.Arguments[0], argExp!.Type, true);
    }

    private static void ResolveInForGeneric(StringBuilder sqlBuilder, string columnName, Expression exp, Type valueType, bool notContainer = false)
    {
        var value = ResolveDynamicInvoke(exp);
        var isValueType = false;
        var list = new List<string>();
        if (valueType.IsEnum)
        {
            isValueType = true;
            var valueList = (IEnumerable)value;
            if (valueList != null)
            {
                foreach (var c in valueList)
                {
                    list.Add(Enum.Parse(valueType, c.ToString()!).ToInt().ToString());
                }
            }
        }
        else
        {
            var typeName = valueType.Name;
            switch (typeName)
            {
                case "Guid":
                    if (value is IEnumerable<Guid> guidValues)
                    {
                        foreach (var c in guidValues)
                        {
                            list.Add(c.ToString());
                        }
                    }
                    break;
                case "DateTime":
                    if (value is IEnumerable<DateTime> dateTimeValues)
                    {
                        foreach (var c in dateTimeValues)
                        {
                            list.Add(c.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }
                    break;
                case "Byte":
                    isValueType = true;
                    if (value is IEnumerable<byte> byteValues)
                    {
                        foreach (var c in byteValues)
                        {
                            list.Add(c.ToString(CultureInfo.InvariantCulture));
                        }
                    }
                    break;
                case "Char":
                    if (value is IEnumerable<char> charValues)
                    {
                        foreach (var c in charValues)
                        {
                            list.Add(c.ToString());
                        }
                    }
                    break;
                case "Int16":
                    isValueType = true;
                    if (value is IEnumerable<short> shortValues)
                    {
                        foreach (var c in shortValues)
                        {
                            list.Add(c.ToString());
                        }
                    }
                    break;
                case "Int32":
                    isValueType = true;
                    if (value is IEnumerable<int> intValues)
                    {
                        foreach (var c in intValues)
                        {
                            list.Add(c.ToString());
                        }
                    }
                    break;
                case "Int64":
                    isValueType = true;
                    if (value is IEnumerable<long> longValues)
                    {
                        foreach (var c in longValues)
                        {
                            list.Add(c.ToString());
                        }
                    }
                    break;
                case "Double":
                    isValueType = true;
                    if (value is IEnumerable<double> doubleValues)
                    {
                        foreach (var c in doubleValues)
                        {
                            list.Add(c.ToString(CultureInfo.InvariantCulture));
                        }
                    }
                    break;
                case "Single":
                    isValueType = true;
                    if (value is IEnumerable<float> floatValues)
                    {
                        foreach (var c in floatValues)
                        {
                            list.Add(c.ToString(CultureInfo.InvariantCulture));
                        }
                    }
                    break;
                case "Decimal":
                    isValueType = true;
                    if (value is IEnumerable<decimal> decimalValues)
                    {
                        foreach (var c in decimalValues)
                        {
                            list.Add(c.ToString(CultureInfo.InvariantCulture));
                        }
                    }
                    break;
            }
        }

        if (list!.Count < 1)
            return;

        sqlBuilder.Append(columnName);
        sqlBuilder.Append(notContainer ? " NOT IN (" : " IN (");

        //值类型不带引号
        if (isValueType)
        {
            for (var i = 0; i < list.Count; i++)
            {
                sqlBuilder.AppendFormat("{0}", list[i]);
                if (i != list.Count - 1)
                {
                    sqlBuilder.Append(",");
                }
            }
        }
        else
        {
            for (var i = 0; i < list.Count; i++)
            {
                sqlBuilder.AppendFormat("'{0}'", list[i].Replace("'", "''"));
                if (i != list.Count - 1)
                {
                    sqlBuilder.Append(",");
                }
            }
        }

        sqlBuilder.Append(")");
    }

    /// <summary>
    /// 解析StartsWith方法
    /// </summary>
    private static void ResolveMethodForStartsWith(QueryBody queryBody, MethodCallExpression exp, StringBuilder sqlBuilder, IQueryParameters parameters)
    {
        if (exp.Object is MemberExpression objExp)
        {
            sqlBuilder.Append(queryBody.GetColumnName(objExp));

            string value;
            if (exp.Arguments[0] is ConstantExpression c)
            {
                value = c.Value!.ToString();
            }
            else
            {
                value = ResolveDynamicInvoke(exp.Arguments[0]).ToString();
            }

            sqlBuilder.Append(" LIKE ");

            AppendValue(queryBody, $"{value}%", sqlBuilder, parameters);
        }
    }

    /// <summary>
    /// 解析EndsWith方法
    /// </summary>
    private static void ResolveMethodForEndsWith(QueryBody queryBody, MethodCallExpression exp, StringBuilder sqlBuilder, IQueryParameters parameters)
    {
        if (exp.Object is MemberExpression objExp)
        {
            sqlBuilder.Append(queryBody.GetColumnName(objExp));

            string value;
            if (exp.Arguments[0] is ConstantExpression c)
            {
                value = c.Value!.ToString();
            }
            else
            {
                value = ResolveDynamicInvoke(exp.Arguments[0]).ToString();
            }

            sqlBuilder.Append(" LIKE ");

            AppendValue(queryBody, $"%{value}", sqlBuilder, parameters);
        }
    }

    /// <summary>
    /// 解析Equals方法
    /// </summary>
    private static void ResolveMethodForEquals(QueryBody queryBody, MethodCallExpression exp, LambdaExpression fullLambda, StringBuilder sqlBuilder, IQueryParameters parameters)
    {
        if (exp.Object is MemberExpression objExp)
        {
            sqlBuilder.Append(queryBody.GetColumnName(objExp));

            sqlBuilder.Append(" = ");

            var arg = exp.Arguments[0];
            switch (arg.NodeType)
            {
                case ExpressionType.Constant:
                    AppendValue(queryBody, (arg as ConstantExpression)!.Value!.ToString(), sqlBuilder, parameters);
                    break;
                case ExpressionType.MemberAccess:
                    ResolveMember(queryBody, arg as MemberExpression, fullLambda, sqlBuilder, parameters);
                    break;
                case ExpressionType.Convert:
                    Resolve(queryBody, (arg as UnaryExpression)!.Operand, fullLambda, sqlBuilder, parameters);
                    break;
                default:
                    AppendValue(queryBody, ResolveDynamicInvoke(arg).ToString(), sqlBuilder, parameters);
                    break;
            }
        }
    }

    /// <summary>
    /// 解析动态代码
    /// </summary>
    /// <param name="exp"></param>
    private static object ResolveDynamicInvoke(Expression exp)
    {
        var value = Expression.Lambda(exp).Compile().DynamicInvoke();
        if (exp.Type.IsEnum)
            value = value.ToInt();

        return value;
    }

    private static bool IsParameter(MemberExpression exp)
    {
        if (exp.Expression is MemberExpression memberExpression)
        {
            return IsParameter(memberExpression);
        }

        return exp.Expression is ParameterExpression;
    }

    #endregion
}