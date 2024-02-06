using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("ncrQA")]
public partial class NcrQa : IValidatableObject
{
    [Key]
    [Column("ncrQAId")]
    public int NcrQaid { get; set; }

    [Display(Name = "Item marked Nonconforming")]
    [Column("ncrQAItemMarNonConforming")]
    public bool NcrQaitemMarNonConforming { get; set; } = false;

    [Display(Name = "Sales Order No.")]
    [Required(ErrorMessage = "You must provide the Sales Order Number.")]
    [Column("ncrQASalesOrder")]
    [StringLength(45)]
    [Unicode(false)]
    public string NcrQasalesOrder { get; set; }

    [Display(Name = "Creation Date")]
    [Required(ErrorMessage = "You must provide the date the NCR was created.")]
    [Column("ncrQACreationDate", TypeName = "date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime NcrQacreationDate { get; set; }

    [Display(Name = "Date")]
    [Required(ErrorMessage = "You must provide the last date the NCR was updated.")]
    [Column("ncrQALastUpdated", TypeName = "datetime")]
    [DataType(DataType.DateTime)]
    public DateTime NcrQalastUpdated { get; set; }

    [Display(Name = "Quality Representative's Name")]
    [Column("ncrQAUserId")]
    public int NcrQauserId { get; set; }

    [Display(Name = "Identify Process Applicable")]
    [Column("proAppId")]
    public int ProAppId { get; set; }

    [Display(Name = "NCR")]
    [Column("ncrId")]
    public int NcrId { get; set; }

    [Display(Name = "NCR")]
    [Required(ErrorMessage = "You must provide the NCR.")]
    [ForeignKey("NcrId")]
    [InverseProperty("NcrQas")]
    public virtual Ncr Ncr { get; set; }

    [Display(Name = "PO or Prod. No.")]
    [InverseProperty("NcrQa")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [Display(Name = "Identify Process Applicable")]
    [Required(ErrorMessage = "You must provide the Applicable Process.")]
    [ForeignKey("ProAppId")]
    [InverseProperty("NcrQas")]
    public virtual ProcessApplicable ProApp { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (NcrQalastUpdated < NcrQacreationDate)
        {
            yield return new ValidationResult("The NCR cannot be updated before it was created.", new[] { "NcrQalastUpdated" });
        }
    }
}
