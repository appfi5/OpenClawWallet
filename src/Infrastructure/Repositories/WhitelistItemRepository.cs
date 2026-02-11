using Microsoft.EntityFrameworkCore;
using NetCorePal.Extensions.Repository.EntityFrameworkCore;
using OpenClawWalletServer.Domain.AggregatesModel.WhitelistAggregate;

namespace OpenClawWalletServer.Infrastructure.Repositories;

/// <summary>
/// Agent 配置仓储
/// </summary>
public class WhitelistItemRepository(
    ApplicationDbContext context
) : RepositoryBase<WhitelistItem, WhitelistItemId, ApplicationDbContext>(context)
{
    /// <summary>
    /// 更加地址查询白名单
    /// </summary>
    public async Task<WhitelistItem?> FindByAddressOrDefault(
        string address,
        CancellationToken cancellationToken
    )
    {
        return await context.WhitelistItems.FirstOrDefaultAsync(
            predicate: item => item.Address == address && !item.Deleted,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// 所有的白名单项
    /// </summary>
    public async Task<List<WhitelistItem>> All(CancellationToken cancellationToken)
    {
        return await context.WhitelistItems
            .Where(item => !item.Deleted)
            .ToListAsync(cancellationToken);
    }
}