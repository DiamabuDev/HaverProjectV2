using System;
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
            string[] sortOptions = new[] { "Created", "NCR #", "Supplier", "Defect", "PO Number" };

            PopulateDropDownLists();

            var ncrQa = _context.NcrQas
                .Include(n => n.Item)
                .Include(n => n.Item).ThenInclude(i => i.Supplier)
                .Include(n => n.Item).ThenInclude(i => i.ItemDefects).ThenInclude(i => i.Defect)
                .Include(n => n.Ncr)
                .AsNoTracking();

            //Filterig values            
            if (!String.IsNullOrEmpty(filter))
            {
                if (filter == "Active")
                {
                    ncrQa = ncrQa.Where(n => n.Ncr.NcrStatus == true);
                }
                else //(filter == "Closed")
                {

                    ncrQa = ncrQa.Where(n => n.Ncr.NcrStatus == false);
                }
            }    
            if (!String.IsNullOrEmpty(SearchCode))
            {
                ncrQa = ncrQa.Where(s => s.Item.ItemDefects.FirstOrDefault().Defect.DefectName.ToUpper().Contains(SearchCode.ToUpper()) 
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
            //Now we know which field and direction to sort by
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
            else if (sortField == "Status")
            {
                if (sortDirection == "asc")
                {
                    ncrQa = ncrQa
                        .OrderBy(p => p.Ncr.NcrStatus);
                    ViewData["filterApplied:Status"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrQa = ncrQa
                        .OrderByDescending(p => p.Ncr.NcrStatus);
                    ViewData["filterApplied:Status"] = "<i class='bi bi-sort-down'></i>";
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

            //Set sort for next time
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
                .Include(n => n.Item)
                .Include(n => n.Ncr)
                .FirstOrDefaultAsync(m => m.NcrQaId == id);
            if (ncrQa == null)
            {
                return NotFound();
            }

            return View(ncrQa);
        }

        // GET: NcrQa/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemName");
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber");
            return View();
        }

        // POST: NcrQa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NcrQaId,NcrQaItemMarNonConforming,NcrQaProcessApplicable,NcrQacreationDate,NcrQaOrderNumber,NcrQaSalesOrder,NcrQaQuanReceived,NcrQaQuanDefective,NcrQaDescriptionOfDefect,NcrQauserId,NcrQaEngDispositionRequired,NcrId,ItemId")] NcrQa ncrQa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ncrQa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemName", ncrQa.ItemId);
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrQa.NcrId);
            return View(ncrQa);
        }

        // GET: NcrQa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NcrQas == null)
            {
                return NotFound();
            }

            var ncrQa = await _context.NcrQas.FindAsync(id);
            if (ncrQa == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemName", ncrQa.ItemId);
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrQa.NcrId);
            return View(ncrQa);
        }

        // POST: NcrQa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NcrQaId,NcrQaItemMarNonConforming,NcrQaProcessApplicable,NcrQacreationDate,NcrQaOrderNumber,NcrQaSalesOrder,NcrQaQuanReceived,NcrQaQuanDefective,NcrQaDescriptionOfDefect,NcrQauserId,NcrQaEngDispositionRequired,NcrId,ItemId")] NcrQa ncrQa)
        {
            if (id != ncrQa.NcrQaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ncrQa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NcrQaExists(ncrQa.NcrQaId))
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
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemName", ncrQa.ItemId);
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrQa.NcrId);
            return View(ncrQa);
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

        private SelectList SupplierSelectList(int? selectedId)
        {
            return new SelectList(_context.Suppliers
                .OrderBy(s => s.SupplierName), "SupplierId", "SupplierName", selectedId);
        }

        private void PopulateDropDownLists(NcrQa ncrQa = null)
        {
            ViewData["SupplierId"] = SupplierSelectList(ncrQa?.Item.Supplier.SupplierId);
            //ViewData["ItemId"] = ItemSelectList(ncr?.NcrQas.FirstOrDefault()?.OrderDetails.FirstOrDefault()?.Item.ItemId);
            //ViewData["DefectId"] = DefectSelectList(ncr?.NcrQas.FirstOrDefault()?.OrderDetails.FirstOrDefault()?.Item.ItemDefects.FirstOrDefault()?.DefectId);
            //ViewData["ProAppId"] = ProAppSelectList(ncr?.NcrQas.FirstOrDefault()?.ProAppId);
            //ViewData["StatusUpdateId"] = StatusSelectList(ncr?.StatusUpdateId);
        }


        private bool NcrQaExists(int id)
        {
          return _context.NcrQas.Any(e => e.NcrQaId == id);
        }
    }
}
