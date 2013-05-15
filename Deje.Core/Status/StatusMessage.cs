namespace Deje.Core.Status
{
    public class StatusMessage
    {
        public StatusType StatusType { get; set; }
        public string Message { get; set; }
        public string TypeDescription { get; set; }

        public StatusMessage()
        {
        }

        public StatusMessage(StatusType statusType, string message, string typeDescription)
        {
            StatusType = statusType;
            Message = message;
            TypeDescription = typeDescription;
        }
    }
}