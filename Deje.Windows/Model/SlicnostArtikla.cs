namespace Deje.Windows.Model
{
    public class SlicnostArtikla : Core.Model.Artikal
    {
        public SlicnostArtikla(Core.Model.Artikal artikal)
        {
            Id = artikal.Id;
            IdKategorijeArtikla = artikal.IdKategorijeArtikla;
            Naziv = artikal.Naziv;
            IdSinonima = artikal.IdSinonima;
            Slika = artikal.Slika;
            Opis = artikal.Opis;
            Aktivan = artikal.Aktivan;
            IdDobavljaca = artikal.IdDobavljaca;
            Cena = artikal.Cena;
        }

        public double Slicnost { get; set; }
    }
}