using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HaverDevProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace HaverDevProject.ViewModels
{    
    public class NcrQaDTO
    {

        //public int NcrId { get; set; }
        public string NcrNumber { get; set; }

        //public DateTime NcrLastUpdated { get; set; }

        public bool NcrStatus { get; set; } = true;        
        public bool NcrQaItemMarNonConforming { get; set; }        
        public bool NcrQaProcessApplicable { get; set; }        
        public DateTime NcrQacreationDate { get; set; }       
        public string NcrQaOrderNumber { get; set; }        
        public string NcrQaSalesOrder { get; set; }        
        public int NcrQaQuanReceived { get; set; }        
        public int NcrQaQuanDefective { get; set; }       
        public string NcrQaDescriptionOfDefect { get; set; }
        public int NcrId { get; set; }
        public int ItemId { get; set; }
        public bool NcrQaEngDispositionRequired { get; set; }

    }
}
