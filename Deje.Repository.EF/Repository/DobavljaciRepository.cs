using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Deje.Core.Model;
using Deje.Core.Repository;
using System.Linq;
using Deje.Core.Utils;

namespace Deje.Repository.EF.Repository
{
    public class DobavljaciRepository : IDobavljaciRepository
    {
        public IEnumerable<Dobavljac> VratiSve()
        {
            using (var ctx = new ModelContext())
            {
                return ctx.Dobavljaci.Include("Kontakt").Include("Status").OrderBy(x => x.Naziv).ToArray();
            }
        }

        public int Save(Dobavljac dobavljac)
        {
            using (var ctx = new ModelContext())
            {
                ctx.Dobavljaci.Add(dobavljac);
                var kontakt = ctx.Entry(dobavljac.Kontakt);
                if (dobavljac.Id != 0)
                {
                    ctx.Entry(dobavljac).State = EntityState.Modified;
                }
                if (dobavljac.Kontakt.Id == 0)
                {
                    kontakt.State = EntityState.Added;
                } else
                {
                    kontakt.State = EntityState.Modified;
                }
                ctx.SaveChanges();
                return dobavljac.Id;
            }
            
        }

        public Dobavljac VratiPoId(int id)
        {
            using (var ctx = new ModelContext())
            {
                return ctx.Dobavljaci.Include("VrstaDobavljaca").Include("Kontakt").Single(x => x.Id == id);
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new ModelContext())
            {
                ctx.Database.ExecuteSqlCommand("DELETE FROM Dobavljaci WHERE Id={0}", id);
            }
        }

        public IEnumerable<DobavljacSaRastojanjem> VratiDobavljaceUOkolini(double latitude, double longitude, double distance)
        {
            var target = GeoUtils.CreatePoint(latitude, longitude);
            using (var ctx = new ModelContext())
            {
                return ctx.Dobavljaci.Where(x => x.Status.PrikaziNaPretragama && x.GpsLokacija.Distance(target) <= distance)
                    .Select(x => new DobavljacSaRastojanjem()
                                     {
                                         Naziv = x.Naziv, 
                                         Latituda = x.GpsLokacija.Latitude ?? 0, 
                                         Longituda = x.GpsLokacija.Longitude ?? 0, 
                                         Rastojanje = Math.Round(x.GpsLokacija.Distance(target) ?? 0, 0)
                                     }).ToArray();
            }
        }

        public IEnumerable<Dobavljac> VratiDobavljaceUOkolini(double latitude, double longitude, int distance, int idArtikla)
        {
            using (var ctx = new ModelContext())
            {
                var point = GeoUtils.CreatePoint(latitude, longitude);
                var dobavljaci = ctx.Artikli.Where(x => x.Id == idArtikla && x.Dobavljac.Status.PrikaziNaPretragama && x.Dobavljac.GpsLokacija.Distance(point) <= distance).Select(
                    x => x.Dobavljac).Include("VrstaDobavljaca").ToArray();
                foreach (var dobavljac in dobavljaci)
                {
                    var udaljenost = dobavljac.GpsLokacija.Distance(point);
                    if (udaljenost != null)
                    {
                        dobavljac.Udaljenost = Math.Round(udaljenost.Value, 0);
                    }
                }
                return dobavljaci;
            }
        }

        public void DodajSliku(SlikeDobavljaca slika)
        {
            using (var ctx = new ModelContext())
            {
                ctx.SlikeDobavljaca.Add(slika);
                ctx.SaveChanges();
            }
        }

        public IEnumerable<SlikeDobavljaca> VratiSlikeDobavljaca(int idDobavljaca)
        {
            using (var ctx = new ModelContext())
            {
                return ctx.SlikeDobavljaca.Where(x => x.IdDobavljaca == idDobavljaca).ToArray();
            }
        }

        public void ObrisiSliku(Guid id)
        {
            using (var ctx = new ModelContext())
            {
                var slika = ctx.SlikeDobavljaca.Single(x => x.Id == id);
                ctx.SlikeDobavljaca.Remove(slika);
                ctx.SaveChanges();
            }
        }

        public Dobavljac VratiDobavljacaSaPonudomIKontaktPodacima(int id)
        {
            using (var ctx = new ModelContext())
            {
                return ctx.Dobavljaci.Include("VrstaDobavljaca")
                                     .Include("Ponuda")
                                     .Include("Ponuda.KategorijaArtikla")
                                     .Include("Kontakt").Single(x => x.Id == id);
            }
        }

        public IEnumerable<Dobavljac> VratiDobavljaceUOkoliniSaSinonimom(double latitude, double longitude, int distance, int idSinonima)
        {
            using (var ctx = new ModelContext())
            {
                var point = GeoUtils.CreatePoint(latitude, longitude);
                var dobavljaci = ctx.Artikli.Where(x => x.IdSinonima == idSinonima && x.Dobavljac.Status.PrikaziNaPretragama && x.Dobavljac.GpsLokacija.Distance(point) <= distance).Select(
                    x => x.Dobavljac).Include("VrstaDobavljaca").ToArray();
                foreach (var dobavljac in dobavljaci)
                {
                    var udaljenost = dobavljac.GpsLokacija.Distance(point);
                    if (udaljenost != null)
                    {
                        dobavljac.Udaljenost = Math.Round(udaljenost.Value, 0);
                    }
                }
                return dobavljaci;
            }
        }
    }
}