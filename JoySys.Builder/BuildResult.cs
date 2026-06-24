namespace JoySys.Builder
{
    public enum BuildStatus
    {
        Success,
        Cancelled,
        Failed
    }

    public class BuildResult
    {
        public BuildStatus Status { get; private set; }
        public string Message { get; private set; }

        private BuildResult(BuildStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public static BuildResult Success(string message = "Builder finished successfully.")
        {
            return new BuildResult(BuildStatus.Success, message);
        }

        public static BuildResult Cancelled(string message)
        {
            return new BuildResult(BuildStatus.Cancelled, message);
        }

        public static BuildResult Failed(string message)
        {
            return new BuildResult(BuildStatus.Failed, message);
        }
    }
}