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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Runtime.ConstrainedExecution;

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
        public async Task<IActionResult> Index(string SearchCode, int? OpDispositionTypeId, DateTime StartDate, DateTime EndDate,
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
            string[] sortOptions = new[] { "Created", "NCR #", "Supplier", "DispositionType", "Phase", "Creation" };

            PopulateDropDownLists();

            var ncrOperation = _context.NcrOperations
                .Include(n => n.NcrEng)
                .Include(n => n.Ncr)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item).ThenInclude(n => n.Supplier)
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
            if (OpDispositionTypeId.HasValue)
            {
                ncrOperation = ncrOperation.Where(n => n.OpDispositionType.OpDispositionTypeId == OpDispositionTypeId);
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
            else if (sortField == "Supplier")
            {
                if (sortDirection == "asc")
                {
                    ncrOperation = ncrOperation
                        .OrderBy(p => p.Ncr.NcrQa.Item.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrOperation = ncrOperation
                        .OrderByDescending(p => p.Ncr.NcrQa.Item.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-down'></i>";
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
            else if (sortField == "Creation")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncrOperation = ncrOperation
                        .OrderBy(p => p.Ncr.NcrQa.NcrQacreationDate);

                    ViewData["filterApplied:Creation"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrOperation = ncrOperation
                        .OrderByDescending(p => p.Ncr.NcrQa.NcrQacreationDate);

                    ViewData["filterApplied:Creation"] = "<i class='bi bi-sort-down'></i>";
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
            else if (sortField == "DispositionType")
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
            else //(sortField == "Phase")
            {
                if (sortDirection == "asc")
                {
                    ncrOperation = ncrOperation
                        .OrderBy(p => p.Ncr.NcrPhase);
                    ViewData["filterApplied:Phase"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrOperation = ncrOperation
                        .OrderByDescending(p => p.Ncr.NcrPhase);
                    ViewData["filterApplied:Phase"] = "<i class='bi bi-sort-down'></i>";
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
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item).ThenInclude(n => n.ItemDefects)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item).ThenInclude(n => n.ItemDefects).ThenInclude(n => n.Defect)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item).ThenInclude(n => n.Supplier)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.ItemDefectPhotos)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrEng)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrEng).ThenInclude(n => n.EngDispositionType)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrEng).ThenInclude(n => n.Drawing)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrEng).ThenInclude(n => n.EngDefectPhotos)
                .Include(n => n.OpDispositionType)
                .Include(n => n.OpDefectPhotos)
                .FirstOrDefaultAsync(m => m.NcrOpId == id);
            if (ncrOperation == null)
            {
                return NotFound();
            }

            ViewBag.IsNCRQaView = false;
            ViewBag.IsNCREngView = false;
            ViewBag.IsNCROpView = true;
            return View(ncrOperation);
        }

        // GET: NcrOperation/Create
        public IActionResult Create(string ncrNumber)
        {
            NcrOperationDTO ncr = new NcrOperationDTO();
            ncr.NcrNumber = ncrNumber; // Set the NcrNumber from the parameter
            ncr.NcrOpCreationDate = DateTime.Now;
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
        public async Task<IActionResult> Create(NcrOperationDTO ncrOperationDTO, int OpDispositionTypeId, List<IFormFile> Photos)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    int ncrIdObt = _context.Ncrs
                        .Where(n => n.NcrNumber == ncrOperationDTO.NcrNumber)
                        .Select(n => n.NcrId)
                        .FirstOrDefault();

                    await AddPictures(ncrOperationDTO, Photos);
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
                        UpdateOp = DateTime.Today,
                        NcrPurchasingUserId = 1,
                        OpDefectPhotos = ncrOperationDTO.OpDefectPhotos,
                        NcrOperationVideo = ncrOperationDTO.NcrOperationVideo

                    };
                    _context.NcrOperations.Add(ncrOperation);
                    await _context.SaveChangesAsync();

                    //update ncr 
                    var ncr = await _context.Ncrs.AsNoTracking().FirstOrDefaultAsync(n => n.NcrId == ncrIdObt);
                    ncr.NcrPhase = NcrPhase.Procurement;
                    _context.Ncrs.Update(ncr);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "NCR saved successfully!";
                    int ncrOpId = ncrOperation.NcrOpId;
                    return RedirectToAction("Details", new { id = ncrOpId });
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

            var ncrOperation = await _context.NcrOperations
                .Include(n => n.NcrEng)
                .Include(n => n.Ncr)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item).ThenInclude(n => n.Supplier)
                .Include(n => n.OpDispositionType)
                .Include(n => n.FollowUpType)
                .Include(n => n.OpDefectPhotos)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NcrOpId == id);

            if (ncrOperation == null)
            {
                return NotFound();
            }

            var ncrOperationDTO = new NcrOperationDTO
            {
                NcrOpId = ncrOperation.NcrOpId,
                NcrNumber = ncrOperation.Ncr.NcrNumber,
                NcrOpCreationDate = ncrOperation.Ncr.NcrQa.NcrQacreationDate,
                NcrId = ncrOperation.NcrId,
                Ncr = ncrOperation.Ncr,
                OpDispositionTypeId = ncrOperation.OpDispositionTypeId,
                OpDispositionType = ncrOperation.OpDispositionType,
                NcrPurchasingDescription = ncrOperation.NcrPurchasingDescription,
                Car = ncrOperation.Car,
                CarNumber = ncrOperation.CarNumber,
                FollowUp = ncrOperation.FollowUp,
                ExpectedDate = ncrOperation.ExpectedDate,
                FollowUpTypeId = ncrOperation.FollowUpTypeId,

                UpdateOp = ncrOperation.UpdateOp,
                NcrPurchasingUserId = ncrOperation.NcrPurchasingUserId,
                NcrEng = ncrOperation.NcrEng,
                NcrOperationVideo = ncrOperation.NcrOperationVideo,
                OpDefectPhotos = ncrOperation.OpDefectPhotos
            };


            ViewData["FollowUpTypeId"] = new SelectList(_context.FollowUpTypes, "FollowUpTypeId", "FollowUpTypeName", ncrOperation.FollowUpTypeId);
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrOperation.NcrId);
            ViewData["OpDispositionTypeId"] = new SelectList(_context.OpDispositionTypes, "OpDispositionTypeId", "OpDispositionTypeName", ncrOperation.OpDispositionTypeId);
            return View(ncrOperationDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int NcrOpId, int id, int NcrId, NcrOperationDTO ncrOperationDTO, NcrOperation ncrOperation, List<IFormFile> Photos)
        {
            if (ModelState.IsValid)
            {
                var ncrToUpdate = await _context.Ncrs
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NcrId == NcrId);

                if (id != ncrOperationDTO.NcrId)
                {
                    return NotFound();
                }

                // Go get the ncrOperation to update
                var ncrOperationToUpdate = await _context.NcrOperations
                    .Include(n => n.Ncr)
                    //.Include(n => n.NcrEng)
                    .Include(n => n.OpDispositionType)
                    .Include(n => n.FollowUpType)
                    .Include(n => n.OpDefectPhotos)
                    //.AsNoTracking()
                    .FirstOrDefaultAsync(n => n.NcrOpId == NcrOpId);

                // Check that we got the function or exit with a not found error
                if (ncrOperationToUpdate == null)
                {
                    return NotFound();
                }
                else
                {
                    await AddPictures(ncrOperationDTO, Photos);
                    try
                    {
                        //Update the related Ncr entity
                        ncrOperationToUpdate.Ncr.NcrPhase = NcrPhase.Procurement;
                        _context.Ncrs.Update(ncrOperationToUpdate.Ncr);
                        await _context.SaveChangesAsync();


                        // Update the NcrOperation entity
                        ncrOperationToUpdate.OpDispositionTypeId = ncrOperation.OpDispositionTypeId;
                        ncrOperationToUpdate.NcrPurchasingDescription = ncrOperation.NcrPurchasingDescription;
                        ncrOperationToUpdate.Car = ncrOperation.Car;
                        ncrOperationToUpdate.CarNumber = ncrOperation.CarNumber;
                        ncrOperationToUpdate.FollowUp = ncrOperation.FollowUp;
                        ncrOperationToUpdate.ExpectedDate = ncrOperationToUpdate.ExpectedDate;
                        ncrOperationToUpdate.FollowUpTypeId = ncrOperation.FollowUpTypeId;
                        ncrOperationToUpdate.UpdateOp = DateTime.Today;
                        ncrOperationToUpdate.NcrPurchasingUserId = ncrOperation.NcrPurchasingUserId;
                        ncrOperationToUpdate.NcrOperationVideo = ncrOperation.NcrOperationVideo;
                        ncrOperationToUpdate.OpDefectPhotos = ncrOperation.OpDefectPhotos;

                        _context.NcrOperations.Update(ncrOperationToUpdate);
                        await _context.SaveChangesAsync();

                        TempData["SuccessMessage"] = "NCR edited successfully!";
                        int updateNcrOperation = ncrOperationToUpdate.NcrOpId;
                        return RedirectToAction("Details", new { id = updateNcrOperation });
                    }
                    catch (RetryLimitExceededException)
                    {
                        ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            }

            PopulateDropDownLists();
            return View(ncrOperationDTO);
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
                .Where(n => n.NcrPhase == NcrPhase.Operations)
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

        public async Task<FileContentResult> Download(int id)
        {
            var theFile = await _context.OpDefectPhotos
                .Include(d => d.OpFileContent)
                .Where(f => f.OpDefectPhotoId == id)
                .FirstOrDefaultAsync();
            return File(theFile.OpDefectPhotoContent, theFile.OpDefectPhotoMimeType, theFile.FileName);
        }

        private async Task AddPictures(NcrOperationDTO ncrOperationDTO, List<IFormFile> pictures)
        {
            if (pictures != null && pictures.Any())
            {
                ncrOperationDTO.OpDefectPhotos = new List<OpDefectPhoto>();

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

                            ncrOperationDTO.OpDefectPhotos.Add(new OpDefectPhoto
                            {
                                OpDefectPhotoContent = ResizeImage.shrinkImageWebp(pictureArray, 500, 600),
                                OpDefectPhotoMimeType = "image/webp",
                                FileName = picture.FileName
                            });
                        }
                    }
                }
            }
        }

        public JsonResult GetPendingCount()
        {
            // Get the list of NcrIds that already exist in NcrOperation
            //List<int> existingNcrIds = _context.NcrOperations.Select(op => op.NcrId).ToList();

            //List<int> ncrOpPending = _context.Ncrs
            //    .Where(n => n.NcrPhase == NcrPhase.Engineer)
            //    .Select(n => n.NcrId)
            //    .ToList();


            //// Count only the unique NcrIds in NcrEngs
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
