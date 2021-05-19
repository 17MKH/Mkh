using System.Diagnostics;

namespace Mkh.Module.Admin.Application.AccountService
{
    public class AccountService : IAccountService
    {
        public void Login()
        {
            Debug.WriteLine("登录成功");
        }
    }
}
