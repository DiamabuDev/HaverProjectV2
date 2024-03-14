using System;
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
using Microsoft.AspNetCore.Authorization;

namespace HaverDevProject.Controllers
{
    [Authorize]
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

            ViewData["Filtering"] = "btn-block invisible";
            int numberFilters = 0;

            //Set the date range filer based on the values in the database
            if (EndDate == DateTime.MinValue)
            {
                StartDate = _context.Ncrs
                .Min(f => f.NcrQa.NcrQacreationDate.Date);

                EndDate = _context.Ncrs
                .Max(f => f.NcrQa.NcrQacreationDate.Date);

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

            //Check the order of the dates and swap them if required
            if (EndDate < StartDate)
            {
                DateTime temp = EndDate;
                EndDate = StartDate;
                StartDate = temp;
            }

            //List of sort options.
            string[] sortOptions = new[] { "Created", "NCR #", "Supplier", "SupplierReturn", "SAP", "Phase", "Last Updated" };

            ViewData["SupplierId"] = SupplierSelectList();

            var ncrProc = _context.NcrProcurements
                .Include(n => n.Ncr)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Supplier)
                //.Where(n => n.Ncr.NcrPhase == NcrPhase.ReInspection)
                .AsNoTracking();

            GetNcrs();

            //Filterig values            
            if (!String.IsNullOrEmpty(filter))
            {
                if (filter == "All")
                {
                    ViewData["filterApplied:ButtonAll"] = "btn-primary";
                    ViewData["filterApplied:ButtonActive"] = "btn-success custom-opacity";
                    ViewData["filterApplied:ButtonClosed"] = "btn-danger custom-opacity";
                }
                else if (filter == "Active")
                {
                    ncrProc = ncrProc.Where(n => n.Ncr.NcrStatus == true);
                    ViewData["filterApplied:ButtonActive"] = "btn-success";
                    ViewData["filterApplied:ButtonAll"] = "btn-primary custom-opacity";
                    ViewData["filterApplied:ButtonClosed"] = "btn-danger custom-opacity";
                }
                else //(filter == "Closed")
                {
                    ncrProc = ncrProc.Where(n => n.Ncr.NcrStatus == false);
                    ViewData["filterApplied:ButtonClosed"] = "btn-danger";
                    ViewData["filterApplied:ButtonAll"] = "btn-primary custom-opacity";
                    ViewData["filterApplied:ButtonActive"] = "btn-success custom-opacity";
                }
            }
            if (!String.IsNullOrEmpty(SearchCode))
            {
                ncrProc = ncrProc.Where(s => s.Ncr.NcrNumber.ToUpper().Contains(SearchCode.ToUpper()));
                numberFilters++;
            }
            if (SupplierID.HasValue)
            {
                ncrProc = ncrProc.Where(n => n.Ncr.NcrQa.Supplier.SupplierId == SupplierID);
                numberFilters++;
            }
            if (StartDate == EndDate)
            {
                ncrProc = ncrProc.Where(n => n.Ncr.NcrQa.NcrQacreationDate == StartDate);
                numberFilters++;
            }
            else
            {
                ncrProc = ncrProc.Where(n => n.Ncr.NcrQa.NcrQacreationDate >= StartDate &&
                         n.Ncr.NcrQa.NcrQacreationDate <= EndDate);
            }

            //keep track of the number of filters 
            if (numberFilters != 0)
            {
                ViewData["Filtering"] = " btn-danger";
                ViewData["numberFilters"] = "(" + numberFilters.ToString()
                    + " Filter" + (numberFilters > 1 ? "s" : "") + " Applied)";
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
            else if (sortField == "Supplier")
            {
                if (sortDirection == "asc")
                {
                    ncrProc = ncrProc
                        .OrderBy(p => p.Ncr.NcrQa.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrProc = ncrProc
                        .OrderByDescending(p => p.Ncr.NcrQa.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-down'></i>";
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
            else if (sortField == "SAP")
            {
                if (sortDirection == "asc")
                {
                    ncrProc = ncrProc
                        .OrderBy(p => p.NcrProcSAPReturnCompleted);
                    ViewData["filterApplied:SAP"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrProc = ncrProc
                        .OrderByDescending(p => p.NcrProcSAPReturnCompleted);
                    ViewData["filterApplied:SAP"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Created")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncrProc = ncrProc
                        .OrderBy(p => p.Ncr.NcrQa.NcrQacreationDate);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrProc = ncrProc
                        .OrderByDescending(p => p.Ncr.NcrQa.NcrQacreationDate);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Phase")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncrProc = ncrProc
                        .OrderBy(p => p.Ncr.NcrPhase);

                    ViewData["filterApplied:Phase"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrProc = ncrProc
                        .OrderByDescending(p => p.Ncr.NcrPhase);

                    ViewData["filterApplied:Phase"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else /*if (sortField == "Last Updated")*/
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncrProc = ncrProc
                        .OrderBy(p => p.Ncr.NcrLastUpdated);

                    ViewData["filterApplied:Last Updated"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrProc = ncrProc
                        .OrderByDescending(p => p.Ncr.NcrLastUpdated);

                    ViewData["filterApplied:Last Updated"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            //else //(sortField == "Status")
            //{
            //    if (sortDirection == "asc")
            //    {
            //        ncrProc = ncrProc
            //            .OrderBy(p => p.Ncr.NcrStatus);
            //        ViewData["filterApplied:Status"] = "<i class='bi bi-sort-up'></i>";
            //    }
            //    else
            //    {
            //        ncrProc = ncrProc
            //            .OrderByDescending(p => p.Ncr.NcrStatus);
            //        ViewData["filterApplied:Status"] = "<i class='bi bi-sort-down'></i>";
            //    }
            //}

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;
            ViewData["filter"] = filter;

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

            var ncrProcurement = await _context.NcrProcurements
                .Include(n => n.Ncr)
                .Include(n => n.ProcDefectPhotos)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Defect)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Supplier)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.ItemDefectPhotos)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrEng)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrEng).ThenInclude(n => n.EngDispositionType)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrEng).ThenInclude(n => n.Drawing)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrEng).ThenInclude(n => n.EngDefectPhotos)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrOperation)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrOperation).ThenInclude(n => n.OpDispositionType)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrOperation).ThenInclude(n => n.FollowUpType)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrOperation).ThenInclude(n => n.OpDefectPhotos)
                .Include(n => n.ProcDefectPhotos)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrReInspect)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrReInspect).ThenInclude(n => n.NcrReInspectPhotos)
                .FirstOrDefaultAsync(m => m.NcrProcurementId == id);
            if (ncrProcurement == null)
            {
                return NotFound();
            }

            ViewBag.IsNCRQaView = false;
            ViewBag.IsNCREngView = false;
            ViewBag.IsNCROpView = false;
            ViewBag.IsNCRProcView = true;
            ViewBag.IsNCRReInspView = false;

            ViewBag.NCRSectionId = id;

            return View(ncrProcurement);
        }

        // GET: NcrProcurement/Create
        public async Task<IActionResult> Create(string ncrNumber)
        {
            int ncrId = _context.Ncrs.Where(n => n.NcrNumber == ncrNumber).Select(n => n.NcrId).FirstOrDefault();

            NcrProcurementDTO ncr = new NcrProcurementDTO();
            ncr.NcrNumber = ncrNumber;
            ncr.NcrProcCompleteDate = DateTime.Now;
            ncr.NcrProcExpectedDate = DateTime.Now;
            ncr.NcrProcSupplierReturnReq = true;
            ncr.NcrStatus = true; //Active

            var readOnlyDetails = await _context.Ncrs
                .Include(n => n.NcrQa)
                        .ThenInclude(item => item.Supplier)
                .Include(n => n.NcrQa)
                            .ThenInclude(defect => defect.Defect)
                .Include(n => n.NcrQa)
                            .ThenInclude(i => i.Item)
                .Include(n => n.NcrQa)
                    .ThenInclude(qa => qa.ItemDefectPhotos)
                .Include(n => n.NcrEng)
                    .ThenInclude(eng => eng.EngDispositionType)
                .Include(n => n.NcrEng)
                    .ThenInclude(eng => eng.Drawing)
                .Include(n => n.NcrEng)
                    .ThenInclude(eng => eng.EngDefectPhotos)
                .Include(n => n.NcrOperation)
                    .ThenInclude(op => op.OpDispositionType)
                .Include(n => n.NcrOperation)
                    .ThenInclude(op => op.FollowUpType)
                .Include(n => n.NcrOperation)
                    .ThenInclude(op => op.OpDefectPhotos)
                .FirstOrDefaultAsync(n => n.NcrId == ncrId);

            ViewBag.IsNCRQaView = false;
            ViewBag.IsNCREngView = false;
            ViewBag.IsNCROpView = false;
            ViewBag.IsNCRProcView = false;
            ViewBag.IsNCRReInspView = false;

            ViewBag.ncrDetails = readOnlyDetails;

            return View(ncr);
        }

        // POST: NcrProcurement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NcrProcurementDTO ncrProcDTO, List<IFormFile> photos)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    int ncrIdObt = _context.Ncrs
                        .Where(n => n.NcrNumber == ncrProcDTO.NcrNumber)
                        .Select(n => n.NcrId)
                        .FirstOrDefault();

                    await AddPictures(ncrProcDTO, photos);

                    NcrProcurement ncrProc = new NcrProcurement
                    {
                        NcrId = ncrIdObt,
                        NcrProcurementId = ncrProcDTO.NcrProcurementId,
                        NcrProcSupplierReturnReq = ncrProcDTO.NcrProcSupplierReturnReq,
                        NcrProcExpectedDate = ncrProcDTO.NcrProcExpectedDate,
                        NcrProcDisposedAllowed = ncrProcDTO.NcrProcDisposedAllowed,
                        NcrProcSAPReturnCompleted = ncrProcDTO.NcrProcSAPReturnCompleted,
                        NcrProcCreditExpected = ncrProcDTO.NcrProcCreditExpected,
                        NcrProcSupplierBilled = ncrProcDTO.NcrProcSupplierBilled,
                        NcrProcRejectedValue = ncrProcDTO.NcrProcRejectedValue,
                        NcrProcFlagStatus = ncrProcDTO.NcrProcFlagStatus,
                        NcrProcUserId = ncrProcDTO.NcrProcUserId,
                        NcrProcCompleteDate = DateTime.Now,
                        NcrProcCreated = ncrProcDTO.NcrProcCreated,
                        SupplierReturnMANum = ncrProcDTO.SupplierReturnMANum,
                        SupplierReturnName = ncrProcDTO.SupplierReturnName,
                        SupplierReturnAccount = ncrProcDTO.SupplierReturnAccount,
                        NcrProcDefectVideo = ncrProcDTO.NcrProcDefectVideo,
                        ProcDefectPhotos = ncrProcDTO.ProcDefectPhotos,
                        //NcrPhase = NcrPhase.ReInspection
                    };


                    _context.NcrProcurements.Add(ncrProc);
                    await _context.SaveChangesAsync();

                    //update ncr
                    var ncr = await _context.Ncrs.AsNoTracking().FirstOrDefaultAsync(n => n.NcrId == ncrIdObt);
                    ncr.NcrPhase = NcrPhase.ReInspection;
                    ncr.NcrLastUpdated = DateTime.Now;
                    _context.Ncrs.Update(ncr);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "NCR " + ncr.NcrNumber + " saved successfully!";
                    int ncrProcId = ncrProc.NcrProcurementId;
                    return RedirectToAction("Details", new { id = ncrProcId });
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
                .Include(n => n.ProcDefectPhotos)
                //.Include(n => n.SupplierReturn)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NcrProcurementId == id);

            if (ncrProc == null)
            {
                return NotFound();
            }

            var ncrProcDTO = new NcrProcurementDTO
            {
                NcrNumber = ncrProc.Ncr.NcrNumber,
                NcrStatus = ncrProc.Ncr.NcrStatus,
                NcrProcurementId = ncrProc.NcrProcurementId,
                NcrProcSupplierReturnReq = ncrProc.NcrProcSupplierReturnReq,
                NcrProcExpectedDate = ncrProc.NcrProcExpectedDate,
                NcrProcDisposedAllowed = ncrProc.NcrProcDisposedAllowed,
                NcrProcSAPReturnCompleted = ncrProc.NcrProcSAPReturnCompleted,
                NcrProcCreditExpected = ncrProc.NcrProcCreditExpected,
                NcrProcSupplierBilled = ncrProc.NcrProcSupplierBilled,
                NcrProcRejectedValue = ncrProc.NcrProcRejectedValue,
                NcrProcFlagStatus = ncrProc.NcrProcFlagStatus,
                NcrProcUserId = ncrProc.NcrProcUserId,
                NcrProcCreated = ncrProc.NcrProcCreated,
                NcrProcCompleteDate = ncrProc.NcrProcCompleteDate,
                NcrId = ncrProc.NcrId,
                SupplierReturnMANum = ncrProc.SupplierReturnMANum,
                SupplierReturnName = ncrProc.SupplierReturnName,
                SupplierReturnAccount = ncrProc.SupplierReturnAccount,
                NcrProcDefectVideo = ncrProc.NcrProcDefectVideo,
                ProcDefectPhotos = ncrProc.ProcDefectPhotos,
            };

            var readOnlyDetails = await _context.Ncrs
                .Include(n => n.NcrProcurement)
                    .ThenInclude(n => n.ProcDefectPhotos)
                .Include(n => n.NcrQa)
                    .ThenInclude(item => item.Supplier)
                .Include(n => n.NcrQa)
                    .ThenInclude(item => item.Item)
                .Include(n => n.NcrQa)
                            .ThenInclude(defect => defect.Defect)
                .Include(n => n.NcrQa)
                    .ThenInclude(qa => qa.ItemDefectPhotos)
                .Include(n => n.NcrEng)
                    .ThenInclude(eng => eng.EngDispositionType)
                .Include(n => n.NcrEng)
                    .ThenInclude(eng => eng.Drawing)
                .Include(n => n.NcrEng)
                    .ThenInclude(eng => eng.EngDefectPhotos)
                .Include(n => n.NcrOperation)
                    .ThenInclude(op => op.OpDispositionType)
                .Include(n => n.NcrOperation)
                    .ThenInclude(op => op.FollowUpType)
                .Include(n => n.NcrOperation)
                    .ThenInclude(op => op.OpDefectPhotos)
                .Include(n => n.NcrReInspect)
                    .ThenInclude(n => n.NcrReInspectPhotos)
                .FirstOrDefaultAsync(n => n.NcrId == id);

            ViewBag.IsNCRQaView = false;
            ViewBag.IsNCREngView = false;
            ViewBag.IsNCROpView = false;
            ViewBag.IsNCRProcView = true;
            ViewBag.IsNCRReInspView = false;

            ViewBag.ncrDetails = readOnlyDetails;

            //ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrProc.NcrId);
            return View(ncrProcDTO);
        }

        // POST: NcrProcurement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NcrProcurementDTO ncrProcDTO, List<IFormFile> photos)
        {
            ncrProcDTO.NcrId = id;
            if (id != ncrProcDTO.NcrId)
            {
                return NotFound();
            }

            //var ncrProcToUpdate = await _context.NcrProcurements.FirstOrDefaultAsync(n => n.NcrProcurementId == id);

            if (ModelState.IsValid)
            {
                await AddPictures(ncrProcDTO, photos);
                try
                {
                    var ncrProc = await _context.NcrProcurements
                        //.Include(n => n.SupplierReturn)
                        .FirstOrDefaultAsync(n => n.NcrProcurementId == id);

                    ncrProc.NcrProcSupplierReturnReq = ncrProcDTO.NcrProcSupplierReturnReq;
                    ncrProc.NcrProcExpectedDate = ncrProcDTO.NcrProcExpectedDate;
                    ncrProc.NcrProcDisposedAllowed = ncrProcDTO.NcrProcDisposedAllowed;
                    ncrProc.NcrProcSAPReturnCompleted = ncrProcDTO.NcrProcSAPReturnCompleted;
                    ncrProc.NcrProcCreditExpected = ncrProcDTO.NcrProcCreditExpected;
                    ncrProc.NcrProcSupplierBilled = ncrProcDTO.NcrProcSupplierBilled;
                    ncrProc.NcrProcRejectedValue = ncrProcDTO.NcrProcRejectedValue;
                    ncrProc.NcrProcFlagStatus = ncrProcDTO.NcrProcFlagStatus;
                    ncrProc.NcrProcUserId = ncrProcDTO.NcrProcUserId;
                    ncrProc.NcrProcCreated = ncrProcDTO.NcrProcCreated;
                    ncrProc.NcrProcCompleteDate = ncrProcDTO.NcrProcCompleteDate;
                    ncrProc.NcrId = ncrProcDTO.NcrId;
                    ncrProc.SupplierReturnMANum = ncrProcDTO.SupplierReturnMANum;
                    ncrProc.SupplierReturnName = ncrProcDTO.SupplierReturnName;
                    ncrProc.SupplierReturnAccount = ncrProcDTO.SupplierReturnAccount;
                    //ncrProc.SupplierReturnId = ncrProcDTO.SupplierReturnId;
                    ncrProc.NcrProcDefectVideo = ncrProcDTO.NcrProcDefectVideo;
                    ncrProc.ProcDefectPhotos = ncrProcDTO.ProcDefectPhotos;
                    //ncrProc.NcrPhase = NcrPhase.ReInspection;


                    //_context.Update(ncrProc);
                    //await _context.SaveChangesAsync();

                    var ncr = await _context.Ncrs.AsNoTracking().FirstOrDefaultAsync(n => n.NcrId == ncrProc.NcrId);
                    //ncrProc.Ncr.NcrPhase = NcrPhase.ReInspection;
                    ncr.NcrLastUpdated = DateTime.Now;
                    _context.Update(ncr);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "NCR " + ncr.NcrNumber + " edited successfully!";
                    int ncrProcId = ncrProc.NcrProcurementId;
                    return RedirectToAction("Details", new { id = ncrProcId });
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
                //return RedirectToAction(nameof(Index));

            }
            return View(ncrProcDTO);
        }


        //// GET: NcrProcurement/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.NcrProcurements == null)
        //    {
        //        return NotFound();
        //    }

        //    var ncrProcurement = await _context.NcrProcurements
        //        .Include(n => n.Ncr)
        //        .FirstOrDefaultAsync(m => m.NcrProcurementId == id);
        //    if (ncrProcurement == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(ncrProcurement);
        //}

        //// POST: NcrProcurement/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.NcrProcurements == null)
        //    {
        //        return Problem("Entity set 'HaverNiagaraContext.NcrProcurements'  is null.");
        //    }
        //    var ncrProcurement = await _context.NcrProcurements.FindAsync(id);
        //    if (ncrProcurement != null)
        //    {
        //        _context.NcrProcurements.Remove(ncrProcurement);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        public JsonResult GetNcrs()
        {

            List<Ncr> pendings = _context.Ncrs
                .Include(n => n.NcrQa).ThenInclude(n => n.Supplier)
                .Where(n => n.NcrPhase == NcrPhase.Procurement)
                .ToList();

            // Extract relevant data for the client-side
            var ncrs = pendings.Select(ncr => new
            {
                NcrId = ncr.NcrId,
                NcrNumber = ncr.NcrNumber,
                SupplierName = ncr.NcrQa.Supplier.SupplierName
            }).ToList();

            return Json(ncrs);
        }

        public JsonResult GetPendingCount()
        {

            int pendingCount = _context.Ncrs
                .Where(n => n.NcrPhase == NcrPhase.Procurement)
                .Count();

            return Json(pendingCount);
        }

        private async Task AddPictures(NcrProcurementDTO ncrProcDTO, List<IFormFile> pictures)
        {
            if (pictures != null && pictures.Any())
            {
                ncrProcDTO.ProcDefectPhotos = new List<ProcDefectPhoto>();

                foreach (var picture in pictures)
                {
                    string mimeType = picture.ContentType;
                    long fileLength = picture.Length;

                    if (!(mimeType == "" || fileLength == 0))
                    {
                        if (mimeType.Contains("image"))
                        {
                            using var memoryStream = new MemoryStream();
                            await picture.CopyToAsync(memoryStream);
                            var pictureArray = memoryStream.ToArray();

                            ncrProcDTO.ProcDefectPhotos.Add(new ProcDefectPhoto
                            {
                                ProcDefectPhotoContent = ResizeImage.shrinkImageWebp(pictureArray, 500, 600),
                                ProcDefectPhotoMimeType = "image/webp",
                                FileName = picture.FileName
                            });
                        }
                    }
                }
            }
        }

        public async Task<FileContentResult> Download(int id)
        {
            var theFile = await _context.ProcDefectPhotos
                .Include(d => d.ProcFileContent)
                .Where(f => f.ProcDefectPhotoId == id)
                .FirstOrDefaultAsync();
            return File(theFile.ProcDefectPhotoContent, theFile.ProcDefectPhotoMimeType, theFile.FileName);
        }

        private SelectList SupplierSelectList()
        {
            return new SelectList(_context.Suppliers
                .Where(s => s.SupplierStatus == true && s.SupplierName != "NO SUPPLIER PROVIDED")
                .OrderBy(s => s.SupplierName), "SupplierId", "SupplierName");
        }
        //private SelectList SupplierSelectCreateList(int? selectedId)
        //{
        //    return new SelectList(_context.Suppliers
        //        .Where(s => s.SupplierStatus == true && s.SupplierName != "NO SUPPLIER PROVIDED")
        //        .OrderBy(s => s.SupplierName), "SupplierId", "SupplierName", selectedId);
        //}

        private SelectList ItemSelectList()
        {
            return new SelectList(_context.Items
                .OrderBy(s => s.ItemName), "ItemId", "ItemName");

            //var query = from c in _context.Items
            //            where c.SupplierId == SupplierID
            //            select c;
            //return new SelectList(query.OrderBy(i => i.ItemName), "ItemId", "ItemName", selectedId);            
        }

        private void PopulateDropDownLists(NcrQa ncrQa = null)
        {            
                if (ncrQa.Item == null)
                {
                    ncrQa.Item = _context.Items.Find(ncrQa.ItemId);
                }
                ViewData["SupplierId"] = SupplierSelectList();
                ViewData["ItemId"] = ItemSelectList();
            
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var photo = await _context.ProcDefectPhotos.FindAsync(photoId);
            if (photo != null)
            {
                _context.ProcDefectPhotos.Remove(photo);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Photo deleted successfully." });
            }
            return Json(new { success = false, message = "Photo not found." });
        }
        private bool NcrProcurementExists(int id)
        {
            return _context.NcrProcurements.Any(e => e.NcrProcurementId == id);
        }
    }
}
