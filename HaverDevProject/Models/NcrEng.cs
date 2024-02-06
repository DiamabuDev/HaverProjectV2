using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("ncrEng")]
public partial class NcrEng : IValidatableObject
{
    [Key]
    [Column("ncrEngId")]
    public int NcrEngId { get; set; }

    [Display(Name = "Does Customer require notification of NCR")]
    [Column("ncrEngCustomerNotification")]
    public bool? NcrEngCustomerNotification { get; set; } = false;

    [Display(Name = "Disposition")]
    [Column("ncrEngDispositionDescription")]
    [StringLength(300)]
    [Unicode(false)]
    public string NcrEngDispositionDescription { get; set; }

    [Display(Name = "Date")]
    [Required(ErrorMessage = "You must provide the last date the NCR was updated.")]
    [Column("ncrEngLastUpdated", TypeName = "datetime")]
    [DataType(DataType.DateTime)]
    public DateTime NcrEngLastUpdated { get; set; }

    [Display(Name = "Creation Date")]
    [Required(ErrorMessage = "You must provide the date the NCR was created.")]
    [Column("ncrEngCreationDate", TypeName = "date")]
    public DateTime NcrEngCreationDate { get; set; }

    [Display(Name = "Engineering")]
    [Column("ncrEngUserId")]
    public int NcrEngUserId { get; set; }

    [Display(Name = "Disposition Type")]
    [Column("engDispositionTypeId")]
    public int EngDispositionTypeId { get; set; }

    [Display(Name = "NCR")]
    [Column("ncrId")]
    public int NcrId { get; set; }

    [Display(Name = "Drawings")]
    [InverseProperty("NcrEng")]
    public virtual ICollection<Drawing> Drawings { get; set; } = new List<Drawing>();

    [Display(Name = "Review by HBC Engineering")]
    [Required(ErrorMessage = "You must provide the Disposition Type.")]
    [ForeignKey("EngDispositionTypeId")]
    [InverseProperty("NcrEngs")]
    public virtual EngDispositionType EngDispositionType { get; set; }

    [Display(Name = "NCR")]
    [Required(ErrorMessage = "You must provide the NCR.")]
    [ForeignKey("NcrId")]
    [InverseProperty("NcrEngs")]
    public virtual Ncr Ncr { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (NcrEngLastUpdated < NcrEngCreationDate)
        {
            yield return new ValidationResult("The NCR cannot be updated before it was created.", new[] { "NcrEngLastUpdated" });
        }
    }
}
