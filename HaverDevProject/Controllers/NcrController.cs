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
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HaverDevProject.Controllers
{
    public class NcrController : ElephantController
    {
        private readonly HaverNiagaraContext _context;

        public NcrController(HaverNiagaraContext context)
        {
            _context = context;
        }

        // GET: Ncr
        public async Task<IActionResult> Index(string filter, string SearchCode, int? SupplierID, DateTime StartDate, DateTime EndDate, int? page, int? pageSizeID,
    string actionButton, string sortDirection = "asc", string sortField = "Created")
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
            string[] sortOptions = new[] { "NCR #", "Defect", "Supplier", "Status", "Created", "PO Number" };

            PopulateDropDownLists();

            var ncr = _context.Ncrs
                .Include(n => n.StatusUpdate)
                .Include(n => n.NcrQas)
                .Include(n => n.NcrEngs).ThenInclude(n => n.EngDispositionType)
                .Include(n => n.NcrQas).ThenInclude(n => n.OrderDetails)
                .Include(n => n.NcrQas).ThenInclude(n => n.OrderDetails).ThenInclude(od => od.Item).ThenInclude(i => i.Supplier)
                .Include(n => n.NcrQas).ThenInclude(n => n.OrderDetails).ThenInclude(n => n.Item).ThenInclude(n => n.ItemDefects).ThenInclude(n => n.Defect)
                .AsNoTracking();


            //Filterig values
            if (!String.IsNullOrEmpty(filter))
            {
                if (filter == "Active")
                {
                    // Lógica para filtrar NCRs activos
                    // Puedes ajustar esto según la estructura de tu modelo y lógica de filtrado
                    ncr = ncr.Where(n => n.StatusUpdate.StatusUpdateName == "Active");
                }
                else if (filter == "Closed")
                {
                    ncr = ncr.Where(n => n.StatusUpdate.StatusUpdateName == "Closed");
                }
            }

            if (!String.IsNullOrEmpty(SearchCode))
            {
                ncr = ncr.Where(s => s.NcrQas.FirstOrDefault().OrderDetails.FirstOrDefault().Item.ItemDefects.FirstOrDefault().Defect.DefectName.ToUpper().Contains(SearchCode.ToUpper())
                || s.NcrNumber.ToUpper().Contains(SearchCode.ToUpper()));
                //ncr = ncr.Where(s => s.NcrNumber.ToUpper().Contains(SearchCode.ToUpper())
                //|| s.NcrQas.FirstOrDefault().OrderDetails.FirstOrDefault().Item.ItemDefects.FirstOrDefault().Defect.DefectName.ToUpper().Contains(SearchCode.ToUpper())
                //|| s.NcrEngs.FirstOrDefault().EngDispositionType.EngDispositionTypeName.ToUpper().Contains(SearchCode.ToUpper())
                //|| s.NcrQas.FirstOrDefault().NcrQacreationDate.Date.ToString() == SearchCode.ToUpper()
                //|| s.NcrLastUpdated.Date.ToString() == SearchCode.ToUpper()
                //|| s.StatusUpdate.StatusUpdateName.ToUpper().Contains(SearchCode.ToUpper()));
            }
            if (SupplierID.HasValue)
            {
                ncr = ncr.Where(n => n.NcrQas.FirstOrDefault().OrderDetails.FirstOrDefault().Item.Supplier.SupplierId == SupplierID);
            }
            if (StartDate == EndDate)
            {
                ncr = ncr.Where(n => n.NcrQas.FirstOrDefault().NcrQacreationDate == StartDate);
            }
            else
            {
                ncr = ncr.Where(n => n.NcrQas.FirstOrDefault().NcrQacreationDate >= StartDate &&
                         n.NcrQas.FirstOrDefault().NcrQacreationDate <= EndDate);
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
                    ncr = ncr
                        .OrderBy(p => p.NcrNumber);
                    ViewData["filterApplied:NcrNumber"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrNumber);
                    ViewData["filterApplied:NcrNumber"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Defect")
            {
                if (sortDirection == "asc")
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrQas.FirstOrDefault().OrderDetails.FirstOrDefault().Item.ItemDefects.FirstOrDefault().Defect.DefectName);
                    ViewData["filterApplied:Defect"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQas.FirstOrDefault().OrderDetails.FirstOrDefault().Item.ItemDefects.FirstOrDefault().Defect.DefectName);
                    ViewData["filterApplied:Defect"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Supplier")
            {
                if (sortDirection == "asc")
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrQas.FirstOrDefault().OrderDetails.FirstOrDefault().Item.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQas.FirstOrDefault().OrderDetails.FirstOrDefault().Item.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Created")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrQas.FirstOrDefault().NcrQacreationDate);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQas.FirstOrDefault().NcrQacreationDate);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Status")
            {
                if (sortDirection == "asc")
                {
                    ncr = ncr
                        .OrderBy(p => p.StatusUpdate.StatusUpdateName);
                    ViewData["filterApplied:Status"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.StatusUpdate.StatusUpdateName);
                    ViewData["filterApplied:Status"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else //(sortField == "PO Number")
            {
                if (sortDirection == "asc")
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrQas.FirstOrDefault().OrderDetails.FirstOrDefault().OrderNumber);
                    ViewData["filterApplied:PONumber"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQas.FirstOrDefault().OrderDetails.FirstOrDefault().OrderNumber);
                    ViewData["filterApplied:PONumber"] = "<i class='bi bi-sort-down'></i>";
                }
            }

            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;


            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Ncr>.CreateAsync(ncr.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: Ncr/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ncrs == null)
            {
                return NotFound();
            }

            var ncr = await _context.Ncrs
                .Include(n => n.StatusUpdate)
                .Include(n => n.NcrQas)
                    .ThenInclude(nqa => nqa.OrderDetails)
                        .ThenInclude(od => od.Item)
                            .ThenInclude(i => i.Supplier)
                .Include(n => n.NcrQas)
                      .ThenInclude(nqa => nqa.OrderDetails)
                        .ThenInclude(od => od.Item)
                        .ThenInclude(i => i.ItemDefects)
                            .ThenInclude(id => id.Defect)
               .Include(n => n.NcrQas)
                   .ThenInclude(n => n.ProApp)
                .Include(n => n.NcrEngs)
                    .ThenInclude(ne => ne.EngDispositionType)
                .Include(n => n.NcrEngs)
                    .ThenInclude(ne => ne.Drawings)
                .Include(n => n.NcrPurchasings)
                    .ThenInclude(np => np.OpDispositionType)
                .Include(n => n.NcrPurchasings)
                    .ThenInclude(np => np.Cars)
                .Include(n => n.NcrPurchasings)
                    .ThenInclude(np => np.FollowUps)
                        .ThenInclude(fu => fu.FollowUpType)
                .Include(n => n.NcrReInspects)
                .FirstOrDefaultAsync(m => m.NcrId == id);

            if (ncr == null)
            {
                return NotFound();
            }

            return View(ncr);
        }

        // GET: Ncr/Create
        public IActionResult Create()
        {
            Ncr ncr = new Ncr();
            PopulateDropDownLists(ncr);
            //PopulateProAppCheckboxes(ncr);
            return View(ncr);
        }

        // POST: Ncr/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NcrId,NcrNumber,StatusUpdateId,OrderId,NcrQasalesOrder,SupplierId,ProAppId,OrderQuanReceived,OrderQuanDefective," +
            "ItemId,ItemNumber,DefectId,DefectDesription,NcrQaitemMarNonConforming,NcrQacreationDate")] Ncr ncr)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ncr);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDownLists(ncr);
            return View(ncr);
        }


        // GET: Ncr/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ncrs == null)
            {
                return NotFound();
            }

            var ncr = await _context.Ncrs.FindAsync(id);
            if (ncr == null)
            {
                return NotFound();
            }
            ViewData["StatusUpdateId"] = new SelectList(_context.StatusUpdates, "StatusUpdateId", "StatusUpdateName", ncr.StatusUpdateId);
            return View(ncr);
        }

        // POST: Ncr/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NcrId,NcrNumber,NcrLastUpdated,StatusUpdateId")] Ncr ncr)
        {
            if (id != ncr.NcrId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ncr);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NcrExists(ncr.NcrId))
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
            ViewData["StatusUpdateId"] = new SelectList(_context.StatusUpdates, "StatusUpdateId", "StatusUpdateName", ncr.StatusUpdateId);
            return View(ncr);
        }

        // GET: Ncr/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ncrs == null)
            {
                return NotFound();
            }

            var ncr = await _context.Ncrs
                .Include(n => n.StatusUpdate)
                .FirstOrDefaultAsync(m => m.NcrId == id);
            if (ncr == null)
            {
                return NotFound();
            }

            return View(ncr);
        }

        // POST: Ncr/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ncrs == null)
            {
                return Problem("Entity set 'HaverNiagaraContext.Ncrs'  is null.");
            }
            var ncr = await _context.Ncrs.FindAsync(id);
            if (ncr != null)
            {
                _context.Ncrs.Remove(ncr);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private SelectList SupplierSelectList(int? selectedId)
        {
            return new SelectList(_context.Suppliers
                .OrderBy(s => s.SupplierName), "SupplierId", "SupplierName", selectedId);
        }

        private SelectList ItemSelectList(int? selectedId)
        {
            return new SelectList(_context.Items
                .OrderBy(s => s.ItemName), "ItemId", "ItemName", selectedId);
        }

        private SelectList DefectSelectList(int? selectedId)
        {
            return new SelectList(_context.Defects
                .OrderBy(s => s.DefectName), "DefectId", "DefectName", selectedId);
        }

        private SelectList ProAppSelectList(int? selectedId)
        {
            return new SelectList(_context.ProcessApplicables
                .OrderBy(s => s.ProAppName), "ProAppId", "ProAppName", selectedId);
        }

        private SelectList StatusSelectList(int? selectedId)
        {
            return new SelectList(_context.StatusUpdates
                .OrderBy(s => s.StatusUpdateName), "StatusUpdateId", "StatusUpdateName", selectedId);
        }

        //private void PopulateProAppCheckboxes(Ncr ncr)
        //{
        //    //For this to work, you must have Included the FunctionRooms 
        //    //in the Function
        //    var allOptions = _context.ProcessApplicables;
        //    var currentOptionIDs = new HashSet<int>(ncr?.NcrQas.Select(b => b.ProAppId));
        //    var checkBoxes = new List<CheckOptionVM>();
        //    foreach (var option in allOptions)
        //    {
        //        checkBoxes.Add(new CheckOptionVM
        //        {
        //            ID = option.ProAppId,
        //            DisplayText = option.ProAppName,
        //            Assigned = currentOptionIDs.Contains(option.ProAppId)
        //        });
        //    }
        //    ViewData["ProAppOptions"] = checkBoxes;
        //}



        private void PopulateDropDownLists(Ncr ncr = null)
        {
            ViewData["SupplierId"] = SupplierSelectList(ncr?.NcrQas.FirstOrDefault()?.OrderDetails.FirstOrDefault()?.Item.Supplier.SupplierId);
            ViewData["ItemId"] = ItemSelectList(ncr?.NcrQas.FirstOrDefault()?.OrderDetails.FirstOrDefault()?.Item.ItemId);
            ViewData["DefectId"] = DefectSelectList(ncr?.NcrQas.FirstOrDefault()?.OrderDetails.FirstOrDefault()?.Item.ItemDefects.FirstOrDefault()?.DefectId);
            ViewData["ProAppId"] = ProAppSelectList(ncr?.NcrQas.FirstOrDefault()?.ProAppId);
            ViewData["StatusUpdateId"] = StatusSelectList(ncr?.StatusUpdateId);
        }

        private bool NcrExists(int id)
        {
            return _context.Ncrs.Any(e => e.NcrId == id);
        }
    }
}