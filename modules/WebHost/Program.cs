
using Mkh.Host.Web;

namespace WebHost;

public class Program
{
    public static void Main(string[] args)
    {
        new HostBootstrap(args).Run();
    }
}