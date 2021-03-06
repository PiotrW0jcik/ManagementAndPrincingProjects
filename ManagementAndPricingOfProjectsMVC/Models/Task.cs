﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAndPricingOfProjectsMVC.Models
{
   
    public class Task
    {
        [Key]
        public int TaskId { get; set; }
        public int ProjectID { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Required]
        public string TaskName { get; set; }
        [Required]
        public string Technology { get; set; }
        [Required]
        public string  Role { get; set; }
        [Required]
        public int Hours { get; set; }
        [Required]
        public int PricePerHour { get; set; }
        public int sum { get; set; }

        public virtual Project Project { get; set; }

    }
}
