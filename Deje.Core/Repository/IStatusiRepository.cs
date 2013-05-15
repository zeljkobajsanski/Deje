using System.Collections.Generic;
using Deje.Core.Model;

namespace Deje.Core.Repository
{
    public interface IStatusiRepository
    {
        IEnumerable<StatusDobavljaca> VratiStatuseDobavljaca();
        int Save(StatusDobavljaca statusDobavljaca);
    }
}