using Ckb.Sdk.Core;

namespace OpenClawWalletServer.Options;

/// <summary>
/// Ckb 配置
/// </summary>
public class CkbOptions
{
    /// <summary>
    /// Ckb 节点的 Url
    /// </summary>
    public string NodeUrl { get; set; } = "https://mainnet.ckb.dev";
    
    /// <summary>
    /// 网络类型
    /// </summary>
    public Network Network { get; set; } = Network.Mainnet;
}