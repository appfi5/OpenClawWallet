using Ckb.Sdk.Ckb.Services;
using Ckb.Sdk.Core.Signs;
using Ckb.Sdk.Core.Types;
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

        var transaction = JsonConvert.DeserializeObject<Transaction>(
            command.Content,
            GenerateJsonSetting()
        );
        if (transaction is null)
        {
            throw new KnownException("Transaction deserialize failed");
        }

        var transactionWithScriptGroups2 = await GenTransactionWithScriptGroups(transaction);

        var signer = TransactionSigner.GetInstance(ckbOptions.Value.Network);

        signer.SignTransaction(
            transaction: transactionWithScriptGroups2,
            privateKeys: keyConfig.PrivateKey
        );

        var signedContent = JsonConvert.SerializeObject(transactionWithScriptGroups2, GenerateJsonSetting());
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

    /// <summary>
    /// 对交易重新分组
    /// </summary>
    private async Task<TransactionWithScriptGroups> GenTransactionWithScriptGroups(Transaction transaction)
    {
        var api = new Api(ckbOptions.Value.NodeUrl, false);
        var dictionary = new Dictionary<Script, ScriptGroup>();
        foreach (var output in transaction.Outputs)
        {
            var type = output.Type;
            if (type == null || dictionary.TryGetValue(type, out var scriptGroup))
            {
                continue;
            }

            scriptGroup = new ScriptGroup
            {
                Script = type,
                GroupType = ScriptType.Type
            };

            dictionary.Add(type, scriptGroup);
        }

        for (var index = 0; index < transaction.Inputs.Count; index++)
        {
            var input = transaction.Inputs[index];
            var inputPreTx = await api.GetTransactionAsync(input.PreviousOutput.TxHash);
            if (inputPreTx is null)
            {
                throw new KnownException($"Invalid transaction, invalid input: {JsonConvert.SerializeObject(input, GenerateJsonSetting())}");
            }
            
            var key = inputPreTx.Transaction.Outputs[input.PreviousOutput.Index].Lock;

            if (!dictionary.TryGetValue(key, out var lockScriptGroup))
            {
                lockScriptGroup = new ScriptGroup
                {
                    Script = key,
                    GroupType = ScriptType.Lock
                };
                dictionary.Add(key, lockScriptGroup);
            }

            lockScriptGroup.InputIndices.Add(index);

            var type = inputPreTx.Transaction.Outputs[input.PreviousOutput.Index].Type;

            if (type == null)
            {
                continue;
            }

            if (!dictionary.TryGetValue(type, out var typeScriptGroup))
            {
                typeScriptGroup = new ScriptGroup
                {
                    Script = type,
                    GroupType = ScriptType.Type
                };
                dictionary.Add(type, typeScriptGroup);
            }

            typeScriptGroup.InputIndices.Add(index);
        }

        return new TransactionWithScriptGroups
        {
            TxView = transaction,
            ScriptGroups = dictionary.Values.ToList(),
        };
    }
}