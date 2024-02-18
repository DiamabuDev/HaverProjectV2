using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HaverDevProject.Models;
using Microsoft.AspNetCore.Mvc;
using Humanizer;

namespace HaverDevProject.ViewModels
{
    [ModelMetadataType(typeof(NcrQaDTOMetaData))]
    public class NcrQaDTO : IValidatableObject
    {                
        public string NcrNumber { get; set; }
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
        public int DefectId { get; set; } 
        public bool NcrQaEngDispositionRequired { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NcrQaQuanDefective > NcrQaQuanReceived)
            {
                yield return new ValidationResult("Quantity Defective must be equal or less than Quantity Received.", new[] { "NcrQaQuanDefective" });
            }
        }
    }
}
