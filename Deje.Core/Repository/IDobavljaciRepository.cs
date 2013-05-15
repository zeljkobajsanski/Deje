using System;
using System.Collections.Generic;
using Deje.Core.Model;

namespace Deje.Core.Repository
{
    public interface IDobavljaciRepository
    {
        IEnumerable<Dobavljac> VratiSve();
        int Save(Dobavljac dobavljac);
        Dobavljac VratiPoId(int id);
        void Delete(int id);
        IEnumerable<DobavljacSaRastojanjem> VratiDobavljaceUOkolini(double latitude, double longitude, double distance);
        IEnumerable<Dobavljac> VratiDobavljaceUOkolini(double latitude, double longitude, int distance, int idArtikla);
        void DodajSliku(SlikeDobavljaca slika);
        IEnumerable<SlikeDobavljaca> VratiSlikeDobavljaca(int idDobavljaca);
        void ObrisiSliku(Guid id);
        Dobavljac VratiDobavljacaSaPonudomIKontaktPodacima(int id);
        IEnumerable<Dobavljac> VratiDobavljaceUOkoliniSaSinonimom(double latitude, double longitude, int distance, int idSinonima);
    }
}