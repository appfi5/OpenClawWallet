namespace OpenClawWalletServer.Domain.Enums;

/// <summary>
/// 签名类型
/// </summary>
public enum SignType
{
    /// <summary>
    /// 未知
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// 以太坊签名
    /// </summary>
    Eth = 1,

    /// <summary>
    /// Ckb 签名
    /// </summary>
    Ckb = 2,
}