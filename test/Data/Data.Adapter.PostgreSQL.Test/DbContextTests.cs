using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Data.Adapter.PostgreSQL.Test;

public class DbContextTests : BaseTest
{
    public DbContextTests(ITestOutputHelper output) : base(output)
    {
    }

    /// <summary>
    /// 数据库上下文状态测试
    /// </summary>
    [Fact]
    public void NewConnectionTest()
    {
        using var con = _dbContext.NewConnection();

        Assert.NotNull(con);
        Assert.Equal(ConnectionState.Closed, con.State);

        con.Open();

        Assert.Equal(ConnectionState.Open, con.State);
    }
}
