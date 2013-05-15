using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Deje.Core.Model;

namespace Deje.Core.Repository
{
    public interface IArtikliRepository
    {
        IEnumerable<Artikal> VratiArtikleDobaljvaca(int idDobavljaca);
        int Sacuvaj(Artikal artikal);
        void Obrisi(int idArtikla);
        Artikal VratiArtikal(int id, Expression<Func<Artikal, object>> include);
        Artikal VratiArtikal(int id);
        IEnumerable<Artikal> VratiSveArtikle();
        IEnumerable<Artikal> NadjiArtikleUOkolini(int idKategorijeArtikla, double latituda, double longituda, int razdaljina);
        IList<Artikal> NadjiArtikleUOkolini(double latituda, double longituda, int razdaljina);
        IEnumerable<Artikal> VratiArtikleBezSinonima();
        int SacuvajSinonim(Sinonim sinonim);
        IEnumerable<Sinonim> VratiSinonime();
        IEnumerable<Sinonim> NadjiSinonimeUOkolini(int idKategorijeArtikla, double latituda, double longituda, int razdaljina);
        IList<Sinonim> NadjiSinonimeUOkolini(double latituda, double longituda, int razdaljina);
        void DodeliSlikuSinonimu(int id, string putanja);
        void ObrisiSinonim(int id);
        IEnumerable<Artikal> VratiArtikleSinonima(int idSinonima);
        IList<Artikal> VratiArtikleKategorije(int id);
        
    }
}