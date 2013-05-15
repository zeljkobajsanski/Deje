using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deje.Core.Model
{
    public class Delatnost
    {
        public int Id { get; set; }
        [MaxLength(255, ErrorMessage = "Dužuina je ograničena na 255 karaktera")]
        [Required(ErrorMessage = "Naziv nije unet")]
        public string Naziv { get; set; }
        [MaxLength(255, ErrorMessage = "Dužuina je ograničena na 255 karaktera")]
        public string StraniNaziv { get; set; }
    }
}
