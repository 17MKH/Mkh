using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Mkh.Data.Abstractions.Queryable;

namespace Mkh.Data.Core.Internal.QueryStructure;

/// <summary>
/// 参数集合
/// </summary>
public class QueryParameters : IQueryParameters
{
    private static readonly Dictionary<Type, DbType> TypeMap = new Dictionary<Type, DbType>
    {
        {typeof(byte), DbType.Byte},
        {typeof(sbyte),DbType.SByte},
        {typeof(short),DbType.Int16},
        {typeof(ushort),DbType.UInt16},
        {typeof(int),DbType.Int32},
        {typeof(uint),DbType.UInt32},
        {typeof(long),DbType.Int64},
        {typeof(ulong),DbType.UInt64},
        {typeof(float),DbType.Single},
        {typeof(double),DbType.Double},
        {typeof(decimal),DbType.Decimal},
        {typeof(bool),DbType.Boolean},
        {typeof(string),DbType.String},
        {typeof(char),DbType.StringFixedLength},
        {typeof(Guid),DbType.Guid},
        {typeof(DateTime),DbType.DateTime},
        {typeof(DateTimeOffset),DbType.DateTimeOffset},
        {typeof(byte[]),DbType.Binary},
        {typeof(byte?),DbType.Byte},
        {typeof(sbyte?),DbType.SByte},
        {typeof(short?),DbType.Int16},
        {typeof(ushort?),DbType.UInt16},
        {typeof(int?),DbType.Int32},
        {typeof(uint?),DbType.UInt32},
        {typeof(long?),DbType.Int64},
        {typeof(ulong?),DbType.UInt64},
        {typeof(float?),DbType.Single},
        {typeof(double?),DbType.Double},
        {typeof(decimal?),DbType.Decimal},
        {typeof(bool?),DbType.Boolean},
        {typeof(char?),DbType.StringFixedLength},
        {typeof(Guid?),DbType.Guid},
        {typeof(DateTime?),DbType.DateTime},
        {typeof(DateTimeOffset?),DbType.DateTimeOffset}
    };

    private readonly List<KeyValuePair<string, object>> _parameters;

    public int Count => _parameters.Count;

    public QueryParameters()
    {
        _parameters = new List<KeyValuePair<string, object>>();
    }

    public string Add(object value)
    {
        var paramName = "P" + (_parameters.Count + 1);
        _parameters.Add(new KeyValuePair<string, object>(paramName, value));
        return paramName;
    }

    public DynamicParameters ToDynamicParameters()
    {
        var dynParams = new DynamicParameters();

        _parameters.ForEach(m =>
        {
            if (m.Value == null)
            {
                dynParams.Add(m.Key, null, DbType.String);
            }
            else
            {
                var t = m.Value.GetType();
                if (t.IsEnum)
                {
                    dynParams.Add(m.Key, m.Value, DbType.Int32);
                }
                else
                {
                    var dbType = TypeMap[m.Value.GetType()];
                    dynParams.Add(m.Key, m.Value, dbType);
                }
            }
        });

        return dynParams;
    }

    public void Clear()
    {
        _parameters.Clear(); 
    }

    public KeyValuePair<string, object> this[int index] => _parameters[index];
}