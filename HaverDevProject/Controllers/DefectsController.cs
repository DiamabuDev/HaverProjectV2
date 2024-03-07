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
using HaverDevProject.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;
using Defect = HaverDevProject.Models.Defect;
using OfficeOpenXml;


namespace HaverDevProject.Controllers
{
    public class DefectsController : ElephantController
    {
        private readonly HaverNiagaraContext _context;

        public DefectsController(HaverNiagaraContext context)
        {
            _context = context;
        }

        // GET: Defects
        public async Task<IActionResult> Index(string SearchName, int? ItemID, int? page, int? pageSizeID,
            string actionButton, string sortDirection = "asc", string sortField = "Defect")
        {
            //List of sort options.
            string[] sortOptions = new[] { "Defect", "Description", "Item" };

            PopulateDropDownList();

            var defects = _context.Defects
                .Include(d => d.ItemDefects).ThenInclude(id => id.Item)
                .AsNoTracking();

            //Filterig values                       
            if (!String.IsNullOrEmpty(SearchName))
            {
                defects = defects.Where(d => d.DefectName.ToUpper().Contains(SearchName.ToUpper()));
            }

            if (ItemID.HasValue)
            {
                defects = defects.Where(d => d.ItemDefects.Any(id => id.ItemId == ItemID));
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
            if (sortField == "Defect")
            {
                if (sortDirection == "asc")
                {
                    defects = defects
                        .OrderBy(d => d.DefectName);
                    ViewData["filterApplied:DefectName"] = "<i class='bi bi-sort-up'></i>";

                }
                else
                {
                    defects = defects
                        .OrderByDescending(d => d.DefectName);
                    ViewData["filterApplied:DefectName"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Description")
            {
                if (sortDirection == "asc")
                {
                    defects = defects
                        .OrderBy(d => d.DefectDesription);
                    ViewData["filterApplied:Description"] = "<i class='bi bi-sort-up'></i>";

                }
                else
                {
                    defects = defects
                        .OrderByDescending(d => d.DefectDesription);
                    ViewData["filterApplied:Description"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else //Sorting by Item
            {
                if (sortDirection == "asc")
                {
                    defects = defects
                        .OrderBy(d => d.ItemDefects.Select(id => id.Item.ItemName).FirstOrDefault()).AsNoTracking();
                    ViewData["filterApplied:Item"] = "<i class='bi bi-sort-up'></i>";

                }
                else
                {
                    defects = defects
                        .OrderByDescending(d => d.ItemDefects.Select(id => id.Item.ItemName).FirstOrDefault()).AsNoTracking();
                    ViewData["filterApplied:Item"] = "<i class='bi bi-sort-down'></i>";
                }
            }

            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Defect>.CreateAsync(defects.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);

        }

        // GET: Defects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Defects == null)
            {
                return NotFound();
            }

            var defect = await _context.Defects
                .Include(d => d.ItemDefects).ThenInclude(id => id.Item)
                .FirstOrDefaultAsync(m => m.DefectId == id);
            if (defect == null)
            {
                return NotFound();
            }

            return View(defect);
        }

        // GET: Defects/Create
        public IActionResult Create()
        {
            Defect defect = new Defect();
            PopulateAssignedItemCheckboxes(defect);
            return View();
        }

        // POST: Defects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DefectId,DefectName,DefectDesription")] Defect defect, string[] selectedOptions)
        {
            try
            {
                if (selectedOptions != null)
                {
                    foreach (var condition in selectedOptions)
                    {
                        var itemToAdd = new ItemDefect { DefectId = defect.DefectId, ItemId = int.Parse(condition) };
                        defect.ItemDefects.Add(itemToAdd);
                    }
                }
                if (ModelState.IsValid)
                {
                    _context.Add(defect);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Defect created successfully!";
                    int newDefectId = defect.DefectId;
                    return RedirectToAction("Details", new { id = newDefectId });
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
            PopulateAssignedItemCheckboxes(defect);
            return View(defect);
        }

        // GET: Defects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.Defects == null)
            {
                return NotFound();
            }

            var defect = await _context.Defects
               .Include(d => d.ItemDefects).ThenInclude(id => id.Item)
               .FirstOrDefaultAsync(d => d.DefectId == id);

            if (defect == null)
            {
                return NotFound();
            }

            PopulateAssignedItemCheckboxes(defect);
            return View(defect);
        }

        // POST: Defects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] selectedOptions)
        {

            var defectToUpdate = await _context.Defects
                .Include(d => d.ItemDefects).ThenInclude(id => id.Item)
               .FirstOrDefaultAsync(d => d.DefectId == id);

            if (defectToUpdate == null)
            {
                return NotFound();
            }

            UpdateDefectItemsCheckboxes(selectedOptions, defectToUpdate);

            if (await TryUpdateModelAsync<Defect>(defectToUpdate, "",
                d => d.DefectName, d => d.DefectDesription))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Defect updated successfully!";
                    int updateDefectId = defectToUpdate.DefectId;
                    return RedirectToAction("Details", new { id = updateDefectId });
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            PopulateAssignedItemCheckboxes(defectToUpdate);
            return View(defectToUpdate);
        }

        // GET: Defects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Defects == null)
            {
                return NotFound();
            }

            var defect = await _context.Defects
                .FirstOrDefaultAsync(m => m.DefectId == id);
            if (defect == null)
            {
                return NotFound();
            }

            return View(defect);
        }

        // POST: Defects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Defects == null)
            {
                return Problem("Entity set 'HaverNiagaraContext.Defects'  is null.");
            }
            var defect = await _context.Defects.FindAsync(id);
            if (defect != null)
            {
                _context.Defects.Remove(defect);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private void PopulateAssignedItemCheckboxes(Defect defect)
        {
            var allItems = _context.Items
                .Select(i => new { i.ItemId, i.ItemName })
                .Distinct();

            var currentItemDefectIDs = new HashSet<int>(defect.ItemDefects.Select(id => id.ItemId));
            var checkBoxes = new List<CheckOptionVM>();
            foreach (var item in allItems)
            {
                checkBoxes.Add(new CheckOptionVM
                {
                    ID = item.ItemId,
                    DisplayText = item.ItemName, // Assuming Item has an ItemName property
                    Assigned = currentItemDefectIDs.Contains(item.ItemId)
                });
            }
            ViewData["ItemOptions"] = checkBoxes;
        }

        private void UpdateDefectItemsCheckboxes(string[] selectedItems, Defect defectToUpdate)
        {
            if (selectedItems == null)
            {
                defectToUpdate.ItemDefects = new List<ItemDefect>();
                return;
            }

            var selectedItemsHS = new HashSet<string>(selectedItems);
            var defectItemsHS = new HashSet<int>(defectToUpdate.ItemDefects.Select(id => id.ItemId));
            foreach (var item in _context.Items)
            {
                if (selectedItemsHS.Contains(item.ItemId.ToString()))
                {
                    if (!defectItemsHS.Contains(item.ItemId))
                    {
                        defectToUpdate.ItemDefects.Add(new ItemDefect { DefectId = defectToUpdate.DefectId, ItemId = item.ItemId });
                    }
                }
                else
                {
                    if (defectItemsHS.Contains(item.ItemId))
                    {
                        ItemDefect itemDefectToRemove = defectToUpdate.ItemDefects.FirstOrDefault(id => id.ItemId == item.ItemId);
                        if (itemDefectToRemove != null) _context.Remove(itemDefectToRemove);
                    }
                }
            }
        }

        private SelectList ItemSelectList(int? selectedId)
        {
            return new SelectList(_context.Items.OrderBy(i => i.ItemName).Select(i => new { i.ItemId, i.ItemName })
            .Distinct(), "ItemId", "ItemName", selectedId);
        }
        private void PopulateDropDownList(Item item = null)
        {
            ViewData["ItemID"] = ItemSelectList(item?.ItemId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                TempData["ErrorMessage"] = "Invalid file type. Please upload an Excel file (.xlsx).";
                return RedirectToAction(nameof(Index));
            }

            var errorMessages = new List<string>();
            var seenDefectNames = new HashSet<string>(); // Tracks defect names seen in the current upload

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    // First pass: Validate all rows
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var defectName = worksheet.Cells[row, 1].Value?.ToString().Trim();

                        // Check for required field
                        if (string.IsNullOrWhiteSpace(defectName))
                        {
                            errorMessages.Add($"Row {row}: Defect Type Name is required.");
                            continue;
                        }

                        // Check for in-memory duplicates
                        if (seenDefectNames.Contains(defectName))
                        {
                            errorMessages.Add($"Row {row}: Defect Type with Defect Type Name {defectName} is duplicated in the file.");
                            continue;
                        }

                        // Check for duplicates in the database
                        if (await _context.Defects.AsNoTracking().AnyAsync(d => d.DefectName == defectName))
                        {
                            errorMessages.Add($"Row {row}: Defect Type with Defect Type Name {defectName} already exists in the database.");
                            continue;
                        }

                        seenDefectNames.Add(defectName); // Mark this defect name as seen
                    }

                    // If errors were found, stop and return errors
                    if (errorMessages.Any())
                    {
                        TempData["ErrorMessage"] = $"Please fix the following errors and try again: {string.Join(" ", errorMessages)}";
                        return RedirectToAction(nameof(Index));
                    }

                    // Second pass: Insert rows into the database, since no errors were found
                    int successfulRows = 0;
                    foreach (var defectName in seenDefectNames)
                    {
                        var row = worksheet.Cells[worksheet.Dimension.Start.Row, 1, worksheet.Dimension.End.Row, 1]
                                            .FirstOrDefault(cell => cell.Value.ToString().Trim() == defectName);

                        if (row != null)
                        {
                            var defectDescription = worksheet.Cells[row.Start.Row, 2].Value?.ToString().Trim();

                            var newDefect = new Defect
                            {
                                DefectName = defectName,
                                DefectDesription = defectDescription
                            };

                            _context.Defects.Add(newDefect);
                            successfulRows++;
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"File uploaded successfully. {successfulRows} row(s) were added.";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Upload()
        {
            return View("UploadExcel");
        }

        private bool DefectExists(int id)
        {
            return _context.Defects.Any(e => e.DefectId == id);
        }

    }

}
