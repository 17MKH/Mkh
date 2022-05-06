using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Mkh.Utils.Annotations;

namespace Mkh.Utils.Encrypt;

/// <summary>
/// AES加解密
/// </summary>
[SingletonInject]
public class AesEncrypt
{
    /// <summary>
    /// 默认密钥
    /// </summary>
    private const string KEY = "Mkh!@123IamOldli)(*&^%$#";

    #region ==加密==

    /// <summary>
    /// AES+Base64加密
    /// <para>采用ECB、PKCS7</para>
    /// </summary>
    /// <param name="encryptString">加密字符串</param>
    /// <param name="key">密钥</param>
    /// <returns></returns>
    public string Encrypt(string encryptString, string key = null)
    {
        return Encrypt(encryptString, key, false, true);
    }

    /// <summary>
    /// AES+16进制加密
    /// <para>采用ECB、PKCS7</para>
    /// </summary>
    /// <param name="encryptString">加密字符串</param>
    /// <param name="key">秘钥</param>
    /// <param name="lowerCase">是否小写</param>
    /// <returns></returns>
    public string Encrypt4Hex(string encryptString, string key = null, bool lowerCase = false)
    {
        return Encrypt(encryptString, key, true, lowerCase);
    }

    private string Encrypt(string encryptString, string key, bool hex, bool lowerCase = false)
    {
        if (encryptString.IsNull())
            return null;
        if (key.IsNull())
            key = KEY;

        var keyBytes = Encoding.UTF8.GetBytes(key);
        var encryptBytes = Encoding.UTF8.GetBytes(encryptString);
        var provider = new RijndaelManaged
        {
            Mode = CipherMode.ECB,
            Key = keyBytes,
            Padding = PaddingMode.PKCS7
        };

        using var stream = new MemoryStream();
        var cStream = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
        cStream.Write(encryptBytes, 0, encryptBytes.Length);
        cStream.FlushFinalBlock();

        var bytes = stream.ToArray();
        return hex ? bytes.ToHex(lowerCase) : bytes.ToBase64();
    }

    #endregion

    #region ==解密==

    /// <summary>
    /// AES+Base64解密
    /// <para>采用ECB、PKCS7</para>
    /// </summary>
    /// <param name="decryptString">解密字符串</param>
    /// <param name="key">秘钥</param>
    /// <returns></returns>
    public string Decrypt(string decryptString, string key = null)
    {
        return Decrypt(decryptString, key, false);
    }

    /// <summary>
    /// AES+16进制解密
    /// <para>采用ECB、PKCS7</para>
    /// </summary>
    /// <param name="decryptString">解密字符串</param>
    /// <param name="key">秘钥</param>
    /// <returns></returns>
    public string Decrypt4Hex(string decryptString, string key = null)
    {
        return Decrypt(decryptString, key, true);
    }

    private string Decrypt(string decryptString, string key, bool hex)
    {
        if (decryptString.IsNull())
            return null;
        if (key.IsNull())
            key = KEY;

        var keyBytes = Encoding.UTF8.GetBytes(key);
        var encryptBytes = hex ? decryptString.Hex2Bytes() : Convert.FromBase64String(decryptString);
        var provider = new RijndaelManaged
        {
            Key = keyBytes,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };

        using var mStream = new MemoryStream();
        using var cStream = new CryptoStream(mStream, provider.CreateDecryptor(), CryptoStreamMode.Write);
        cStream.Write(encryptBytes, 0, encryptBytes.Length);
        cStream.FlushFinalBlock();
        return Encoding.UTF8.GetString(mStream.ToArray());
    }

    #endregion
}