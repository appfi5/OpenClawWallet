using NetCorePal.Extensions.Primitives;
using OpenClawWalletServer.Domain.Enums;

namespace OpenClawWalletServer.Application.Queries;

/// <summary>
/// 所以 KeyConfig 查询
/// </summary>
public record AllKeyConfigQuery : IQuery<List<KeyConfigInfo>>;

/// <summary>
/// KeyConfig 信息
/// </summary>
public class KeyConfigInfo
{
    /// <summary>
    /// 签名类型
    /// </summary>
    public required AddressType AddressType { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// 公钥
    /// </summary>
    public required string PublicKey { get; set; }
}
