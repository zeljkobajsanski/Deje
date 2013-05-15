using System.Collections.Generic;
using System.Data;
using Deje.Core.Model;
using Deje.Core.Repository;
using System.Linq;
using Deje.Core.Utils;

namespace Deje.Repository.EF.Repository
{
    public class KategorijeArtikalaRepository : IKategorijeArtikalaRepository
    {
        public IEnumerable<KategorijaArtikla> VratiSve()
        {
            using (var ctx = new ModelContext())
            {
                return ctx.KategorijeArtikala.OrderBy(x => x.Naziv).ToArray();
            }
        }

        public void Sacuvaj(KategorijaArtikla kategorijaArtikla)
        {
            using (var ctx = new ModelContext())
            {
                ctx.KategorijeArtikala.Add(kategorijaArtikla);
                if (kategorijaArtikla.Id != 0)
                {
                    ctx.Entry(kategorijaArtikla).State = EntityState.Modified;
                }
                ctx.SaveChanges();
            }
        }

        public void Obrisi(int id)
        {
            using (var ctx = new ModelContext())
            {
                ctx.Database.ExecuteSqlCommand("DELETE FROM KategorijeArtikala WHERE Id={0}", id);
            }
        }

        public IEnumerable<KategorijaArtikla> VratiKategorijeArtikalaUOkolini(double latituda, double longituda, int udaljenost, int idDelatnosti)
        {
            var point = GeoUtils.CreatePoint(latituda, longituda);
            using (var ctx = new ModelContext())
            {
                var artikli = ctx.Artikli.Where(x => x.Dobavljac.Status.PrikaziNaPretragama && x.Dobavljac.GpsLokacija.Distance(point) <= udaljenost);
                if (idDelatnosti > 0)
                {
                    artikli = artikli.Where(x => x.Dobavljac.IdDelatnosti == idDelatnosti);
                }
                var artikliArray = artikli.ToArray();
                var artikliLookup = artikliArray.ToLookup(x => x.IdKategorijeArtikla);
                var kategorijeArtikala =  artikli.Select(x => x.KategorijaArtikla).Distinct().ToArray();
                foreach (var kategorijaArtikla in kategorijeArtikala)
                {
                    kategorijaArtikla.BrojArtikala = artikliLookup[kategorijaArtikla.Id].Count();
                }
                return kategorijeArtikala;
            }
        }

        public int Sacuvaj(string nazivKategorije)
        {
            using (var ctx = new ModelContext())
            {
                var kategorija = ctx.KategorijeArtikala.SingleOrDefault(x => x.Naziv == nazivKategorije);
                if (kategorija != null) return kategorija.Id;
                kategorija = new KategorijaArtikla {Naziv = nazivKategorije};
                ctx.KategorijeArtikala.Add(kategorija);
                ctx.SaveChanges();
                return kategorija.Id;
            }
        }
    }
}