using System.Security.Cryptography;
using System.Text;
using NetCorePal.Extensions.Primitives;
using OpenClawWalletServer.Shared;

namespace OpenClawWalletServer.Utils;

/// <summary>
/// 密码摘要生成器
/// </summary>
public static class PasswordHashGenerator
{
    private const char PaddingCharNone = '\0';

    /// <summary>
    /// 生成密码 hash 摘要
    /// </summary>
    /// <exception cref="KnownException"></exception>
    public static SecretData Hash(string password, int iterations = 27500, int keySize = 256)
    {
        var salt = Salt();
        var rawPasswordWithPadding = Padding(password, 0);
        var saltBytes = Convert.FromBase64String(salt);
        using var spec = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(rawPasswordWithPadding), saltBytes, iterations,
            HashAlgorithmName.SHA256);
        try
        {
            var key = spec.GetBytes(keySize / 4);
            return new SecretData(Convert.ToBase64String(key), salt);
        }
        catch (Exception e)
        {
            throw new KnownException("Generate Password hash failed!", e);
        }
    }

    /// <summary>
    /// 验证密码是否正确
    /// </summary>
    public static bool Verify(string password, string hash, string salt, int iterations = 27500, int keySize = 256)
    {
        var rawPasswordWithPadding = Padding(password, 0);
        var saltBytes = Convert.FromBase64String(salt);
        using var spec = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(rawPasswordWithPadding), saltBytes, iterations,
            HashAlgorithmName.SHA256);
        try
        {
            var key = spec.GetBytes(keySize / 4);
            var passwordHash = Convert.ToBase64String(key);
            return hash == passwordHash;
        }
        catch (Exception e)
        {
            throw new KnownException("Verify Password hash failed!", e);
        }
    }

    /// <summary>
    /// 生成随机盐噪声
    /// </summary>
    private static string Salt()
    {
        var randomBytes = new byte[16];
        Random.Shared.NextBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    /// <summary>
    /// 生成数据填充
    /// </summary>
    private static string Padding(string rawString, int maxPaddingLength)
    {
        if (rawString.Length >= maxPaddingLength)
        {
            return rawString;
        }

        var nPad = maxPaddingLength - rawString.Length;
        var result = new StringBuilder(rawString);
        for (var i = 0; i < nPad; i++)
        {
            result.Append(PaddingCharNone);
        }

        return result.ToString();
    }
}