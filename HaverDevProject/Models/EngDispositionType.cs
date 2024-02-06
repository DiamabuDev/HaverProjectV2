﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("engDispositionType")]
public partial class EngDispositionType
{
    [Key]
    [Column("engDispositionTypeId")]
    public int EngDispositionTypeId { get; set; }

    [Display(Name = "Review by HBC Engineering")]
    [Required]
    [Column("engDispositionTypeName")]
    [StringLength(45)]
    [Unicode(false)]
    public string EngDispositionTypeName { get; set; }

    [InverseProperty("EngDispositionType")]
    public virtual ICollection<NcrEng> NcrEngs { get; set; } = new List<NcrEng>();
}
