using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("defect")]
public partial class Defect
{
    [Key]
    [Column("defectId")]
    public int DefectId { get; set; }

    [Display(Name = "Defect Name")]
    [Required(ErrorMessage = "The Defect Name field is required.")]
    [Column("defectName")]
    [StringLength(45)]
    [Unicode(false)]
    public string DefectName { get; set; }

    [Display(Name = "Description")]
    [Column("defectDesription")]
    [StringLength(300)]
    [Unicode(false)]
    public string DefectDesription { get; set; }

    [InverseProperty("Defect")]
    public virtual ICollection<ItemDefect> ItemDefects { get; set; } = new List<ItemDefect>();
}
