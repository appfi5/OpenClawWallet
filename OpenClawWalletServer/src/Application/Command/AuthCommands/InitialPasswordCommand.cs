using NetCorePal.Extensions.Primitives;
using Newtonsoft.Json;
using OpenClawWalletServer.Domain.AggregatesModel.LoginPasswordAggregate;
using OpenClawWalletServer.Infrastructure.Repositories;
using OpenClawWalletServer.Utils;

namespace OpenClawWalletServer.Application.Command.AuthCommands;

/// <summary>
/// 设置初始密码命令
/// </summary>
public record InitialPasswordCommand(string Password) : ICommand<bool>;

/// <summary>
/// 设置初始密码命令处理器
/// </summary>
public class InitialPasswordCommandHandler(
    LoginPasswordRepository loginPasswordRepository
) : ICommandHandler<InitialPasswordCommand, bool>
{
    public async Task<bool> Handle(InitialPasswordCommand request, CancellationToken cancellationToken)
    {
        // 检查是否已存在密码
        var existingPassword = await loginPasswordRepository.FirstOrDefault();
        if (existingPassword is not null)
        {
            throw new KnownException("密码已存在，无法初始化");
        }

        // 生成密码哈希
        var secretData = PasswordHashGenerator.Hash(request.Password);
        var secretDataJson = JsonConvert.SerializeObject(secretData);

        var newPassword = LoginPassword.Create(secretDataJson);
        await loginPasswordRepository.AddAsync(newPassword, cancellationToken);

        return true;
    }
}