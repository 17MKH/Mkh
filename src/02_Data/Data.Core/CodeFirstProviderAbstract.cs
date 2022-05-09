using System;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Options;

namespace Mkh.Data.Core;

public abstract class CodeFirstProviderAbstract : ICodeFirstProvider
{
    protected readonly CodeFirstOptions Options;
    protected readonly IDbContext Context;
    protected readonly IServiceCollection Service;

    protected CodeFirstProviderAbstract(CodeFirstOptions options, IDbContext context, IServiceCollection service)
    {
        Options = options;
        Context = context;
        Service = service;
    }

    public abstract void CreateDatabase();

    public abstract void CreateTable();

    public abstract void CreateNextTable();

    /// <summary>
    /// 解析分表的下一个表名
    /// </summary>
    /// <returns></returns>
    protected string ResolveTableName(IEntityDescriptor descriptor, bool next = false)
    {
        var tableName = descriptor.TableName;
        if (descriptor.IsSharding)
        {
            tableName = descriptor.ShardingPolicyProvider.ResolveTableName(descriptor, DateTime.Now, next);
        }

        return tableName;
    }
}