using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Logistyx.Bios.WebApp.Models
{
    public class ResourceForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a language code.")]
        [MaxLength(10, ErrorMessage = "The language code shouldn't have more than 10 characters.")]
        public string LanguageCode { get; set; }
        
        public string Translation { get; set; }
    }
}
