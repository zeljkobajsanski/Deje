namespace Deje.Core.Status
{
    public class StatusSaved : StatusMessage
    {
        public StatusSaved() : base(StatusType.Success, "Podaci su uspešno sačuvani", "Bravo.")
        {
            
        }
    }
}