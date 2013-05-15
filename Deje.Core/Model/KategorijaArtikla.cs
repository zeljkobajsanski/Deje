using System.ComponentModel.DataAnnotations;

namespace Deje.Core.Model
{
    public class KategorijaArtikla
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv nije unet")]
        [MaxLength(100, ErrorMessage = "Naziv je predugačak")]
        public string Naziv { get; set; }

        public int BrojArtikala { get; set; }
    }
}