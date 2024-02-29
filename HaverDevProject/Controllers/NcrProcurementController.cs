﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaverDevProject.Data;
using HaverDevProject.Models;
using HaverDevProject.ViewModels;
using HaverDevProject.CustomControllers;
using HaverDevProject.Utilities;
using Microsoft.EntityFrameworkCore.Storage;

namespace HaverDevProject.Controllers
{
    public class NcrProcurementController : ElephantController
    {
        private readonly HaverNiagaraContext _context;

        public NcrProcurementController(HaverNiagaraContext context)
        {
            _context = context;
        }

        // GET: NcrProcurement
        public async Task<IActionResult> Index(string SearchCode, int? SupplierID, DateTime StartDate, DateTime EndDate,
            int? page, int? pageSizeID, string actionButton, string sortDirection = "desc", string sortField = "Created", string filter = "Active")
        {
            //Set the date range filer based on the values in the database
            if (EndDate == DateTime.MinValue)
            {
                StartDate = _context.NcrProcurements
                .Min(f => f.NcrProcUpdate.Date);

                EndDate = _context.NcrProcurements
                .Max(f => f.NcrProcUpdate.Date);

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
            string[] sortOptions = new[] { "Created", "NCR #", "SupplierReturn", "ExpectedDate" };

            var ncrProc = _context.NcrProcurements
                .Include(n => n.Ncr)
                //.Include(n => n.SupplierReturn)
                .AsNoTracking();

            GetNcrs();

            //Filterig values            
            if (!String.IsNullOrEmpty(filter))
            {
                if (filter == "Active")
                {
                    ncrProc = ncrProc.Where(n => n.Ncr.NcrStatus == true);
                }
                else //(filter == "Closed")
                {

                    ncrProc = ncrProc.Where(n => n.Ncr.NcrStatus == false);
                }
            }
            if (!String.IsNullOrEmpty(SearchCode))
            {
                ncrProc = ncrProc.Where(s => s.Ncr.NcrNumber.ToUpper().Contains(SearchCode.ToUpper()));
            }
            if (StartDate == EndDate)
            {
                ncrProc = ncrProc.Where(n => n.NcrProcUpdate == StartDate);
            }
            else
            {
                ncrProc = ncrProc.Where(n => n.NcrProcUpdate >= StartDate &&
                         n.NcrProcUpdate <= EndDate);
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
                    ncrProc = ncrProc
                        .OrderBy(p => p.Ncr.NcrNumber);
                    ViewData["filterApplied:NcrNumber"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrProc = ncrProc
                        .OrderByDescending(p => p.Ncr.NcrNumber);
                    ViewData["filterApplied:NcrNumber"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "SupplierReturn")
            {
                if (sortDirection == "asc")
                {
                    ncrProc = ncrProc
                        .OrderBy(p => p.NcrProcSupplierReturnReq);
                    ViewData["filterApplied:SupplierReturn"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrProc = ncrProc
                        .OrderByDescending(p => p.NcrProcSupplierReturnReq);
                    ViewData["filterApplied:SupplierReturn"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "ExpectedDate")
            {
                if (sortDirection == "asc")
                {
                    ncrProc = ncrProc
                        .OrderBy(p => p.NcrProcExpectedDate);
                    ViewData["filterApplied:ExpectedDate"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrProc = ncrProc
                        .OrderByDescending(p => p.NcrProcExpectedDate);
                    ViewData["filterApplied:ExpectedDate"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Created")
            {
                if (sortDirection == "asc") //desc by default
                {
                    ncrProc = ncrProc
                        .OrderBy(p => p.NcrProcUpdate);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrProc = ncrProc
                        .OrderByDescending(p => p.NcrProcUpdate);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else //(sortField == "Status")
            {
                if (sortDirection == "asc")
                {
                    ncrProc = ncrProc
                        .OrderBy(p => p.Ncr.NcrStatus);
                    ViewData["filterApplied:Status"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrProc = ncrProc
                        .OrderByDescending(p => p.Ncr.NcrStatus);
                    ViewData["filterApplied:Status"] = "<i class='bi bi-sort-down'></i>";
                }
            }

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<NcrProcurement>.CreateAsync(ncrProc.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);


        }

        // GET: NcrProcurement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NcrProcurements == null)
            {
                return NotFound();
            }

            var ncrProc = await _context.NcrProcurements
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa)
                .Include(n => n.Ncr)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item).ThenInclude(n => n.ItemDefects)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item).ThenInclude(n => n.ItemDefects).ThenInclude(n => n.Defect)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item).ThenInclude(n => n.Supplier)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrEng).ThenInclude(n => n.Drawing)
                .FirstOrDefaultAsync(m => m.NcrProcurementId == id);

            if (ncrProc == null)
            {
                return NotFound();
            }

            return View(ncrProc);
        }

        // GET: NcrProcurement/Create
        public IActionResult Create(string ncrNumber)
        {
            NcrProcurementDTO ncr = new NcrProcurementDTO();
            ncr.NcrNumber = ncrNumber;
            ncr.NcrProcUpdate = DateTime.Now;
            ncr.NcrStatus = true; //Active


            return View(ncr);
        }

        // POST: NcrProcurement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NcrProcurementDTO ncrProcDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int ncrIdObt = _context.Ncrs
                        .Where(n => n.NcrNumber == ncrProcDTO.NcrNumber)
                        .Select(n => n.NcrId)
                        .FirstOrDefault();

                    NcrProcurement ncrProc = new NcrProcurement
                    {
                        NcrProcurementId = ncrProcDTO.NcrProcurementId,
                        NcrProcSupplierReturnReq = ncrProcDTO.NcrProcSupplierReturnReq,
                        NcrProcExpectedDate = ncrProcDTO.NcrProcExpectedDate,
                        NcrProcDisposedAllowed = ncrProcDTO.NcrProcDisposedAllowed,
                        NcrProcSAPReturnCompleted = ncrProcDTO.NcrProcSAPReturnCompleted,
                        NcrProcCreditExpected = ncrProcDTO.NcrProcCreditExpected,
                        NcrProcSupplierBilled = ncrProcDTO.NcrProcSupplierBilled,
                        NcrProcFlagStatus = ncrProcDTO.NcrProcFlagStatus,
                        NcrProcUserId = ncrProcDTO.NcrProcUserId,
                        NcrProcUpdate = ncrProcDTO.NcrProcUpdate,
                        NcrId = ncrProcDTO.NcrId,
                        //SupplierReturnId = ncrProcDTO.SupplierReturnId,
                        //SupplierReturn = ncrProcDTO.SupplierReturn,

                    };

                    _context.NcrProcurements.Add(ncrProc);
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

            //PopulateDropDownLists();
            return View(ncrProcDTO);
        }

        // GET: NcrProcurement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ncrProc = await _context.NcrProcurements
                .Include(n => n.Ncr)
                //.Include(n => n.SupplierReturn)
                .FirstOrDefaultAsync(n => n.NcrId == id);

            if (ncrProc == null)
            {
                return NotFound();
            }

            var ncrProcDTO = new NcrProcurementDTO
            {
                NcrNumber = ncrProc.Ncr.NcrNumber,
                NcrProcurementId = ncrProc.NcrProcurementId,
                NcrProcSupplierReturnReq = ncrProc.NcrProcSupplierReturnReq,
                NcrProcExpectedDate = ncrProc.NcrProcExpectedDate,
                NcrProcDisposedAllowed = ncrProc.NcrProcDisposedAllowed,
                NcrProcSAPReturnCompleted = ncrProc.NcrProcSAPReturnCompleted,
                NcrProcCreditExpected = ncrProc.NcrProcCreditExpected,
                NcrProcSupplierBilled = ncrProc.NcrProcSupplierBilled,
                NcrProcFlagStatus = ncrProc.NcrProcFlagStatus,
                NcrProcUserId = ncrProc.NcrProcUserId,
                NcrProcUpdate = ncrProc.NcrProcUpdate,
                NcrId = ncrProc.NcrId,
                //SupplierReturnId = ncrProc.SupplierReturnId,
                
            };

            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrProc.NcrId);
            return View(ncrProcDTO);
        }

        // POST: NcrProcurement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NcrProcurementDTO ncrProcDTO)
        {
            ncrProcDTO.NcrId = id;

            if(id != ncrProcDTO.NcrId)
            {
                return NotFound();
            }

            //var ncrProcToUpdate = await _context.NcrProcurements.FirstOrDefaultAsync(n => n.NcrProcurementId == id);

            if (ModelState.IsValid)
            {
                try
                {
                    var ncrProc = await _context.NcrProcurements
                        //.Include(n => n.SupplierReturn)
                        .FirstOrDefaultAsync(n => n.NcrId == id);

                    ncrProc.NcrProcSupplierReturnReq = ncrProcDTO.NcrProcSupplierReturnReq;
                    ncrProc.NcrProcExpectedDate = ncrProcDTO.NcrProcExpectedDate;
                    ncrProc.NcrProcDisposedAllowed = ncrProcDTO.NcrProcDisposedAllowed;
                    ncrProc.NcrProcSAPReturnCompleted = ncrProcDTO.NcrProcSAPReturnCompleted;
                    ncrProc.NcrProcCreditExpected = ncrProcDTO.NcrProcCreditExpected;
                    ncrProc.NcrProcSupplierBilled = ncrProcDTO.NcrProcSupplierBilled;
                    ncrProc.NcrProcFlagStatus = ncrProcDTO.NcrProcFlagStatus;
                    ncrProc.NcrProcUserId = ncrProcDTO.NcrProcUserId;
                    ncrProc.NcrProcUpdate = DateTime.Now;
                    ncrProc.NcrId = ncrProcDTO.NcrId;
                    //ncrProc.SupplierReturnId = ncrProcDTO.SupplierReturnId;

                    _context.Update(ncrProc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NcrProcurementExists(ncrProcDTO.NcrProcurementId))
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
            return View(ncrProcDTO);
        }

        // GET: NcrProcurement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NcrProcurements == null)
            {
                return NotFound();
            }

            var ncrProcurement = await _context.NcrProcurements
                .Include(n => n.Ncr)
                .FirstOrDefaultAsync(m => m.NcrProcurementId == id);
            if (ncrProcurement == null)
            {
                return NotFound();
            }

            return View(ncrProcurement);
        }

        // POST: NcrProcurement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NcrProcurements == null)
            {
                return Problem("Entity set 'HaverNiagaraContext.NcrProcurements'  is null.");
            }
            var ncrProcurement = await _context.NcrProcurements.FindAsync(id);
            if (ncrProcurement != null)
            {
                _context.NcrProcurements.Remove(ncrProcurement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public JsonResult GetNcrs()
        {
            // Get the list of NcrIds that already exist in NcrOperation
            //List<int> ncrOpPending = _context.Ncrs
            //    .Where(n => n.NcrPhase == NcrPhase.Operations)
            //    .Select(n => n.NcrId)
            //    .ToList();

            //// Include related data in the query for NcrEng
            //List<NcrEng> pendings = _context.NcrEngs
            //    .Include(n => n.Ncr)
            //        .ThenInclude(n => n.NcrQa)
            //            .ThenInclude(n => n.Item)
            //                .ThenInclude(n => n.Supplier) 
            //    .Where(ncrEng => !existingNcrIds.Contains(ncrEng.NcrId))
            //    .ToList();

            List<Ncr> pendings = _context.Ncrs
                .Include(n => n.NcrQa).ThenInclude(n => n.Item).ThenInclude(n => n.Supplier)
                .Where(n => n.NcrPhase == NcrPhase.Procurement)
                .ToList();



            // Extract relevant data for the client-side
            var ncrs = pendings.Select(ncr => new
            {
                NcrId = ncr.NcrId,
                NcrNumber = ncr.NcrNumber,
                SupplierName = ncr.NcrQa.Item.Supplier.SupplierName
            }).ToList();

            return Json(ncrs);
        }

        public JsonResult GetPendingCount()
        {
            // Get the list of NcrIds that already exist in NcrOperation
            //List<int> existingNcrIds = _context.NcrOperations.Select(op => op.NcrId).ToList();

            //List<int> ncrOpPending = _context.Ncrs
            //    .Where(n => n.NcrPhase == NcrPhase.Operations)
            //    .Select(n => n.NcrId)
            //    .ToList();


            //// Count only the unique NcrIds in NcrOp
            //int pendingCount = _context.NcrEngs
            //    .Where(ncrEng => ncrOpPending.Contains(ncrEng.NcrId))
            //    .Select(ncrEng => ncrEng.NcrId)
            //    .Distinct()
            //    .Count();

            int pendingCount = _context.Ncrs
                .Where(n => n.NcrPhase == NcrPhase.Operations)
                .Count();

            return Json(pendingCount);
        }
        private bool NcrProcurementExists(int id)
        {
          return _context.NcrProcurements.Any(e => e.NcrProcurementId == id);
        }
    }
}
