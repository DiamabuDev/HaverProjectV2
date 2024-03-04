using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

public class NcrReInspectMetaData
{
    [Key]
    [Column("ncrReInspectId")]
    public int NcrReInspectId { get; set; }

    [Display(Name = "Re-Inspected Acceptable")]
    [Column("ncrReInspectAcceptable")]
    public bool NcrReInspectAcceptable { get; set; } = false;

    [Display(Name = "New NCR Number")]
    [Column("ncrReInspectNewNcrNumber")]
    public string NcrReInspectNewNcrNumber { get; set; }

    //[Display(Name = "NCR Number")]
    //[Column("ncrNumber")]
    //public string NcrNumber { get; set; }

    [Display(Name = "Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime NcrReInspectCreationDate { get; set; }

    [Display(Name = "Inspector's Name")]
    [Column("ncrReInspectUserId")]
    public int NcrReInspectUserId { get; set; }

    public bool NcrQaStatusFlag { get; set; }

    [Display(Name = "Video Link")]
    [StringLength(100, ErrorMessage = "Video link cannot be more than 100 characters.")]
    public string NcrReInspectDefectVideo { get; set; }

    [Display(Name = "NCR")]
    [Required(ErrorMessage = "You must provide the NCR.")]
    [Column("ncrId")]
    public int NcrId { get; set; }

    [Display(Name = "NCR")]  
    [ForeignKey("NcrId")]
    public Ncr Ncr { get; set; }

    public ICollection<NcrReInspectPhoto> NcrReInspectPhotos { get; set; } = new HashSet<NcrReInspectPhoto>();

}
