using System.Collections.Generic;
using System.Data;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Logger;
using Mkh.Data.Abstractions.Options;
using Mkh.Data.Abstractions.Schema;

namespace Mkh.Data.Core;

public abstract class DbContext : IDbContext
{
    #region ==属性==

    public DbOptions Options { get; internal set; }

    public DbLogger Logger { get; internal set; }

    public IDbAdapter Adapter { get; internal set; }

    public ISchemaProvider SchemaProvider { get; internal set; }

    public ICodeFirstProvider CodeFirstProvider { get; internal set; }

    public IOperatorResolver AccountResolver { get; internal set; }

    public IList<IEntityDescriptor> EntityDescriptors { get; } = new List<IEntityDescriptor>();

    public IList<IRepositoryDescriptor> RepositoryDescriptors { get; } = new List<IRepositoryDescriptor>();

    #endregion

    #region ==方法==

    public IDbConnection NewConnection()
    {
        return Adapter.NewConnection(Options.ConnectionString);
    }

    public IDbConnection NewConnection(string connectionString)
    {
        return Adapter.NewConnection(connectionString);
    }

    public IUnitOfWork NewUnitOfWork(IsolationLevel? isolationLevel = null)
    {
        return new UnitOfWork(this, isolationLevel);
    }

    #endregion
}