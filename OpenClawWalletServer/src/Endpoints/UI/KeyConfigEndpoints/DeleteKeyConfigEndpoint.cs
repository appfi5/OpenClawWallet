using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using NetCorePal.Extensions.Dto;
using OpenClawWalletServer.Application.Commands.KeyConfigCommands;
using OpenClawWalletServer.Extensions;

namespace OpenClawWalletServer.Endpoints.UI.KeyConfigEndpoints;

/// <summary>
/// 删除KeyConfig端点
/// </summary>
[Tags("UI")]
[HttpPost("api/v1/ui/key-config/delete")]
public class DeleteKeyConfigEndpoint(
    IMediator mediator
) : Endpoint<DeleteKeyConfigRequest, ResponseData<bool>>
{
    public override async Task HandleAsync(DeleteKeyConfigRequest req, CancellationToken ct)
    {
        var command = new DeleteKeyConfigCommand
        {
            Address = req.Address
        };

        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result.AsSuccessResponseData(), ct);
    }
}

/// <summary>
/// 删除KeyConfig请求
/// </summary>
public class DeleteKeyConfigRequest
{
    /// <summary>
    /// 地址
    /// </summary>
    public required string Address { get; set; }
}