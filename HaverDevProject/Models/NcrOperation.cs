using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("ncrOperation")]
public partial class NcrOperation : Auditable
{
    [Key]
    [Column("ncrPurchId")]
    public int NcrPurchId { get; set; }

    [Column("ncrPurchasingDescription")]
    [StringLength(300)]
    [Display(Name = "Description")]
    [DataType(DataType.MultilineText)] 
    [Unicode(false)]
    public string NcrPurchasingDescription { get; set; }

    [Display(Name = "Operations Manager")]
    [Column("ncrPurchasingUserId")]
    public int NcrPurchasingUserId { get; set; }

    [Display(Name = "Purchasing's Preliminary Decision")]
    [Column("opDispositionTypeId")]
    public int OpDispositionTypeId { get; set; }

    [Display(Name = "NCR")]
    [Column("ncrId")]
    public int NcrId { get; set; }

    [Display(Name = "Was a CAR raised")]
    [InverseProperty("NcrPurch")]
    public virtual Car Car { get; set; }

    [Display(Name = "Follow-up Required")]
    [InverseProperty("NcrPurch")]
    public virtual FollowUp FollowUp { get; set; }

    [Display(Name = "NCR")]
    [Required(ErrorMessage = "You must provide the NCR.")]
    [ForeignKey("NcrId")]
    public virtual Ncr Ncr { get; set; }

    [Display(Name = "Purchasing's Preliminary Decision")]
    [Required(ErrorMessage = "You must provide the Disposition Type.")]
    [ForeignKey("OpDispositionTypeId")]
    [InverseProperty("NcrOperations")]
    public virtual OpDispositionType OpDispositionType { get; set; }
}
