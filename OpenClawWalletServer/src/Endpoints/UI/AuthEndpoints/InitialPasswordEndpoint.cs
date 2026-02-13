using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;
using OpenClawWalletServer.Application.Commands.AuthCommands;
using OpenClawWalletServer.Application.Queries;
using OpenClawWalletServer.Extensions;

namespace OpenClawWalletServer.Endpoints.UI.AuthEndpoints;

/// <summary>
/// 设置初始密码端点
/// </summary>
[Tags("UI")]
[HttpPost("api/v1/ui/auth/setup-password")]
[AllowAnonymous]
public class InitialPasswordEndpoint(
    IMediator mediator
) : Endpoint<InitialPasswordReq, ResponseData<bool>>
{
    public override async Task HandleAsync(InitialPasswordReq req, CancellationToken ct)
    {
        // 首先检查密码是否已经存在
        var checkQuery = new UserPasswordInfoQuery();
        var passwordInfo = await mediator.Send(checkQuery, ct);

        if (passwordInfo is not null) // 如果查询返回结果，说明密码已存在
        {
            throw new KnownException("已初始化密码");
        }

        // 执行设置初始密码命令
        var command = new InitialPasswordCommand(req.Password);
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result.AsSuccessResponseData(), ct);
    }
}

/// <summary>
/// 设置初始密码请求
/// </summary>
public class InitialPasswordReq
{
    /// <summary>
    /// 密码
    /// </summary>
    public required string Password { get; set; }
}