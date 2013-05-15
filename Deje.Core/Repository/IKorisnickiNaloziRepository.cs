using Deje.Core.Model;

namespace Deje.Core.Repository
{
    public interface IKorisnickiNaloziRepository
    {
        KorisnickiNalog PostojiKorisnik(string korisnickoIme, byte[] lozinka);
        void Save(KorisnickiNalog korisnickiNalog);
    }
}