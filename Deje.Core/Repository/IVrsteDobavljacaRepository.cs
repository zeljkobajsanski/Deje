using System.Collections.Generic;
using Deje.Core.Model;

namespace Deje.Core.Repository
{
    public interface IVrsteDobavljacaRepository
    {
        int Save(VrstaDobavljaca vrstaDobavljaca);
        void Delete(int id);
        IEnumerable<VrstaDobavljaca> VratiZaDelatnost(int idDelatnosti);
    }
}