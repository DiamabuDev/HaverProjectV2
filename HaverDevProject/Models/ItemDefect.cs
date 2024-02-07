using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("itemDefect")]
public partial class ItemDefect
{
    [Key]
    [Column("itemDefectId")]
    public int ItemDefectId { get; set; }

    [Column("itemId")]
    public int ItemId { get; set; }

    [Column("defectId")]
    public int DefectId { get; set; }

    [ForeignKey("DefectId")]
    [InverseProperty("ItemDefects")]
    public virtual Defect Defect { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ItemDefects")]
    public virtual Item Item { get; set; }

    [InverseProperty("ItemDefect")]
    public virtual ICollection<NcrQa> NcrQas { get; set; } = new List<NcrQa>();
    

    //[InverseProperty("ItemDefect")]
    //public virtual ICollection<ItemDefectPhoto> ItemDefectPhotos { get; set; } = new List<ItemDefectPhoto>();

    //[InverseProperty("ItemDefect")]
    //public virtual ICollection<ItemDefectVideo> ItemDefectVideos { get; set; } = new List<ItemDefectVideo>();
}
