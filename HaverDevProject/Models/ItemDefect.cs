using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("itemDefect")]
public class ItemDefect
{    
    [Display(Name = "Item")]
    [Column("itemId")]
    public int ItemId { get; set; }   
    [InverseProperty("ItemDefects")]
    public Item Item { get; set; }

    [Display(Name = "Defect")]
    [Column("defectId")]
    public int DefectId { get; set; }      
    [InverseProperty("ItemDefects")]
    public Defect Defect { get; set; }       
}
