using Microsoft.EntityFrameworkCore;
using NetCorePal.Extensions.Repository.EntityFrameworkCore;
using OpenClawWalletServer.Domain.AggregatesModel.KeyConfigAggregate;

namespace OpenClawWalletServer.Infrastructure.Repositories;

/// <summary>
/// 密钥配置仓储
/// </summary>
public class KeyConfigRepository(
    ApplicationDbContext context
) : RepositoryBase<KeyConfig, KeyConfigId, ApplicationDbContext>(context)
{
    /// <summary>
    /// 通过 Address 查询密钥, 没有时返回 null
    /// </summary>
    public async Task<KeyConfig?> FindByAddressOrDefault(string address, CancellationToken cancellationToken = default)
    {
        return await context.KeyConfigs
            .Where(config => !config.Deleted &&
                             config.Address == address)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// 通过 Address 查询密钥
    /// </summary>
    public async Task<KeyConfig> FindByAddress(string address, CancellationToken cancellationToken = default)
    {
        return await context.KeyConfigs
            .Where(config => !config.Deleted &&
                             config.Address == address)
            .FirstAsync(cancellationToken);
    }

    /// <summary>
    /// 查配置的地址
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<string>> FindAll(CancellationToken cancellationToken = default)
    {
        return await context.KeyConfigs
            .Where(config => !config.Deleted)
            .Select(config => config.Address)
            .ToListAsync(cancellationToken);
    }
}