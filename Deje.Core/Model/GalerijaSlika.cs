using System;

namespace Deje.Core.Model
{
    public class SlikeDobavljaca
    {
        public Guid Id { get; set; }
        public int IdDobavljaca { get; set; }
        public Dobavljac Dobavljac { get; set; }
        public string Ekstenzija { get; set; }

        public string SlikaSaEkstenzijom
        {
            get { return Id + Ekstenzija; }
        }
    }
}