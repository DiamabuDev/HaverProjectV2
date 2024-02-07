using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("car")]
public partial class Car
{
    [Key]
    [Column("carId")]
    public int CarId { get; set; }

    [Display(Name = "CAR #")]
    [Column("carNumber")]
    public int CarNumber { get; set; }

    [Display(Name = "Purchase ID")]
    [Column("ncrPurchId")]
    public int NcrPurchId { get; set; }

    [Display(Name = "Purchase ID")]
    [ForeignKey("NcrPurchId")]
    [InverseProperty("Car")]
    public virtual NcrPurchasing NcrPurch { get; set; }
}
