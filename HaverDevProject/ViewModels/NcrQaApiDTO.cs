﻿using HaverDevProject.Models;
using System.ComponentModel.DataAnnotations;

namespace HaverDevProject.ViewModels
{
    public class NcrQaApiDTO
    {
        //public int NcrQaId { get; set; }
        //public int? ParentId { get; set; }
        public string NcrNumber { get; set; }
        public bool NcrStatus { get; set; } = true;
        public NcrPhase NcrPhase { get; set; }
        public bool NcrQaItemMarNonConforming { get; set; }
        public bool NcrQaProcessApplicable { get; set; }
        public DateTime NcrQacreationDate { get; set; }
        public string NcrQaOrderNumber { get; set; }
        public string NcrQaSalesOrder { get; set; }
        public int NcrQaQuanReceived { get; set; }
        public int NcrQaQuanDefective { get; set; }
        public int SupplierId { get; set; }
        public SupplierApiDTO SupplierApiDTO { get; set; }

        //public int NcrId { get; set; }
        //public Ncr Ncr { get; set; }
        public int ItemId { get; set; }
        public ItemApiDTO ItemApiDTO { get; set; }
        public int DefectId { get; set; }    
        public DefectApiDTO DefectApiDTO { get; set; }
    }
}
