using Microsoft.EntityFrameworkCore;
using NetCorePal.Extensions.Primitives;
using OpenClawWalletServer.Domain.Enums;
using OpenClawWalletServer.Infrastructure;

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

/// <summary>
/// 查询处理器
/// </summary>
public class AllKeyConfigQueryHandler(
    ApplicationDbContext dbContext
) : IQueryHandler<AllKeyConfigQuery, List<KeyConfigInfo>>
{
    public async Task<List<KeyConfigInfo>> Handle(AllKeyConfigQuery query, CancellationToken cancellationToken)
    {
        return await dbContext.KeyConfigs
            .Where(config => !config.Deleted)
            .Select(config => new KeyConfigInfo
            {
                AddressType = config.AddressType,
                Address = config.Address,
                PublicKey = config.PublicKey,
            })
            .ToListAsync(cancellationToken);
    }
}