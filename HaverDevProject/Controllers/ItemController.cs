
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
using Microsoft.EntityFrameworkCore.Storage;
using HaverDevProject.ViewModels;

namespace HaverDevProject.Controllers
{
    public class ItemController : ElephantController
    {
        private readonly HaverNiagaraContext _context;

        public ItemController(HaverNiagaraContext context)
        {
            _context = context;
        }

        // GET: Item
        public async Task<IActionResult> Index(string SearchCode, int? SupplierID, int? page, int? pageSizeID,
            string actionButton, string sortDirection = "asc", string sortField = "Code")
        {
            //List of sort options.
            string[] sortOptions = new[] { "Code", "Item", "Description", "Supplier" };

            PopulateDrodDownList();

            var items = _context.Items
                .Include(i => i.Supplier)
                .AsNoTracking();

            //Filterig values                       
            if (!String.IsNullOrEmpty(SearchCode))
            {
                items = items.Where(s => s.ItemName.ToUpper().Contains(SearchCode.ToUpper())
                                        || s.Supplier.SupplierName.ToUpper().Contains(SearchCode.ToUpper()));
            }

            if (SupplierID.HasValue)
            {
                items = items.Where(s => s.Supplier.SupplierId == SupplierID);
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
            if (sortField == "Code")
            {
                if (sortDirection == "asc")
                {
                    items = items
                        .OrderBy(p => p.ItemNumber);
                    ViewData["filterApplied:ItemNumber"] = "<i class='bi bi-sort-up'></i>";

                }
                else
                {
                    items = items
                        .OrderByDescending(p => p.ItemNumber);
                    ViewData["filterApplied:ItemNumber"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Item")
            {
                if (sortDirection == "asc")
                {
                    items = items
                        .OrderBy(p => p.ItemName);
                    ViewData["filterApplied:ItemName"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    items = items
                        .OrderByDescending(p => p.ItemName);
                    ViewData["filterApplied:ItemName"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            //else if (sortField == "Description")
            //{
            //    if (sortDirection == "asc")
            //    {
            //        items = items
            //            .OrderBy(p => p.ItemDescription);
            //        ViewData["filterApplied:Description"] = "<i class='bi bi-sort-up'></i>";

            //    }
            //    else
            //    {
            //        items = items
            //            .OrderByDescending(p => p.ItemDescription);
            //        ViewData["filterApplied:Description"] = "<i class='bi bi-sort-down'></i>";

            //    }
            //}
            else //Sorting by Supplier Name
            {
                if (sortDirection == "asc")
                {
                    items = items
                        .OrderBy(p => p.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-up'></i>";

                }
                else
                {
                    items = items
                        .OrderByDescending(p => p.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            //return View(await suppliers.ToListAsync());

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Item>.CreateAsync(items.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);

        }

        // GET: Item/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Supplier)
                .Include(d => d.ItemDefects).ThenInclude(id => id.Defect)
                .FirstOrDefaultAsync(m => m.ItemId == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Item/Create
        public IActionResult Create()
        {
            //ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName");
            //Item item = new Item();
            PopulateDrodDownList();
            Item item = new Item();
            PopulateAssignedItemCheckboxes(item);
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,ItemNumber,ItemName,ItemDescription,SupplierId")] Item item, string[] selectedOptions)
        {
            try
            {
                if (selectedOptions != null)
                {
                    foreach (var condition in selectedOptions)
                    {
                        var defectToAdd = new ItemDefect { ItemId = item.ItemId, DefectId = int.Parse(condition) };
                        item.ItemDefects.Add(defectToAdd);
                    }
                }

                if (ModelState.IsValid)
                {
                    _context.Add(item);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Item created successfully!";
                    int newItemId = item.ItemId;
                    return RedirectToAction("Details", new { id = newItemId });
                    //return RedirectToAction(nameof(Index));
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    ModelState.AddModelError("ItemNumber", "Unable to save changes. Remember, you cannot have duplicate SAP Number.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            ViewBag.SupplierId = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", item.SupplierId);
            return View(item);
        }

        // GET: Item/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }
            var item = await _context.Items
                .Include(d => d.ItemDefects).ThenInclude(id => id.Defect)
                .Include(i => i.Supplier)                
                .FirstOrDefaultAsync(d => d.ItemId == id);

            if (item == null)
            {
                return NotFound();
            }
            PopulateAssignedItemCheckboxes(item);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", item.SupplierId);
            return View(item);
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] selectedOptions)
        {
            var itemToUpdate = await _context.Items
                .Include(d => d.ItemDefects).ThenInclude(id =>id.Defect)
                .Include (i => i.Supplier)
                .FirstOrDefaultAsync(i => i.ItemId == id);

            if (itemToUpdate == null)
            {
                return NotFound();
            }

            UpdateDefectItemsCheckboxes(selectedOptions, itemToUpdate);

            if (await TryUpdateModelAsync<Item>(itemToUpdate, "",
                    i => i.ItemNumber, i => i.ItemName, i => i.ItemDescription))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Item updated successfully!";
                    int updateItemId = itemToUpdate.ItemId;
                    return RedirectToAction("Details", new { id = updateItemId });
                    //return RedirectToAction(nameof(Index));
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateException dex)
                {
                    if (dex.GetBaseException().Message.Contains("UNIQUE"))
                    {
                        ModelState.AddModelError("ItemNumber", "Unable to save changes. Remember, you cannot have duplicate SAP Number.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }

            }
            PopulateAssignedItemCheckboxes(itemToUpdate);
            ViewBag.SupplierId = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", itemToUpdate.SupplierId);
            return View(itemToUpdate);
        }

        // GET: Item/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(m => m.ItemId == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'HaverNiagaraContext.Items'  is null.");
            }
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private SelectList SupplierSelectList(int? selectedId)
        {
            return new SelectList(_context.Suppliers.OrderBy(s => s.SupplierName), "SupplierId", "SupplierName", selectedId);
        }

        private void PopulateDrodDownList(Supplier supplier = null)
        {
            ViewData["SupplierID"] = SupplierSelectList(supplier?.SupplierId);
        }

        private void PopulateAssignedItemCheckboxes(Item item)
        {
            var allDefects = _context.Defects
                .Select(d => new { d.DefectId, d.DefectName })
                .Distinct();

            var currentItemDefectIDs = new HashSet<int>(item.ItemDefects.Select(id => id.DefectId)); //checar
            var checkBoxes = new List<CheckOptionVM>();
            foreach (var defect in allDefects)
            {
                checkBoxes.Add(new CheckOptionVM
                {
                    ID = defect.DefectId,
                    DisplayText = defect.DefectName,
                    Assigned = currentItemDefectIDs.Contains(defect.DefectId)
                });
            }
            ViewData["ItemOptions"] = checkBoxes;
            //ViewData["ItemOptions"] = new MultiSelectList(checkBoxes.OrderBy(d => d.DisplayText), "ID", "DisplayTex");
        }

        private void UpdateDefectItemsCheckboxes(string[] selectedDefects, Item itemToUpdate)
        {
            if (selectedDefects == null)
            {
                itemToUpdate.ItemDefects = new List<ItemDefect>();
                return;
            }

            var selectedDefectsHS = new HashSet<string>(selectedDefects);
            var defectItemsHS = new HashSet<int>(itemToUpdate.ItemDefects.Select(id => id.DefectId));
            foreach (var defect in _context.Defects)
            {
                if (selectedDefectsHS.Contains(defect.DefectId.ToString()))
                {
                    if (!defectItemsHS.Contains(defect.DefectId))
                    {
                        itemToUpdate.ItemDefects.Add(new ItemDefect { ItemId = itemToUpdate.ItemId, DefectId = defect.DefectId });
                    }
                }
                else
                {
                    if (defectItemsHS.Contains(defect.DefectId))
                    {
                        ItemDefect itemDefectToRemove = itemToUpdate.ItemDefects.FirstOrDefault(id => id.DefectId == defect.DefectId);
                        if (itemDefectToRemove != null) _context.Remove(itemDefectToRemove);
                    }
                }
            }
        }

        private bool ItemExists(int id)
        {
          return _context.Items.Any(e => e.ItemId == id);
        }
    }
}
