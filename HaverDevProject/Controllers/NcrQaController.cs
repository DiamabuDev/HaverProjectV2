﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaverDevProject.Data;
using HaverDevProject.Models;
using HaverDevProject.CustomControllers;
using HaverDevProject.Utilities;
using HaverDevProject.ViewModels;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Storage;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HaverDevProject.Controllers
{
    public class NcrQaController : ElephantController
    {
        private readonly HaverNiagaraContext _context;

        public NcrQaController(HaverNiagaraContext context)
        {
            _context = context;
        }

        
        // GET: NcrQa
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
            string[] sortOptions = new[] { "Created", "NCR #", "Supplier", "Defect", "PO Number", "Phase"};

            //PopulateDropDownLists();
            ViewData["SupplierId"] = SupplierSelectList();

            var ncrQa = _context.NcrQas
                //.Include(n => n.Item).ThenInclude(n => n.ItemDefects).ThenInclude(n => n.Defect)
                .Include(n => n.Item.Supplier)
                .Include(n => n.Defect)
                .Include(n => n.Ncr)
                .AsNoTracking();

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
                    ncrQa = ncrQa.Where(n => n.Ncr.NcrStatus == true);
                    ViewData["filterApplied:ButtonActive"] = "btn-success";
                    ViewData["filterApplied:ButtonAll"] = "btn-primary custom-opacity";
                    ViewData["filterApplied:ButtonClosed"] = "btn-danger custom-opacity";
                }
                else //(filter == "Closed")
                {
                    ncrQa = ncrQa.Where(n => n.Ncr.NcrStatus == false);
                    ViewData["filterApplied:ButtonClosed"] = "btn-danger";
                    ViewData["filterApplied:ButtonAll"] = "btn-primary custom-opacity";
                    ViewData["filterApplied:ButtonActive"] = "btn-success custom-opacity";
                }
            }
            if (!String.IsNullOrEmpty(SearchCode))
            {
                ncrQa = ncrQa.Where(s => s.Defect.DefectName.ToUpper().Contains(SearchCode.ToUpper() ) //(s => s.Item.ItemDefects.FirstOrDefault().Defect.DefectName.ToUpper().Contains(SearchCode.ToUpper()) 
                || s.Ncr.NcrNumber.ToUpper().Contains(SearchCode.ToUpper())); 
            }
            if (SupplierID.HasValue)
            {
                ncrQa = ncrQa.Where(n => n.Item.Supplier.SupplierId == SupplierID); 
            }
            if (StartDate == EndDate)
            {
                ncrQa = ncrQa.Where(n => n.NcrQacreationDate == StartDate); 
            }
            else
            {
                ncrQa = ncrQa.Where(n => n.NcrQacreationDate >= StartDate && 
                         n.NcrQacreationDate <= EndDate);   
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
                    ncrQa = ncrQa
                        .OrderBy(p => p.Ncr.NcrNumber);
                    ViewData["filterApplied:NcrNumber"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrQa = ncrQa
                        .OrderByDescending(p => p.Ncr.NcrNumber);
                    ViewData["filterApplied:NcrNumber"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Defect")
            {
                if (sortDirection == "asc")
                {
                    ncrQa = ncrQa
                        .OrderBy(p => p.Item.ItemDefects.FirstOrDefault().Defect.DefectName); 
                    ViewData["filterApplied:Defect"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrQa = ncrQa
                        .OrderByDescending(p => p.Item.ItemDefects.FirstOrDefault().Defect.DefectName);  
                    ViewData["filterApplied:Defect"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Supplier")
            {
                if (sortDirection == "asc")
                {
                    ncrQa = ncrQa
                        .OrderBy(p => p.Item.Supplier.SupplierName); 
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrQa = ncrQa
                        .OrderByDescending(p => p.Item.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Created")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncrQa = ncrQa
                        .OrderBy(p => p.NcrQacreationDate); 

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrQa = ncrQa
                        .OrderByDescending(p => p.NcrQacreationDate);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Phase")
            {
                if (sortDirection == "asc")
                {
                    ncrQa = ncrQa
                        .OrderBy(p => p.Ncr.NcrPhase); //.OrderBy(p => p.Ncr.NcrStatus);
                    ViewData["filterApplied:Phase"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrQa = ncrQa
                        .OrderByDescending(p => p.Ncr.NcrPhase); //.OrderByDescending(p => p.Ncr.NcrStatus);
                    ViewData["filterApplied:Phase"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else //(sortField == "PO Number")
            {
                if (sortDirection == "asc")
                {
                    ncrQa = ncrQa
                        .OrderBy(p => p.NcrQaOrderNumber); 
                    ViewData["filterApplied:PONumber"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrQa = ncrQa
                        .OrderByDescending(p => p.NcrQaOrderNumber); 
                    ViewData["filterApplied:PONumber"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<NcrQa>.CreateAsync(ncrQa.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);            
        }

        // GET: NcrQa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NcrQas == null)
            {
                return NotFound();
            }

            var ncrQa = await _context.NcrQas
                .Include(n => n.Ncr)
                .Include(n => n.Item).ThenInclude(i => i.Supplier)
                .Include(n => n.Defect)
                .Include(n => n.ItemDefectPhotos)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrEng)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrEng).ThenInclude(n => n.EngDispositionType)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrEng).ThenInclude(n => n.Drawing)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrEng).ThenInclude(n => n.EngDefectPhotos)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrOperation)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrOperation).ThenInclude(n => n.OpDispositionType)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrOperation).ThenInclude(n => n.FollowUpType)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrOperation).ThenInclude(n => n.OpDefectPhotos)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrProcurement)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrProcurement).ThenInclude(n => n.ProcDefectPhotos)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrReInspect)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrReInspect).ThenInclude(n => n.NcrReInspectPhotos)
                .FirstOrDefaultAsync(n => n.NcrQaId == id);

            if (ncrQa == null)
            {
                return NotFound();
            }

            ViewBag.IsNCRQaView = true;
            ViewBag.IsNCREngView = false;
            ViewBag.IsNCROpView = false;
            ViewBag.IsNCRProcView = false;
            ViewBag.IsNCRReInspView = false;

            ViewBag.NCRSectionId = id;

            return View(ncrQa);
        }

        // GET: NcrQa/Create
        public IActionResult Create()
        {
            NcrQaDTO ncr = new NcrQaDTO();
            ncr.NcrNumber = GetNcrNumber();
            ncr.NcrQacreationDate = DateTime.Today;
            ncr.NcrStatus = true; //Active
            ncr.NcrQaProcessApplicable = true; //Supplier or Rec-Insp
            ncr.NcrQaItemMarNonConforming = true; //Yes
            ncr.NcrQaEngDispositionRequired = true; //Yes
            
            PopulateDropDownLists();
            //ViewData["SupplierId"] = SupplierSelectCreateList(null);

            return View(ncr);
        }

        // POST: NcrQa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NcrQaDTO ncrQaDTO, List<IFormFile> Photos)
        {
            if (ModelState.IsValid)
            {
                bool engReq = ncrQaDTO.NcrQaEngDispositionRequired == true ? true : false;                

                Ncr ncr = new Ncr
                {
                    NcrNumber = ncrQaDTO.NcrNumber,
                    NcrLastUpdated = DateTime.Now,
                    NcrStatus = ncrQaDTO.NcrStatus,
                    NcrPhase = ncrQaDTO.NcrQaEngDispositionRequired == true ? NcrPhase.Engineer : NcrPhase.Operations
                };

                _context.Add(ncr);
                await _context.SaveChangesAsync();

                //getting the ncrId through the NcrNumber 
                int ncrIdObt = _context.Ncrs
                    .Where(n => n.NcrNumber == ncrQaDTO.NcrNumber)
                    .Select(n => n.NcrId)
                    .FirstOrDefault();

                await AddPictures(ncrQaDTO, Photos);   
                NcrQa ncrQa = new NcrQa
                {
                    NcrQaItemMarNonConforming = ncrQaDTO.NcrQaItemMarNonConforming,
                    NcrQaProcessApplicable = ncrQaDTO.NcrQaProcessApplicable,
                    NcrQacreationDate = ncrQaDTO.NcrQacreationDate,
                    NcrQaOrderNumber = ncrQaDTO.NcrQaOrderNumber,
                    NcrQaSalesOrder = ncrQaDTO.NcrQaSalesOrder,
                    NcrQaQuanReceived = ncrQaDTO.NcrQaQuanReceived,
                    NcrQaQuanDefective = ncrQaDTO.NcrQaQuanDefective,
                    NcrQaDescriptionOfDefect = ncrQaDTO.NcrQaDescriptionOfDefect,
                    NcrQaDefectVideo = ncrQaDTO.NcrQaDefectVideo,
                    ItemDefectPhotos = ncrQaDTO.ItemDefectPhotos,
                    NcrQauserId = 1,  //Change when we have this information
                    NcrId = ncrIdObt,
                    ItemId = ncrQaDTO.ItemId,
                    DefectId = ncrQaDTO.DefectId,
                    NcrQaEngDispositionRequired = ncrQaDTO.NcrQaEngDispositionRequired
                };              
                
                _context.NcrQas.Add(ncrQa);
                await _context.SaveChangesAsync();                

                TempData["SuccessMessage"] = "NCR created successfully!";
                int ncrQaId = ncrQa.NcrQaId;
                return RedirectToAction("Details", new { id = ncrQaId });                                
            }

            //PopulateDropDownLists();
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", ncrQaDTO.SupplierId);
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemName", ncrQaDTO.ItemId);
            ViewData["DefectId"] = new SelectList(_context.Defects, "DefectId", "DefectName", ncrQaDTO.DefectId);
            return View(ncrQaDTO);
        }

        // GET: NcrQa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NcrQas == null)
            {
                return NotFound();
            }

            var ncrQa = await _context.NcrQas
                .Include(n =>n.Ncr)
                .Include(n =>n.Item).ThenInclude(n => n.Supplier)
                .Include(n =>n.Defect)
                .Include(n =>n.ItemDefectPhotos)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NcrQaId == id);

            if (ncrQa == null)
            {
                return NotFound();
            }

            var ncrQaDTO = new NcrQaDTO
            {
                NcrQaId = ncrQa.NcrQaId,
                NcrQaItemMarNonConforming = ncrQa.NcrQaItemMarNonConforming,
                NcrQaProcessApplicable = ncrQa.NcrQaProcessApplicable,
                NcrQacreationDate = ncrQa.NcrQacreationDate,
                NcrQaOrderNumber = ncrQa.NcrQaOrderNumber,
                NcrQaSalesOrder = ncrQa.NcrQaSalesOrder,
                NcrQaQuanReceived = ncrQa.NcrQaQuanReceived,
                NcrQaQuanDefective = ncrQa.NcrQaQuanDefective,
                NcrQaDescriptionOfDefect = ncrQa.NcrQaDescriptionOfDefect,
                NcrId = ncrQa.NcrId,                
                SupplierId = ncrQa.Item.SupplierId,
                NcrNumber = ncrQa.Ncr.NcrNumber,
                ItemId = ncrQa.ItemId,
                DefectId = ncrQa.DefectId,
                NcrQaEngDispositionRequired = ncrQa.NcrQaEngDispositionRequired,
                NcrQaDefectVideo = ncrQa.NcrQaDefectVideo,
                ItemDefectPhotos = ncrQa.ItemDefectPhotos
            };

            //PopulateDropDownLists();
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", ncrQaDTO.SupplierId);
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemName", ncrQaDTO.ItemId);
            ViewData["DefectId"] = new SelectList(_context.Defects, "DefectId", "DefectName", ncrQaDTO.DefectId);

            return View(ncrQaDTO);
        }

        // POST: NcrQa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int NcrQaId, int NcrId, NcrQaDTO ncrQaDTO, List<IFormFile> Photos)
        {
            if (ModelState.IsValid)
            {
                var ncrToUpdate = await _context.Ncrs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(n =>n.NcrId == NcrId);

                if (ncrToUpdate == null)
                {
                    return NotFound();
                }
                else
                {                    
                    ncrToUpdate.NcrPhase = ncrQaDTO.NcrQaEngDispositionRequired ? NcrPhase.Engineer : NcrPhase.Operations;
                    ncrToUpdate.NcrLastUpdated = DateTime.Now;

                    _context.Ncrs.Update(ncrToUpdate);
                    await _context.SaveChangesAsync();
                }

                // Go get the ncrQa to update
                var ncrQaToUpdate = await _context.NcrQas
                    .Include(n => n.Item)
                    .Include(n => n.Item).ThenInclude(n => n.Supplier)
                    .Include(n => n.Defect)
                    .Include(n => n.ItemDefectPhotos)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(n => n.NcrQaId == NcrQaId);

                // Check that we got the function or exit with a not found error
                if (ncrQaToUpdate == null)
                {
                    return NotFound();
                }
                else
                {
                    await AddPictures(ncrQaDTO, Photos);
                    try
                    {
                        ncrQaToUpdate.NcrQaItemMarNonConforming = ncrQaDTO.NcrQaItemMarNonConforming;
                        ncrQaToUpdate.NcrQaProcessApplicable = ncrQaDTO.NcrQaProcessApplicable;
                        ncrQaToUpdate.NcrQacreationDate = ncrQaDTO.NcrQacreationDate; //checar
                        ncrQaToUpdate.NcrQaOrderNumber = ncrQaDTO.NcrQaOrderNumber;
                        ncrQaToUpdate.NcrQaSalesOrder = ncrQaDTO.NcrQaSalesOrder;
                        ncrQaToUpdate.NcrQaQuanReceived = ncrQaDTO.NcrQaQuanReceived;
                        ncrQaToUpdate.NcrQaQuanDefective = ncrQaDTO.NcrQaQuanDefective;
                        //pendiente userId
                        ncrQaToUpdate.NcrQaDescriptionOfDefect = ncrQaDTO.NcrQaDescriptionOfDefect;
                        ncrQaToUpdate.NcrQaDefectVideo = ncrQaDTO.NcrQaDefectVideo;                        
                        ncrQaToUpdate.ItemId = ncrQaDTO.ItemId;
                        ncrQaToUpdate.Item = null;
                        ncrQaToUpdate.DefectId = ncrQaDTO.DefectId;
                        ncrQaToUpdate.Defect = null;
                        ncrQaToUpdate.NcrQaEngDispositionRequired = ncrQaDTO.NcrQaEngDispositionRequired;
                        ncrQaToUpdate.ItemDefectPhotos = ncrQaDTO.ItemDefectPhotos;                   

                        _context.NcrQas.Update(ncrQaToUpdate);
                        await _context.SaveChangesAsync();

                        TempData["SuccessMessage"] = "NCR edited successfully!";
                        int updateNcrQa = ncrQaToUpdate.NcrQaId;
                        return RedirectToAction("Details", new { id = updateNcrQa });
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
            //PopulateDropDownLists();
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", ncrQaDTO.SupplierId);
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemName", ncrQaDTO.ItemId);
            ViewData["DefectId"] = new SelectList(_context.Defects, "DefectId", "DefectName", ncrQaDTO.DefectId);
            return View(ncrQaDTO);            
        }

        // GET: NcrQa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NcrQas == null)
            {
                return NotFound();
            }

            var ncrQa = await _context.NcrQas
                .Include(n => n.Item)
                .Include(n => n.Ncr)
                .FirstOrDefaultAsync(m => m.NcrQaId == id);
            if (ncrQa == null)
            {
                return NotFound();
            }

            return View(ncrQa);
        }

        // POST: NcrQa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NcrQas == null)
            {
                return Problem("Entity set 'HaverNiagaraContext.NcrQas'  is null.");
            }
            var ncrQa = await _context.NcrQas.FindAsync(id);
            if (ncrQa != null)
            {
                _context.NcrQas.Remove(ncrQa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public string GetNcrNumber()
        {
            string lastNcrNumber = _context.Ncrs
                .OrderByDescending(n => n.NcrNumber)
                .Select(n => n.NcrNumber)
                .FirstOrDefault();

            if(lastNcrNumber != null)
            {
                string lastYear = lastNcrNumber.Substring(0, 4);
                string lastConsecutiveNumber = lastNcrNumber.Substring(5);

                if(lastYear == DateTime.Today.Year.ToString())
                {
                    int nextNumber = int.Parse(lastConsecutiveNumber) + 1;
                    string nextNumberString = nextNumber.ToString("000");

                    //Ncr Format
                    return $"{lastYear}-{nextNumberString}";
                }                
            }

            string currentYear = DateTime.Today.Year.ToString();
            int nextConsecutiveNumber = 1;
            string nextConsecutiveNumberString = nextConsecutiveNumber.ToString("000");

            //Ncr Format
            return $"{currentYear}-{nextConsecutiveNumberString}";
        }

        private SelectList SupplierSelectList()
        {
            return new SelectList(_context.Suppliers
                .Where(s => s.SupplierName != "NO SUPPLIER PROVIDED")
                .OrderBy(s => s.SupplierName), "SupplierId", "SupplierName");
        }
        private SelectList SupplierSelectCreateList(int? selectedId)
        {
            return new SelectList(_context.Suppliers
                .Where(s => s.SupplierStatus == true && s.SupplierName != "NO SUPPLIER PROVIDED")
                .OrderBy(s => s.SupplierName), "SupplierId", "SupplierName", selectedId);
        }

        private SelectList ItemSelectList(int? SupplierID, int? selectedId)
        {
            var query = from c in _context.Items
                        where c.SupplierId == SupplierID
                        select c;
            return new SelectList(query.OrderBy(i => i.ItemName), "ItemId", "ItemName", selectedId);            
        }

        private void PopulateDropDownLists(NcrQa ncrQa = null)
        {
            if ((ncrQa?.ItemId).HasValue)
            {   
                if (ncrQa.Item == null)
                {
                    ncrQa.Item = _context.Items.Find(ncrQa.ItemId);
                }
                ViewData["SupplierId"] = SupplierSelectCreateList(ncrQa?.Item.Supplier.SupplierId);
                ViewData["ItemId"] = ItemSelectList(ncrQa.Item.SupplierId, ncrQa.ItemId);
            }
            else
            {
                ViewData["SupplierId"] = SupplierSelectCreateList(null);
                ViewData["ItemId"] = ItemSelectList(null, null);
            }
        }

        [HttpGet]
        public JsonResult GetSuppliers(int? id)
        {
            return Json(SupplierSelectCreateList(id));
        }


        [HttpGet]
        public JsonResult GetItems(int SupplierId)
        {
            return Json(ItemSelectList(SupplierId, null));
        }

        [HttpGet]
        public JsonResult GetDefects(int ItemId)
        {
            var defects = _context.ItemDefects
                .Where(id => id.ItemId == ItemId)
                .Select(id => new { id.DefectId, id.Defect.DefectName })
                .ToList();

            return Json(new SelectList(defects, "DefectId", "DefectName"));
        }
         
        private async Task AddPictures(NcrQaDTO ncrQaDTO, List<IFormFile> pictures)
        {
            if (pictures != null && pictures.Any())
            {
                ncrQaDTO.ItemDefectPhotos = new List<ItemDefectPhoto>();

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

                            ncrQaDTO.ItemDefectPhotos.Add(new ItemDefectPhoto
                            {
                                ItemDefectPhotoContent = ResizeImage.shrinkImageWebp(pictureArray, 500, 600),
                                ItemDefectPhotoMimeType = "image/webp",
                                FileName = picture.FileName                                
                            });
                        }
                    }
                }
            }
        }


        public async Task<FileContentResult> Download(int id)
        {
            var theFile = await _context.ItemDefectPhotos
                .Include(d => d.FileContent)
                .Where(f => f.ItemDefectPhotoId == id)
                .FirstOrDefaultAsync();
            return File(theFile.ItemDefectPhotoContent, theFile.ItemDefectPhotoMimeType, theFile.FileName);
        }

        public async Task<IActionResult> ArchiveNcr(int id)
        {
            var ncrToUpdate = await _context.Ncrs                    
                    .AsNoTracking()
                    .FirstOrDefaultAsync(n => n.NcrId == id);

            if (ncrToUpdate != null) 
            {
                //Update the phase
                ncrToUpdate.NcrPhase = NcrPhase.Archive;

                //saving the values
                _context.Ncrs.Update(ncrToUpdate);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "NCR Archive successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "NCR not found for archiving.";
                return RedirectToAction("Index");
            }                

        }

        public async Task<IActionResult> RestoreNcr(int id)
        {
            var ncrToUpdate = await _context.Ncrs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(n => n.NcrId == id);

            if (ncrToUpdate != null)
            {
                //Update the phase
                ncrToUpdate.NcrPhase = NcrPhase.Closed;

                //saving the values
                _context.Ncrs.Update(ncrToUpdate);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "NCR Restore successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "NCR not found for archiving.";
                return RedirectToAction("Index");
            }

        }


        private bool NcrQaExists(int id)
        {
          return _context.NcrQas.Any(e => e.NcrQaId == id);
        }
    }
}
