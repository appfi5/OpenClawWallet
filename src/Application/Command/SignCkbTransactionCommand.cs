using Ckb.Sdk.Core.Signs;
using Microsoft.Extensions.Options;
using NetCorePal.Extensions.Primitives;
using Newtonsoft.Json;
using OpenClawWalletServer.Domain.AggregatesModel.SignRecordAggregate;
using OpenClawWalletServer.Domain.Enums;
using OpenClawWalletServer.Infrastructure.Repositories;
using OpenClawWalletServer.Options;
using static OpenClawWalletServer.Utils.TransactionSerializerConfigUtils;

namespace OpenClawWalletServer.Application.Command;

/// <summary>
/// 签名 Ckb 交易命令
/// </summary>
public class SignCkbTransactionCommand : ICommand<SignCkbTransactionCommandResult>
{
    /// <summary>
    /// 签名地址
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// 签名内容，交易
    /// </summary>
    public required string Content { get; set; }
}

/// <summary>
/// 命令结果
/// </summary>
public class SignCkbTransactionCommandResult
{
    /// <summary>
    /// 签名地址
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// 签名内容，交易
    /// </summary>
    public required string Content { get; set; }
}

/// <summary>
/// 命令处理器
/// </summary>
public class SignCkbTransactionCommandHandler(
    SignRecordRepository signRecordRepository,
    KeyConfigRepository keyConfigRepository,
    IOptions<CkbOptions> ckbOptions
) : ICommandHandler<SignCkbTransactionCommand, SignCkbTransactionCommandResult>
{
    public async Task<SignCkbTransactionCommandResult> Handle(
        SignCkbTransactionCommand command,
        CancellationToken cancellationToken
    )
    {
        var keyConfig = await keyConfigRepository.FindByAddress(command.Address, cancellationToken);
        if (keyConfig is null)
        {
            throw new KnownException("KeyConfig not found");
        }

        var transaction = JsonConvert.DeserializeObject<TransactionWithScriptGroups>(
            command.Content,
            GenerateJsonSetting()
        );
        if (transaction is null)
        {
            throw new KnownException("Transaction deserialize failed");
        }

        var signer = TransactionSigner.GetInstance(ckbOptions.Value.Network);

        signer.SignTransaction(
            transaction: transaction,
            privateKeys: keyConfig.PrivateKey
        );

        var signedContent = JsonConvert.SerializeObject(transaction, GenerateJsonSetting());
        var signRecord = SignRecord.Create(
            addressType: AddressType.Ckb,
            content: signedContent
        );

        await signRecordRepository.AddAsync(signRecord, cancellationToken);

        return new SignCkbTransactionCommandResult
        {
            Address = command.Address,
            Content = signedContent,
        };
    }
}