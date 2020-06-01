using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace ManagementAndPricingOfProjectsMVC.Models
{
    public class Project
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ProjectId { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string Description { get; set; }
        
        public int PriceForProject { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
