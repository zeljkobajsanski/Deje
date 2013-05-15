using System;
using System.Collections.Generic;
using System.Data;
using Deje.Core.Model;
using Deje.Core.Repository;
using System.Linq;

namespace Deje.Repository.EF.Repository
{
    public class DelatnostiRepository : IDelatnostiRepository
    {
        public void Save(Delatnost delatnost)
        {
            using (var ctx = new ModelContext())
            {
                ctx.Delatnosti.Add(delatnost);
                if (delatnost.Id != 0)
                {
                    ctx.Entry(delatnost).State = EntityState.Modified;
                }
                ctx.SaveChanges();
            }
        }

        public void Delete(int idDelatnosti)
        {
            using (var ctx = new ModelContext())
            {
                if (ctx.Dobavljaci.Any(x => x.IdDelatnosti == idDelatnosti))
                {
                    throw new Exception("Brisanje izabrane delatnosti nije dozvoljeno");
                }
                ctx.Database.ExecuteSqlCommand("DELETE FROM Delatnosti WHERE Id={0}", idDelatnosti);
            }
        }

        public IEnumerable<Delatnost> VratiSve()
        {
            using (var ctx = new ModelContext())
            {
                return ctx.Delatnosti.OrderBy(x => x.Naziv).ToArray();
            }
        }
    }
}