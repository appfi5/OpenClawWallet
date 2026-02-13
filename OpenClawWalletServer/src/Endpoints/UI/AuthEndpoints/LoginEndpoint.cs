using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;
using Newtonsoft.Json;
using OpenClawWalletServer.Application.Queries;
using OpenClawWalletServer.Extensions;
using OpenClawWalletServer.Shared;
using OpenClawWalletServer.Utils;

namespace OpenClawWalletServer.Endpoints.UI.AuthEndpoints;

/// <summary>
/// 登录端点
/// </summary>
[Tags("UI")]
[HttpPost("api/v1/ui/auth/login")]
[AllowAnonymous]
public class LoginEndpoint(
    IMediator mediator
) : Endpoint<LoginReq, ResponseData<LoginResp>>
{
    public override async Task HandleAsync(LoginReq req, CancellationToken ct)
    {
        // 获取密码信息
        var query = new UserPasswordInfoQuery();
        var passwordInfo = await mediator.Send(query, ct);

        if (passwordInfo is null)
        {
            throw new KnownException("未初始化密码");
        }

        // 反序列化SecretData
        var secretData = JsonConvert.DeserializeObject<SecretData>(passwordInfo.SecretData)!;

        // 验证密码
        var isValid = PasswordHashGenerator.Verify(req.Password, secretData.Value, secretData.Salt);

        if (!isValid)
        {
            throw new KnownException("密码错误");
        }

        // 设置身份验证Cookie
        var claims = new[] { new System.Security.Claims.Claim("role", "user") };
        var identity = new System.Security.Claims.ClaimsIdentity(claims, "Cookies");
        var principal = new System.Security.Claims.ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme, principal);

        var response = new LoginResp { Success = true };
        await Send.OkAsync(response.AsSuccessResponseData(), ct);
    }
}

/// <summary>
/// 登录请求
/// </summary>
public class LoginReq
{
    /// <summary>
    /// 密码
    /// </summary>
    public required string Password { get; set; }
}

/// <summary>
/// 登录响应
/// </summary>
public class LoginResp
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }
}