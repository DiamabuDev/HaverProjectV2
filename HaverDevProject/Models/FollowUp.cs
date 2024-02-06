using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("followUp")]
public partial class FollowUp
{
    [Key]
    [Column("followUpId")]
    public int FollowUpId { get; set; }

    [Display(Name = "Expected Date")]
    [Column("followUpExpectedDate", TypeName = "date")]
    public DateTime FollowUpExpectedDate { get; set; }

    [Display(Name = "Follow-up Type")]
    [Column("followUpTypeId")]
    public int FollowUpTypeId { get; set; }

    [Display(Name = "Operations Manager")]
    [Column("ncrPurchId")]
    public int NcrPurchId { get; set; }

    [Display(Name = "Follow-up Type")]
    [ForeignKey("FollowUpTypeId")]
    [InverseProperty("FollowUps")]
    public virtual FollowUpType FollowUpType { get; set; }

    [Display(Name = "Operations Manager")]
    [ForeignKey("NcrPurchId")]
    [InverseProperty("FollowUps")]
    public virtual NcrPurchasing NcrPurch { get; set; }
}
