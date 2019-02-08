using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Logistyx.Bios.WebApp.Models
{
    public class ResourceForCreationDto : ResourceForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a key.")]
        [MaxLength(100, ErrorMessage = "The key shouldn't have more than 200 characters.")]
        public string Key { get; set; }
    }
}
