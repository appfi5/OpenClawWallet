using System.Reflection;
using Ckb.Sdk.Core;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using NetCorePal.Extensions.DependencyInjection;
using NetCorePal.Extensions.Domain.Json;
using OpenClawWalletServer.Domain.AggregatesModel.KeyConfigAggregate;
using OpenClawWalletServer.Domain.Enums;
using OpenClawWalletServer.Extensions;
using OpenClawWalletServer.Infrastructure;
using OpenClawWalletServer.Options;
using OpenClawWalletServer.Utils;

namespace OpenClawWalletServer;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHealthChecks();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Ckb & Eth 配置
        var ckbOption = new CkbOptions();
        builder.Services.Configure<CkbOptions>(builder.Configuration.GetSection("Ckb"));
        builder.Configuration.GetSection("Ckb").Bind(ckbOption);

        builder.Services.Configure<EthOptions>(builder.Configuration.GetSection("Eth"));

        // 当前环境 配置
        var currentEnvironmentOptions = new CurrentEnvironmentOptions();
        builder.Services.Configure<CurrentEnvironmentOptions>(builder.Configuration.GetSection("CurrentEnvironment"));
        builder.Configuration.GetSection("CurrentEnvironment").Bind(currentEnvironmentOptions);

        // 配置密钥数据库
        builder.Services.AddDbContext<KeyDbContext>(options =>
            options.UseSqlite(
                builder.Configuration.GetConnectionString("KeyDb"),
                sqliteOptionsAction: optionsBuilder =>
                    optionsBuilder.MigrationsAssembly(typeof(Program).Assembly.FullName)
            )
        );

        // 配置 DataProtect
        builder.Services.AddDataProtection().PersistKeysToDbContext<KeyDbContext>();

        // 配置应用数据库
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(
                builder.Configuration.GetConnectionString("AppDb"),
                sqliteOptionsAction: optionBuilder =>
                    optionBuilder.MigrationsAssembly(typeof(Program).Assembly.FullName)
            );

            if (currentEnvironmentOptions.IsDebug)
            {
                options.UseAsyncSeeding(async (context, _, ct) =>
                {
                    var appContext = context as ApplicationDbContext;
                    var keyConfig = await appContext!.KeyConfigs.FirstOrDefaultAsync(ct);
                    if (keyConfig is null)
                    {
                        var addressInfo = GenAddressUtils.GenSingleAddress(Network.Testnet);
                        await appContext.KeyConfigs.AddAsync(
                            KeyConfig.Create(
                                SignType.Ckb,
                                addressInfo.PrivateKey,
                                addressInfo.PublicKey,
                                addressInfo.PrivateKey
                            ),
                            ct
                        );
                        await appContext.SaveChangesAsync(ct);
                    }
                });
            }
        });


        builder.Services.AddMediatR(configuration => configuration
            .RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly())
            .AddOpenBehavior(typeof(AgentValidationBehavior<,>))
            .AddUnitOfWorkBehaviors()
        );

        builder.Services.AddUnitOfWork<ApplicationDbContext>();

        // 配置仓储
        builder.Services.AddRepositories(typeof(ApplicationDbContext).Assembly);

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new EntityIdJsonConverterFactory());
        });
        builder.Services.AddSwaggerGenNewtonsoftSupport();

        #region AddAddEthereumMessageSigner

        builder.Services.AddEthereumMessageSigner();

        #endregion

        builder.Services.AddHttpContextAccessor();

        // Cookie 认证
        // builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        //     .AddCookie(configureOptions =>
        //         {
        //             configureOptions.Events.OnRedirectToLogin = context =>
        //             {
        //                 context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //                 return Task.CompletedTask;
        //             };
        //             configureOptions.Cookie.Name = "token";
        //             configureOptions.ExpireTimeSpan = TimeSpan.FromDays(1);
        //             configureOptions.SlidingExpiration = true;
        //         }
        //     );

        var app = builder.Build();

        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Agent 启动");
        if (!Directory.Exists("data"))
        {
            logger.LogError("data 目录不存在");
            Directory.CreateDirectory("data");
        }

        using var scope = app.Services.CreateScope();


        // 创建数据库文件
        var appDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await appDbContext.Database.MigrateAsync();
        var keyDbContext = scope.ServiceProvider.GetRequiredService<KeyDbContext>();
        await keyDbContext.Database.MigrateAsync();

        if (currentEnvironmentOptions.IsDebug)
        {
            app.UseSwagger(options => { options.RouteTemplate = "devops/swagger/{documentName}/swagger.{json|yaml}"; });
            app.UseSwaggerUI(options => { options.RoutePrefix = "devops/swagger"; });
        }

        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.MapControllers();
        app.MapHealthChecks("/healthz");

        await app.RunAsync();
    }
}