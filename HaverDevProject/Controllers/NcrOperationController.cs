using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaverDevProject.Data;
using HaverDevProject.Models;
using HaverDevProject.ViewModels;
using HaverDevProject.Utilities;
using HaverDevProject.CustomControllers;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Storage;

namespace HaverDevProject.Controllers
{
    public class NcrOperationController : ElephantController
    {
        private readonly HaverNiagaraContext _context;

        public NcrOperationController(HaverNiagaraContext context)
        {
            _context = context;
        }

        // GET: NcrOperation
        public async Task<IActionResult> Index(string SearchCode, DateTime StartDate, DateTime EndDate,
            int? page, int? pageSizeID, string actionButton, string sortDirection = "desc", string sortField = "Created", string filter = "Active")
        {
            
            //Set the date range filer based on the values in the database
            if (EndDate == DateTime.MinValue)
            {
                StartDate = _context.NcrOperations
                    .Min(f => f.UpdateOp.Date)
                    .Subtract(TimeSpan.FromDays(1));

                EndDate = _context.NcrOperations
                    .Max(f => f.UpdateOp.Date)
                    .Add(TimeSpan.FromDays(1));

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
            string[] sortOptions = new[] { "Created", "NCR #", "Disposition Type", "Purchasing Description", "Car", "FollowUp-up", };

            PopulateDropDownLists();

            var ncrOperation = _context.NcrOperations
                .Include(n => n.NcrEng)
                .Include(n => n.Ncr)
                .Include(n => n.OpDispositionType)
                .Include(n => n.FollowUpType)
                .AsNoTracking();

            GetNcrs();

            //Filterig values            
            if (!String.IsNullOrEmpty(filter))
            {
                if (filter == "Active")
                {
                    ncrOperation = ncrOperation.Where(n => n.Ncr.NcrStatus == true);
                }
                else //(filter == "Closed")
                {

                    ncrOperation = ncrOperation.Where(n => n.Ncr.NcrStatus == false);
                }
            }
            if (!String.IsNullOrEmpty(SearchCode))
            {
                ncrOperation = ncrOperation.Where(s => s.Ncr.NcrNumber.ToUpper().Contains(SearchCode.ToUpper()));
            }
            if (StartDate == EndDate)
            {
                ncrOperation = ncrOperation.Where(n => n.UpdateOp == StartDate);
            }
            else
            {
                ncrOperation = ncrOperation.Where(n => n.UpdateOp >= StartDate &&
                         n.UpdateOp <= EndDate);
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
            //Now we know which field and direction to sort by
            if (sortField == "NCR #")
            {
                if (sortDirection == "asc")
                {
                    ncrOperation = ncrOperation
                        .OrderBy(p => p.Ncr.NcrNumber);
                    ViewData["filterApplied:NcrNumber"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrOperation = ncrOperation
                        .OrderByDescending(p => p.Ncr.NcrNumber);
                    ViewData["filterApplied:NcrNumber"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Disposition Type")
            {
                if (sortDirection == "asc")
                {
                    ncrOperation = ncrOperation
                        .OrderBy(p => p.OpDispositionType.OpDispositionTypeName);
                    ViewData["filterApplied:DispositionType"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrOperation = ncrOperation
                        .OrderByDescending(p => p.OpDispositionType.OpDispositionTypeName);
                    ViewData["filterApplied:DispositionType"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Purchasing Description")
            {
                if (sortDirection == "asc")
                {
                    ncrOperation = ncrOperation
                        .OrderBy(p => p.NcrPurchasingDescription);
                    ViewData["filterApplied:PurchasingDescription"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrOperation = ncrOperation
                        .OrderByDescending(p => p.NcrPurchasingDescription);
                    ViewData["filterApplied:PurchasingDescription"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Created")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncrOperation = ncrOperation
                        .OrderBy(p => p.UpdateOp);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrOperation = ncrOperation
                        .OrderByDescending(p => p.UpdateOp);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Status")
            {
                if (sortDirection == "asc")
                {
                    ncrOperation = ncrOperation
                        .OrderBy(p => p.Ncr.NcrStatus);
                    ViewData["filterApplied:Status"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrOperation = ncrOperation
                        .OrderByDescending(p => p.Ncr.NcrStatus);
                    ViewData["filterApplied:Status"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Status")
            {
                if (sortDirection == "asc")
                {
                    ncrOperation = ncrOperation
                        .OrderBy(p => p.FollowUp);
                    ViewData["filterApplied:FollowUp"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrOperation = ncrOperation
                        .OrderByDescending(p => p.FollowUp);
                    ViewData["filterApplied:FollowUp"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else //(sortField == "CAR")
            {
                if (sortDirection == "asc")
                {
                    ncrOperation = ncrOperation
                        .OrderBy(p => p.Car);
                    ViewData["filterApplied:Car"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrOperation = ncrOperation
                        .OrderByDescending(p => p.Car);
                    ViewData["filterApplied:Car"] = "<i class='bi bi-sort-down'></i>";
                }
            }


            //Set sort for next time

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<NcrOperation>.CreateAsync(ncrOperation.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }

    


    // GET: NcrOperation/Details/5
    public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NcrOperations == null)
            {
                return NotFound();
            }

            var ncrOperation = await _context.NcrOperations
                .Include(n => n.FollowUpType)
                .Include(n => n.Ncr)
                .Include(n => n.OpDispositionType)
                .FirstOrDefaultAsync(m => m.NcrOpId == id);
            if (ncrOperation == null)
            {
                return NotFound();
            }

            return View(ncrOperation);
        }

        // GET: NcrOperation/Create
        public IActionResult Create(string ncrNumber)
        {
            NcrOperationDTO ncr = new NcrOperationDTO();
            ncr.NcrNumber = ncrNumber; // Set the NcrNumber from the parameter
            ncr.UpdateOp = DateTime.Now;
            ncr.NcrStatus = true; // Active

            ViewData["FollowUpTypeId"] = new SelectList(_context.FollowUpTypes, "FollowUpTypeId", "FollowUpTypeName");
            ViewData["OpDispositionTypeId"] = new SelectList(_context.OpDispositionTypes, "OpDispositionTypeId", "OpDispositionTypeName");
            return View(ncr);
        }

        // POST: NcrOperation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NcrOperationDTO ncrOperationDTO, int FollowUpTypeId, int OpDispositionTypeId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Find the Ncr entity based on the NcrNumber in the DTO
                    int ncrIdObt = _context.Ncrs
                        .Where(n => n.NcrNumber == ncrOperationDTO.NcrNumber)
                        .Select(n => n.NcrId)
                        .FirstOrDefault();

                    NcrOperation ncrOperation = new NcrOperation
                    {
                        NcrId = ncrIdObt, // Assign the NcrId from the found Ncr entity
                        OpDispositionTypeId = ncrOperationDTO.OpDispositionTypeId,
                        NcrPurchasingDescription = ncrOperationDTO.NcrPurchasingDescription,
                        Car = ncrOperationDTO.Car,
                        CarNumber = ncrOperationDTO.CarNumber,
                        FollowUp = ncrOperationDTO.FollowUp,
                        ExpectedDate = ncrOperationDTO.ExpectedDate,
                        FollowUpTypeId = ncrOperationDTO.FollowUpTypeId,
                        UpdateOp = DateTime.Now,
                        NcrPurchasingUserId = 1,
                    };

                    _context.NcrOperations.Add(ncrOperation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            PopulateDropDownLists();
            return View(ncrOperationDTO);
        }

        // GET: NcrOperation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NcrOperations == null)
            {
                return NotFound();
            }

            var ncrOperation = await _context.NcrOperations.FindAsync(id);
            if (ncrOperation == null)
            {
                return NotFound();
            }
            ViewData["FollowUpTypeId"] = new SelectList(_context.FollowUpTypes, "FollowUpTypeId", "FollowUpTypeName", ncrOperation.FollowUpTypeId);
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrOperation.NcrId);
            ViewData["OpDispositionTypeId"] = new SelectList(_context.OpDispositionTypes, "OpDispositionTypeId", "OpDispositionTypeName", ncrOperation.OpDispositionTypeId);
            return View(ncrOperation);
        }

        // POST: NcrOperation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NcrOpId,NcrId,OpDispositionTypeId,NcrPurchasingDescription,Car,CarNumber,FollowUp,ExpectedDate,FollowUpTypeId,UpdateOp,NcrPurchasingUserId")] NcrOperation ncrOperation)
        {
            if (id != ncrOperation.NcrOpId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ncrOperation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NcrOperationExists(ncrOperation.NcrOpId))
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
            ViewData["FollowUpTypeId"] = new SelectList(_context.FollowUpTypes, "FollowUpTypeId", "FollowUpTypeName", ncrOperation.FollowUpTypeId);
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrOperation.NcrId);
            ViewData["OpDispositionTypeId"] = new SelectList(_context.OpDispositionTypes, "OpDispositionTypeId", "OpDispositionTypeName", ncrOperation.OpDispositionTypeId);
            return View(ncrOperation);
        }

        // GET: NcrOperation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NcrOperations == null)
            {
                return NotFound();
            }

            var ncrOperation = await _context.NcrOperations
                .Include(n => n.FollowUpType)
                .Include(n => n.Ncr)
                .Include(n => n.OpDispositionType)
                .FirstOrDefaultAsync(m => m.NcrOpId == id);
            if (ncrOperation == null)
            {
                return NotFound();
            }

            return View(ncrOperation);
        }

        // POST: NcrOperation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NcrOperations == null)
            {
                return Problem("Entity set 'HaverNiagaraContext.NcrOperations'  is null.");
            }
            var ncrOperation = await _context.NcrOperations.FindAsync(id);
            if (ncrOperation != null)
            {
                _context.NcrOperations.Remove(ncrOperation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NcrOperationExists(int id)
        {
          return _context.NcrOperations.Any(e => e.NcrOpId == id);
        }


        private SelectList OpDispositionTypeSelectList(int? selectedId)
        {
            return new SelectList(_context.OpDispositionTypes
                .OrderBy(s => s.OpDispositionTypeName), "OpDispositionTypeId", "OpDispositionTypeName", selectedId);
        }

        private SelectList FollowUpTypeSelectList(int? selectedId)
        {
            return new SelectList(_context.FollowUpTypes
                .OrderBy(s => s.FollowUpTypeName), "FollowUpTypeId", "FollowUpTypeName", selectedId);
        }

        //public PartialViewResult All()
        //{
        //    List<NcrEng> model = _context.NcrEngs.ToList();
        //    return PartialView("_NcrEng", model);
        //}

        public JsonResult GetNcrs()
        {
            // Get the list of NcrIds that already exist in NcrOperation
            List<int> existingNcrIds = _context.NcrOperations.Select(op => op.NcrId).ToList();

            // Include related data in the query for NcrEng
            List<NcrEng> pendings = _context.NcrEngs
                .Include(ncrEng => ncrEng.Ncr)
                .Where(ncrEng => !existingNcrIds.Contains(ncrEng.NcrId))
                .ToList();

            // Extract relevant data for the client-side
            var ncrs = pendings.Select(ncrEng => new
            {
                NcrId = ncrEng.NcrId,
                NcrEngDispositionDescription = ncrEng.NcrEngDispositionDescription,
                NcrNumber = ncrEng.Ncr.NcrNumber
            }).ToList();

            return Json(ncrs);
        }

        public JsonResult GetPendingCount()
        {
            // Get the list of NcrIds that already exist in NcrOperation
            List<int> existingNcrIds = _context.NcrOperations.Select(op => op.NcrId).ToList();

            // Count only the unique NcrIds in NcrEngs
            int pendingCount = _context.NcrEngs
                .Where(ncrEng => !existingNcrIds.Contains(ncrEng.NcrId))
                .Select(ncrEng => ncrEng.NcrId)
                .Distinct()
                .Count();

            return Json(pendingCount);
        }

        //public JsonResult GetNcrs()      
        //{
        //    List<NcrEng> pendings = _context.NcrEngs.ToList();
        //    return Json(pendings);
        //}

        private void PopulateDropDownLists(NcrOperation ncrOperation = null)
        {
            if ((ncrOperation?.OpDispositionTypeId).HasValue)
            {
                if (ncrOperation.OpDispositionType == null)
                {
                    ncrOperation.OpDispositionType = _context.OpDispositionTypes.Find(ncrOperation.OpDispositionTypeId);
                }
                ViewData["OpDispositionTypeId"] = OpDispositionTypeSelectList(ncrOperation?.OpDispositionTypeId);
                ViewData["FollowUpTypeId"] = FollowUpTypeSelectList(ncrOperation?.FollowUpTypeId);
            }
            else
            {
                ViewData["OpDispositionTypeId"] = OpDispositionTypeSelectList(null);
                ViewData["FollowUpTypeId"] = FollowUpTypeSelectList(null);
            }
        }
    }
}
