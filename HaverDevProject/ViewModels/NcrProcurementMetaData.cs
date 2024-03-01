using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models
{
    public class NcrProcurementMetaData : Auditable
    {
        [Key]
        [Column("ncrProcurementId")]
        public int NcrProcurementId { get; set; }

        [Display(Name = "Supplier Return Request")]
        [Column("ncrProcSupplierReturnReq")]
        public bool NcrProcSupplierReturnReq { get; set; }

        [Display(Name = "Expected Date")]
        [Column("ncrProcExpectedDate")]
        public DateTime NcrProcExpectedDate { get; set; }

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

        [Display(Name = "Procurement")]
        [Column("ncrProcUserId")]
        public int NcrProcUserId { get; set; }

        //[ScaffoldColumn(false)]
        //[Timestamp]
        //public Byte[] RowVersion { get; set; }//Added for concurrency

        [Display(Name = "NCR")]
        [Column("ncrId")]
        public int NcrId { get; set; }

    }
}
