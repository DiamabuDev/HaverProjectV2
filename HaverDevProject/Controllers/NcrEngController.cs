using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaverDevProject.Data;
using HaverDevProject.Models;
//using NcrEng = HaverDevProject.Models.NcrEng;
//using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore.Storage;
using HaverDevProject.CustomControllers;
using HaverDevProject.Utilities;
using HaverDevProject.ViewModels;

namespace HaverDevProject.Controllers
{
	public class NcrEngController : ElephantController
	{
		private readonly HaverNiagaraContext _context;

		public NcrEngController(HaverNiagaraContext context)
		{
			_context = context;
		}


		// GET: NcrEng
		public async Task<IActionResult> Index(string SearchCode, int? SupplierID, DateTime StartDate, DateTime EndDate,
			int? page, int? pageSizeID, string actionButton, string sortDirection = "desc", string sortField = "Created", string filter = "Active")
		{
			//Set the date range filer based on the values in the database
			if (EndDate == DateTime.MinValue)
			{
				StartDate = _context.NcrQas
				.Min(f => f.NcrQacreationDate.Date);

				EndDate = _context.NcrQas
				.Max(f => f.NcrQacreationDate.Date);

				ViewData["StartDate"] = StartDate.ToString("yyyy-MM-dd");
				ViewData["EndDate"] = EndDate.ToString("yyyy-MM-dd");
			}
			//Check the order of the dates and swap them if required
			if (EndDate < StartDate)
			{
				DateTime temp = EndDate;
				EndDate = StartDate;
				StartDate = temp;
			}

			//List of sort options.
			string[] sortOptions = new[] { "Created", "NCR #", "Disposition", "Description" };

			//PopulateDropDownLists();

			var ncrEng = _context.NcrEngs
				.Include(n => n.EngDispositionType)
				.Include(n => n.Ncr).ThenInclude(n => n.NcrQa)
				.Include(n => n.Ncr)
				.Include(n => n.Drawing)
				.AsNoTracking();

			//Filterig values            
			if (!String.IsNullOrEmpty(filter))
			{
				if (filter == "Active")
				{
					ncrEng = ncrEng.Where(n => n.Ncr.NcrStatus == true);
				}
				else //(filter == "Closed")
				{

					ncrEng = ncrEng.Where(n => n.Ncr.NcrStatus == false);
				}
			}
			if (!String.IsNullOrEmpty(SearchCode))
			{
				ncrEng = ncrEng.Where(s => s.Ncr.NcrNumber.ToUpper().Contains(SearchCode.ToUpper()));
			}
			if (StartDate == EndDate)
			{
				ncrEng = ncrEng.Where(n => n.Ncr.NcrQa.NcrQacreationDate == StartDate);
			}
			else
			{
				ncrEng = ncrEng.Where(n => n.Ncr.NcrQa.NcrQacreationDate >= StartDate &&
						 n.Ncr.NcrQa.NcrQacreationDate <= EndDate);
			}

			//Sorting columns
			if (!String.IsNullOrEmpty(actionButton)) //Form Submitted!
			{
				page = 1;//Reset page to start

				if (sortOptions.Contains(actionButton))//Change of sort is requested
				{
					if (actionButton == sortField) //Reverse order on same field
					{
						sortDirection = sortDirection == "asc" ? "desc" : "asc";
					}
					sortField = actionButton;//Sort by the button clicked
				}
			}

			if (sortField == "NCR #")
			{
				if (sortDirection == "asc")
				{
					ncrEng = ncrEng
						.OrderBy(p => p.Ncr.NcrNumber);
					ViewData["filterApplied:NcrNumber"] = "<i class='bi bi-sort-up'></i>";
				}
				else
				{
					ncrEng = ncrEng
						.OrderByDescending(p => p.Ncr.NcrNumber);
					ViewData["filterApplied:NcrNumber"] = "<i class='bi bi-sort-down'></i>";
				}
			}
			else if (sortField == "Disposition")
			{
				if (sortDirection == "asc")
				{
					ncrEng = ncrEng
						.OrderBy(p => p.Ncr.NcrEng.EngDispositionType.EngDispositionTypeName);
					ViewData["filterApplied:Disposition"] = "<i class='bi bi-sort-up'></i>";
				}
				else
				{
					ncrEng = ncrEng
						.OrderByDescending(p => p.Ncr.NcrEng.EngDispositionType.EngDispositionTypeName);
					ViewData["filterApplied:Disposition"] = "<i class='bi bi-sort-down'></i>";
				}
			}
			else if (sortField == "Description")
			{
				if (sortDirection == "asc")
				{
					ncrEng = ncrEng
						.OrderBy(p => p.Ncr.NcrEng.NcrEngDispositionDescription);
					ViewData["filterApplied:Description"] = "<i class='bi bi-sort-up'></i>";
				}
				else
				{
					ncrEng = ncrEng
						.OrderByDescending(p => p.Ncr.NcrEng.NcrEngDispositionDescription);
					ViewData["filterApplied:Description"] = "<i class='bi bi-sort-down'></i>";
				}
			}
			else if (sortField == "Created")
			{
				if (sortDirection == "desc") //desc by default
				{
					ncrEng = ncrEng
						.OrderBy(p => p.Ncr.NcrQa.NcrQacreationDate);

					ViewData["filterApplied:Created"] = "<i class='bi bi-sort-up'></i>";
				}
				else
				{
					ncrEng = ncrEng
						.OrderByDescending(p => p.Ncr.NcrQa.NcrQacreationDate);

					ViewData["filterApplied:Created"] = "<i class='bi bi-sort-down'></i>";
				}
			}
			else //(sortField == "Status")
			{
				if (sortDirection == "asc")
				{
					ncrEng = ncrEng
						.OrderBy(p => p.Ncr.NcrStatus);
					ViewData["filterApplied:Status"] = "<i class='bi bi-sort-up'></i>";
				}
				else
				{
					ncrEng = ncrEng
						.OrderByDescending(p => p.Ncr.NcrStatus);
					ViewData["filterApplied:Status"] = "<i class='bi bi-sort-down'></i>";
				}
			}


			ViewData["sortField"] = sortField;
			ViewData["sortDirection"] = sortDirection;

			int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
			ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
			var pagedData = await PaginatedList<NcrEng>.CreateAsync(ncrEng.AsNoTracking(), page ?? 1, pageSize);

			return View(pagedData);
		}

		// GET: NcrEng/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.NcrEngs == null)
			{
				return NotFound();
			}

			var ncrEng = await _context.NcrEngs
				.Include(n => n.EngDispositionType)
				.Include(n => n.Ncr)
				.Include(n => n.Drawing)
				.FirstOrDefaultAsync(m => m.NcrEngId == id);
			if (ncrEng == null)
			{
				return NotFound();
			}

			return View(ncrEng);
		}


        // GET: NcrEng/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ncrEng = await _context.NcrEngs
						.Include(ne => ne.Ncr)
  						.Include(ne => ne.EngDispositionType)
  						.Include(ne => ne.Drawing)
						.FirstOrDefaultAsync(ne => ne.NcrId == id);
			//var ncrEng = await _context.NcrEngs.FindAsync(id);

            if (ncrEng == null)
            {
                return NotFound();
            }


			var ncrEngDTO = new NcrEngDTO
			{
				NcrEngId = ncrEng.NcrEngId,
				NcrNumber = ncrEng.Ncr.NcrNumber,
				NcrStatus = ncrEng.Ncr.NcrStatus,
				NcrEngCustomerNotification = ncrEng.NcrEngCustomerNotification,
				NcrEngDispositionDescription = ncrEng.NcrEngDispositionDescription,
				NcrEngStatusFlag = ncrEng.NcrEngStatusFlag,
				NcrEngUserId = ncrEng.NcrEngUserId,
				EngDispositionTypeId = ncrEng.EngDispositionTypeId,
				NcrId = ncrEng.NcrId,
				DrawingId = ncrEng.DrawingId,
				DrawingRequireUpdating = ncrEng.Drawing.DrawingRequireUpdating,
				DrawingOriginalRevNumber = ncrEng.Drawing.DrawingOriginalRevNumber,
				DrawingUpdatedRevNumber = ncrEng.Drawing.DrawingUpdatedRevNumber,
				DrawingRevDate = ncrEng.Drawing.DrawingRevDate,
				DrawingUserId = ncrEng.Drawing.DrawingUserId
			};

			ViewData["EngDispositionTypeId"] = new SelectList(_context.EngDispositionTypes, "EngDispositionTypeId", "EngDispositionTypeName", ncrEng.EngDispositionTypeId);
			return View(ncrEngDTO);
        }

        // POST: NcrEng/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NcrEngDTO ncrEngDTO)
        {
			ncrEngDTO.NcrId = id;
            if (id != ncrEngDTO.NcrId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var ncrEng = await _context.NcrEngs
						.Include(ne => ne.Drawing)
						.FirstOrDefaultAsync(ne => ne.NcrId == id);

                    //ncrEng.NcrEngId = ncrEngDTO.NcrEngId;
                    //ncrEng.Ncr.NcrNumber = ncrEngDTO.NcrNumber;
                    //ncrEng.Ncr.NcrStatus = ncrEngDTO.NcrStatus;
                    ncrEng.NcrEngCustomerNotification = ncrEngDTO.NcrEngCustomerNotification;
					ncrEng.NcrEngDispositionDescription = ncrEngDTO.NcrEngDispositionDescription;
					ncrEng.NcrEngStatusFlag = ncrEngDTO.NcrEngStatusFlag;
					ncrEng.NcrEngUserId = ncrEngDTO.NcrEngUserId;
					ncrEng.EngDispositionTypeId = ncrEngDTO.EngDispositionTypeId;
					ncrEng.NcrId = ncrEngDTO.NcrId;
					ncrEng.DrawingId = ncrEngDTO.DrawingId;
					ncrEng.Drawing.DrawingRequireUpdating = ncrEngDTO.DrawingRequireUpdating;
					ncrEng.Drawing.DrawingOriginalRevNumber = ncrEngDTO.DrawingOriginalRevNumber;
					ncrEng.Drawing.DrawingUpdatedRevNumber = ncrEngDTO.DrawingUpdatedRevNumber;
					ncrEng.Drawing.DrawingRevDate = ncrEngDTO.DrawingRevDate;
					ncrEng.Drawing.DrawingUserId = ncrEngDTO.DrawingUserId;

                    _context.Update(ncrEng);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NcrEngExists(ncrEngDTO.NcrEngId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ncrEngDTO);
        }



  //      // GET: NcrEng/Edit/5
  //      public async Task<IActionResult> Edit(int? id)
		//{

  //          //if (id == null || _context.NcrEngs == null)
  //          //{
  //          //    return NotFound();
  //          //}

  //          ////var ncrEng = await _context.NcrEngs.FindAsync(id);
  //          //var ncrEng = await _context.NcrEngs
  //          //    .Include(n => n.EngDispositionType)
  //          //    .Include(n => n.Ncr)
  //          //    .Include(n => n.Drawing)
  //          //    .FirstOrDefaultAsync(m => m.NcrEngId == id);


  //          //if (ncrEng == null)
  //          //{
  //          //    return NotFound();
  //          //}
  //          //ViewData["EngDispositionTypeId"] = new SelectList(_context.EngDispositionTypes, "EngDispositionTypeId", "EngDispositionTypeName", ncrEng.EngDispositionTypeId);
  //          //ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrEng.NcrId);
  //          //return View(ncrEng);

		//	if (id == null)
		//	{
		//		return NotFound();
		//	}

		//	var ncrEng = await _context.Ncrs
		//		.Include(ne => ne.NcrEng)
		//		.ThenInclude(ne => ne.EngDispositionType)
		//		.Include(ne => ne.NcrEng)
		//		.ThenInclude(ne => ne.Drawing)
		//		.FirstOrDefaultAsync(ne => ne.NcrId == id);

		//	if (ncrEng == null)
		//	{
		//		return NotFound();
		//	}
		//	var ncrEngDTO = new NcrEngDTO
		//	{
		//		NcrId = ncrEng.NcrId,
		//		NcrEngCustomerNotification = ncrEng.NcrEng.NcrEngCustomerNotification,
		//		NcrEngDispositionDescription = ncrEng.NcrEng.NcrEngDispositionDescription,
		//		NcrEngUserId = ncrEng.NcrEng.NcrEngUserId,
		//		DrawingUpdatedRevNumber = ncrEng.NcrEng.Drawing.DrawingUpdatedRevNumber,
		//		DrawingOriginalRevNumber = ncrEng.NcrEng.Drawing.DrawingOriginalRevNumber,
		//		DrawingRevDate = ncrEng.NcrEng.Drawing.DrawingRevDate,
		//		DrawingUserId = ncrEng.NcrEng.Drawing.DrawingUserId,
		//		DrawingRequireUpdating = ncrEng.NcrEng.Drawing.DrawingRequireUpdating,
		//		EngDispositionTypeId = ncrEng.NcrEng.EngDispositionTypeId,
		//		DrawingId = ncrEng.NcrEng.DrawingId,
		//	};

		//	ViewData["EngDispositionTypeId"] = new SelectList(_context.EngDispositionTypes, "EngDispositionTypeId", "EngDispositionTypeName", ncrEng.NcrEng.EngDispositionTypeId);
		//	ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrEng.NcrId);
		//	return View(ncrEngDTO);
		//}


		//// POST: NcrEng/Edit/5
		//// To protect from overposting attacks, enable the specific properties you want to bind to.
		//// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
  //      public async Task<IActionResult> Edit(int id, NcrEngDTO ncrEngDTO)
  //      {
		//	ncrEngDTO.NcrId = id;
  //          if (id != ncrEngDTO.NcrId)
  //          {
		//		return NotFound();
  //          }
  //          //ncrEngDTO.EngDispositionType = await _context.EngDispositionTypes.FindAsync(ncrEngDTO.EngDispositionTypeId);
  //  //        ncrEngDTO.EngDispositionType = await _context.EngDispositionTypes
  //  //.FirstOrDefaultAsync(edt => edt.EngDispositionTypeId == ncrEngDTO.EngDispositionTypeId);


  //          if (!ModelState.IsValid)
		//	{
  //              var booBoos = ModelState.Where(x => x.Value.Errors.Count > 0)
  //          .Select(x => new { x.Key, x.Value.Errors });

  //              foreach (var booBoo in booBoos)
  //              {
  //                  string key = booBoo.Key;
  //                  foreach (var error in booBoo.Errors)
  //                  {
  //                      var errorMessage = error?.ErrorMessage;
  //                      ModelState.AddModelError("", "For " + key + ": " + errorMessage);
  //                  }
  //              }
  //          }

  //          if (ModelState.IsValid)
  //          {
  //              try
  //              {
  //                  var ncrEng = await _context.NcrEngs
  //                      .Include(ne => ne.Ncr)
		//				.Include(ne => ne.EngDispositionType)
		//				.Include(ne => ne.Drawing)
  //                      .FirstOrDefaultAsync(ne => ne.NcrId == id);

  //                  if (ncrEng == null)
  //                  {
  //                      return NotFound();
  //                  }

  //                  // Update the properties of ncrEng based on the properties of ncrEngDTO
  //   //               ncrEng.Ncr = ncrEngDTO.Ncr;
  //   //               ncrEng.NcrEngCustomerNotification = ncrEngDTO.NcrEngCustomerNotification;
  //   //               ncrEng.NcrEngDispositionDescription = ncrEngDTO.NcrEngDispositionDescription;
  //   //               ncrEng.NcrEngUserId = ncrEngDTO.NcrEngUserId;
  //   //               ncrEng.EngDispositionTypeId = ncrEngDTO.EngDispositionTypeId;
		//			//ncrEng.EngDispositionType = ncrEngDTO.EngDispositionType;
  //   //               ncrEng.DrawingId = ncrEngDTO.DrawingId;

  //                  _context.Update(ncrEng);
  //                  await _context.SaveChangesAsync();
  //              }
  //              catch (DbUpdateConcurrencyException)
  //              {
  //                  if (!NcrEngExists(ncrEngDTO.NcrId))
  //                  {
		//				return NotFound();
		//			}
  //                  else
  //                  {
  //                      throw;
  //                  }
  //              }
  //              return RedirectToAction(nameof(Index));
  //          }
  //          ViewData["EngDispositionTypeId"] = new SelectList(_context.EngDispositionTypes, "EngDispositionTypeId", "EngDispositionTypeName", ncrEngDTO.EngDispositionTypeId);
  //          ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrEngDTO.NcrId);
  //          return View(ncrEngDTO);
  //      }





  //      public async Task<IActionResult> Edit(int id, [Bind("NcrEngId,NcrEngCustomerNotification,NcrEngDispositionDescription,NcrEngUserId,EngDispositionTypeId,Drawing.DrawingRequireUpdating,Drawing.DrawingUpdatedRevNumber,Drawing.DrawingOriginalRevNumber,Drawing.DrawingRevDate")] NcrEngDTO ncrEngDTO)
		//{



		//	var ncrEngToUpdate = await _context.NcrEngs.FirstOrDefaultAsync(r => r.NcrId == id);

		//	if (ncrEngToUpdate == null)
		//	{
		//		return NotFound();
		//	}

		//	if (await TryUpdateModelAsync<NcrEng>(ncrEngToUpdate, "",
		//		n => n.NcrEngCustomerNotification,
		//		n => n.NcrEngDispositionDescription,
		//		n => n.NcrEngUserId,
		//		n => n.EngDispositionTypeId,
		//		n => n.Drawing.DrawingRequireUpdating,
		//		n => n.Drawing.DrawingUpdatedRevNumber,
		//		n => n.Drawing.DrawingOriginalRevNumber,
		//		n => n.Drawing.DrawingRevDate))

		//	{
		//		try
		//		{
		//			await _context.SaveChangesAsync();
		//			return RedirectToAction(nameof(Index));
		//		}
		//		catch (DbUpdateConcurrencyException)
		//		{
		//			if (!NcrEngExists(ncrEngToUpdate.NcrId))
		//			{
		//				return NotFound();
		//			}
		//			else
		//			{
		//				throw;
		//			}
		//		}
		//		catch (DbUpdateException)
		//		{
		//			ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
		//		}
		//	}

		//	else
		//	{
		//		//Debugging Approach: Not for production code.
		//		//This code will list validation errors at the top of the View.
		//		//Use it to diagnose when there seems to be a Validation Error
		//		//that is going unreported.  Remove this code when you are
		//		//finished debugging.
		//		var booBoos = ModelState.Where(x => x.Value.Errors.Count > 0)
		//			.Select(x => new { x.Key, x.Value.Errors });

		//		foreach (var booBoo in booBoos)
		//		{
		//			string key = booBoo.Key;
		//			foreach (var error in booBoo.Errors)
		//			{
		//				var errorMessage = error?.ErrorMessage;
		//				ModelState.AddModelError("", "For " + key + ": " + errorMessage);
		//			}
		//		}
		//	}


		//	ViewData["EngDispositionTypeId"] = new SelectList(_context.EngDispositionTypes, "EngDispositionTypeId", "EngDispositionTypeName");
		//	ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrEngToUpdate.NcrId);
		//	return View(ncrEngToUpdate);
		//}


		//if (id != ncrEng.NcrEngId)
		//{
		//    return NotFound();
		//}

		//if (ModelState.IsValid)
		//{
		//    try
		//    {
		//        _context.Update(ncrEng);
		//        await _context.SaveChangesAsync();
		//    }
		//    catch (DbUpdateConcurrencyException)
		//    {
		//        if (!NcrEngExists(ncrEng.NcrEngId))
		//        {
		//            return NotFound();
		//        }
		//        else
		//        {
		//            throw;
		//        }
		//    }
		//    return RedirectToAction(nameof(Index));
		//}
		//ViewData["EngDispositionTypeId"] = new SelectList(_context.EngDispositionTypes, "EngDispositionTypeId", "EngDispositionTypeName", ncrEng.EngDispositionTypeId);
		//ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrEng.NcrId);
		//return View(ncrEng);


		private bool NcrEngExists(int id)
		{
			return _context.NcrEngs.Any(e => e.NcrEngId == id);
		}
	}
}
