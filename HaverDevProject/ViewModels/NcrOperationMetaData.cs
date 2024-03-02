using HaverDevProject.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaverDevProject.ViewModels
{
    public class NcrOperationMetaData
    {
        [Key]
        [Column("ncrOpId")]
        public int NcrOpId { get; set; }

        [Column("ncrId")]
        [Display(Name = "NCR")]
        public int NcrId { get; set; }

        [Column("opDispositionTypeId")]
        [Display(Name = "Purchasing's Preliminary Decision")]
        [Required(ErrorMessage = "You must provide the Disposition Type.")]
        public int OpDispositionTypeId { get; set; }

        [ForeignKey("OpDispositionTypeId")]
        [InverseProperty("NcrPurchasings")]
        [Display(Name = "Purchasing's Preliminary Decision")]
        public OpDispositionType OpDispositionType { get; set; }

        [Column("ncrPurchasingDescription")]
        [StringLength(300)]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "You must include a description")]
        public string NcrPurchasingDescription { get; set; }

        [Display(Name = "Was a CAR raised")]
        [Required(ErrorMessage = "You select an option if the CAR was raised.")]
        public bool Car { get; set; }

        [Column("ncrCarNumber")]
        [Display(Name = "If \"Yes\" indicate CAR #")]
        public string CarNumber { get; set; }

        [Display(Name = "Follow-up Required?")]
        [Required(ErrorMessage = "You select an option if a Follow-Up is Required.")]
        public bool FollowUp { get; set; }

        [Column("ncrExpectedDate")]
        [Display(Name = "Expected Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpectedDate { get; set; }

        [Column("ncrFollowUpType")]
        [Display(Name = "If \"Yes\" indicate type & expected date")]
        public int? FollowUpTypeId { get; set; }

        [Display(Name = "If \"Yes\" indicate type & expected date" )]
        public FollowUpType FollowUpType { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdateOp { get; set; }

        [Column("ncrOperationUserId")]
        [Display(Name = "Operation Manager")]
        public int NcrPurchasingUserId { get; set; }

        [Display(Name = "Video Link")]
        public string NcrOperationVideo { get; set; }
    }
}
