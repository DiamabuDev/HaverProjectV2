using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaverDevProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Models;

[ModelMetadataType(typeof(NcrEngMetaData))]
[Table("ncrEng")]
public class NcrEng : Auditable
{
	public int NcrEngId { get; set; }
	public bool NcrEngCustomerNotification { get; set; } = false;
	public string NcrEngDispositionDescription { get; set; }
    public bool NcrEngStatusFlag { get; set; }
	public int NcrEngUserId { get; set; }
	public int EngDispositionTypeId { get; set; }
	public EngDispositionType EngDispositionType { get; set; }
	public int NcrId { get; set; }
	public Ncr Ncr { get; set; }
	public int DrawingId { get; set; }
	public Drawing Drawing { get; set; }

}
