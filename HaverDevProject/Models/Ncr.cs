using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("ncr")]
public partial class Ncr
{
    [Key]
    [Column("ncrId")]
    public int NcrId { get; set; }

    [Display(Name = "NCR No.")]
    [Required(ErrorMessage = "You must provide the NCR Number.")]
    [Column("ncrNumber")]
    [StringLength(10, ErrorMessage = "The NCR Number cannot be more than 10 characters long.")]
    [Unicode(false)]
    public string NcrNumber { get; set; }

    [Display(Name = "Last Updated")]
    [Required(ErrorMessage = "You must provide the last date the NCR was updated.")]
    [Column("ncrLastUpdated", TypeName = "datetime")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime NcrLastUpdated { get; set; }

    [Display(Name = "Status Update")]
    [Column("statusUpdateId")]
    public int StatusUpdateId { get; set; }

    [Display(Name = "Engineer")]
    [InverseProperty("Ncr")]
    public virtual ICollection<NcrEng> NcrEngs { get; set; } = new List<NcrEng>();

    [Display(Name = "Purchasing")]
    [InverseProperty("Ncr")]
    public virtual ICollection<NcrPurchasing> NcrPurchasings { get; set; } = new List<NcrPurchasing>();

    [Display(Name = "Quality Representative")]
    [InverseProperty("Ncr")]
    public virtual ICollection<NcrQa> NcrQas { get; set; } = new List<NcrQa>();

    [Display(Name = "Re-Inspector")]
    [InverseProperty("Ncr")]
    public virtual ICollection<NcrReInspect> NcrReInspects { get; set; } = new List<NcrReInspect>();

    [Display(Name = "Status Update")]
    [Required(ErrorMessage = "You must provide the Status.")]
    [ForeignKey("StatusUpdateId")]
    [InverseProperty("Ncrs")]
    public virtual StatusUpdate StatusUpdate { get; set; }
}
