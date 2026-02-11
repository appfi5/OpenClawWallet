namespace OpenClawWalletServer.Options;

/// <summary>
/// 当前环境 配置
/// </summary>
public class CurrentEnvironmentOptions
{
    /// <summary>
    /// 网络类型
    /// </summary>
    public bool IsDebug { get; set; } = false;
}