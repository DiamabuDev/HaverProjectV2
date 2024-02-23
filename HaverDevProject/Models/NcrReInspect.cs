using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("ncrReInspect")]
public class NcrReInspect : Auditable
{
    [Key]
    [Column("ncrReInspectId")]
    public int NcrReInspectId { get; set; }

    [Display(Name = "Re-Inspected Acceptable")]
    [Column("ncrReInspectAcceptable")]
    public bool NcrReInspectAcceptable { get; set; } = false;

    [Display(Name = "New NCR Number")]
    [Column("ncrReInspectNewNcrNumber")]
    public int? NcrReInspectNewNcrNumber { get; set; }

    [Display(Name = "Inspector's Name")]
    [Column("ncrReInspectUserId")]
    public int NcrReInspectUserId { get; set; }

    [ScaffoldColumn(false)]
    [Timestamp]
    public Byte[] RowVersion { get; set; }

    //public bool NcrQaStatusFlag { get; set; } //would this be needed?

    [Display(Name = "NCR")]
    [Column("ncrId")]
    public int NcrId { get; set; }

    [Display(Name = "NCR")]
    [Required(ErrorMessage = "You must provide the NCR.")]
    [ForeignKey("NcrId")]
    public Ncr Ncr { get; set; }

}
