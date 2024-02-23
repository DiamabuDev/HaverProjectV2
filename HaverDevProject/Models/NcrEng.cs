using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HaverDevProject.Models;

[Table("ncrEng")]
public partial class NcrEng : Auditable
{
    [Key]
    [Column("ncrEngId")]
    public int NcrEngId { get; set; }

    [Display(Name = "Does Customer require notification of NCR")]
    [Column("ncrEngCustomerNotification")]
    public bool NcrEngCustomerNotification { get; set; } = false;

    [Display(Name = "Disposition")]
    [Column("ncrEngDispositionDescription")]
    [StringLength(300, ErrorMessage = "Only 300 characters for disposition description.")]
    [DataType(DataType.MultilineText)]
    [Unicode(false)]
    public string NcrEngDispositionDescription { get; set; }

    [Display(Name = "Engineering")]
    [Column("ncrEngUserId")]
    public int NcrEngUserId { get; set; }

    [Display(Name = "Disposition Type")]
    [Column("engDispositionTypeId")]
    public int EngDispositionTypeId { get; set; }

    [Display(Name = "Review by HBC Engineering")]
    [Required(ErrorMessage = "You must provide the Disposition Type.")]
    [ForeignKey("EngDispositionTypeId")]
    [InverseProperty("NcrEngs")]
    public virtual EngDispositionType EngDispositionType { get; set; }

    [Display(Name = "NCR")]
    [Column("ncrId")]
    public int NcrId { get; set; }

    [Display(Name = "NCR")]
    [Required(ErrorMessage = "You must provide the NCR.")]
    [Column("ncr")]
    [ForeignKey("NcrId")]
    public virtual Ncr Ncr { get; set; }

    [Display(Name = "Drawings")]
    [InverseProperty("NcrEng")]
    public virtual Drawing Drawing { get; set; }
}
