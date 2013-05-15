namespace Deje.Core.Model
{
    public class KorisnickiNalog
    {
        public string KorisnickoIme { get; set; }

        public byte[] Lozinka { get; set; }

        public int IdDobavljaca { get; set; }

        public Dobavljac Dobavljac { get; set; }

        public string EMail { get; set; }
    }
}