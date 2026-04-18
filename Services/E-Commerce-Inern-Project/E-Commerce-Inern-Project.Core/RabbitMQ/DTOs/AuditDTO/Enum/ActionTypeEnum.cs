namespace E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum
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
