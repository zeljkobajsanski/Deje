using Deje.Core.Model;
using Deje.Core.Repository;
using System.Linq;

namespace Deje.Repository.EF.Repository
{
    public class KorisnickiNaloziRepository : IKorisnickiNaloziRepository
    {
        public KorisnickiNalog PostojiKorisnik(string korisnickoIme, byte[] lozinka)
        {
            using (var ctx = new ModelContext())
            {
                var korisnik = ctx.KorisnickiNalozi.SingleOrDefault(x => x.KorisnickoIme == korisnickoIme);
                if (korisnik == null) return null;
                if (korisnik.Lozinka.SequenceEqual(lozinka))
                {
                    return korisnik;
                }
                return null;
            }
        }

        public void Save(KorisnickiNalog korisnickiNalog)
        {
            using (var ctx = new ModelContext())
            {
                var postojeciNalog = ctx.KorisnickiNalozi.SingleOrDefault(x => x.KorisnickoIme == korisnickiNalog.KorisnickoIme);
                if (postojeciNalog == null)
                {
                    ctx.KorisnickiNalozi.Add(korisnickiNalog);
                }
                else
                {
                    postojeciNalog.KorisnickoIme = korisnickiNalog.KorisnickoIme;
                    postojeciNalog.Lozinka = korisnickiNalog.Lozinka;
                    postojeciNalog.IdDobavljaca = korisnickiNalog.IdDobavljaca;
                    postojeciNalog.EMail = korisnickiNalog.EMail;
                }
                ctx.SaveChanges();
            }
        }
    }
}