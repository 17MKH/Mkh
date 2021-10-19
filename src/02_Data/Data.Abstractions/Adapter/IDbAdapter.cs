using System;
using System.ComponentModel;
using System.Data;
using System.Text;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Options;

namespace Mkh.Data.Abstractions.Adapter;

/// <summary>
/// 数据库适配器
/// </summary>
public interface IDbAdapter
{
    #region ==属性==

    /// <summary>
    /// 数据库提供器
    /// </summary>
    DbProvider Provider { get; }

    /// <summary>
    /// 数据库配置项
    /// </summary>
    DbOptions Options { get; }

    /// <summary>
    /// 左引号
    /// </summary>
    char LeftQuote { get; }

    /// <summary>
    /// 右引号
    /// </summary>
    char RightQuote { get; }

    /// <summary>
    /// 参数前缀符号
    /// </summary>
    char ParameterPrefix { get; }

    /// <summary>
    /// 获取新增ID的SQL语句
    /// </summary>
    string IdentitySql { get; }

    /// <summary>
    /// SQL语句小写
    /// </summary>
    bool SqlLowerCase { get; }

    /// <summary>
    /// 布尔类型True对应的值
    /// </summary>
    string BooleanTrueValue { get; }

    /// <summary>
    /// 布尔类型Flase对应的值
    /// </summary>
    string BooleanFalseValue { get; }

    #endregion

    #region ==方法==

    /// <summary>
    /// 给定的值附加引号
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    string AppendQuote(string value);

    /// <summary>
    /// 给定的值附加引号
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    void AppendQuote(StringBuilder sb, string value);

    /// <summary>
    /// 附加参数
    /// </summary>
    /// <param name="parameterName">参数名</param>
    /// <returns></returns>
    string AppendParameter(string parameterName);

    /// <summary>
    /// 附加参数
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="parameterName">参数名</param>
    /// <returns></returns>
    void AppendParameter(StringBuilder sb, string parameterName);

    /// <summary>
    /// 创建新数据库连接
    /// </summary>
    /// <param name="connectionString">连接字符串</param>
    /// <returns></returns>
    IDbConnection NewConnection(string connectionString);

    /// <summary>
    /// 分页
    /// </summary>
    string GeneratePagingSql(string select, string table, string where, string sort, int skip, int take, string groupBy = null, string having = null);

    /// <summary>
    /// 生成获取第一条数据的Sql
    /// </summary>
    string GenerateFirstSql(string select, string table, string where, string sort, string groupBy = null, string having = null);

    /// <summary>
    /// 解析列
    /// </summary>
    /// <param name="columnDescriptor"></param>
    void ResolveColumn(IColumnDescriptor columnDescriptor);

    /// <summary>
    /// 函数映射
    /// </summary>
    /// <param name="sourceName">源名称</param>
    /// <param name="columnName">列名称</param>
    /// <param name="dataType">数据类型</param>
    /// <param name="args">参数数组</param>
    /// <returns></returns>
    string FunctionMapper(string sourceName, string columnName, Type dataType = null, object[] args = null);

    /// <summary>
    /// 创建有序Guid
    /// </summary>
    /// <returns></returns>
    Guid CreateSequentialGuid();

    #endregion
}

/// <summary>
/// 数据库提供器
/// </summary>
public enum DbProvider
{
    /// <summary>
    /// SqlServer
    /// </summary>
    [Description("SqlServer")]
    SqlServer,
    /// <summary>
    /// MySql
    /// </summary>
    [Description("MySql")]
    MySql,
    /// <summary>
    /// Sqlite
    /// </summary>
    [Description("Sqlite")]
    Sqlite,
    /// <summary>
    /// PostgreSQL
    /// </summary>
    [Description("PostgreSQL")]
    PostgreSQL,
    /// <summary>
    /// Oracle
    /// </summary>
    [Description("Oracle")]
    Oracle
}