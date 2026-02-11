using NetCorePal.Extensions.Domain;
using OpenClawWalletServer.Domain.Enums;

namespace OpenClawWalletServer.Domain.AggregatesModel.SignRecordAggregate;

/// <summary>
/// 签名记录 Id
/// </summary>
public partial record SignRecordId : IGuidStronglyTypedId;

/// <summary>
/// 资金流出订单 AgentTask Id
/// </summary>
public partial record BusinessCashOutOrderAgentTaskId : IGuidStronglyTypedId;

/// <summary>
/// 签名记录
/// </summary>
public class SignRecord : Entity<SignRecordId>, IAggregateRoot
{
    protected SignRecord()
    {
    }

    /// <summary>
    /// 签名类型
    /// </summary>
    public AddressType AddressType { get; private set; } = AddressType.Unknown;

    /// <summary>
    /// 签名内容
    /// </summary>
    public string Content { get; private set; } = string.Empty;

    /// <summary>
    /// 签名时间
    /// </summary>
    public DateTime SignTime { get; private set; } = DateTime.MinValue;

    /// <summary>
    /// 是否已删除
    /// </summary>
    public bool Deleted { get; private set; } = false;

    /// <summary>
    /// 创建签名记录
    /// </summary>
    public static SignRecord Create(
        AddressType addressType
    )
    {
        var record = new SignRecord
        {
            AddressType = addressType,
            SignTime = DateTime.Now
        };
        return record;
    }
}