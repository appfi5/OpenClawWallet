namespace OpenClawWalletServer.Shared;

/// <summary>
/// 密码 Hash 摘要
/// </summary>
public record SecretData(string Value, string Salt);