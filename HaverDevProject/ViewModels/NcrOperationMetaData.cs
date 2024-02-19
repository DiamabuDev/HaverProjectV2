using HaverDevProject.Models;
using System.ComponentModel.DataAnnotations;

namespace HaverDevProject.ViewModels
{
    public class NcrOperationMetaData : Auditable
    {
        [Display(Name = "NCR")]
        public int NcrId { get; set; }

        [Display(Name = "Purchasing's Preliminary Decision")]
        public int OpDispositionTypeId { get; set; }

        [Display(Name = "Purchasing's Preliminary Decision")]
        [Required(ErrorMessage = "You must provide the Disposition Type.")]
        public OpDispositionType OpDispositionType { get; set; }

        [StringLength(300)]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "You must include a description")]
        public string NcrPurchasingDescription { get; set; }

        [Display(Name = "Was a CAR raised")]
        [Required(ErrorMessage = "You select an option if the CAR was raised.")]
        public bool Car { get; set; }

        [Display(Name = "If \"Yes\" indicate CAR #")]
        public string CarNumber { get; set; }

        [Display(Name = "Follow-up Required?")]
        [Required(ErrorMessage = "You select an option if a Follow-Up is Required.")]
        public bool FollowUp { get; set; }

        [Display(Name = "Expected Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpectedDate { get; set; }

        [Display(Name = "Follow-Up type")]
        public int? FollowUpTypeId { get; set; }

        [Display(Name = "Follow-Up type")]
        public FollowUpType FollowUpType { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdateOp { get; set; }

        [Display(Name = "Operation Manager")]
        public int NcrPurchasingUserId { get; set; }
    }
}
