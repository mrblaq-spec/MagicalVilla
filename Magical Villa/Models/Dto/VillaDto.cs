﻿using System.ComponentModel.DataAnnotations;

namespace Magical_Villa.Models.Dto
{
    public class VillaDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
        public int Occupancy { get; set; }
        public int SqFt { get; set; }

    }
}
