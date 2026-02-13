using Microsoft.EntityFrameworkCore;
using NetCorePal.Extensions.Primitives;
using OpenClawWalletServer.Infrastructure;

namespace OpenClawWalletServer.Application.Queries;

/// <summary>
/// 用户密码信息查询 - 支持登录验证和密码初始化检查两种场景
/// </summary>
public record UserPasswordInfoQuery : IQuery<UserPasswordInfoResponse?>;

/// <summary>
/// 用户密码信息响应
/// </summary>
public record UserPasswordInfoResponse(
    string SecretData
);

/// <summary>
/// 查询处理器
/// </summary>
public class UserPasswordInfoQueryHandler(
    ApplicationDbContext dbContext
) : IQueryHandler<UserPasswordInfoQuery, UserPasswordInfoResponse?>
{
    public async Task<UserPasswordInfoResponse?> Handle(UserPasswordInfoQuery query, CancellationToken cancellationToken)
    {
        return await dbContext.LoginPasswords
            .Where(p => !p.Deleted)
            .Select(p => new UserPasswordInfoResponse(
                SecretData:  p.SecretData
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}