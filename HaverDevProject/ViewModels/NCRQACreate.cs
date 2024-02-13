using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HaverDevProject.ViewModels
{
    public class NCRQACreateDTO
    {
        //public int NcrId { get; set; }
        public string NcrNumber { get; set; }

        //public DateTime NcrLastUpdated { get; set; }

        //public bool NcrStatus { get; set; } = true;

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

        [Display(Name = "Item")]
        [Column("itemId")]
        public int ItemId { get; set; }

        //[Display(Name = "NCR")]
        //[Column("ncrId")]
        //public int NcrId { get; set; }

    }
}
