using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaverDevProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[ModelMetadataType(typeof(NcrOperationMetaData))]
public class NcrOperation : Auditable
{
    public int NcrOpId { get; set; }
    public int NcrId { get; set; }
    public Ncr Ncr { get; set; }
    public int OpDispositionTypeId { get; set; }
    public virtual OpDispositionType OpDispositionType { get; set; }
    public string NcrPurchasingDescription { get; set; }
    public bool Car { get; set; }
    public string CarNumber { get; set; }
    public bool FollowUp { get; set; }
    public DateTime ExpectedDate { get; set; }
    public int? FollowUpTypeId { get; set; }
    public FollowUpType FollowUpType { get; set; }
    public DateTime UpdateOp { get; set; }
    public int NcrPurchasingUserId { get; set; }
    public NcrEng NcrEng { get; set; }
    public ICollection<ItemDefectPhoto> ItemDefectPhotos { get; set; } = new HashSet<ItemDefectPhoto>();
    public ICollection<ItemDefectVideo> ItemDefectVideos { get; set; } = new HashSet<ItemDefectVideo>();
}
