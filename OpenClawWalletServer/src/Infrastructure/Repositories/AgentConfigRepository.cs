using Microsoft.EntityFrameworkCore;
using NetCorePal.Extensions.Repository.EntityFrameworkCore;
using OpenClawWalletServer.Domain.AggregatesModel.AgentConfigAggregate;

namespace OpenClawWalletServer.Infrastructure.Repositories;

/// <summary>
/// Agent 配置仓储
/// </summary>
public class AgentConfigRepository(
    ApplicationDbContext context
) : RepositoryBase<AgentConfig, AgentConfigId, ApplicationDbContext>(context)
{
    /// <summary>
    /// 获取可用的配置
    /// </summary>
    public async Task<AgentConfig?> ValidConfig()
    {
        var config = await context.AgentConfigs
            .Where(config => !config.Deleted)
            .FirstOrDefaultAsync();
        return config;
    }
}