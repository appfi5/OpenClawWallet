using NetCorePal.Extensions.Repository.EntityFrameworkCore;
using OpenClawWalletServer.Domain.AggregatesModel.LoginPasswordAggregate;

namespace OpenClawWalletServer.Infrastructure.Repositories;

/// <summary>
/// 登录密码仓储
/// </summary>
public class LoginPasswordRepository(
    ApplicationDbContext context
) : RepositoryBase<LoginPassword, LoginPasswordId, ApplicationDbContext>(context)
{
    public async Task<LoginPassword?> FirstOrDefault()
    {
        return context.LoginPasswords.FirstOrDefault(password => !password.Deleted);
    }
}