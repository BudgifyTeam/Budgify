﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels
{
    public class Pocket
    {
        [Key]
        [MaxLength(10)]
        public int Pocket_id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
        [Required]
        [MaxLength(8)]
        public double Total { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Icon { get; set; }
        [Required]
        [MaxLength(8)]
        public double Goal { get; set; }
        //Relationship
        [ForeignKey("Users_id")]
        public user User { get; set; }
        public int Users_id { get; set; }
    }
}
