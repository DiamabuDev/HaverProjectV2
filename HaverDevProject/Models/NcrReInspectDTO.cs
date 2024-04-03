using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[ModelMetadataType(typeof(NcrReInspectMetaData))]
public class NcrReInspectDTO
{
    public int DaysSinceCreated

    {

        get

        {

            return (DateTime.Now - NcrReInspectCreationDate).Days;

        }

    }
    public int NcrReInspectId { get; set; }
    public bool NcrReInspectAcceptable { get; set; } = false;
    public string NcrReInspectNewNcrNumber { get; set; }
    public int NcrReInspectUserId { get; set; }
    public DateTime NcrReInspectCreationDate { get; set; }
    public bool NcrQaStatusFlag { get; set; }
    public bool NcrStatus { get; set; }
    public string NcrReInspectDefectVideo { get; set; }
    public int NcrId { get; set; }
    public Ncr Ncr { get; set; }
    public int NcrQaId { get; set; }
    public NcrQa NcrQa { get; set; }
    public int NcrProcurementId { get; set; }
    public NcrProcurement NcrProcurement { get; set;}
    public ICollection<NcrReInspectPhoto> NcrReInspectPhotos { get; set; } = new HashSet<NcrReInspectPhoto>();

}
