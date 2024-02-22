using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("ncrEng")]
public partial class NcrEng : Auditable, IAuditable
{
    [Key]
    [Column("ncrEngId")]
    public int NcrEngId { get; set; }

    [Display(Name = "Check the box if customer requires notification of NCR (if checked, raise message to customer detailing issues)")]
    [Column("ncrEngCustomerNotification")]
    public bool NcrEngCustomerNotification { get; set; } = false;

    [Display(Name = "Disposition (Sequence of work steps required when 'repair' or 'rework' indicated)")]
    [Column("ncrEngDispositionDescription")]
    [StringLength(300, ErrorMessage = "Only 300 characters for disposition description.")]
    [DataType(DataType.MultilineText)]
    [Unicode(false)]
    public string NcrEngDispositionDescription { get; set; }

    [ScaffoldColumn(false)]
    [Timestamp]
    public Byte[] RowVersion { get; set; }

    [Display(Name = "Engineering")]
    [Column("ncrEngUserId")]
    public int NcrEngUserId { get; set; }

    [Display(Name = "Disposition Type")]
    [Column("engDispositionTypeId")]
    public int EngDispositionTypeId { get; set; }

    [Display(Name = "Review by HBC Engineering: (indicate disposition by 'checking' one of the following)")]
    [Required(ErrorMessage = "You must provide the Disposition Type.")]
    [ForeignKey("EngDispositionTypeId")]
    [InverseProperty("NcrEngs")]
    public virtual EngDispositionType EngDispositionType { get; set; }

    [Display(Name = "NCR")]
    [Column("ncrId")]
    public int NcrId { get; set; }

    [Display(Name = "NCR")]
    [Required(ErrorMessage = "You must provide the NCR.")]
    [ForeignKey("NcrId")]
    public virtual Ncr Ncr { get; set; }

    [Display(Name = "Drawings")]
    [InverseProperty("NcrEng")]
    public virtual Drawing Drawing { get; set; }
}
