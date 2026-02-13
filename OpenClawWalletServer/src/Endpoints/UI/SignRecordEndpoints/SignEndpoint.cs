using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using NetCorePal.Extensions.Dto;
using OpenClawWalletServer.Application.Commands.SignRecordCommands;
using OpenClawWalletServer.Extensions;

namespace OpenClawWalletServer.Endpoints.UI.SignRecordEndpoints;

/// <summary>
/// 签名 Ckb 交易 Endpoint
/// </summary>
[Tags("UI")]
[HttpPost("api/sign/sign-ckb-transaction")]
[AllowAnonymous]
public class SignEndpoint(
    IMediator mediator
) : Endpoint<SignReq, ResponseData<SignResp>>
{
    public override async Task HandleAsync(SignReq req, CancellationToken ct)
    {
        var command = req.ToCommand();
        var result = SignResp.From(await mediator.Send(command, ct));
        await Send.OkAsync(result.AsSuccessResponseData(), ct);
    }
}

/// <summary>
/// 签名请求
/// </summary>
public class SignReq
{
    /// <summary>
    /// 签名地址
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// 签名内容，交易
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// 请求转命令
    /// </summary>
    public SignCkbTransactionCommand ToCommand()
    {
        return new SignCkbTransactionCommand
        {
            Address = Address,
            Content = Content,
        };
    }
}

/// <summary>
/// 签名响应
/// </summary>
public class SignResp
{
    /// <summary>
    /// 签名地址
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// 签名内容，交易
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// 交易 Hash
    /// </summary>
    public required string TxHash { get; set; }

    public static SignResp From(SignCkbTransactionCommandResult result)
    {
        return new SignResp
        {
            Address = result.Address,
            Content = result.Content,
            TxHash = result.TxHash,
        };
    }
}