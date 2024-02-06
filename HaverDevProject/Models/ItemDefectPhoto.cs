using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("itemDefectPhoto")]
public partial class ItemDefectPhoto
{
    [Key]
    [Column("itemDefectPhotoId")]
    public int ItemDefectPhotoId { get; set; }

    [Required]
    [Column("itemDefectPhotoContent")]
    public byte[] ItemDefectPhotoContent { get; set; }

    [Required]
    [Column("itemDefectPhotoMimeType")]
    [StringLength(45)]
    [Unicode(false)]
    public string ItemDefectPhotoMimeType { get; set; }

    [Column("itemDefectPhotoDescription")]
    [StringLength(300)]
    [Unicode(false)]
    public string ItemDefectPhotoDescription { get; set; }

    [Column("itemDefectId")]
    public int ItemDefectId { get; set; }

    [ForeignKey("ItemDefectId")]
    [InverseProperty("ItemDefectPhotos")]
    public virtual ItemDefect ItemDefect { get; set; }
}
