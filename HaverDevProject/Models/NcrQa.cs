﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaverDevProject.Data;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[Table("ncrQA")]
public class NcrQa : Auditable
{
    [Key]
    [Column("ncrQAId")]
    public int NcrQaId { get; set; }

    [Display(Name = "Item marked Nonconforming")]
    [Column("ncrQaItemMarNonConforming")]
    public bool NcrQaItemMarNonConforming { get; set; }

    [Display(Name = "Identify Process Applicable")]
    [Column("ncrQAProcessApplicable")]
    public bool NcrQaProcessApplicable { get; set; }
    [Display(Name = "Creation Date")]
    [Required(ErrorMessage = "You must provide the date the NCR was created.")]
    [Column("ncrQACreationDate", TypeName = "date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime NcrQacreationDate { get; set; }

    [Display(Name = "PO or Prod. No.")]
    [Required(ErrorMessage = "You must provide the PO or Prod. No.")]
    [StringLength(45, ErrorMessage = "The PO or Prod. No. cannot be more than 45 characters.")]
    [Column("ncrQaOrderNumber")]
    public string NcrQaOrderNumber { get; set; }

    [Display(Name = "Sales Order No.")]
    [Required(ErrorMessage = "You must provide the Sales Order No.")]
    [StringLength(45, ErrorMessage = "The Sales Order No. cannot be more than 45 characters.")]
    [Column("ncrQaSalesOrder")]
    public string NcrQaSalesOrder { get; set; }

    [Display(Name = "Quantity Received")]
    [Required(ErrorMessage = "You must provide the Quantity Received.")]
    [Column("ncrQaQuanReceived")]
    public int NcrQaQuanReceived { get; set; }

    [Display(Name = "Quantity Defective")]
    [Required(ErrorMessage = "You must provide the Defective Quantity.")]
    [Column("ncrQaQuanDefective")]
    public int NcrQaQuanDefective { get; set; }

    [Display(Name = "Defect description")]
    [Required(ErrorMessage = "You must provide the defect description.")]
    [StringLength(300, ErrorMessage = "Only 300 characters for defect description.")]
    [DataType(DataType.MultilineText)]
    [Column("ncrQaDescriptionOfDefect")]
    public string NcrQaDescriptionOfDefect { get; set; }   

    [Display(Name = "Quality Representative's Name")]
    [Column("ncrQAUserId")]
    public int NcrQauserId { get; set; }

    [Display(Name = "Engineer Disposition Required?")]
    [Column("ncrQaEngDispositionRequired")]
    public bool NcrQaEngDispositionRequired { get; set; }

    [ScaffoldColumn(false)]
    [Timestamp]
    public Byte[] RowVersion { get; set; }//Added for concurrency

    [Display(Name = "NCR")]
    [Column("ncrId")]
    public int NcrId { get; set; }

    [Display(Name = "NCR")]
    [Required(ErrorMessage = "You must provide the NCR.")]
    [ForeignKey("NcrId")]
    public Ncr Ncr { get; set; }

    [Display(Name = "Item")]
    [Column("itemId")]
    public int ItemId { get; set; }

    [ForeignKey("ItemId")]
    //[InverseProperty("NcrQas")]
    [Required(ErrorMessage = "You must select an item.")]
    public Item Item { get; set; }

    [InverseProperty("NcrQa")]
    public ICollection<ItemDefectPhoto> ItemDefectPhotos { get; set; } = new HashSet<ItemDefectPhoto>();

    [InverseProperty("NcrQa")]
    public ICollection<ItemDefectVideo> ItemDefectVideos { get; set; } = new HashSet<ItemDefectVideo>();
}
