using Nethereum.Signer;

namespace OpenClawWalletServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEthereumMessageSigner(this IServiceCollection services)
    {
        services.AddSingleton<EthereumMessageSigner>(p => new EthereumMessageSigner());
        return services;
    }
}