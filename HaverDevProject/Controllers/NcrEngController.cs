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
using NuGet.Protocol;

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
        public async Task<IActionResult> Index(string SearchCode, int? EngDispositionTypeId, DateTime StartDate, DateTime EndDate,
            int? page, int? pageSizeID, string actionButton, string sortDirection = "desc", string sortField = "Created", string filter = "Active")
        {
            //Set the date range filer based on the values in the database
            if (EndDate == DateTime.MinValue)
            {
                StartDate = _context.Ncrs
                .Min(f => f.NcrLastUpdated.Date);

                EndDate = _context.Ncrs
                .Max(f => f.NcrLastUpdated.Date);

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
            string[] sortOptions = new[] { "Creation Date", "NCR #", "Supplier", "Disposition", "Phase", "Last Updated" };

            
            PopulateDropDownLists();

            var ncrEng = _context.NcrEngs
                .Include(n => n.EngDispositionType)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa)
                .Include(n => n.Ncr)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item).ThenInclude(n => n.Supplier)
                .Include(n => n.Drawing)
                //.Where(n => n.Ncr.NcrPhase == NcrPhase.Operations)
                .AsNoTracking();

            
            GetNcrs();

            //Filtering values            
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
            if (EngDispositionTypeId.HasValue)
            {
                ncrEng = ncrEng.Where(n => n.EngDispositionType.EngDispositionTypeId == EngDispositionTypeId);
            }
            if (StartDate == EndDate)
            {
                ncrEng = ncrEng.Where(n => n.Ncr.NcrLastUpdated == StartDate);
            }
            else
            {
                ncrEng = ncrEng.Where(n => n.Ncr.NcrLastUpdated >= StartDate &&
                         n.Ncr.NcrLastUpdated <= EndDate);
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
            else if (sortField == "Supplier")
            {
                if (sortDirection == "asc")
                {
                    ncrEng = ncrEng
                        .OrderBy(p => p.Ncr.NcrQa.Item.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrEng = ncrEng
                        .OrderByDescending(p => p.Ncr.NcrQa.Item.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-down'></i>";
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

            else if (sortField == "Creation Date")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncrEng = ncrEng
                        .OrderBy(p => p.Ncr.NcrQa.NcrQacreationDate);

                    ViewData["filterApplied:Creation Date"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrEng = ncrEng
                        .OrderByDescending(p => p.Ncr.NcrQa.NcrQacreationDate);

                    ViewData["filterApplied:Creation Date"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Phase")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncrEng = ncrEng
                        .OrderBy(p => p.Ncr.NcrPhase);

                    ViewData["filterApplied:Phase"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrEng = ncrEng
                        .OrderByDescending(p => p.Ncr.NcrPhase);

                    ViewData["filterApplied:Phase"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Last Updated")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncrEng = ncrEng
                        .OrderBy(p => p.Ncr.NcrLastUpdated);

                    ViewData["filterApplied:Last Updated"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrEng = ncrEng
                        .OrderByDescending(p => p.Ncr.NcrLastUpdated);

                    ViewData["filterApplied:Last Updated"] = "<i class='bi bi-sort-down'></i>";
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
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa)
                .Include(n => n.Ncr)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item).ThenInclude(n => n.ItemDefects)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item).ThenInclude(n => n.ItemDefects).ThenInclude(n => n.Defect)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item).ThenInclude(n => n.Supplier)
                .Include(n => n.Drawing)
                .Include(n => n.EngDefectPhotos)
                .FirstOrDefaultAsync(m => m.NcrEngId == id);

            if (ncrEng == null)
            {
                return NotFound();
            }

            ViewBag.IsNCRQaView = false;
            ViewBag.IsNCREngView = true;
            return View(ncrEng);
        }


        // GET: NcrEng/Create
        public IActionResult Create(string ncrNumber)
        {
            NcrEngDTO ncr = new NcrEngDTO();
            ncr.NcrNumber = ncrNumber; // Set the NcrNumber from the parameter
            ncr.DrawingRevDate = DateTime.Now;
            ncr.NcrEngCreationDate = DateTime.Now;
            ncr.DrawingOriginalRevNumber = 1;
            ncr.DrawingRequireUpdating = false;
            ncr.NcrEngCustomerNotification = false;

            //ncr.NcrStatus = true; // Active

            PopulateDropDownLists();
            //ViewData["EngDispositionTypeId"] = new SelectList(_context.EngDispositionTypes, "EngDispositionTypeId", "EngDispositionTypeName");
            return View(ncr);
        }

        // POST: NcrEng/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NcrEngDTO ncrEngDTO, List<IFormFile> Photos)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Find the Ncr entity based on the NcrNumber in the DTO
                    int ncrIdObt = _context.Ncrs
                        .Where(n => n.NcrNumber == ncrEngDTO.NcrNumber)
                        .Select(n => n.NcrId)
                        .FirstOrDefault();

                    await AddPictures(ncrEngDTO, Photos);

                    NcrEng ncrEng = new NcrEng
                    {
                        NcrId = ncrIdObt, // Assign the NcrId from the found Ncr entity
                        NcrEngCustomerNotification = ncrEngDTO.NcrEngCustomerNotification,
                        NcrEngDispositionDescription = ncrEngDTO.NcrEngDispositionDescription,
                        NcrEngStatusFlag = ncrEngDTO.NcrEngStatusFlag,
                        NcrEngUserId = 1,
                        NcrEngCreationDate = DateTime.Now,
                        EngDispositionTypeId = ncrEngDTO.EngDispositionTypeId,
                        DrawingId = ncrEngDTO.DrawingId,
                        DrawingRequireUpdating = false,
                        DrawingOriginalRevNumber = ncrEngDTO.DrawingOriginalRevNumber,
                        DrawingUpdatedRevNumber = ncrEngDTO.DrawingUpdatedRevNumber,
                        DrawingRevDate = DateTime.Now,
                        DrawingUserId = ncrEngDTO.DrawingUserId,
                        EngDefectPhotos = ncrEngDTO.EngDefectPhotos,
                        NcrEngDefectVideo = ncrEngDTO.NcrEngDefectVideo,
                        NcrPhase = NcrPhase.Operations
                    };
                    _context.NcrEngs.Add(ncrEng);
                    await _context.SaveChangesAsync();

                    //update ncr 
                    var ncr = await _context.Ncrs.AsNoTracking().FirstOrDefaultAsync(n => n.NcrId == ncrIdObt);
                    ncr.NcrPhase = NcrPhase.Operations;
                    ncr.NcrLastUpdated = DateTime.Now;
                    _context.Ncrs.Update(ncr);
                    await _context.SaveChangesAsync();



                    TempData["SuccessMessage"] = "NCR saved successfully!";
                    int ncrEngId = ncrEng.NcrEngId;
                    return RedirectToAction("Details", new { id = ncrEngId });

                    //return RedirectToAction(nameof(Index));
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
            //ViewData["EngDispositionTypeId"] = new SelectList(_context.EngDispositionTypes, "EngDispositionTypeId", "EngDispositionTypeName", ncrEngDTO.EngDispositionTypeId);
            return View(ncrEngDTO);
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
                        .Include(n => n.EngDefectPhotos)
                        .AsNoTracking()
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
                //NcrStatus = ncrEng.Ncr.NcrStatus,
                NcrEngCustomerNotification = ncrEng.NcrEngCustomerNotification,
                NcrEngDispositionDescription = ncrEng.NcrEngDispositionDescription,
                NcrEngCreationDate = ncrEng.NcrEngCreationDate,
                NcrEngStatusFlag = ncrEng.NcrEngStatusFlag,
                NcrEngUserId = ncrEng.NcrEngUserId,
                EngDispositionTypeId = ncrEng.EngDispositionTypeId,
                NcrId = ncrEng.NcrId,
                DrawingId = ncrEng.DrawingId,
                DrawingRequireUpdating = ncrEng.DrawingRequireUpdating,
                DrawingOriginalRevNumber = ncrEng.DrawingOriginalRevNumber,
                DrawingUpdatedRevNumber = ncrEng.DrawingUpdatedRevNumber,
                DrawingRevDate = DateTime.Now,
                DrawingUserId = ncrEng.DrawingUserId,
                EngDefectPhotos = ncrEng.EngDefectPhotos,
                NcrEngDefectVideo = ncrEng.NcrEngDefectVideo,
                NcrPhase = ncrEng.NcrPhase

            };


            ViewData["EngDispositionTypeId"] = new SelectList(_context.EngDispositionTypes, "EngDispositionTypeId", "EngDispositionTypeName", ncrEng.EngDispositionTypeId);
            return View(ncrEngDTO);
        }

        // POST: NcrEng/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NcrEngDTO ncrEngDTO, List<IFormFile> Photos)
        {
            ncrEngDTO.NcrId = id;
            if (id != ncrEngDTO.NcrId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await AddPictures(ncrEngDTO, Photos);
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
                    ncrEng.NcrEngCreationDate = ncrEngDTO.NcrEngCreationDate;
                    ncrEng.NcrEngStatusFlag = ncrEngDTO.NcrEngStatusFlag;
                    ncrEng.NcrEngUserId = ncrEngDTO.NcrEngUserId;
                    ncrEng.EngDispositionTypeId = ncrEngDTO.EngDispositionTypeId;
                    ncrEng.NcrId = ncrEngDTO.NcrId;
                    ncrEng.DrawingId = ncrEngDTO.DrawingId;
                    ncrEng.DrawingRequireUpdating = ncrEngDTO.DrawingRequireUpdating;
                    ncrEng.DrawingOriginalRevNumber = ncrEngDTO.DrawingOriginalRevNumber;
                    ncrEng.DrawingUpdatedRevNumber = ncrEngDTO.DrawingUpdatedRevNumber;
                    ncrEng.DrawingRevDate = DateTime.Now;
                    ncrEng.DrawingUserId = ncrEngDTO.DrawingUserId;
                    ncrEng.EngDefectPhotos = ncrEngDTO.EngDefectPhotos;
                    ncrEng.NcrEngDefectVideo = ncrEngDTO.NcrEngDefectVideo;
                    ncrEng.NcrPhase = NcrPhase.Operations;


                    await _context.Ncrs.AsNoTracking().FirstOrDefaultAsync(n => n.NcrId == ncrEng.NcrId);
                    ncrEng.NcrPhase = NcrPhase.Operations;
                    //ncrEng.Ncr.NcrLastUpdated = DateTime.Now;
                    _context.Update(ncrEng);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "NCR edited successfully!";
                    int ncrEngId = ncrEng.NcrEngId;
                    return RedirectToAction("Details", new { id = ncrEngId });
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
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }


                //return RedirectToAction(nameof(Index));
            }
            return View(ncrEngDTO);
        }


        public JsonResult GetNcrs()
        {

            List<Ncr> pendings = _context.Ncrs
                .Include(n => n.NcrQa).ThenInclude(n => n.Item).ThenInclude(n => n.Supplier)
                .Where(n => n.NcrPhase == NcrPhase.Engineer)
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

            int pendingCount = _context.Ncrs
                .Where(n => n.NcrPhase == NcrPhase.Engineer)
                .Count();

            return Json(pendingCount);
        }


        private async Task AddPictures(NcrEngDTO ncrEngDTO, List<IFormFile> pictures)
        {
            if (pictures != null && pictures.Any())
            {
                ncrEngDTO.EngDefectPhotos = new List<EngDefectPhoto>();

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

                            ncrEngDTO.EngDefectPhotos.Add(new EngDefectPhoto
                            {
                                EngDefectPhotoContent = ResizeImage.shrinkImageWebp(pictureArray, 500, 600),
                                EngDefectPhotoMimeType = "image/webp",
                                FileName = picture.FileName
                            });
                        }
                    }
                }
            }
        }

        public async Task<FileContentResult> Download(int id)
        {
            var theFile = await _context.EngDefectPhotos
                .Include(d => d.EngFileContent)
                .Where(f => f.EngDefectPhotoId == id)
                .FirstOrDefaultAsync();
            return File(theFile.EngDefectPhotoContent, theFile.EngDefectPhotoMimeType, theFile.FileName);
        }


        private bool NcrEngExists(int id)
        {
            return _context.NcrEngs.Any(e => e.NcrEngId == id);
        }

        private SelectList EngDispositionTypeSelectList(int? selectedId)
        {
            return new SelectList(_context.EngDispositionTypes
                .OrderBy(s => s.EngDispositionTypeName), "EngDispositionTypeId", "EngDispositionTypeName", selectedId);
        }
        private void PopulateDropDownLists(NcrEng ncrEng = null)
        {
            if ((ncrEng?.EngDispositionTypeId).HasValue)
            {
                if (ncrEng.EngDispositionType == null)
                {
                    ncrEng.EngDispositionType = _context.EngDispositionTypes.Find(ncrEng.EngDispositionTypeId);
                }
                ViewData["EngDispositionTypeId"] = EngDispositionTypeSelectList(ncrEng?.EngDispositionTypeId);
            }
            else
            {
                ViewData["EngDispositionTypeId"] = EngDispositionTypeSelectList(null);
            }
        }
    }
}
