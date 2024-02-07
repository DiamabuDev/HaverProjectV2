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
    [StringLength(300, ErrorMessage = "Only 300 characters for photo description.")]
    [DataType(DataType.MultilineText)]
    [Unicode(false)]
    public string ItemDefectPhotoDescription { get; set; }

    [Column("ncrQaId")]
    public int NcrQaId { get; set; }

    [ForeignKey("NcrQaId")]
    [InverseProperty("ItemDefectPhotos")]
    public virtual NcrQa NcrQa { get; set; }
}
