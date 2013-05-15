using System.ComponentModel.DataAnnotations;

namespace Deje.Core.Model
{
    public class VrstaDobavljaca
    {
        public int Id { get; set; }
        [MaxLength(255, ErrorMessage = "Dužuina je ograničena na 255 karaktera")]
        [Required(ErrorMessage = "Naziv nije unet")]
        public string Naziv { get; set; }
        [MaxLength(255, ErrorMessage = "Dužuina je ograničena na 255 karaktera")]
        public string StraniNaziv { get; set; }

        [Required(ErrorMessage = "Delatnost nije izbarana")]
        public int? IdDelatnosti { get; set; }
        public Delatnost Delatnost { get; set; }
    }
}