using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("item")]
public partial class Item
{
    [Key]
    [Column("itemId")]
    public int ItemId { get; set; }

    [Column("itemNumber")]
    [Display(Name = "SAP No.")]
    public int ItemNumber { get; set; }

    [Required]
    [Display(Name = "Description")]
    [Column("itemName")]
    [StringLength(45)]
    [Unicode(false)]
    public string ItemName { get; set; }

    [Display(Name = "Description of Item (including SAP No.)")]
    [Column("itemDescription")]
    [StringLength(300)]
    [Unicode(false)]
    public string ItemDescription { get; set; }

    [Column("supplierId")]
    [Display(Name = "Supplier")]

    public int SupplierId { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<ItemDefect> ItemDefects { get; set; } = new List<ItemDefect>();

    //[InverseProperty("Item")]
    //public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Items")]
    public virtual Supplier Supplier { get; set; }
}
