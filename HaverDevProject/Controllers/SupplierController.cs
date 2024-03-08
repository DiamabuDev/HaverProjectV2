using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaverDevProject.CustomControllers;
using HaverDevProject.Data;
using HaverDevProject.Models;
using HaverDevProject.Utilities;
using HaverDevProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml;

//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HaverDevProject.Controllers
{
    public class SupplierController : ElephantController
    {
        private readonly HaverNiagaraContext _context;

        public SupplierController(HaverNiagaraContext context)
        {
            _context = context;
        }

        // GET: Supplier
        public async Task<IActionResult> Index(
            string SearchCode,
            string SearchContact,
            int? page,
            int? pageSizeID,
            string actionButton,
            string sortDirection = "asc",
            string sortField = "Code",
            string filter = "Active"
        )
        {
            //List of sort options.
            string[] sortOptions = new[] { "Code", "Name", "Email", "Contact" };

            var suppliers = _context.Suppliers.AsNoTracking();

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
                    suppliers = suppliers.Where(s => s.SupplierStatus == true);
                    ViewData["filterApplied:ButtonActive"] = "btn-success";
                    ViewData["filterApplied:ButtonAll"] = "btn-primary custom-opacity";
                    ViewData["filterApplied:ButtonClosed"] = "btn-danger custom-opacity";
                }
                else //(filter == "Closed")
                {
                    suppliers = suppliers.Where(s => s.SupplierStatus == false);
                    ViewData["filterApplied:ButtonClosed"] = "btn-danger";
                    ViewData["filterApplied:ButtonAll"] = "btn-primary custom-opacity";
                    ViewData["filterApplied:ButtonActive"] = "btn-success custom-opacity";
                }
            }

            if (!String.IsNullOrEmpty(SearchCode))
            {
                suppliers = suppliers.Where(s =>
                    s.SupplierCode.ToUpper().Contains(SearchCode.ToUpper())
                    || s.SupplierName.ToUpper().Contains(SearchCode.ToUpper())
                );
            }

            if (!String.IsNullOrEmpty(SearchContact))
            {
                suppliers = suppliers.Where(s =>
                    s.SupplierContactName.ToUpper().Contains(SearchContact.ToUpper())
                );
            }

            //Sorting columns
            if (!String.IsNullOrEmpty(actionButton)) //Form Submitted!
            {
                page = 1; //Reset page to start

                if (sortOptions.Contains(actionButton)) //Change of sort is requested
                {
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton; //Sort by the button clicked
                }
            }
            //Now we know which field and direction to sort by
            if (sortField == "Code")
            {
                if (sortDirection == "asc")
                {
                    suppliers = suppliers.OrderBy(p => p.SupplierCode);
                    ViewData["filterApplied:SupplierCode"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    suppliers = suppliers.OrderByDescending(p => p.SupplierCode);
                    ViewData["filterApplied:SupplierCode"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Name")
            {
                if (sortDirection == "asc")
                {
                    suppliers = suppliers.OrderBy(p => p.SupplierName);
                    ViewData["filterApplied:SupplierName"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    suppliers = suppliers.OrderByDescending(p => p.SupplierName);
                    ViewData["filterApplied:SupplierName"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Email") //Sorting by Email
            {
                if (sortDirection == "asc")
                {
                    suppliers = suppliers.OrderBy(p => p.SupplierEmail);
                    ViewData["filterApplied:SupplierEmail"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    suppliers = suppliers.OrderByDescending(p => p.SupplierEmail);
                    ViewData["filterApplied:SupplierEmail"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Status")
            {
                if (sortDirection == "asc")
                {
                    suppliers = suppliers.OrderBy(s => s.SupplierStatus);
                    ViewData["filterApplied:Status"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    suppliers = suppliers.OrderByDescending(s => s.SupplierStatus);
                    ViewData["filterApplied:Status"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else //Sorting by Contact
            {
                if (sortDirection == "asc")
                {
                    suppliers = suppliers.OrderBy(s => s.SupplierContactName);
                    ViewData["filterApplied:SupplierContact"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    suppliers = suppliers.OrderByDescending(s => s.SupplierContactName);
                    ViewData["filterApplied:SupplierContact"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            ViewData["filter"] = filter;

            //return View(await suppliers.ToListAsync());

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Supplier>.CreateAsync(
                suppliers.AsNoTracking(),
                page ?? 1,
                pageSize
            );

            return View(pagedData);
        }

        // GET: Supplier/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await _context
                .Suppliers.Include(s => s.Items)
                .ThenInclude(s => s.NcrQas)
                .ThenInclude(s => s.Ncr)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.SupplierId == id);
            if (supplier == null)
            {
                return NotFound();
            }

            var supplierViewModel = new SupplierDetailsViewModel
            {
                Supplier = supplier,
                RelatedNCRs =
                    supplier.Items.FirstOrDefault()?.NcrQas?.Select(nqa => nqa.Ncr).ToList()
                    ?? new List<Ncr>()
            };

            return View(supplierViewModel);
        }

        // GET: Supplier/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Supplier/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("SupplierId,SupplierCode,SupplierName,SupplierContactName,SupplierEmail")]
                Supplier supplier
        )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(supplier);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Supplier created successfully!";
                    int newSupplierId = supplier.SupplierId;
                    return RedirectToAction("Details", new { id = newSupplierId });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator."
                );
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    ModelState.AddModelError(
                        "SupplierCode",
                        "Unable to save changes. Remember, you cannot have duplicate Supplier Code."
                    );
                }
                else
                {
                    ModelState.AddModelError(
                        "",
                        "Unable to save changes. Try again, and if the problem persists see your system administrator."
                    );
                }
            }

            return View(supplier);
        }

        // GET: Supplier/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Supplier/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind(
                "SupplierId,SupplierCode,SupplierName,SupplierContactName,SupplierEmail,SupplierStatus"
            )]
                Supplier supplier
        )
        {
            if (id != supplier.SupplierId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Supplier updated successfully!";
                    int updateSupplierId = supplier.SupplierId;
                    return RedirectToAction("Details", new { id = updateSupplierId });
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError(
                        "",
                        "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator."
                    );
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.SupplierId))
                    {
                        ModelState.AddModelError(
                            "",
                            "Unable to save changes. The Supplier was deleted by another user."
                        );
                    }
                }
                catch (DbUpdateException dex)
                {
                    if (dex.GetBaseException().Message.Contains("UNIQUE"))
                    {
                        ModelState.AddModelError(
                            "SupplierCode",
                            "Unable to save changes. Remember, you cannot have duplicate Supplier Code."
                        );
                    }
                    else
                    {
                        ModelState.AddModelError(
                            "",
                            "Unable to save changes. Try again, and if the problem persists see your system administrator."
                        );
                    }
                }
            }
            return View(supplier);
        }

        // GET: Supplier/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FirstOrDefaultAsync(m => m.SupplierId == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Suppliers == null)
            {
                return Problem("There are no Suppliers to delete");
            }
            var supplier = await _context.Suppliers.FindAsync(id);

            try
            {
                if (supplier != null)
                {
                    _context.Suppliers.Remove(supplier);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError(
                        "",
                        "Unable to Delete Supplier. Remember, you cannot delete a Supplier that has a NCR in the system."
                    );
                }
                else
                {
                    ModelState.AddModelError(
                        "",
                        "Unable to save changes. Try again, and if the problem persists see your system administrator."
                    );
                }
            }
            return View(supplier);
        }

        public async Task<IActionResult> SupplierReport(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ncrData = await _context
                .Ncrs.Include(n => n.NcrQa)
                .ThenInclude(qa => qa.Item)
                .ThenInclude(i => i.Supplier)
                .Include(n => n.NcrQa)
                .ThenInclude(qa => qa.Item)
                .ThenInclude(i => i.ItemDefects)
                .ThenInclude(i => i.Defect)
                .Include(n => n.NcrEng)
                .ThenInclude(e => e.EngDispositionType)
                .Include(n => n.NcrOperation)
                .ThenInclude(o => o.FollowUpType)
                .FirstOrDefaultAsync(n => n.NcrId == id);

            if (ncrData == null)
            {
                return NotFound();
            }

            NcrSupplierReportDTO reportDto = new NcrSupplierReportDTO
            {
                NcrNumber = ncrData.NcrNumber,
                NcrStatus = ncrData.NcrStatus,
                SupplierName = ncrData.NcrQa?.Item?.Supplier?.SupplierName ?? "Not Available",
                NcrQaOrderNumber = ncrData.NcrQa?.NcrQaOrderNumber ?? "Not Available",
                ItemSAP = ncrData.NcrQa?.Item?.ItemNumber ?? 0,
                ItemName = ncrData.NcrQa?.Item?.ItemName ?? "Not Available",
                NcrQaQuanReceived = ncrData.NcrQa?.NcrQaQuanReceived ?? 0,
                NcrQaQuanDefective = ncrData.NcrQa?.NcrQaQuanDefective ?? 0,
                NcrQaDefect = ncrData.NcrQa?.Defect.DefectName ?? "Not Available",
                NcrQaDescriptionOfDefect =
                    ncrData.NcrQa?.NcrQaDescriptionOfDefect ?? "Not Available",
                EngDispositionType =
                    ncrData.NcrEng?.EngDispositionType?.EngDispositionTypeName ?? "Not Available",
                EngDispositionDescription =
                    ncrData.NcrEng?.NcrEngDispositionDescription ?? "Not Available",
                OpDispositionType =
                    ncrData.NcrOperation?.OpDispositionType?.OpDispositionTypeName
                    ?? "Not Available",
                OperationDescription =
                    ncrData.NcrOperation?.NcrPurchasingDescription ?? "Not Available",
            };

            foreach (var itemDefect in ncrData.NcrQa.Item.ItemDefects)
            {
                if (itemDefect.Defect != null && itemDefect.Defect.DefectName != null)
                {
                    reportDto.DefectNames.Add(itemDefect.Defect.DefectName);
                }
                else
                {
                    reportDto.DefectNames.Add("No Defect Available");
                }
            }

            return View("SupplierReport", reportDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (
                file.ContentType
                != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            )
            {
                TempData["ErrorMessage"] =
                    "Invalid file type. Please upload an Excel file (.xlsx).";
                return RedirectToAction(nameof(Index));
            }

            var errorMessages = new List<string>(); // Initialize a list to store error messages
            int expectedSuccessRows = 0;
            var validSuppliers = new List<Supplier>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    expectedSuccessRows = rowCount - 1;

                    for (int row = 2; row <= rowCount; row++)
                    {

                        var supplierCodeStr = worksheet.Cells[row, 1].Value?.ToString().Trim();
                        var supplierName = worksheet.Cells[row, 2].Value?.ToString().Trim();
                        var contactName = worksheet.Cells[row, 3].Value?.ToString().Trim();
                        var email = worksheet.Cells[row, 4].Value?.ToString().Trim();

                        // Supplier Code and Supplier Name are required
                        if (string.IsNullOrWhiteSpace(supplierCodeStr))
                        {
                            errorMessages.Add($"Row {row}: Supplier Code is required.");
                        }                        
                        // Check if Supplier Code is a number
                        if (!int.TryParse(supplierCodeStr, out int supplierCode))
                        {
                            errorMessages.Add($"Row {row}: Supplier Code must be a number.");
                        }

                        if (string.IsNullOrWhiteSpace(supplierName))
                        {
                            errorMessages.Add($"Row {row}: Supplier Name is required.");
                        }

                        var existingSupplier = await _context
                            .Suppliers.AsNoTracking() // Use AsNoTracking for read-only query
                            .FirstOrDefaultAsync(s =>
                                s.SupplierCode == supplierCodeStr
                                && s.SupplierName == supplierName
                            );

                        if (existingSupplier != null)
                        {
                            errorMessages.Add(
                                $"Row {row}: Supplier with Supplier Code {supplierCodeStr} and Supplier Name {supplierName} already exists."
                            );
                        }

                        if (!errorMessages.Any())
                        {
                            var newSupplier = new Supplier
                            {
                                SupplierCode = supplierCodeStr,
                                SupplierName = supplierName,
                                SupplierContactName = string.IsNullOrWhiteSpace(contactName)
                                    ? null
                                    : contactName,
                                SupplierEmail = string.IsNullOrWhiteSpace(email) ? null : email
                            };

                            validSuppliers.Add(newSupplier);
                        }
                    }
                }
            }

            if (validSuppliers.Count != expectedSuccessRows)
            {
                if (errorMessages.Count > 10)
                {
                    TempData["ErrorMessage"] = $"There are errors in {errorMessages.Count} rows. Please review and fix the errors before uploading again.";
                }else
                {
                    TempData["ErrorMessage"] =
                         $"Fix error(s) and try to upload again: <br><li>{string.Join("<li>", errorMessages)}";
                }
            }
            else
            {
                foreach (var item in validSuppliers)
                {
                    var transaction = _context.Database.BeginTransaction(); // Start transaction for each row

                    try
                    {
                        _context.Suppliers.Add(item);
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        break;
                    }
                }

                TempData["SuccessMessage"] =
                    $"File uploaded successfully. {expectedSuccessRows} rows added.";

            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Upload()
        {
            return View("UploadExcel");
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.SupplierId == id);
        }
    }
}
