using System;
using System.ComponentModel.DataAnnotations;

namespace Deje.Core.Model
{
    public class Artikal : Entity
    {
        private byte[] m_SlikaRaw;
        public int Id { get; set; }
        
        public KategorijaArtikla KategorijaArtikla { get; set; }
        
        [Required(ErrorMessage = "Kategorija artikla nije izabrana")]
        public int? IdKategorijeArtikla { get; set; }

        [Required(ErrorMessage = "Naziv nije unet")]
        [MaxLength(255, ErrorMessage = "Naziv je predugačak")]
        public string Naziv { get; set; }

        public int? IdSinonima { get; set; }

        public Sinonim Sinonim { get; set; }

        public Dobavljac Dobavljac { get; set; }

        [Required(ErrorMessage = "Dobavljač nije odabran")]
        public int? IdDobavljaca { get; set; }

        public string Slika { get; set; }

        public byte[] SlikaRaw
        {
            get { return m_SlikaRaw; }
            set
            {
                if (SlikaRaw != value)
                {
                    m_SlikaRaw = value;
                    OnPropertyChanged("SlikaRaw");
                }
                
            }
        }

        [MaxLength(2000, ErrorMessage = "Opis je predugačak")]
        public string Opis { get; set; }

        [Range(0.00, Double.MaxValue, ErrorMessage = "Vrednost nije dozvoljena")]
        public decimal Cena { get; set; }

        public bool Aktivan { get; set; }

        public float Score { get; set; }
    }
}