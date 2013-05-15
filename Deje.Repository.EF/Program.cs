using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deje.Core.Model;
using Deje.Repository.EF.Repository;

namespace Deje.Repository.EF
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            var d = new Delatnost { Id = 1, Naziv = "Radionica" };
            var r = new DelatnostiRepository();
            r.Delete(d.Id);
        }
    }
}
