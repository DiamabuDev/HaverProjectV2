using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("followUpType")]
public partial class FollowUpType
{
    [Key]
    [Column("followUpTypeId")]
    public int FollowUpTypeId { get; set; }

    [Display(Name = "Follow Up Type")]
    [Required]
    [Column("followUpTypeName")]
    [StringLength(45)]
    [Unicode(false)]
    public string FollowUpTypeName { get; set; }

    [InverseProperty("FollowUpType")]
    public virtual ICollection<FollowUp> FollowUps { get; set; } = new List<FollowUp>();
}
