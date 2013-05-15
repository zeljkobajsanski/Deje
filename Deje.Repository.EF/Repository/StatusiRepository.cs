using System.Collections.Generic;
using System.Data;
using Deje.Core.Model;
using Deje.Core.Repository;
using System.Linq;

namespace Deje.Repository.EF.Repository
{
    public class StatusiRepository : IStatusiRepository
    {
        public IEnumerable<StatusDobavljaca> VratiStatuseDobavljaca()
        {
            using (var ctx = new ModelContext())
            {
                return ctx.StatusiDobavljaca.OrderBy(x => x.Naziv).ToArray();
            }
        }

        public int Save(StatusDobavljaca statusDobavljaca)
        {
            using (var ctx = new ModelContext())
            {
                ctx.StatusiDobavljaca.Add(statusDobavljaca);
                if (statusDobavljaca.Id != 0)
                {
                    ctx.Entry(statusDobavljaca).State = EntityState.Modified;
                }
                ctx.SaveChanges();
                return statusDobavljaca.Id;
            }
        }
    }
}