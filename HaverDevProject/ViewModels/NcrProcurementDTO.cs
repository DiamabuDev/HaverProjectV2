using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models
{
    [ModelMetadataType(typeof(NcrProcurementDTOMetaData))]
    public class NcrProcurementDTO : Auditable
    {
        public string NcrNumber { get; set; }

        public bool NcrStatus { get; set; } = true;

        public int NcrProcurementId { get; set; }

        public bool NcrProcSupplierReturnReq { get; set; }

        public DateTime NcrProcExpectedDate { get; set; }

        public bool NcrProcDisposedAllowed { get; set; }

        public bool NcrProcSAPReturnCompleted { get; set; }

        public bool NcrProcCreditExpected { get; set; }

        public bool NcrProcSupplierBilled { get; set; }

        public bool NcrProcFlagStatus { get; set; }

        public int NcrProcUserId { get; set; }

        //[ScaffoldColumn(false)]
        //[Timestamp]
        //public Byte[] RowVersion { get; set; }//Added for concurrency

        public int NcrId { get; set; }

        public virtual Ncr Ncr { get; set; }
    }
}
