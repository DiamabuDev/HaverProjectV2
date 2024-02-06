using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("orderDetail")]
public partial class OrderDetail
{
    [Key]
    [Column("orderId")]
    public int OrderId { get; set; }

    [Display(Name = "PO or Prod. No.")]
    [Required(ErrorMessage = "You must provide the Order Number.")]
    [Column("orderNumber")]
    public int OrderNumber { get; set; }

    [Display(Name = "Quantity Received")]
    [Required(ErrorMessage = "You must provide the Quantity Received.")]
    [Column("orderQuanReceived")]
    public int OrderQuanReceived { get; set; }

    [Display(Name = "Quantity Defective")]
    [Required(ErrorMessage = "You must provide the Defective Quantity.")]
    [Column("orderQuanDefective")]
    public int OrderQuanDefective { get; set; }

    [Display(Name = "Item")]
    [Column("itemId")]
    public int ItemId { get; set; }

    [Display(Name = "Quality Inspector")]
    [Column("ncrQAId")]
    public int NcrQaid { get; set; }

    [Display(Name = "Item")]
    [Required(ErrorMessage = "You must provide an Item.")]
    [ForeignKey("ItemId")]
    [InverseProperty("OrderDetails")]
    public virtual Item Item { get; set; }

    [Display(Name = "Quality Inspector")]
    [Required(ErrorMessage = "You must provide the Disposition Type.")]
    [ForeignKey("NcrQaid")]
    [InverseProperty("OrderDetails")]
    public virtual NcrQa NcrQa { get; set; }
}
