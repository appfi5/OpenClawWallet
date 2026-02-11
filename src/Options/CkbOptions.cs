using Ckb.Sdk.Core;

namespace OpenClawWalletServer.Options;

/// <summary>
/// Ckb 配置
/// </summary>
public class CkbOptions
{
    /// <summary>
    /// 网络类型
    /// </summary>
    public Network Network { get; set; } = Network.Mainnet;
}