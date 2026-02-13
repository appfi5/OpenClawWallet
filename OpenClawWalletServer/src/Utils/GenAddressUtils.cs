using Ckb.Sdk.Core;
using Ckb.Sdk.Core.Types;
using Ckb.Sdk.Core.Utils.Addresses;
using Ckb.Sdk.Utils.Crypto.Secp256k1;
using Ckb.Sdk.Utils.Utils;

namespace OpenClawWalletServer.Utils;

/// <summary>
/// 地址生成工具
/// </summary>
public class GenAddressUtils
{
    /// <summary>
    /// 生成单签地址
    /// </summary>
    public static AddressInfo GenSingleAddress(Network network = Network.Mainnet)
    {
        var keyPair = Keys.CreateSecp256k1KeyPair();
        var ecKeyPair = ECKeyPair.Create(keyPair);
        var privateKey = Numeric.ToHexString(ecKeyPair.GetEncodedPrivateKey());
        var publicKey = Numeric.ToHexString(ecKeyPair.GetEncodedPublicKey(true));
        var script = Script.GenerateSecp256K1Blake160SignhashAllScript(ecKeyPair);
        var address = new Address(script, network).Encode();
        return new AddressInfo
        {
            Address = address,
            PublicKey = publicKey,
            PrivateKey = privateKey
        };
    }
}

/// <summary>
/// 地址私钥对
/// </summary>
public class AddressInfo
{
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