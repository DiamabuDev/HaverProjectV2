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
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace HaverDevProject.Controllers
{
    [Authorize(Roles = "Engineer, Admin")]
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

            //List of sort options.
            string[] sortOptions = new[] { "Created", "NCR #", "Supplier", "Disposition", "Phase", "Last Updated" };

            
            PopulateDropDownLists();
            GetNcrs();

            var ncrEng = _context.NcrEngs
                .Include(n => n.EngDispositionType)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa)
                .Include(n => n.Ncr)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Item)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Supplier)
                .Include(n => n.Drawing)
                .Where(n => n.Ncr.NcrPhase != NcrPhase.Archive
                //&&
                //(n.Ncr.NcrPhase == NcrPhase.Engineer ||
                // n.Ncr.NcrPhase == NcrPhase.Operations ||
                // n.Ncr.NcrPhase == NcrPhase.Procurement ||
                // n.Ncr.NcrPhase == NcrPhase.ReInspection ||
                // n.Ncr.NcrPhase == NcrPhase.Closed)
                 )
                .AsNoTracking();

            //Filtering values
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
                    ncrEng = ncrEng.Where(n => n.Ncr.NcrStatus == true);
                    ViewData["filterApplied:ButtonActive"] = "btn-success";
                    ViewData["filterApplied:ButtonAll"] = "btn-primary custom-opacity";
                    ViewData["filterApplied:ButtonClosed"] = "btn-danger custom-opacity";
                    
                }
                else //(filter == "Closed")
                {
                    ncrEng = ncrEng.Where(n => n.Ncr.NcrStatus == false);
                    ViewData["filterApplied:ButtonClosed"] = "btn-danger";
                    ViewData["filterApplied:ButtonAll"] = "btn-primary custom-opacity";
                    ViewData["filterApplied:ButtonActive"] = "btn-success custom-opacity";
                    
                }
            }

            if (!String.IsNullOrEmpty(SearchCode))
            {
                ncrEng = ncrEng.Where(s => s.Ncr.NcrNumber.ToUpper().Contains(SearchCode.ToUpper()));
                numberFilters++;
            }
            if (EngDispositionTypeId.HasValue)
            {
                ncrEng = ncrEng.Where(n => n.EngDispositionType.EngDispositionTypeId == EngDispositionTypeId);
                numberFilters++;
            }
            if (StartDate == EndDate)
            {
                ncrEng = ncrEng.Where(n => n.Ncr.NcrQa.NcrQacreationDate == StartDate);
                numberFilters++;
            }
            else 
            {
                ncrEng = ncrEng.Where(n => n.Ncr.NcrQa.NcrQacreationDate >= StartDate &&
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
                        .OrderBy(p => p.Ncr.NcrQa.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrEng = ncrEng
                        .OrderByDescending(p => p.Ncr.NcrQa.Supplier.SupplierName);
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
            ViewData["filter"] = filter;

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
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Defect)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.Supplier)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa).ThenInclude(n => n.ItemDefectPhotos)
                .Include(n => n.Drawing)
                .Include(n => n.EngDefectPhotos)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrOperation)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrOperation).ThenInclude(n => n.OpDispositionType)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrOperation).ThenInclude(n => n.FollowUpType)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrOperation).ThenInclude(n => n.OpDefectPhotos)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrProcurement)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrProcurement).ThenInclude(n => n.ProcDefectPhotos)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrReInspect)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrReInspect).ThenInclude(n => n.NcrReInspectPhotos)
                .FirstOrDefaultAsync(m => m.NcrEngId == id);

            if (ncrEng == null)
            {
                return NotFound();
            }

            ViewBag.IsNCRQaView = false;
            ViewBag.IsNCREngView = true;
            ViewBag.IsNCROpView = false;
            ViewBag.IsNCRProcView = false;
            ViewBag.IsNCRReInspView = false;

            ViewBag.NCRSectionId = id;

            return View(ncrEng);
        }


        // GET: NcrEng/Create
        public async Task<IActionResult> Create(string ncrNumber)
        {
            NcrEngDTO ncrEngDTO;

            int ncrId = _context.Ncrs.Where(n => n.NcrNumber == ncrNumber).Select(n => n.NcrId).FirstOrDefault();

            // Verificar si hay datos de borrador guardados en las cookies
            if (Request.Cookies.TryGetValue("DraftNCREng"+ncrNumber, out string draftJson))
            {
                // Deserializar los datos de borrador desde JSON al modelo de vista
                ncrEngDTO = JsonConvert.DeserializeObject<NcrEngDTO>(draftJson);
                TempData["SuccessMessage"] = "Draft successfully retrieved";
            }
            else
            {
                
                ncrEngDTO = new NcrEngDTO
                {
                    NcrNumber = ncrNumber, // Set the NcrNumber from the parameter
                    DrawingRevDate = DateTime.Now,
                    NcrEngCompleteDate = DateTime.Now,
                    DrawingOriginalRevNumber = 1,
                    DrawingRequireUpdating = false,
                    NcrEngCustomerNotification = false
                };
            }


            //    int ncrId = _context.Ncrs.Where(n => n.NcrNumber == ncrNumber).Select(n => n.NcrId).FirstOrDefault();
            
            //NcrEngDTO ncr = new NcrEngDTO();
            //ncr.NcrNumber = ncrNumber; // Set the NcrNumber from the parameter
            //ncr.DrawingRevDate = DateTime.Now;
            //ncr.NcrEngCompleteDate = DateTime.Now;
            //ncr.DrawingOriginalRevNumber = 1;
            //ncr.DrawingRequireUpdating = false;
            //ncr.NcrEngCustomerNotification = false;

            //ncr.NcrStatus = true; // Active

            var readOnlyDetails = await _context.Ncrs
                .Include(n => n.NcrQa)
                        .ThenInclude(item => item.Supplier)
                .Include(n => n.NcrQa)
                        .ThenInclude(defect => defect.Item)
                .Include(n => n.NcrQa)
                        .ThenInclude(defect => defect.Defect)
                .Include(n => n.NcrQa)
                    .ThenInclude(qa => qa.ItemDefectPhotos)
                .FirstOrDefaultAsync(n => n.NcrId == ncrId);

            //ncr.NcrEngCreationDate = readOnlyDetails.NcrQa.NcrQacreationDate;

            ViewBag.IsNCRQaView = false;
            ViewBag.IsNCREngView = false;
            ViewBag.IsNCROpView = false;
            ViewBag.IsNCRProcView = false;
            ViewBag.IsNCRReInspView = false;

            ViewBag.ncrDetails = readOnlyDetails;

            PopulateDropDownLists();
            return View(ncrEngDTO);
        }

        // POST: NcrEng/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NcrEngDTO ncrEngDTO, List<IFormFile> Photos, bool isDraft = false)
        {
            try
            {
                if (isDraft)
                {
                    // convert the object to json format
                    var json = JsonConvert.SerializeObject(ncrEngDTO);

                    // Save the object in a cookie with name "DraftData"
                    Response.Cookies.Append("DraftNCREng"+ncrEngDTO.NcrNumber, json, new CookieOptions
                    {
                        // Define time for cookies
                        Expires = DateTime.Now.AddMinutes(2880) // Cookied will expire in 48 hrs
                    });

                    return Ok(new { success = true, message = "Draft saved successfully.\nNote: This draft will be available for the next 48 hours." });
                }

                if (ModelState.IsValid)
                {
                    // Find the Ncr entity based on the NcrNumber in the DTO
                    int ncrIdObt = _context.Ncrs
                        .Where(n => n.NcrNumber == ncrEngDTO.NcrNumber)
                        .Select(n => n.NcrId)
                        .FirstOrDefault();

                    //Checking if NcrOperations exist...
                    var ncrOperations = await _context.NcrOperations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(n => n.NcrId == ncrIdObt);
                    bool ncrOperationExist = ncrOperations != null;

                    //Checking if NcrProcurement exist...
                    var ncrProcurement = await _context.NcrProcurements
                    .AsNoTracking()
                    .FirstOrDefaultAsync(n => n.NcrId == ncrIdObt);
                    bool ncrProcurementExist = ncrProcurement != null;

                    //PopulateDropDownLists();
                    await AddPictures(ncrEngDTO, Photos);

                    NcrEng ncrEng = new NcrEng
                    {
                        NcrId = ncrIdObt, // Assign the NcrId from the found Ncr entity
                        NcrEngCustomerNotification = ncrEngDTO.NcrEngCustomerNotification,
                        NcrEngDispositionDescription = ncrEngDTO.NcrEngDispositionDescription,
                        NcrEngStatusFlag = ncrEngDTO.NcrEngStatusFlag,
                        NcrEngUserId = 1,
                        NcrEngCompleteDate = DateTime.Now,
                        NcrEngCreationDate = ncrEngDTO.NcrEngCreationDate,
                        EngDispositionTypeId = ncrEngDTO.EngDispositionTypeId,
                        DrawingId = ncrEngDTO.DrawingId,
                        DrawingRequireUpdating = ncrEngDTO.DrawingRequireUpdating,
                        DrawingOriginalRevNumber = ncrEngDTO.DrawingOriginalRevNumber,
                        DrawingUpdatedRevNumber = ncrEngDTO.DrawingUpdatedRevNumber,
                        DrawingRevDate = DateTime.Now,
                        DrawingUserId = ncrEngDTO.DrawingUserId, 
                        EngDefectPhotos = ncrEngDTO.EngDefectPhotos,
                        NcrEngDefectVideo = ncrEngDTO.NcrEngDefectVideo
                       
                    };
                    _context.NcrEngs.Add(ncrEng);
                    await _context.SaveChangesAsync();

                    //update ncr 
                    var ncr = await _context.Ncrs.AsNoTracking().FirstOrDefaultAsync(n => n.NcrId == ncrIdObt);

                    if (ncrOperationExist == false)
                    {
                        ncr.NcrPhase = NcrPhase.Operations;
                    }
                    else if (ncrOperationExist == true && ncrProcurementExist == false)
                    {
                        ncr.NcrPhase = NcrPhase.Procurement;
                    }
                    else
                    {
                        ncr.NcrPhase = NcrPhase.ReInspection;
                    }

                    //ncr.NcrPhase = NcrPhase.Operations;
                    ncr.NcrLastUpdated = DateTime.Now;
                    _context.Ncrs.Update(ncr);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "NCR " + ncr.NcrNumber + " saved successfully!";
                    //Delete cookies
                    Response.Cookies.Delete("DraftNCREng"+ncr.NcrNumber);
                    int ncrEngId = ncrEng.NcrEngId;
                    return RedirectToAction("Details", new { id = ncrEngId });
                }

            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (DbUpdateException )
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");         
            }

            PopulateDropDownLists();
          
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
                          //.Include(ne => ne.Ncr).ThenInclude(ne=>ne.NcrQa)
                        .Include(n => n.EngDefectPhotos)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(ne => ne.NcrEngId == id);

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
                NcrEngCompleteDate = ncrEng.NcrEngCompleteDate,
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

            var readOnlyDetails = await _context.Ncrs
                .Include(n => n.NcrQa)
                        .ThenInclude(item => item.Supplier)
                .Include(n => n.NcrQa)
                            .ThenInclude(defect => defect.Item)
                .Include(n => n.NcrQa)
                            .ThenInclude(defect => defect.Defect)
                .Include(n => n.NcrQa)
                    .ThenInclude(qa => qa.ItemDefectPhotos)
                .FirstOrDefaultAsync(n => n.NcrId == id);

            ViewBag.IsNCRQaView = false;
            ViewBag.IsNCREngView = false;
            ViewBag.IsNCROpView = false;
            ViewBag.IsNCRProcView = false;
            ViewBag.IsNCRReInspView = false;

            ViewBag.ncrDetails = readOnlyDetails;

            ViewData["EngDispositionTypeId"] = new SelectList(_context.EngDispositionTypes, "EngDispositionTypeId", "EngDispositionTypeName", ncrEng.EngDispositionTypeId);
            return View(ncrEngDTO);
        }

        // POST: NcrEng/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NcrEngDTO ncrEngDTO, List<IFormFile> Photos)
        {

            //ncrEngDTO.NcrEngId = id;
            //if (id != ncrEngDTO.NcrEngId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                await AddPictures(ncrEngDTO, Photos);
                try
                {
                    var ncrEng = await _context.NcrEngs
                        .Include(ne => ne.Drawing)
                        .FirstOrDefaultAsync(ne => ne.NcrEngId == id);

                    //ncrEng.NcrEngId = ncrEngDTO.NcrEngId;

                    ncrEng.NcrEngCustomerNotification = ncrEngDTO.NcrEngCustomerNotification;
                    ncrEng.NcrEngDispositionDescription = ncrEngDTO.NcrEngDispositionDescription;
                    ncrEng.NcrEngCreationDate = ncrEngDTO.NcrEngCreationDate;
                    ncrEng.NcrEngCompleteDate = ncrEngDTO.NcrEngCompleteDate;
                    ncrEng.NcrEngStatusFlag = ncrEngDTO.NcrEngStatusFlag;
                    ncrEng.NcrEngUserId = ncrEngDTO.NcrEngUserId;
                    ncrEng.EngDispositionTypeId = ncrEngDTO.EngDispositionTypeId;
                   // ncrEng.NcrId = ncrEngDTO.NcrId;
                    ncrEng.DrawingId = ncrEngDTO.DrawingId;
                    ncrEng.DrawingRequireUpdating = ncrEngDTO.DrawingRequireUpdating;
                    ncrEng.DrawingOriginalRevNumber = ncrEngDTO.DrawingOriginalRevNumber;
                    ncrEng.DrawingUpdatedRevNumber = ncrEngDTO.DrawingUpdatedRevNumber;
                    ncrEng.DrawingRevDate = DateTime.Now;
                    ncrEng.DrawingUserId = ncrEngDTO.DrawingUserId;
                    ncrEng.EngDefectPhotos = ncrEngDTO.EngDefectPhotos;
                    ncrEng.NcrEngDefectVideo = ncrEngDTO.NcrEngDefectVideo;
                    //ncrEng.NcrPhase = NcrPhase.Operations;

                    _context.Update(ncrEng);
                    await _context.SaveChangesAsync();


                    var ncr = await _context.Ncrs.AsNoTracking().FirstOrDefaultAsync(n => n.NcrId == ncrEng.NcrId);
                    //ncr.NcrPhase = NcrPhase.Operations;
                    ncr.NcrLastUpdated = DateTime.Now;
                    _context.Update(ncr);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "NCR " + ncr.NcrNumber + " edited successfully!";

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
                .Include(n => n.NcrQa).ThenInclude(n => n.Supplier)
                .Where(n => n.NcrPhase == NcrPhase.Engineer)
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

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var photo = await _context.EngDefectPhotos.FindAsync(photoId);
            if (photo != null)
            {
                _context.EngDefectPhotos.Remove(photo);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Photo deleted successfully." });
            }
            return Json(new { success = false, message = "Photo not found." });
        }
    }
}
