using Ckb.Sdk.Ckb.Transactions;
using Ckb.Sdk.Core;
using Newtonsoft.Json;
using Xunit;
using static OpenClawWalletServer.Utils.TransactionSerializerConfigUtils;

namespace Test;

/// <summary>
/// 生成交易测试
/// </summary>
public class GenTransactionTest
{
    /// <summary>
    /// 生成交易
    /// </summary>
    [Fact]
    public async Task GenTransaction()
    {
        const string fromAddress = "ckt1qzda0cr08m85hc8jlnfp3zer7xulejywt49kt2rr0vthywaa50xwsq2egvtymu794xayka3a4hd6ynnj84xe7wgght7pt";
        const string toAddress = "ckt1qzda0cr08m85hc8jlnfp3zer7xulejywt49kt2rr0vthywaa50xwsq2ga95nmn2p27ut4e3yn3f9e04wh798flgtcwrkc";
        const long amount = 100L;
        
        var network = fromAddress.StartsWith("ckb") ? Network.Mainnet : Network.Testnet;
        var iterator = new InputEnumerator(fromAddress);
        var ckbTransactionBuilder = new CkbTransactionBuilder(new TransactionBuilderConfiguration(network), iterator);

        var txWithGroup = ckbTransactionBuilder
            .AddOutput(toAddress, amount * 100_000_000)
            .SetChangeOutput(fromAddress)
            .Build();

        var txWithGroupJson = JsonConvert.SerializeObject(txWithGroup, GenerateJsonSetting());
    }
}