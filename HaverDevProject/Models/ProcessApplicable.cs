using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("processApplicable")]
public partial class ProcessApplicable
{
    [Key]
    [Column("proAppId")]
    public int ProAppId { get; set; }

    [Display(Name = "Identify Process Applicable")]
    [Required(ErrorMessage = "You must provide name of the Applicable Process.")]
    [Column("proAppName")]
    [StringLength(45, ErrorMessage = "The Applicable Process cannot be more than 45 characters.")]
    [Unicode(false)]
    public string ProAppName { get; set; }

    [Display(Name = "Quality Inspector")]
    [InverseProperty("ProApp")]
    public virtual ICollection<NcrQa> NcrQas { get; set; } = new List<NcrQa>();
}
