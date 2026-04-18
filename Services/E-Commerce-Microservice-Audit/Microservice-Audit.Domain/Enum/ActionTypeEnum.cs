 
namespace Microservice_Audit.Domain.Enum
{
    public enum ActionTypeEnum
    {
        Create,
        Update,
        Delete,
        Login,
        Logout,
        Register,
        ChangePassword,
        ResetPassword,
        AssignRole,
        RemoveRole,
        Blocked,
        Approve,
        Reject,
        Submit,
        Cancel,
        Complete,
        Archive,
        Restore,
        SoftDelete,
        HardDelete,
        StatusChanged,
        FailedAttempt,
        UnauthorizedAccess,
        ForbiddenAction
    }
}
