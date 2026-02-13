using Ckb.Sdk.Core.Utils.Addresses;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using NetCorePal.Extensions.Primitives;
using OpenClawWalletServer.Domain.AggregatesModel.KeyConfigAggregate;
using OpenClawWalletServer.Domain.Enums;
using OpenClawWalletServer.Infrastructure.Repositories;
using OpenClawWalletServer.Options;

namespace OpenClawWalletServer.Application.Commands.KeyConfigCommands;

/// <summary>
/// 创建KeyConfig命令
/// </summary>
public class CreateKeyConfigCommand : ICommand<bool>
{
    /// <summary>
    /// 地址类型
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
    
    /// <summary>
    /// 私钥
    /// </summary>
    public required string PrivateKey { get; set; }
}

/// <summary>
/// 创建KeyConfig命令处理器
/// </summary>
public class CreateKeyConfigCommandHandler(
    KeyConfigRepository keyConfigRepository,
    IOptions<CkbOptions> ckbOptions,
    IDataProtectionProvider dataProtectionProvider
) : ICommandHandler<CreateKeyConfigCommand, bool>
{
    private const string ProviderKey = "PrivateKey";

    public async Task<bool> Handle(CreateKeyConfigCommand command, CancellationToken cancellationToken)
    {
        // 检查地址是否已存在
        var existingConfig = await keyConfigRepository.FindByAddress(command.Address, cancellationToken);
        if (existingConfig != null)
        {
            throw new KnownException("Address already exists");
        }

        var address = Address.Decode(command.Address).ScriptValue.Args;

        // 加密私钥
        var dataProtector = dataProtectionProvider.CreateProtector(ProviderKey);
        var encryptedPrivateKey = dataProtector.Protect(command.PrivateKey);

        var keyConfig = KeyConfig.Create(
            command.AddressType,
            command.Address,
            command.PublicKey,
            encryptedPrivateKey
        );

        await keyConfigRepository.AddAsync(keyConfig, cancellationToken);

        return true;
    }
}