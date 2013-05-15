using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Deje.Core.Model
{
    public class StatusDobavljaca
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv je obavezan")]
        [MaxLength(50, ErrorMessage = "Naziv je predugačak")]
        public string Naziv { get; set; }
        [DefaultValue(true)]
        public bool PrikaziNaPretragama { get; set; }
    }
}