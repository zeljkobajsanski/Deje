using System.Collections.Generic;
using Deje.Core.Model;

namespace Deje.Core.Repository
{
    public interface IKategorijeArtikalaRepository
    {
        IEnumerable<KategorijaArtikla> VratiSve();
        void Sacuvaj(KategorijaArtikla kategorijaArtikla);
        void Obrisi(int id);
        IEnumerable<KategorijaArtikla> VratiKategorijeArtikalaUOkolini(double latituda, double longituda,
                                                                       int udaljenost, int idDelatnosti);

        int Sacuvaj(string nazivKategorije);
    }
}