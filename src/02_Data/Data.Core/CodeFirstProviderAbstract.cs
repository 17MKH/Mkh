using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions;
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
}