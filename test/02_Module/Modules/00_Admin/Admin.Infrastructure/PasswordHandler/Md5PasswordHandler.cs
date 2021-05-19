using System;
using Mkh.Utils.Encrypt;

namespace Mkh.Module.Admin.Infrastructure.PasswordHandler
{
    public class Md5PasswordHandler : IPasswordHandler
    {
        private readonly Md5Encrypt _md5Encrypt;

        public Md5PasswordHandler(Md5Encrypt md5Encrypt)
        {
            _md5Encrypt = md5Encrypt;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public string Encrypt(string userName, string password)
        {
            return _md5Encrypt.Encrypt($"{userName.ToLower()}_{password}");
        }

        public string Decrypt(string encryptPassword)
        {
            throw new NotSupportedException("MD5加密无法解密");
        }
    }
}
