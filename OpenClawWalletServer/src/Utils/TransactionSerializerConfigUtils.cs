using Ckb.Sdk.Core.Services.Converters;
using Newtonsoft.Json;

namespace OpenClawWalletServer.Utils;

/// <summary>
/// 交易序列化配置工具
/// </summary>
public class TransactionSerializerConfigUtils
{
    /// <summary>
    /// 生成 Json 序列化配置
    /// </summary>
    public static JsonSerializerSettings GenerateJsonSetting()
    {
        var setting = new JsonSerializerSettings();
        //Hex类型转换int32 设置
        setting.Converters.Add(new HexToIntConverter());
        //Hex类型转换byte[] 设置
        setting.Converters.Add(new HexToByteArrayConverter());
        //Hex 类型转换设置
        setting.Converters.Add(new HexToLongConverter());

        //HexToBigInteger 类型转换设置
        setting.Converters.Add(new HexToBigIntegerConverter());
        setting.Converters.Add(new HexToByteArrayKeyDictionaryConverter());
        setting.Converters.Add(new CheckedEnumTypeConverter());
        setting.Converters.Add(new TxWithCellsConverter());
        return setting;
    }
}