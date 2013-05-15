using System.Collections.Generic;
using Deje.Windows.Model;

namespace Deje.Windows.Services.ImportArtikala
{
    public interface IImportArtiakalaService
    {
        IList<Artikal> Importuj(string url);
    }
}