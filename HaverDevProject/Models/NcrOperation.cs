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
    public DateTime NcrOpCreationDate { get; set; }
    public int OpDispositionTypeId { get; set; }
    public virtual OpDispositionType OpDispositionType { get; set; }
    public string NcrPurchasingDescription { get; set; }
    public bool Car { get; set; }
    public string CarNumber { get; set; }
    public bool FollowUp { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public int? FollowUpTypeId { get; set; }
    public FollowUpType FollowUpType { get; set; }
    public DateTime UpdateOp { get; set; }
    public int NcrPurchasingUserId { get; set; }
    public NcrEng NcrEng { get; set; }
    public string NcrOperationVideo { get; set; }
    public ICollection<OpDefectPhoto> OpDefectPhotos { get; set; } = new HashSet<OpDefectPhoto>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FollowUp && (!FollowUpTypeId.HasValue || !ExpectedDate.HasValue))
        {
            yield return new ValidationResult(
                "Follow-up Type and Expected Date are required when Follow-up is selected.",
                new[] { nameof(FollowUp), nameof(FollowUpType), nameof(ExpectedDate) });
        }
    }
}
