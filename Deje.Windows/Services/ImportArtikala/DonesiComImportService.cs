using System;
using System.Collections.Generic;
using System.Linq;
using Deje.Windows.Model;
using HtmlAgilityPack;
using Deje.Windows.Utils;

namespace Deje.Windows.Services.ImportArtikala
{
    public class DonesiComImportService : IImportArtiakalaService
    {
        public IList<Artikal> Importuj(string url)
        {
            var html = new HtmlWeb().Load(url);
            var rootNode = html.DocumentNode;
            var foodListing =
                rootNode.Descendants("div").Single(
                    x => x.Attributes["id"] != null && x.Attributes["id"].Value == "food_listing");

            var categories = (from c in foodListing.Descendants("h3")
                              where c.Attributes["class"] != null && c.Attributes["class"].Value == "food"
                              select c).ToArray();
            var menustretchers =
                foodListing.Descendants("div").Where(
                    x => x.Attributes["class"] != null && x.Attributes["class"].Value == "menustretcher");
            var artikli = new List<Artikal>();
            for (int i = 0; i < categories.Length; i++)
            {
                var category = categories[i];
                var ms = menustretchers.ElementAt(i);
                var foodList = ms.ChildNodes.Where(x => x.Name == "div");
                foreach (var food in foodList)
                {
                    var artikal = new Artikal { Kategorija = category.InnerText.ConvertFromCyrilic() };
                    var opis = food.Descendants("div").SingleOrDefault(x => x.Attributes["class"] != null && x.Attributes["class"].Value == "ingr");
                    if (opis != null)
                    {
                        artikal.Opis = opis.InnerText.ConvertFromCyrilic();
                    }
                    var naziv = food.Descendants("span").SingleOrDefault(x => !string.IsNullOrEmpty(x.InnerText));
                    if (naziv != null)
                    {
                        artikal.Naziv = naziv.InnerText.ConvertFromCyrilic();
                    }
                    else continue;
                    var cenaNode = food.Descendants("dd").SingleOrDefault();
                    if (cenaNode != null)
                    {
                        var cena = cenaNode.InnerText.ConvertFromCyrilic();
                        var rsdIndex = cena.IndexOf(" RSD");
                        artikal.Cena = Decimal.Parse(cena.Substring(0, rsdIndex));
                    }
                   artikli.Add(artikal);
                }
            }
            return artikli;
        }
    }
}