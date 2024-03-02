
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
using Microsoft.AspNetCore.Http.HttpResults;
using OfficeOpenXml;

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
        public async Task<IActionResult> Index(string SearchCode, int? SupplierID, int? DefectID, int? page, int? pageSizeID,
            string actionButton, string sortDirection = "asc", string sortField = "Code")
        {
            //List of sort options.
            string[] sortOptions = new[] { "Code", "Item", "Description", "Supplier", "Defect" };

            PopulateDropDownList();

            var items = _context.Items
                .Include(i => i.Supplier)
                .Include(d => d.ItemDefects).ThenInclude(id => id.Defect)
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

            if (DefectID.HasValue)
            {
                items = items.Where(d => d.ItemDefects.Any(id => id.DefectId == DefectID));
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
            else if (sortField == "Supplier")
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
            else //Sorting by Item
            {
                if (sortDirection == "asc")
                {
                    items = items
                        .OrderBy(d => d.ItemDefects.Select(id => id.Defect.DefectName).FirstOrDefault()).AsNoTracking();
                    ViewData["filterApplied:Defect"] = "<i class='bi bi-sort-up'></i>";

                }
                else
                {
                    items = items
                        .OrderByDescending(d => d.ItemDefects.Select(id => id.Defect.DefectName).FirstOrDefault()).AsNoTracking();
                    ViewData["filterApplied:Defect"] = "<i class='bi bi-sort-down'></i>";
                }

            }
            
            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

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
            PopulateDropDownList();
            Item item = new Item();
            PopulateAssignedDefectCheckboxes(item);
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
            PopulateAssignedDefectCheckboxes(item);
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
            PopulateAssignedDefectCheckboxes(itemToUpdate);
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
        private SelectList DefectSelectList(int? selectedId)
        {
            return new SelectList(_context.Defects.OrderBy(i => i.DefectName).Select(i => new { i.DefectId, i.DefectName })
            .Distinct(), "DefectId", "DefectName", selectedId);
        }
        private void PopulateDropDownList(Supplier supplier = null, Defect defect = null)
        {
            ViewData["SupplierID"] = SupplierSelectList(supplier?.SupplierId);
            ViewData["DefectID"] = DefectSelectList(defect?.DefectId);
        }

        private void PopulateAssignedDefectCheckboxes(Item item)
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
            ViewData["DefectOptions"] = checkBoxes;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                TempData["ErrorMessage"] = "Invalid file type. Please upload an Excel file (.xlsx). \n";
                return RedirectToAction(nameof(Index));
            }
            var errorMessages = new List<string>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var transaction = _context.Database.BeginTransaction(); // Start transaction for each row
                        try
                        {
                            var itemNumber = worksheet.Cells[row, 1].Value?.ToString().Trim();
                            var itemName = worksheet.Cells[row, 2].Value?.ToString().Trim();
                            var supplierCode = worksheet.Cells[row, 3].Value?.ToString().Trim();
                            var supplierName = worksheet.Cells[row, 4].Value?.ToString().Trim();
                            var defectTypeName = worksheet.Cells[row, 5].Value?.ToString().Trim();

                            if (string.IsNullOrEmpty(itemNumber) || string.IsNullOrEmpty(itemName))
                            {
                                errorMessages.Add($"Item Number and Item Name are required. \n");
                            }

                            var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.ItemNumber.ToString() == itemNumber);
                            if (existingItem != null)
                            {
                                errorMessages.Add($"Item with Item Number {itemNumber} already exists. \n");
                            }

                            int supplierId = 0;
                            if (!string.IsNullOrEmpty(supplierCode) && !string.IsNullOrEmpty(supplierName))
                            {
                                var supplier = await _context.Suppliers
                                                              .FirstOrDefaultAsync(s => s.SupplierCode == supplierCode && s.SupplierName == supplierName);
                                if (supplier == null)
                                {
                                    supplier = new Supplier { SupplierCode = supplierCode, SupplierName = supplierName };
                                    _context.Suppliers.Add(supplier);
                                    await _context.SaveChangesAsync(); // Ensure SupplierId is generated
                                }
                                supplierId = supplier.SupplierId;
                            }
                            else if (!string.IsNullOrEmpty(supplierCode) || !string.IsNullOrEmpty(supplierName))
                            {
                                errorMessages.Add($"Both Supplier Code and Supplier Name are required. \n");
                            }
                            else
                            {
                                // Handle "NO SUPPLIER PROVIDED" case
                                var defaultSupplier = await _context.Suppliers
                                                                     .FirstOrDefaultAsync(s => s.SupplierName == "NO SUPPLIER PROVIDED");
                                if (defaultSupplier == null)
                                {
                                    defaultSupplier = new Supplier { SupplierName = "NO SUPPLIER PROVIDED" };
                                    _context.Suppliers.Add(defaultSupplier);
                                    await _context.SaveChangesAsync();
                                }
                                supplierId = defaultSupplier.SupplierId;
                            }

                            var item = new Item
                            {
                                ItemNumber = Int32.Parse(itemNumber),
                                ItemName = itemName,
                                SupplierId = supplierId
                            };

                            _context.Items.Add(item);
                            await _context.SaveChangesAsync();

                            if (!string.IsNullOrEmpty(defectTypeName))
                            {
                                var defect = await _context.Defects.FirstOrDefaultAsync(d => d.DefectName == defectTypeName);
                                if (defect == null)
                                {
                                    defect = new Defect { DefectName = defectTypeName };
                                    _context.Defects.Add(defect);
                                    await _context.SaveChangesAsync();
                                }

                                var itemDefect = new ItemDefect
                                {
                                    ItemId = item.ItemId,
                                    DefectId = defect.DefectId
                                };

                                _context.ItemDefects.Add(itemDefect);
                                await _context.SaveChangesAsync();
                            }

                            transaction.Commit(); // Commit transaction if all operations succeed
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback(); // Rollback transaction on error
                            errorMessages.Add($"Row {row}: {ex.Message}");
                        }
                    }
                }
            }

            if (errorMessages.Any())
            {
                TempData["ErrorMessage"] = $"Fix error(s) and try to upload again: {string.Join(" ", errorMessages)}";
            }
            else
            {
                TempData["SuccessMessage"] = "File uploaded successfully.";

            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Upload()
        {
            return View("UploadExcel");
        }

        private bool ItemExists(int id)
        {
          return _context.Items.Any(e => e.ItemId == id);
        }
    }
}
