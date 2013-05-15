using System.ComponentModel.DataAnnotations;

namespace Deje.Core.Model
{
    public class Kontakt
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Adresa nije uneta")]
        [MaxLength(100, ErrorMessage = "Adresa je predugačka")]
        public string Adresa { get; set; }

        [Required(ErrorMessage = "Mesto nije uneto")]
        [MaxLength(100, ErrorMessage = "Mesto je predugačko")]
        public string Mesto { get; set; }

        [MaxLength(100, ErrorMessage = "Fiksni telefon je predugačak")]
        public string FiksniTelefon { get; set; }

        [MaxLength(100, ErrorMessage = "Mobilni telefon je predugačak")]
        public string MobilniTelefon { get; set; }

        [MaxLength(100, ErrorMessage = "E-mail je predugačak")]
        public string EMail { get; set; }

        [MaxLength(100, ErrorMessage = "Web adresa je predugačka")]
        public string Www { get; set; }
    }
}