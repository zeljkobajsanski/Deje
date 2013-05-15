using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using Deje.Core.Model;
using Deje.Core.Repository;
using System.Linq;
using Deje.Core.Utils;
using System.Data.Entity;

namespace Deje.Repository.EF.Repository
{
    public class ArtikliRepository : IArtikliRepository
    {
        public IEnumerable<Artikal> VratiArtikleDobaljvaca(int idDobavljaca)
        {
            using (var ctx = new ModelContext())
            {
                return ctx.Artikli.Where(x => x.IdDobavljaca == idDobavljaca)
                                  .OrderBy(x => x.KategorijaArtikla.Naziv).ToArray();
            }
        }

        public int Sacuvaj(Artikal artikal)
        {
            using (var ctx = new ModelContext())
            {
                ctx.Artikli.Add(artikal);
                if (artikal.Id != 0)
                {
                    ctx.Entry(artikal).State = EntityState.Modified;
                }
                ctx.SaveChanges();
                return artikal.Id;
            }
        }

        public void Obrisi(int idArtikla)
        {
            using (var ctx = new ModelContext())
            {
                ctx.Database.ExecuteSqlCommand("DELETE FROM Artikli WHERE Id={0}", idArtikla);
            }
        }

        public Artikal VratiArtikal(int id)
        {
            using (var ctx = new ModelContext())
            {
                return ctx.Artikli.SingleOrDefault(x => x.Id == id);
            }
        }

        public Artikal VratiArtikal(int id, Expression<Func<Artikal, object>> include)
        {
            using (var ctx = new ModelContext())
            {
                return ctx.Artikli.Include(include).Single(x => x.Id == id);
            }
        }

        public IEnumerable<Artikal> VratiSveArtikle()
        {
            using (var ctx = new ModelContext())
            {
                return ctx.Artikli.Include("Dobavljac").OrderBy(x => x.Naziv).ToArray();
            }
        }

        public IEnumerable<Artikal> NadjiArtikleUOkolini(int idKategorijeArtikla, double latituda, double longituda, int razdaljina)
        {
            using (var ctx = new ModelContext())
            {
                var point = GeoUtils.CreatePoint(latituda, longituda);
                return ctx.Artikli.Where(
                    x =>
                    x.Aktivan &&
                    x.IdSinonima == null &&
                    x.IdKategorijeArtikla == idKategorijeArtikla &&
                    x.Dobavljac.Status.PrikaziNaPretragama &&
                    x.Dobavljac.GpsLokacija.Distance(point) <= razdaljina).ToArray();
            }
        }

        public IList<Artikal> NadjiArtikleUOkolini(double latituda, double longituda, int razdaljina)
        {
            using (var ctx = new ModelContext())
            {
                var point = GeoUtils.CreatePoint(latituda, longituda);
                return ctx.Artikli.Where(
                    x =>
                    x.Aktivan &&
                    x.IdSinonima == null &&
                    x.Dobavljac.Status.PrikaziNaPretragama &&
                    x.Dobavljac.GpsLokacija.Distance(point) <= razdaljina).ToArray();
            }
        }

        public IEnumerable<Artikal> VratiArtikleBezSinonima()
        {
            using (var ctx = new ModelContext())
            {
                return ctx.Artikli.Include("KategorijaArtikla").Where(x => x.IdSinonima == null).OrderBy(x => x.Naziv).ToArray();
            }
        }

        public int SacuvajSinonim(Sinonim sinonim)
        {
            using (var ctx = new ModelContext())
            {
                ctx.Sinonimi.Add(sinonim);
                if (sinonim.Id != 0)
                {
                    ctx.Entry(sinonim).State = EntityState.Modified;
                }
                ctx.SaveChanges();
            }
            return sinonim.Id;
        }

        public IEnumerable<Sinonim> VratiSinonime()
        {
            using (var ctx = new ModelContext())
            {
                return ctx.Sinonimi.ToArray();
            }
        }

        public IEnumerable<Sinonim> NadjiSinonimeUOkolini(int idKategorijeArtikla, double latituda, double longituda, int razdaljina)
        {
            using (var ctx = new ModelContext())
            {
                var point = GeoUtils.CreatePoint(latituda, longituda);

                var artikli = ctx.Artikli.Include(x => x.Sinonim).Where(
                    x =>
                    x.IdSinonima.HasValue &&
                    x.Aktivan &&
                    x.IdKategorijeArtikla == idKategorijeArtikla &&
                    x.Dobavljac.Status.PrikaziNaPretragama &&
                    x.Dobavljac.GpsLokacija.Distance(point) <= razdaljina).ToLookup(x => x.IdSinonima);
                var sinonimi = new List<Sinonim>();
                foreach (var a in artikli)
                {
                    var s = artikli[a.Key].First().Sinonim;
                    var sinonim = new Sinonim
                                      {
                                          Id = s.Id,
                                          Naziv = s.Naziv,
                                          Opis = s.Opis,
                                          Slika = s.Slika,
                                          BrojArtikala = a.Count()
                                      };
                    sinonimi.Add(sinonim);
                }
                return sinonimi;
            }
        }

        public IList<Sinonim> NadjiSinonimeUOkolini(double latituda, double longituda, int razdaljina)
        {
            using (var ctx = new ModelContext())
            {
                var point = GeoUtils.CreatePoint(latituda, longituda);

                var artikli = ctx.Artikli.Include(x => x.Sinonim).Where(
                    x =>
                    x.IdSinonima.HasValue &&
                    x.Aktivan &&
                    x.Dobavljac.Status.PrikaziNaPretragama &&
                    x.Dobavljac.GpsLokacija.Distance(point) <= razdaljina).ToLookup(x => x.IdSinonima);
                var sinonimi = new List<Sinonim>();
                foreach (var a in artikli)
                {
                    var s = artikli[a.Key].First().Sinonim;
                    var sinonim = new Sinonim
                    {
                        Id = s.Id,
                        Naziv = s.Naziv,
                        Opis = s.Opis,
                        Slika = s.Slika,
                        BrojArtikala = a.Count()
                    };
                    sinonimi.Add(sinonim);
                }
                return sinonimi;
            }
        }

        public void DodeliSlikuSinonimu(int id, string slika)
        {
            using (var ctx = new ModelContext())
            {
                ctx.Sinonimi.Single(x => x.Id == id).Slika = slika;
                ctx.SaveChanges();
            }
        }

        public IEnumerable<Artikal> VratiArtikleSinonima(int idSinonima)
        {
            using (var ctx = new ModelContext())
            {
                return ctx.Artikli.Where(x => x.IdSinonima == idSinonima).ToArray();
            }
        }

        public IList<Artikal> VratiArtikleKategorije(int id)
        {
            using(var ctx = new ModelContext()) 
            {
                return ctx.Artikli.Where(x => x.IdKategorijeArtikla == id).ToList();
            }
        }

        

        public void ObrisiSinonim(int id)
        {
            using (var ctx = new ModelContext())
            {
                var artikli = ctx.Artikli.Where(x => x.IdSinonima == id);
                foreach (var artikal in artikli)
                {
                    artikal.IdSinonima = null;
                }
                ctx.SaveChanges();
                ctx.Database.ExecuteSqlCommand("DELETE FROM Sinonimi WHERE Id={0}", id);
            }
        }
    }
}