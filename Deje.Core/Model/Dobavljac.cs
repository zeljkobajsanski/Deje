using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deje.Core.Utils;

namespace Deje.Core.Model
{
    public class Dobavljac : Entity
    {
        private string m_Opis;

        public Dobavljac()
        {
            Zoom = 13;
            //Kontakt = new Kontakt();
        }

        public int Id { get; set; }
        
        [Required(ErrorMessage = "Naziv nije unet")]
        [MaxLength(255, ErrorMessage = "Naziv je predugačak")]
        public string Naziv { get; set; }
        
        [Required(ErrorMessage = "Delatnost nije izabrana")]
        public int? IdDelatnosti { get; set; }
        
        public Delatnost Delatnost { get; set; }

        [Required(ErrorMessage = "Unesite vrstu dobavljača")]
        public int? IdVrsteDobavljaca { get; set; }

        public VrstaDobavljaca VrstaDobavljaca { get; set; }
        
        public DbGeography GpsLokacija { get; set; }
        
        public double GpsLatitude
        {
            get { return GpsLokacija != null && GpsLokacija.Latitude.HasValue ? GpsLokacija.Latitude.Value : 0; }
            set { GpsLokacija = GeoUtils.CreatePoint(value, GpsLongitude); }
        }

        public double GpsLongitude
        {
            get { return GpsLokacija != null && GpsLokacija.Longitude.HasValue ? GpsLokacija.Longitude.Value : 0; }
            set { GpsLokacija = GeoUtils.CreatePoint(GpsLatitude, value); }
        }

        public int IdKontakta { get; set; }
        
        public Kontakt Kontakt { get; set; }
        
        [MaxLength(1000, ErrorMessage = "Opis je predugačak")]
        public string Opis
        {
            get { return m_Opis; }
            set
            {
                if (Opis != value)
                {
                    m_Opis = value;
                    OnPropertyChanged("Opis");
                }
            }
        }

        public int Zoom { get; set; }

        public string Slika { get; set; }

        public byte[] SlikaRaw { get; set; }

        [Required(ErrorMessage = "Status nije postavljen")]
        public int? IdStatusa { get; set; }

        public StatusDobavljaca Status { get; set; }

        public double? Udaljenost { get; set; }

        public IList<Artikal> Ponuda { get; set; }
    }
}
