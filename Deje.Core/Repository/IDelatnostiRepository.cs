using System.Collections.Generic;
using Deje.Core.Model;

namespace Deje.Core.Repository
{
    public interface IDelatnostiRepository
    {
        void Save(Delatnost delatnost);
        void Delete(int idDelatnosti);
        IEnumerable<Delatnost> VratiSve();
    }
}