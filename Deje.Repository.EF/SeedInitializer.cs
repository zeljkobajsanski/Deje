using System.Data.Entity;
using Deje.Core.Model;
using Deje.Core.Utils;

namespace Deje.Repository.EF
{
    public class SeedInitializer : DropCreateDatabaseIfModelChanges<ModelContext>
    {
        protected override void Seed(ModelContext context)
        {
            var restorani = new Delatnost {Naziv = "Restoran"};
            context.Delatnosti.Add(restorani);
            context.SaveChanges();
            var pizzerije = new VrstaDobavljaca {Naziv = "Pizzerije", IdDelatnosti = restorani.Id};
            context.VrsteDobavljaca.Add(pizzerije);
            context.SaveChanges();
            var aktivan = new StatusDobavljaca {Naziv = "Aktivan", PrikaziNaPretragama = true};
            context.StatusiDobavljaca.Add(aktivan);
            context.SaveChanges();
            var pizza = new KategorijaArtikla() {Naziv = "Pizza"};
            context.KategorijeArtikala.Add(pizza);
            context.SaveChanges();
            var jail = new Dobavljac
                           {
                               Naziv = "Jail",
                               IdDelatnosti = restorani.Id,
                               IdVrsteDobavljaca = pizzerije.Id,
                               Kontakt = new Kontakt {Adresa = "Ađanska 50", Mesto = "24400 Senta"},
                               IdStatusa = aktivan.Id,
                               GpsLokacija = GeoUtils.CreatePoint(0, 0)
                           };
            context.Dobavljaci.Add(jail);
            context.SaveChanges();
        }
    }
}