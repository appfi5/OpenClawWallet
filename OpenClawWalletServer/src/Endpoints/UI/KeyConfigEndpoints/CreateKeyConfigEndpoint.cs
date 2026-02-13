using FastEndpoints;
using MediatR;
using NetCorePal.Extensions.Dto;
using OpenClawWalletServer.Application.Commands.KeyConfigCommands;
using OpenClawWalletServer.Domain.Enums;
using OpenClawWalletServer.Extensions;

namespace OpenClawWalletServer.Endpoints.UI.KeyConfigEndpoints;

/// <summary>
/// 创建KeyConfig端点
/// </summary>
[Tags("UI")]
[HttpPost("api/v1/ui/key-config/create")]
public class CreateKeyConfigEndpoint(
    IMediator mediator
) : Endpoint<CreateKeyConfigReq, ResponseData<bool>>
{
    public override async Task HandleAsync(CreateKeyConfigReq req, CancellationToken ct)
    {
        var command = req.ToCommand();

        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result.AsSuccessResponseData(), ct);
    }
}

/// <summary>
/// 创建KeyConfig请求
/// </summary>
public class CreateKeyConfigReq
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

    /// <summary>
    /// 请求转命令
    /// </summary>
    public CreateKeyConfigCommand ToCommand()
    {
        return new CreateKeyConfigCommand
        {
            AddressType = AddressType,
            Address = Address,
            PublicKey = PublicKey,
            PrivateKey = PrivateKey
        };
    }
}