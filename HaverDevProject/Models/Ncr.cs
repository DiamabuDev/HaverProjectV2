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
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime NcrLastUpdated { get; set; } = DateTime.Now;

    [Display(Name = "Status")]
    [Column("ncrStatus")]
    public bool NcrStatus { get; set; } = true; //default value

    [Display(Name = "Engineer")]
    public virtual NcrEng NcrEng { get; set; }

    [Display(Name = "Purchasing")]
    public virtual NcrPurchasing NcrPurchasing { get; set; }

    [Display(Name = "Quality Representative")]
    public virtual NcrQa NcrQa { get; set; }

    [Display(Name = "Re-Inspector")]
    public virtual NcrReInspect NcrReInspect { get; set; }    
}
