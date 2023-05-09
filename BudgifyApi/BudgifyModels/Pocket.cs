﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels
{
    internal class Pocket
    {
        [Key]
        [MaxLength(10)]
        public int pocket_id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? name { get; set; }
        [Required]
        [MaxLength(8)]
        public double total { get; set; }
        [Required]
        [MaxLength(100)]
        public string? icon { get; set; }
        [Required]
        [MaxLength(8)]
        public double goal { get; set; }
    }
}