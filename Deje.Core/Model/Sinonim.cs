using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Deje.Core.Model
{
    public class Sinonim : Entity
    {
        private byte[] m_SlikaRaw;
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv nije unet")]
        [MaxLength(255, ErrorMessage = "Naziv je predugačak")]
        public string Naziv { get; set; }

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

        public IList<Artikal> Artikli { get; set; }

        public int BrojArtikala { get; set; }

        public float Score { get; set; }
    }
}