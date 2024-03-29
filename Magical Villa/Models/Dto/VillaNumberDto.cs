﻿using System.ComponentModel.DataAnnotations;

namespace MagicalVilla_API.Models.Dto
{
    public class VillaNumberDto
    {
        [Required]
        public string VillaNo { get; set; }
        [Required]
        public int VillaID { get; set; }
        public string SpecialDetails { get; set; }

        public VillaDto Villa { get; set; }
    }
}