namespace Domain.Constants
{
    public static class BatchStatuses
    {
        public const string Planned = "Planned";
        public const string InProgress = "InProgress";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
        public const string Reworking = "Reworking";
    }

    public static class AssignmentStatuses
    {
        public const string Planned = "Planned";
        public const string Active = "Active";
        public const string InProgress = "InProgress";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
        public const string Reworking = "Reworking";
    }

    public static class MaterialRequestStatuses
    {
        public const string Pending = "Pending";
        public const string Approved = "Approved";
        public const string Confirmed = "Confirmed";
        public const string Rejected = "Rejected";
        public const string Cancelled = "Cancelled";
    }

    public static class TransferRequestStatuses
    {
        public const string Pending = "Pending";
        public const string InProgress = "InProgress";
        public const string Approved = "Approved";
        public const string Confirmed = "Confirmed";
        public const string Rejected = "Rejected";
        public const string Cancelled = "Cancelled";
    }

    public static class EvaluateStatuses
    {
        public const string Passed = "Passed";
        public const string Failed = "Failed";
        public const string Pending = "Pending";
        public const string Rejected = "Rejected";
    }

    public static class ComponentDefectStatuses
    {
        public const string Pending = "Pending";
        public const string Resolved = "Resolved";
        public const string Confirmed = "Confirmed";
        public const string Unfixable = "Unfixable";
        public const string Rejected = "Rejected";
    }

    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Lead = "Lead";
        public const string QC = "QC";
        public const string Staff = "Staff";
        public const string QCTransport = "QCTransport";
        public const string GuardQC = "GuardQC";
        public const string QCK = "QCK";
    }
}
