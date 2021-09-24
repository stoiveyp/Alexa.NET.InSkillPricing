using System.Runtime.Serialization;

namespace Alexa.NET.InSkillPricing
{
    public enum TransactionStatus
    {
        [EnumMember(Value="PENDING_APPROVAL_BY_PARENT")]
        PendingApprovalByParent,
        [EnumMember(Value="APPROVED_BY_PARENT")]
        ApprovedByParent,
        [EnumMember(Value="DENIED_BY_PARENT")]
        DeniedByParent,
        [EnumMember(Value="EXPIRED_NO_ACTION_BY_PARENT")]
        ExpiredNoActionByParent,
        [EnumMember(Value="ERROR")]
        Error
    }
}