namespace OpenClawWalletServer.Domain.Enums;

/// <summary>
/// 币种 0-未知;1-USDT(ERC20);2-USDC(ERC20);3-USDI(CKB)
/// </summary>
public enum CurrencyType
{
    /// <summary>
    /// 未知
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// USDT(ERC20)
    /// </summary>
    USDT = 1,

    /// <summary>
    /// USDC(ERC20)
    /// </summary>
    USDC = 2,

    /// <summary>
    /// USDI(CKB)
    /// </summary>
    USDI = 3,
}