using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

public partial class ItemDefectPhoto
{
    [Key]
    [Column("reInspectPhotoId")]
    public int ReInspectPhotoId { get; set; }

    [Required]
    [Column("reInspectPhotoContent")]
    public byte[] ReInspectPhotoContent { get; set; }

    [Required]
    [Column("reInspectPhotoMimeType")]
    [StringLength(45)]
    [Unicode(false)]
    public string ReInspectPhotoMimeType { get; set; }

    [Column("reInspectPhotoDescription")]
    [StringLength(300)]
    [Unicode(false)]
    public string ReInspectPhotoDescription { get; set; }

    [Column("reInspectId")]
    public int ReInspectId { get; set; }
}