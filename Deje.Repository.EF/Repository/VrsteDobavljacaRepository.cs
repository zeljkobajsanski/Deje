using System;
using System.Collections.Generic;
using System.Data;
using Deje.Core.Model;
using Deje.Core.Repository;
using System.Linq;

namespace Deje.Repository.EF.Repository
{
    public class VrsteDobavljacaRepository : IVrsteDobavljacaRepository
    {
        public int Save(VrstaDobavljaca vrstaDobavljaca)
        {
            using (var ctx = new ModelContext())
            {
                ctx.VrsteDobavljaca.Add(vrstaDobavljaca);
                if (vrstaDobavljaca.Id != 0)
                {
                    ctx.Entry(vrstaDobavljaca).State = EntityState.Modified;
                }
                ctx.SaveChanges();
                return vrstaDobavljaca.Id;
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new ModelContext())
            {
                if (ctx.Dobavljaci.Any(x => x.IdVrsteDobavljaca == id)) throw new Exception("Brisanje izabrane vrste nije dozvoljeno");
                ctx.Database.ExecuteSqlCommand("DELETE FROM VrsteDobavljaca WHERE Id={0}", id);
            }
        }

        public IEnumerable<VrstaDobavljaca> VratiZaDelatnost(int idDelatnosti)
        {
            using (var ctx = new ModelContext())
            {
                return ctx.VrsteDobavljaca.Where(x => x.IdDelatnosti == idDelatnosti).OrderBy(x => x.Naziv).ToArray();
            }
        }
    }
}