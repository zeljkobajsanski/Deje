namespace Deje.Core.Status
{
    public class StatusValidationError : StatusMessage
    {
        public StatusValidationError() : base(StatusType.Error, "Podaci nisu validni za snimanje. Ispravite sve greške pre snimanja.", "Greška!")
        {
        }
    }
}