using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaverDevProject.Data;
using HaverDevProject.Models;
using HaverDevProject.Utilities;
using HaverDevProject.CustomControllers;
using NuGet.Versioning;

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
        public async Task<IActionResult> Index(string SearchField, int? SupplierID, int? page, int? pageSizeID,
            string actionButton, string sortDirection = "asc", string sortField = "Date")
        {
            //List of sort options.
            string[] sortOptions = new[] { "Date", "NCR #", "Supplier", "Item", "Defect", "Qty Received", "Qty Defective" }; 

            PopulateDropDownLists();

            var ncrQa = _context.NcrQas
                .Include(nqa => nqa.OrderDetails).ThenInclude(od => od.Item).ThenInclude(i =>i.Supplier)
                .Include(nqa => nqa.OrderDetails).ThenInclude(od => od.Item).ThenInclude(i => i.ItemDefects).ThenInclude(id => id.Defect)
                .Include(nqa => nqa.Ncr)
                .AsNoTracking();

                        
            //Filterig values                       
            if (!String.IsNullOrEmpty(SearchField)) //Find by NCR Number or Item
            {
                ncrQa = ncrQa.Where(nqa => nqa.Ncr.NcrNumber.Contains(SearchField.ToUpper())
                                        || nqa.OrderDetails.FirstOrDefault().Item.ItemName.ToUpper().Contains(SearchField.ToUpper()));
            }
            if (SupplierID.HasValue)
            {
                ncrQa = ncrQa.Where(nqa => nqa.OrderDetails.FirstOrDefault().Item.Supplier.SupplierId == SupplierID);
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
            if (sortField == "Date")
            {
                if (sortDirection == "asc")
                {
                    ncrQa = ncrQa
                        .OrderBy(nqa => nqa.NcrQacreationDate);
                }
                else
                {
                    ncrQa = ncrQa
                        .OrderByDescending(nqa => nqa.NcrQacreationDate);
                }
            }
            else if (sortField == "NCR #")
            {
                if (sortDirection == "asc")
                {
                    ncrQa = ncrQa
                        .OrderBy(nqa => nqa.Ncr.NcrNumber);
                }
                else
                {
                    ncrQa = ncrQa
                        .OrderByDescending(nqa => nqa.Ncr.NcrNumber);
                }
            }
            else if (sortField == "Supplier")
            {
                if (sortDirection == "asc")
                {
                    ncrQa = ncrQa
                        .OrderBy(nqa => nqa.OrderDetails.FirstOrDefault().Item.Supplier.SupplierName);
                }
                else
                {
                    ncrQa = ncrQa
                        .OrderByDescending(nqa => nqa.OrderDetails.FirstOrDefault().Item.Supplier.SupplierName);
                }
            }
            else if (sortField == "Item")
            {
                if (sortDirection == "asc")
                {
                    ncrQa = ncrQa
                        .OrderBy(nqa => nqa.OrderDetails.FirstOrDefault().Item.ItemName);
                }
                else
                {
                    ncrQa = ncrQa
                        .OrderByDescending(nqa => nqa.OrderDetails.FirstOrDefault().Item.ItemName);
                }
            }
            else if (sortField == "Defect")
            {
                if (sortDirection == "asc")
                {
                    ncrQa = ncrQa
                        .OrderBy(nqa => nqa.OrderDetails.FirstOrDefault().Item.ItemDefects.FirstOrDefault().Defect.DefectName);
                }
                else
                {
                    ncrQa = ncrQa
                        .OrderByDescending(nqa => nqa.OrderDetails.FirstOrDefault().Item.ItemDefects.FirstOrDefault().Defect.DefectName);
                }
            }
            else if (sortField == "Qty Received")
            {
                if (sortDirection == "asc")
                {
                    ncrQa = ncrQa
                        .OrderBy(nqa => nqa.OrderDetails.FirstOrDefault().OrderQuanReceived);
                }
                else
                {
                    ncrQa = ncrQa
                        .OrderByDescending(nqa => nqa.OrderDetails.FirstOrDefault().OrderQuanReceived);
                }
            }
            else //Sorting by Quantity Defective
            {
                if (sortDirection == "asc")
                {
                    ncrQa = ncrQa
                        .OrderBy(nqa => nqa.OrderDetails.FirstOrDefault().OrderQuanDefective);
                }
                else
                {
                    ncrQa = ncrQa
                        .OrderByDescending(nqa => nqa.OrderDetails.FirstOrDefault().OrderQuanDefective);
                }
            }
            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            //var haverNiagaraContext = _context.NcrQas.Include(n => n.Ncr).Include(n => n.ProApp);
            //return View(await haverNiagaraContext.ToListAsync());

            //return View(await ncrQa.ToListAsync());
            //Handle Paging
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
                .Include(n => n.ProApp)
                .FirstOrDefaultAsync(m => m.NcrQaid == id);
            if (ncrQa == null)
            {
                return NotFound();
            }

            return View(ncrQa);
        }

        // GET: NcrQa/Create
        public IActionResult Create()
        {
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber");
            ViewData["ProAppId"] = new SelectList(_context.ProcessApplicables, "ProAppId", "ProAppName");
            return View();
        }

        // POST: NcrQa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NcrQaid,NcrQaitemMarNonConforming,NcrQasalesOrder,NcrQacreationDate,NcrQalastUpdated,NcrQauserId,ProAppId,NcrId")] NcrQa ncrQa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ncrQa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrQa.NcrId);
            ViewData["ProAppId"] = new SelectList(_context.ProcessApplicables, "ProAppId", "ProAppName", ncrQa.ProAppId);
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
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrQa.NcrId);
            ViewData["ProAppId"] = new SelectList(_context.ProcessApplicables, "ProAppId", "ProAppName", ncrQa.ProAppId);
            return View(ncrQa);
        }

        // POST: NcrQa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NcrQaid,NcrQaitemMarNonConforming,NcrQasalesOrder,NcrQacreationDate,NcrQalastUpdated,NcrQauserId,ProAppId,NcrId")] NcrQa ncrQa)
        {
            if (id != ncrQa.NcrQaid)
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
                    if (!NcrQaExists(ncrQa.NcrQaid))
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
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrQa.NcrId);
            ViewData["ProAppId"] = new SelectList(_context.ProcessApplicables, "ProAppId", "ProAppName", ncrQa.ProAppId);
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
                .Include(n => n.Ncr)
                .Include(n => n.ProApp)
                .FirstOrDefaultAsync(m => m.NcrQaid == id);
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
            ViewData["SupplierID"] = SupplierSelectList(ncrQa?.OrderDetails.FirstOrDefault()?.Item.Supplier.SupplierId);
        }       

        private bool NcrQaExists(int id)
        {
          return _context.NcrQas.Any(e => e.NcrQaid == id);
        }
    }
}
