namespace Deje.Core.Status
{
    public class StatusError : StatusMessage
    {
        public StatusError() : base(StatusType.Error, "Operacija nije uspela", null)
        {
        }
    }
}