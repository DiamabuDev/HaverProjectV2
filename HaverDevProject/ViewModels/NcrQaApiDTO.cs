using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HaverDevProject.Models;
using Microsoft.AspNetCore.Mvc;
using Humanizer;

namespace HaverDevProject.ViewModels
{    
    public class NcrQaApiDTO
    {                
        public string NcrNumber { get; set; }
        public bool NcrStatus { get; set; }        
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
        public int DefectId { get; set; }
        public DateTime NcrLastUpdated { get; set; }

    }
}
