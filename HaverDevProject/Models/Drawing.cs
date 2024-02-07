using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("drawing")]
public partial class Drawing
{
    [Key]
    [Column("drawingId")]
    public int DrawingId { get; set; }

    [Display(Name = "Original Rev. Number")]
    [Column("drawingOriginalRevNumber")]
    public int DrawingOriginalRevNumber { get; set; }

    [Display(Name = "Updated Rev. Number")]
    [Column("drawingUpdatedRevNumber")]
    public int DrawingUpdatedRevNumber { get; set; }

    [Display(Name = "Revision Date")]
    [Column("drawingRevDate", TypeName = "date")]
    public DateTime DrawingRevDate { get; set; }

    [Display(Name = "Drawing User ID")]
    [Column("drawingUserId")]
    public int DrawingUserId { get; set; }

    [Display(Name = "Name of Engineer")]
    [Column("ncrEngId")]
    public int NcrEngId { get; set; }

    [Display(Name = "Engineering")]
    [ForeignKey("NcrEngId")]
    public virtual NcrEng NcrEng { get; set; }
}
