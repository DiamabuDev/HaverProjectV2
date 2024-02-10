using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("item")]
public class Item
{
    [Key]
    [Column("itemId")]
    public int ItemId { get; set; }

    [Column("itemNumber")]
    [Display(Name = "SAP No.")]
    [Required(ErrorMessage = "You must provide an item Number")]
    public int ItemNumber { get; set; }

    [Required(ErrorMessage = "You must provide the Item Name.")]
    [Display(Name = "Name")]
    [Column("itemName")]
    [StringLength(45, ErrorMessage = "The Item Name cannot be more than 45 characters long.")]
    [Unicode(false)]
    public string ItemName { get; set; }

    [Display(Name = "Description of Item (including SAP No.)")]
    [Column("itemDescription")]
    [StringLength(300, ErrorMessage = "The Item Description cannot be more than 300 characters long.")]
    [Unicode(false)]
    public string ItemDescription { get; set; }
    
    [Display(Name = "Supplier")]
    [Required(ErrorMessage = "You must select a Supplier")]
    [Column("supplierId")]
    public int SupplierId { get; set; }

    [ForeignKey("SupplierId")]
    [InverseProperty("Items")]
    public Supplier Supplier { get; set; }

    [InverseProperty("Item")]
    public ICollection<ItemDefect> ItemDefects { get; set; } = new HashSet<ItemDefect>();

    [InverseProperty("Item")]
    public ICollection<NcrQa> NcrQas { get; set; } = new HashSet<NcrQa>();
}
