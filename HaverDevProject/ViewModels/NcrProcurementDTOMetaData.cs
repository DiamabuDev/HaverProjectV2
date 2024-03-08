﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HaverDevProject.Models
{
    public class NcrProcurementDTOMetaData : Auditable
    {
        [Display(Name = "NCR No.")]
        [Required(ErrorMessage = "You must provide the NCR Number.")]
        [Column("ncrNumber")]
        [StringLength(10, ErrorMessage = "The NCR Number cannot be more than 10 characters long.")]
        [Unicode(false)]
        public string NcrNumber { get; set; }

        [Display(Name = "Status")]
        [Column("ncrStatus")]
        public bool NcrStatus { get; set; }

        [Key]
        [Column("ncrProcurementId")]
        public int NcrProcurementId { get; set; }

        [Display(Name = "Supplier Return Request")]
        [Column("ncrProcSupplierReturnReq")]
        public bool NcrProcSupplierReturnReq { get; set; }

        [Display(Name = "Expected Date")]
        [Column("ncrProcExpectedDate")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}")]
        public DateTime? NcrProcExpectedDate { get; set; }

        [Display(Name = "Dispose on Site")]
        [Column("ncrProcDisposedAllowed")]
        public bool NcrProcDisposedAllowed { get; set; }

        [Display(Name = "Supplier Return Completed")]
        [Column("ncrProcSAPReturnCompleted")]
        public bool NcrProcSAPReturnCompleted { get; set; }

        [Display(Name = "Supplier Credit")]
        [Column("ncrProcCreditExpected")]
        public bool NcrProcCreditExpected { get; set; }

        [Display(Name = "Supplier Billed")]
        [Column("ncrProcSupplierBilled")]
        public bool NcrProcSupplierBilled { get; set; }

        public bool NcrProcFlagStatus { get; set; }

        [Display(Name = "Procurement")]
        [Column("ncrProcUserId")]
        public int NcrProcUserId { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime NcrProcCreated { get; set; }

        [Display(Name = "NCR")]
        [Column("ncrId")]
        public int NcrId { get; set; }

        //[Display(Name = "Supplier Return")]
        //[Column("supplierReturnId")]
        //public int SupplierReturnId { get; set; }

        //[Display(Name = "SupplierReturn")]
        //[ForeignKey("SupplierReturnId")]
        //public virtual SupplierReturn SupplierReturn { get; set; }

        [Display(Name = "RMA Number")]
        
        [Column("supplierReturnMANum")]
        public string SupplierReturnMANum { get; set; }

        [Display(Name = "Carrier Name")]
        [StringLength(45, ErrorMessage = "Only 45 characters for Carrier Name")]
        
        [Column("supplierReturnName")]
        public string SupplierReturnName { get; set; }

        [Display(Name = "Account Number")]
        [StringLength(45, ErrorMessage = "Only 45 characters for the Account Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only numbers are allowed.")]
        
        [Column("supplierReturnAccount")]
        public string SupplierReturnAccount { get; set; }

        [Display(Name = "Video Link")]
        [StringLength(100, ErrorMessage = "Video link cannot be more than 100 characters.")]
        public string NcrProcDefectVideo { get; set; }

        [Display(Name = "Procurement Photos")]
        public ICollection<ProcDefectPhoto> ProcDefectPhotos { get; set; } = new HashSet<ProcDefectPhoto>();
    }
}
