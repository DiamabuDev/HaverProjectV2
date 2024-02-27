using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaverDevProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.ViewModels;

public class NcrEngMetaData
{

	////Summary properties
	//[Display(Name = "Check the box if customer requires notification of NCR (if checked, raise message to customer detailing issues)")]
	//public string CustomerNotificationSummary
	//{
	//	get
	//	{
	//		return NcrEngCustomerNotification.ToString();
	//	}
	//}

	//[Display(Name = "Disposition (Sequence of work steps required when 'repair' or 'rework' indicated)")]
	//public string DispositionDescriptionSummary
	//{
	//	get
	//	{
	//		return NcrEngDispositionDescription.ToString();
	//	}
	//}

	//[Display(Name = "Review by HBC Engineering: (indicate disposition by 'checking' one of the following)")]
	//public string EngDispositionTypeSummary
	//{
	//	get
	//	{
	//		return EngDispositionTypeId.ToString();
	//	}
	//}

	[Key]
	[Column("ncrEngId")]
	public int NcrEngId { get; set; }

	[Display(Name = "Customer Notification")]
	[Column("ncrEngCustomerNotification")]
	public bool NcrEngCustomerNotification { get; set; } = false;

	[Display(Name = "Disposition")]
	[Column("ncrEngDispositionDescription")]
	[StringLength(300, ErrorMessage = "Only 300 characters for disposition description.")]
	[DataType(DataType.MultilineText)]
	[Unicode(false)]
	public string NcrEngDispositionDescription { get; set; }

    public bool NcrEngStatusFlag { get; set; }

	[Display(Name = "Engineering")]
	[Column("ncrEngUserId")]
	public int NcrEngUserId { get; set; }

	[Display(Name = "Disposition Type")]
    [Required(ErrorMessage = "You must provide the Disposition Type.")]
    [Column("engDispositionTypeId")]
	public int EngDispositionTypeId { get; set; }

	[Display(Name = "Disposition Type")]
	//[Required(ErrorMessage = "You must provide the Disposition Type.")]
	[ForeignKey("EngDispositionTypeId")]
	//[InverseProperty("NcrEngs")]
	public virtual EngDispositionType EngDispositionType { get; set; }

	[Display(Name = "NCR")]
    [Required(ErrorMessage = "You must provide the NCR.")]
    [Column("ncrId")]
	public int NcrId { get; set; }

	[Display(Name = "NCR")]
    //[Required(ErrorMessage = "You must provide the NCR.")]
    [ForeignKey("NcrId")]
	public virtual Ncr Ncr { get; set; }

	[Display(Name = "Drawings")]
	//[InverseProperty("NcrEng")]
	public virtual Drawing Drawing { get; set; }

    //[Display(Name = "Check the box if drawing requires updating")]
    //[Column("DrawingRequireUpdating")]
    //public bool DrawingRequireUpdating { get; set; } = false;

    //[Display(Name = "Original Rev. Number")]
    //[Column("drawingOriginalRevNumber")]
    //public int DrawingOriginalRevNumber { get; set; }

    //[Display(Name = "Updated Rev. Number")]
    //[Column("drawingUpdatedRevNumber")]
    //public int DrawingUpdatedRevNumber { get; set; }

    //[Display(Name = "Revision Date")]
    //[Column("drawingRevDate", TypeName = "date")]
    //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //[DataType(DataType.Date)]
    //public DateTime DrawingRevDate { get; set; } = DateTime.Now;

    //[Display(Name = "Drawing User ID")]
    //[Column("drawingUserId")]
    //public int DrawingUserId { get; set; }

    [Display(Name = "Engineer Photos")]
    public ICollection<EngDefectPhoto> EngDefectPhotos { get; set; } = new HashSet<EngDefectPhoto>();
}
