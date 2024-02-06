using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("statusUpdate")]
public partial class StatusUpdate
{
    [Key]
    [Column("statusUpdateId")]
    public int StatusUpdateId { get; set; }

    [Display(Name = "Status Update")]
    [Required(ErrorMessage = "You must provide name of the Status Update.")]
    [Column("statusUpdateName")]
    [StringLength(45, ErrorMessage = "The Status Update cannot be more than 45 characters.")]
    [Unicode(false)]
    public string StatusUpdateName { get; set; }

    [Display(Name = "NCR")]
    [InverseProperty("StatusUpdate")]
    public virtual ICollection<Ncr> Ncrs { get; set; } = new List<Ncr>();
}
