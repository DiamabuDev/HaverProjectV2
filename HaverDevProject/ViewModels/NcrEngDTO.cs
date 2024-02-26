﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaverDevProject.Models;
using HaverDevProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.ViewModels;

[ModelMetadataType(typeof(NcrEngDTOMetaData))]
public class NcrEngDTO : Auditable
{

	public int NcrEngId { get; set; }
    public string NcrNumber { get; set; }
    public bool NcrStatus { get; set; } = true;
    public bool NcrEngCustomerNotification { get; set; } = false;
	public string NcrEngDispositionDescription { get; set; }
    public bool NcrEngStatusFlag { get; set; }
	public int NcrEngUserId { get; set; }
	public int EngDispositionTypeId { get; set; }
	public int NcrId { get; set; }
	public int DrawingId { get; set; }
	public bool DrawingRequireUpdating { get; set; } = false;
	public int DrawingOriginalRevNumber { get; set; }
	public int DrawingUpdatedRevNumber { get; set; }
	public DateTime DrawingRevDate { get; set; }
	public int DrawingUserId { get; set; }
}