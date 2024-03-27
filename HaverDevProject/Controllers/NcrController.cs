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
using HaverDevProject.Services;
using Microsoft.AspNetCore.Authorization;
using Spire.Xls;
using OfficeOpenXml;
using System.Drawing;
using System.IO;
using HaverDevProject.Configurations;

namespace HaverDevProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NcrController : ElephantController
    {
        private readonly HaverNiagaraContext _context;
        private readonly ITargetYearService _targetYearService;

        public NcrController(HaverNiagaraContext context, ITargetYearService targetYearService)
        {
            _context = context;
            _targetYearService = targetYearService;
        }

        // GET: Ncr
        public async Task<IActionResult> Index(string SearchCode, string SearchSupplier, DateTime StartDate, DateTime EndDate,
            int? page, int? pageSizeID, string actionButton, string sortDirection = "desc", string sortField = "Created", string filter = "Active")
        {
            ViewData["Filtering"] = "btn-block invisible";
            int numberFilters = 0;

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
            string[] sortOptions = new[] { "Created", "NCR #", "Supplier", "Defect", "PO Number", "Phase" };

            var ncr = _context.Ncrs
                //.Include(n => n.Item).ThenInclude(n => n.ItemDefects).ThenInclude(n => n.Defect)
                .Include(n => n.NcrQa.Supplier)
                .Include(n => n.NcrQa.Defect)
                .AsNoTracking();

            //foreach (var ncrItem in ncr)
            //{
            //    if (ncrItem.NcrQa.NcrQacreationDate.AddYears(5) <= DateTime.UtcNow)
            //    {
            //        // Call the ArchiveNcr method for this specific item
            //        await ArchiveDateNcr(ncrItem.NcrId);
            //    }
            //}

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
                    ncr = ncr.Where(n => n.NcrStatus == true);
                    ViewData["filterApplied:ButtonActive"] = "btn-success";
                    ViewData["filterApplied:ButtonAll"] = "btn-primary custom-opacity";
                    ViewData["filterApplied:ButtonClosed"] = "btn-danger custom-opacity";
                }
                else //(filter == "Closed")
                {
                    ncr = ncr.Where(n => n.NcrStatus == false);
                    ViewData["filterApplied:ButtonClosed"] = "btn-danger";
                    ViewData["filterApplied:ButtonAll"] = "btn-primary custom-opacity";
                    ViewData["filterApplied:ButtonActive"] = "btn-success custom-opacity";
                }
            }
            if (!String.IsNullOrEmpty(SearchCode))
            {
                ncr = ncr.Where(s => s.NcrQa.Defect.DefectName.ToUpper().Contains(SearchCode.ToUpper()) //(s => s.Item.ItemDefects.FirstOrDefault().Defect.DefectName.ToUpper().Contains(SearchCode.ToUpper()) 
                || s.NcrNumber.ToUpper().Contains(SearchCode.ToUpper()));
                numberFilters++;
            }
            if (!String.IsNullOrEmpty(SearchSupplier))
            {
                ncr = ncr.Where(n => n.NcrQa.Supplier.SupplierName == SearchSupplier);
                numberFilters++;
            }
            if (StartDate == EndDate)
            {
                ncr = ncr.Where(n => n.NcrQa.NcrQacreationDate == StartDate);
                numberFilters++;
            }
            else
            {
                ncr = ncr.Where(n => n.NcrQa.NcrQacreationDate >= StartDate &&
                         n.NcrQa.NcrQacreationDate <= EndDate);
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
                        .OrderBy(p => p.NcrQa.Defect.DefectName);
                    ViewData["filterApplied:Defect"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQa.Defect.DefectName);
                    ViewData["filterApplied:Defect"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Supplier")
            {
                if (sortDirection == "asc")
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrQa.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQa.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Created")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrQa.NcrQacreationDate);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQa.NcrQacreationDate);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Phase")
            {
                if (sortDirection == "asc")
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrPhase); //.OrderBy(p => p.Ncr.NcrStatus);
                    ViewData["filterApplied:Phase"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrPhase); //.OrderByDescending(p => p.Ncr.NcrStatus);
                    ViewData["filterApplied:Phase"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Last Updated")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrLastUpdated);
                    ViewData["filterApplied:Last Updated"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrLastUpdated);
                    ViewData["filterApplied:Last Updated"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else //(sortField == "PO Number")
            {
                if (sortDirection == "asc")
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrQa.NcrQaOrderNumber);
                    ViewData["filterApplied:PONumber"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQa.NcrQaOrderNumber);
                    ViewData["filterApplied:PONumber"] = "<i class='bi bi-sort-down'></i>";
                }
            }

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;
            ViewData["filter"] = filter;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Ncr>.CreateAsync(ncr.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }
        // GET: Archived Ncrs
        public async Task<IActionResult> Archived(string SearchCode, string SearchSupplier, DateTime StartDate, DateTime EndDate,
            int? page, int? pageSizeID, string actionButton, string sortDirection = "desc", string sortField = "Created", string filter = "Active")
        {
            ViewData["Filtering"] = "btn-block invisible";
            int numberFilters = 0;

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
            string[] sortOptions = new[] { "Created", "NCR #", "Supplier", "Defect", "PO Number", "Phase" };

            var ncr = _context.Ncrs
                //.Include(n => n.Item).ThenInclude(n => n.ItemDefects).ThenInclude(n => n.Defect)
                .Include(n => n.NcrQa.Supplier)
                .Include(n => n.NcrQa.Defect)
                .Where(ncr => ncr.NcrPhase == NcrPhase.Archive)
                .AsNoTracking();

            //Filterig values            
            //if (!String.IsNullOrEmpty(filter))
            //{
            //    if (filter == "Archived")
            //    {
            //        // Filter only archived records
            //        ncr = ncr.Where(ncr => ncr.NcrPhase == NcrPhase.Archive);
            //    }
            //}
            //    else if (filter == "Active")
            //    {
            //        ncr = ncr.Where(n => n.NcrStatus == true);
            //        ViewData["filterApplied:ButtonActive"] = "btn-success";
            //        ViewData["filterApplied:ButtonAll"] = "btn-primary custom-opacity";
            //        ViewData["filterApplied:ButtonClosed"] = "btn-danger custom-opacity";
            //    }
            //    else //(filter == "Closed")
            //    {
            //        ncr = ncr.Where(n => n.NcrStatus == false);
            //        ViewData["filterApplied:ButtonClosed"] = "btn-danger";
            //        ViewData["filterApplied:ButtonAll"] = "btn-primary custom-opacity";
            //        ViewData["filterApplied:ButtonActive"] = "btn-success custom-opacity";
            //    }
            //}
            if (!String.IsNullOrEmpty(SearchCode))
            {
                ncr = ncr.Where(s => s.NcrQa.Defect.DefectName.ToUpper().Contains(SearchCode.ToUpper()) //(s => s.Item.ItemDefects.FirstOrDefault().Defect.DefectName.ToUpper().Contains(SearchCode.ToUpper()) 
                || s.NcrNumber.ToUpper().Contains(SearchCode.ToUpper()));
                numberFilters++;
            }
            if (!String.IsNullOrEmpty(SearchSupplier))
            {
                ncr = ncr.Where(n => n.NcrQa.Supplier.SupplierName == SearchSupplier);
                numberFilters++;
            }
            if (StartDate == EndDate)
            {
                ncr = ncr.Where(n => n.NcrQa.NcrQacreationDate == StartDate);
                numberFilters++;
            }
            else
            {
                ncr = ncr.Where(n => n.NcrQa.NcrQacreationDate >= StartDate &&
                         n.NcrQa.NcrQacreationDate <= EndDate);
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
                        .OrderBy(p => p.NcrQa.Defect.DefectName);
                    ViewData["filterApplied:Defect"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQa.Defect.DefectName);
                    ViewData["filterApplied:Defect"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Supplier")
            {
                if (sortDirection == "asc")
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrQa.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQa.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Created")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrQa.NcrQacreationDate);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQa.NcrQacreationDate);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Phase")
            {
                if (sortDirection == "asc")
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrPhase); //.OrderBy(p => p.Ncr.NcrStatus);
                    ViewData["filterApplied:Phase"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrPhase); //.OrderByDescending(p => p.Ncr.NcrStatus);
                    ViewData["filterApplied:Phase"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Last Updated")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrLastUpdated);
                    ViewData["filterApplied:Last Updated"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrLastUpdated);
                    ViewData["filterApplied:Last Updated"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else //(sortField == "PO Number")
            {
                if (sortDirection == "asc")
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrQa.NcrQaOrderNumber);
                    ViewData["filterApplied:PONumber"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQa.NcrQaOrderNumber);
                    ViewData["filterApplied:PONumber"] = "<i class='bi bi-sort-down'></i>";
                }
            }

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;
            ViewData["filter"] = filter;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Ncr>.CreateAsync(ncr.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }
        // GET: Archived Ncrs
        public async Task<IActionResult> Void(string SearchCode, string SearchSupplier, DateTime StartDate, DateTime EndDate,
            int? page, int? pageSizeID, string actionButton, string sortDirection = "desc", string sortField = "Created", string filter = "Active")
        {
            ViewData["Filtering"] = "btn-block invisible";
            int numberFilters = 0;

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
            string[] sortOptions = new[] { "Created", "NCR #", "Supplier", "Defect", "PO Number", "Phase" };

            var ncr = _context.Ncrs
                //.Include(n => n.Item).ThenInclude(n => n.ItemDefects).ThenInclude(n => n.Defect)
                .Include(n => n.NcrQa.Supplier)
                .Include(n => n.NcrQa.Defect)
                .Where(ncr => ncr.NcrPhase == NcrPhase.Void)
                .AsNoTracking();

            //Filterig values            
            //if (!String.IsNullOrEmpty(filter))
            //{
            //    if (filter == "Archived")
            //    {
            //        // Filter only archived records
            //        ncr = ncr.Where(ncr => ncr.NcrPhase == NcrPhase.Archive);
            //    }
            //}
            //    else if (filter == "Active")
            //    {
            //        ncr = ncr.Where(n => n.NcrStatus == true);
            //        ViewData["filterApplied:ButtonActive"] = "btn-success";
            //        ViewData["filterApplied:ButtonAll"] = "btn-primary custom-opacity";
            //        ViewData["filterApplied:ButtonClosed"] = "btn-danger custom-opacity";
            //    }
            //    else //(filter == "Closed")
            //    {
            //        ncr = ncr.Where(n => n.NcrStatus == false);
            //        ViewData["filterApplied:ButtonClosed"] = "btn-danger";
            //        ViewData["filterApplied:ButtonAll"] = "btn-primary custom-opacity";
            //        ViewData["filterApplied:ButtonActive"] = "btn-success custom-opacity";
            //    }
            //}
            if (!String.IsNullOrEmpty(SearchCode))
            {
                ncr = ncr.Where(s => s.NcrQa.Defect.DefectName.ToUpper().Contains(SearchCode.ToUpper()) //(s => s.Item.ItemDefects.FirstOrDefault().Defect.DefectName.ToUpper().Contains(SearchCode.ToUpper()) 
                || s.NcrNumber.ToUpper().Contains(SearchCode.ToUpper()));
                numberFilters++;
            }
            if (!String.IsNullOrEmpty(SearchSupplier))
            {
                ncr = ncr.Where(n => n.NcrQa.Supplier.SupplierName == SearchSupplier);
                numberFilters++;
            }
            if (StartDate == EndDate)
            {
                ncr = ncr.Where(n => n.NcrQa.NcrQacreationDate == StartDate);
                numberFilters++;
            }
            else
            {
                ncr = ncr.Where(n => n.NcrQa.NcrQacreationDate >= StartDate &&
                         n.NcrQa.NcrQacreationDate <= EndDate);
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
                        .OrderBy(p => p.NcrQa.Defect.DefectName);
                    ViewData["filterApplied:Defect"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQa.Defect.DefectName);
                    ViewData["filterApplied:Defect"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Supplier")
            {
                if (sortDirection == "asc")
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrQa.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQa.Supplier.SupplierName);
                    ViewData["filterApplied:Supplier"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Created")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrQa.NcrQacreationDate);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQa.NcrQacreationDate);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Phase")
            {
                if (sortDirection == "asc")
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrPhase); //.OrderBy(p => p.Ncr.NcrStatus);
                    ViewData["filterApplied:Phase"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrPhase); //.OrderByDescending(p => p.Ncr.NcrStatus);
                    ViewData["filterApplied:Phase"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Last Updated")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrLastUpdated);
                    ViewData["filterApplied:Last Updated"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrLastUpdated);
                    ViewData["filterApplied:Last Updated"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else //(sortField == "PO Number")
            {
                if (sortDirection == "asc")
                {
                    ncr = ncr
                        .OrderBy(p => p.NcrQa.NcrQaOrderNumber);
                    ViewData["filterApplied:PONumber"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncr = ncr
                        .OrderByDescending(p => p.NcrQa.NcrQaOrderNumber);
                    ViewData["filterApplied:PONumber"] = "<i class='bi bi-sort-down'></i>";
                }
            }

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;
            ViewData["filter"] = filter;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Ncr>.CreateAsync(ncr.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: Ncr/Details/5
        public async Task<IActionResult> Details(int? id, NcrPhase section)
        {
            if (id == null || _context.Ncrs == null)
            {
                return NotFound();
            }

            var ncr = await _context.Ncrs
                .Include(n => n.NcrQa)
                .Include(n => n.NcrQa).ThenInclude(n => n.Item)
                .Include(n => n.NcrQa).ThenInclude(n => n.Defect)
                .Include(n => n.NcrQa).ThenInclude(n => n.Supplier)
                .Include(n => n.NcrQa).ThenInclude(n => n.ItemDefectPhotos)
                .Include(n => n.NcrEng)
                .Include(n => n.NcrEng).ThenInclude(n => n.EngDispositionType)
                .Include(n => n.NcrEng).ThenInclude(n => n.Drawing)
                .Include(n => n.NcrEng).ThenInclude(n => n.EngDefectPhotos)
                .Include(n => n.NcrOperation)
                .Include(n => n.NcrOperation).ThenInclude(n => n.OpDispositionType)
                .Include(n => n.NcrOperation).ThenInclude(n => n.FollowUpType)
                .Include(n => n.NcrOperation).ThenInclude(n => n.OpDefectPhotos)
                .Include(n => n.NcrProcurement)
                .Include(n => n.NcrProcurement).ThenInclude(n => n.ProcDefectPhotos)
                .Include(n => n.NcrReInspect)
                .Include(n => n.NcrReInspect).ThenInclude(n => n.NcrReInspectPhotos)
                .FirstOrDefaultAsync(m => m.NcrId == id);

            if (ncr == null)
            {
                return NotFound();
            }

            ViewBag.IsNCRQaView = true;
            ViewBag.IsNCREngView = false;
            ViewBag.IsNCROpView = false;
            ViewBag.IsNCRProcView = false;
            ViewBag.IsNCRReInspView = false;

            ViewBag.NCRSectionId = id;

            return View(ncr);
        }
        // GET: Ncr/Create
        public IActionResult Create()
        {
            //string example = "2024-0018";
            //ViewData["NCRNumber"] = example;
            Ncr ncr = new Ncr();
            return View(ncr);
        }

        // POST: Ncr/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NcrId,NcrNumber,NcrLastUpdated,NcrStatus")] Ncr ncr)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ncr);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            return View(ncr);
        }

        // POST: Ncr/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NcrId,NcrNumber,NcrLastUpdated,NcrStatus")] Ncr ncr)
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
                .FirstOrDefaultAsync(m => m.NcrId == id);

            if (ncr != null)
            {

                _context.Ncrs.Remove(ncr);
                await _context.SaveChangesAsync();

                if (ncr.NcrPhase == NcrPhase.Archive)
                {
                    TempData["SuccessMessage"] = "NCR deleted successfully!";
                    return RedirectToAction("Archived");
                }
                else
                {
                    TempData["SuccessMessage"] = "NCR deleted successfully!";
                    return RedirectToAction("Index");
                }

            }
            else
            {
                if (ncr.NcrPhase == NcrPhase.Archive)
                {
                    TempData["ErrorMessage"] = "NCR could not be deleted.";
                    return RedirectToAction("Archived");
                }
                else
                {
                    TempData["ErrorMessage"] = "NCR could not be deleted.";
                    return RedirectToAction("Index");
                }

            }
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
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        //Manually Archiving many Ncrs
        [HttpPost]
        public async Task<IActionResult> ArchiveManyNcrs(int archiveYear, [FromServices] NcrArchivingService ncrArchivingService)
        {
            try
            {

                // Call the ArchiveNcrsByYear method from the injected NcrArchivingService
                var archivedCount = await ncrArchivingService.ArchiveNcrsByYear(archiveYear);

                // Set success message in TempData
                TempData["SuccessMessage"] = $"{archivedCount} NCRs archived successfully!";

                // Redirect back to the previous page or any other desired page
                return RedirectToAction("Archived"); // Change "Archived" to the action name you want to redirect to
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during archiving
                TempData["ErrorMessage"] = $"Error occurred while archiving NCR objects: {ex.Message}";

                // Redirect back to the previous page or any other desired page
                return RedirectToAction("Archived"); // Change "Archived" to the action name you want to redirect to
            }
        }

        [HttpPost]
        public IActionResult AutomaticArchiveYear(int targetYear)
        {
            _targetYearService.TargetYear = targetYear;
            return RedirectToAction("Archived"); // Redirect to a different action
        }

        //[HttpPost]
        //public async Task<IActionResult> ArchiveNcrs(int years)
        //{
        //    // Call the method in NcrArchiveManager to archive NCRs based on the specified number of years
        //    await _ncrArchiveManager.ArchiveNcrsByYear(years);

        //    // Optionally, redirect to a relevant page after archiving
        //    return RedirectToAction("Index", "Home");
        //}

        private SelectList SupplierSelectList(int? selectedId)
        {
            return new SelectList(_context.Suppliers
                .Where(s => s.SupplierStatus == true && s.SupplierName != "NO SUPPLIER PROVIDED")
                .OrderBy(s => s.SupplierName), "SupplierId", "Summary", selectedId);
        }

        private SelectList ItemSelectList()
        {
            return new SelectList(_context.Items
                .OrderBy(s => s.ItemName), "ItemId", "Summary");
        }

        private SelectList DefectSelectList()
        {
            return new SelectList(_context.Defects
                .OrderBy(s => s.DefectName), "DefectId", "DefectName");
        }

        private void PopulateDropDownLists()
        {
            ViewData["SupplierId"] = SupplierSelectList(null);
            ViewData["ItemId"] = ItemSelectList();
            ViewData["DefectId"] = DefectSelectList();
        }


        [HttpGet]
        public JsonResult GetSuppliers(int? id)
        {
            return Json(SupplierSelectList(id));
        }

        public JsonResult GetSuppliersAuto(string term)
        {
            var result = from s in _context.Suppliers
                         where s.SupplierName.ToUpper().Contains(term.ToUpper())
                         //|| d.FirstName.ToUpper().Contains(term.ToUpper())
                         orderby s.SupplierName
                         select new { value = s.SupplierName };
            return Json(result);
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

                TempData["SuccessMessage"] = "NCR Archived successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "NCR not found for Archiving.";
                return RedirectToAction("Index");
            }

        }
        //public async Task<IActionResult> ArchiveDateNcr(int id)
        //{
        //    var ncrToUpdate = await _context.Ncrs
        //            .AsNoTracking()
        //            .FirstOrDefaultAsync(n => n.NcrId == id);

        //    if (ncrToUpdate != null)
        //    {
        //        //Update the phase
        //        ncrToUpdate.NcrPhase = NcrPhase.Archive;

        //        //saving the values
        //        _context.Ncrs.Update(ncrToUpdate);
        //        await _context.SaveChangesAsync();

        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index");
        //    }

        //}
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

                TempData["SuccessMessage"] = "NCR Restored successfully!";
                return RedirectToAction("Archived");
            }
            else
            {
                TempData["ErrorMessage"] = "NCR not found for Archiving.";
                return RedirectToAction("Archived");
            }

        }
        public async Task<IActionResult> VoidNcr(int id, string voidReason)
        {
            var ncrToUpdate = await _context.Ncrs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(n => n.NcrId == id);

            if (ncrToUpdate != null)
            {
                // Update the phase and void reason
                ncrToUpdate.NcrStatus = false;
                ncrToUpdate.NcrPhase = NcrPhase.Void;
                ncrToUpdate.NcrVoidReason = voidReason;

                // Saving the values
                _context.Ncrs.Update(ncrToUpdate);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "NCR Voided successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "NCR not found for Voiding.";
                return RedirectToAction("Index");
            }
        }



        #region DownloadPDF
        public async Task<IActionResult> DownloadPDF(int id)
        {
            var ncr = await _context.Ncrs
                .Include(n => n.NcrQa)
                .Include(n => n.NcrQa).ThenInclude(n => n.Item)
                .Include(n => n.NcrQa).ThenInclude(n => n.Defect)
                .Include(n => n.NcrQa).ThenInclude(n => n.Supplier)
                .Include(n => n.NcrQa).ThenInclude(n => n.ItemDefectPhotos)
                .Include(n => n.NcrEng)
                .Include(n => n.NcrEng).ThenInclude(n => n.EngDispositionType)
                .Include(n => n.NcrEng).ThenInclude(n => n.Drawing)
                .Include(n => n.NcrEng).ThenInclude(n => n.EngDefectPhotos)
                .Include(n => n.NcrOperation)
                .Include(n => n.NcrOperation).ThenInclude(n => n.OpDispositionType)
                .Include(n => n.NcrOperation).ThenInclude(n => n.FollowUpType)
                .Include(n => n.NcrOperation).ThenInclude(n => n.OpDefectPhotos)
                .Include(n => n.NcrProcurement)
                .Include(n => n.NcrProcurement).ThenInclude(n => n.ProcDefectPhotos)
                .Include(n => n.NcrReInspect)
                .Include(n => n.NcrReInspect).ThenInclude(n => n.NcrReInspectPhotos)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NcrId == id);
            try
            {
                // Load NCR excel template 
                //string solutionDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                //string excelPictureFilePath = Path.Combine(solutionDir, "picture-template.xlsx");
                //string solutionDirtwo = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                //string excelPictureFilePathtwo = Path.Combine(solutionDirtwo, "ncr-template.xlsx");

               
                var excelFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads", "ncr-template.xlsx");
                var excelPictureFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads", "picture-template.xlsx");
                Workbook workbook = new Workbook();
                Workbook workbookPicture = new Workbook();
                workbook.LoadTemplateFromFile(excelFilePath);
                workbookPicture.LoadTemplateFromFile(excelPictureFilePath);


                //Fill the data 
                var worksheet = workbook.Worksheets["NCR"];
                bool checkpictures = false;
                int firstpage = 1;
                int morepages = 1;
                worksheet.Range["Z4"].Value = firstpage.ToString();
                worksheet.Range["AC4"].Value = morepages.ToString();



                //Quality Representative
                if (ncr.NcrQa != null)
                {

                    worksheet.Range["AC5"].Value = ncr.NcrNumber;
                    worksheet.Range["AC6"].Value = ncr.NcrQa.NcrQaOrderNumber;
                    worksheet.Range["AC7"].Value = ncr.NcrQa.NcrQaSalesOrder;
                    worksheet.Range["M7"].Value = ncr.NcrQa.Supplier.SupplierName;
                    worksheet.Range["AF8"].Value = ncr.NcrQa.NcrQaQuanReceived.ToString();
                    worksheet.Range["AF9"].Value = ncr.NcrQa.NcrQaQuanDefective.ToString();
                    worksheet.Range["B9"].Value = ncr.NcrQa.Item.ItemName;
                    worksheet.Range["B11"].Value = ncr.NcrQa.NcrQaDescriptionOfDefect;
                    //worksheet.Range["U15"].Value = ncr.NcrQa.CreatedBy;
                    worksheet.Range["AE15"].Value = ncr.NcrQa.CreatedOn.ToString();
                    if (ncr.NcrQa.NcrQaProcessApplicable.Equals(true))
                    {
                        worksheet.Range["C6"].Value = "X";
                    }
                    else
                    {
                        worksheet.Range["C7"].Value = "X";
                    }
                    if (ncr.NcrQa.NcrQaItemMarNonConforming.Equals(true))
                    {
                        worksheet.Range["C14"].Value = "X";
                    }
                    else
                    {
                        worksheet.Range["H14"].Value = "X";
                    }
                    if (ncr.NcrQa.NcrQaEngDispositionRequired.Equals(true))
                    {
                        worksheet.Range["T14"].Value = "X";
                    }
                    else
                    {
                        worksheet.Range["Y14"].Value = "X";
                    }
                    if (ncr.NcrQa.ItemDefectPhotos.Count > 0 || ncr.NcrQa.NcrQaDefectVideo != null)
                    {

                        firstpage += 1;
                        morepages += 1;
                        int qapages = morepages;
                        checkpictures = true;
                        var wb = workbookPicture.Worksheets["Pictures"];


                        //ExcelPackage excel = new ExcelPackage();
                        //var worksheeet = excel.Workbook.Worksheets.Add("Pictures");


                        //var QaPic = _context.ItemDefectPhotos.FirstOrDefault();
                        //byte[] imageBytes = QaPic.ItemDefectPhotoContent;
                        //MemoryStream stream = new MemoryStream(imageBytes);
                        //Image image = Image.FromStream(stream);

                        //var picture = worksheeet.Drawings.AddPicture("Pic", image);
                        //picture.SetPosition(4, 0, 2, 0);



                        //for (int a = 0; a < 5; a++)
                        //{
                        //    wb.Row(a * 4).Height = 39.000;
                        //}

                        //byte[] imageBytes = ncr.NcrQa.ItemDefectPhotos.FirstOrDefault().FileContent.Content.ToArray();

                        //Image logo = Image.FromStream(new MemoryStream(imageBytes));

                        //for (int a = 0; a < 5; a++)
                        //{
                        //    var picture = worksheeet.Drawings.AddPicture(a.ToString(), logo);
                        //    picture.SetPosition(a * 4, 0, 2, 0);
                        //}

                        wb.Range["A6"].Value = "Quality";
                        //wb.Range["B6"].Value = "picture";
                        //wb.Range["R6"].Value = "picture";
                        //wb.Range["B21"].Value = "picture";
                        //wb.Range["R21"].Value = "picture";
                        wb.Range["B37"].Value = ncr.NcrQa.NcrQaDefectVideo;
                        wb.Range["Z4"].Value = firstpage.ToString();
                        //wb.Range["AC4"].Value = qapages.ToString();
                        worksheet.Range["AC4"].Value = morepages.ToString();
                    }
                }

                //Engineering
                if (ncr.NcrEng != null)
                {
                    worksheet.Range["B21"].Value = ncr.NcrEng.NcrEngDispositionDescription;
                    //worksheet.Range["T26"].Value = ncr.NcrEng.CreatedBy;
                    worksheet.Range["AD26"].Value = ncr.NcrEng.CreatedOn.ToString();
                    if (ncr.NcrEng.NcrEngCustomerNotification.Equals(true))
                    {
                        worksheet.Range["Q18"].Value = "X";
                    }
                    else
                    {
                        worksheet.Range["U18"].Value = "X";
                    }
                    if (ncr.NcrEng.DrawingRequireUpdating.Equals(true))
                    {
                        worksheet.Range["O23"].Value = "X";
                        worksheet.Range["J24"].Value = ncr.NcrEng.DrawingOriginalRevNumber.ToString();
                        worksheet.Range["T24"].Value = ncr.NcrEng.DrawingUpdatedRevNumber.ToString();
                        worksheet.Range["T25"].Value = ncr.NcrEng.DrawingRevDate.ToString();
                    }
                    else
                    {
                        worksheet.Range["T23"].Value = "X";
                    }
                    switch (ncr.NcrEng.EngDispositionType.EngDispositionTypeId)
                    {
                        case 1:
                            worksheet.Range["E17"].Value = "X";
                            break;
                        case 2:
                            worksheet.Range["J17"].Value = "X";
                            break;
                        case 3:
                            worksheet.Range["N17"].Value = "X";
                            break;
                        case 4:
                            worksheet.Range["S17"].Value = "X";
                            break;
                    }
                    if (ncr.NcrEng.EngDefectPhotos.Count > 0 || ncr.NcrEng.NcrEngDefectVideo != null)
                    {

                        firstpage += 1;
                        morepages += 1;
                        int engpages = morepages;
                        checkpictures = true;
                        var wbE = workbookPicture.Worksheets["Pictures"];
                        workbookPicture.Worksheets.AddCopy(wbE);
                        wbE.Range["A6"].Value = "Engineering";




                        //wb.Pictures.Add(4, 4, @"E:\HaverNiagara\V6\HaverProjectV2\HaverDevProject\wwwroot\images\1.jpg");

                        //ExcelPicture picture4 = wb.Pictures.Add(12, 14, @"D:\HaverNiagara\V6\HaverProjectV2\HaverDevProject\wwwroot\images\4.jpg");
                        //picture4.Width = 300;
                        //picture4.Height = 300;
                        //picture4.Left = 350;
                        //picture4.Top = 475;
                        wbE.Range["B37"].Value = ncr.NcrEng.NcrEngDefectVideo;
                        wbE.Range["Z4"].Value = firstpage.ToString();
                        //wbE.Range["AC4"].Value = engpages.ToString();
                        worksheet.Range["AC4"].Value = morepages.ToString();
                    }
                }

                //Operations
                if (ncr.NcrOperation != null)
                {
                    worksheet.Range["B29"].Value = ncr.NcrOperation.NcrPurchasingDescription;
                    //worksheet.Range["V33"].Value = ncr.NcrOperation.CreatedBy;
                    worksheet.Range["AD33"].Value = ncr.NcrOperation.CreatedOn.ToString();
                    switch (ncr.NcrOperation.OpDispositionType.OpDispositionTypeId)
                    {
                        case 1:
                            worksheet.Range["B28"].Value = "X";
                            break;
                        case 2:
                            worksheet.Range["K28"].Value = "X";
                            break;
                        case 3:
                            worksheet.Range["027"].Value = "X";
                            break;
                        case 4:
                            worksheet.Range["S28"].Value = "X";
                            break;
                    }
                    if (ncr.NcrOperation.Car.Equals(true))
                    {
                        worksheet.Range["L31"].Value = "X";
                        worksheet.Range["Z31"].Value = ncr.NcrOperation.CarNumber;
                    }
                    else
                    {
                        worksheet.Range["N31"].Value = "X";
                    }
                    if (ncr.NcrOperation.FollowUp.Equals(true))
                    {
                        worksheet.Range["L32"].Value = "X";
                        worksheet.Range["AD32"].Value = ncr.NcrOperation.ExpectedDate.ToString();
                    }
                    else
                    {
                        worksheet.Range["N32"].Value = "X";
                    }
                    if (ncr.NcrOperation.OpDefectPhotos.Count > 0 || ncr.NcrOperation.NcrOperationVideo != null)
                    {
                        firstpage += 1;
                        morepages += 1;
                        int oppages = morepages;
                        checkpictures = true;
                        var wbO = workbookPicture.Worksheets["Pictures"];
                        workbookPicture.Worksheets.AddCopy(wbO);
                        wbO.Range["A6"].Value = "Operations";
                        wbO.Range["B37"].Value = ncr.NcrOperation.NcrOperationVideo;
                        wbO.Range["Z4"].Value = firstpage.ToString();
                        //wbO.Range["AC4"].Value = oppages.ToString();
                        worksheet.Range["AC4"].Value = morepages.ToString();
                    }
                }

                //Procurement
                if (ncr.NcrProcurement != null)
                {
                    //worksheet.Range["T40"].Value = ncr.NcrProcurement.CreatedBy;
                    worksheet.Range["AD40"].Value = ncr.NcrProcurement.CreatedOn.ToString();
                    if (ncr.NcrProcurement.NcrProcSupplierReturnReq.Equals(true))
                    {
                        worksheet.Range["L34"].Value = "X";
                        worksheet.Range["T36"].Value = ncr.NcrProcurement.SupplierReturnAccount;
                        worksheet.Range["T35"].Value = ncr.NcrProcurement.SupplierReturnName;
                        worksheet.Range["H35"].Value = ncr.NcrProcurement.SupplierReturnMANum;
                        worksheet.Range["H36"].Value = ncr.NcrProcurement.NcrProcExpectedDate.ToString();
                    }
                    if (ncr.NcrProcurement.NcrProcSupplierBilled.Equals(true))
                    {
                        worksheet.Range["L37"].Value = "X";
                    }
                    else
                    {
                        worksheet.Range["N37"].Value = "X";
                    }
                    if (ncr.NcrProcurement.NcrProcCreditExpected.Equals(true))
                    {
                        worksheet.Range["L38"].Value = "X";
                        worksheet.Range["AB38"].Value = ncr.NcrProcurement.NcrProcRejectedValue.ToString();

                    }
                    else
                    {
                        worksheet.Range["N38"].Value = "X";
                    }
                    if (ncr.NcrProcurement.NcrProcSAPReturnCompleted.Equals(true))
                    {
                        worksheet.Range["L39"].Value = "X";
                    }
                    else
                    {
                        worksheet.Range["N39"].Value = "X";
                    }
                    if (ncr.NcrProcurement.ProcDefectPhotos.Count > 0 || ncr.NcrProcurement.NcrProcDefectVideo != null)
                    {
                        firstpage += 1;
                        morepages += 1;
                        int procpages = morepages;
                        checkpictures = true;
                        var wbR = workbookPicture.Worksheets["Pictures"];
                        workbookPicture.Worksheets.AddCopy(wbR);
                        wbR.Range["A6"].Value = "Procurement";
                        wbR.Range["B37"].Value = ncr.NcrProcurement.NcrProcDefectVideo;
                        wbR.Range["Z4"].Value = firstpage.ToString();
                        //wbR.Range["AC4"].Value = procpages.ToString();
                        worksheet.Range["AC4"].Value = morepages.ToString();
                    }
                }

                //Reinspection
                if (ncr.NcrReInspect != null)
                {
                    worksheet.Range["F42"].Value = ncr.NcrReInspect.NcrReInspectNotes;
                    //worksheet.Range["V43"].Value = ncr.NcrReInspect.CreatedBy;
                    worksheet.Range["AE43"].Value = ncr.NcrReInspect.CreatedOn.ToString();
                    if (ncr.NcrReInspect.NcrReInspectAcceptable.Equals(true))
                    {
                        worksheet.Range["L41"].Value = "X";


                    }
                    else
                    {
                        worksheet.Range["N41"].Value = "X";
                        worksheet.Range["J43"].Value = ncr.NcrReInspect.NcrReInspectNewNcrNumber;
                    }
                    if (ncr.NcrPhase == NcrPhase.Closed)
                    {
                        worksheet.Range["G44"].Value = "X";
                    }

                    if (ncr.NcrReInspect.NcrReInspectPhotos.Count > 0 || ncr.NcrReInspect.NcrReInspectDefectVideo != null)
                    {
                        firstpage += 1;
                        morepages += 1;
                        int repages = morepages;
                        checkpictures = true;
                        var wbP = workbookPicture.Worksheets["Pictures"];
                        workbookPicture.Worksheets.AddCopy(wbP);
                        wbP.Range["A6"].Value = "Reinspection";
                        //wbP.Range["B6"].Value = "picture";
                        //wbP.Range["R6"].Value = "picture";
                        //wbP.Range["B21"].Value = "picture";
                        //wbP.Range["R21"].Value = "picture";
                        wbP.Range["B37"].Value = ncr.NcrReInspect.NcrReInspectDefectVideo;
                        wbP.Range["Z4"].Value = firstpage.ToString();
                        //wbP.Range["AC4"].Value = repages.ToString();
                        worksheet.Range["AC4"].Value = morepages.ToString();
                    }
                }




                Workbook exportWorkbook = new Workbook();
                foreach (Worksheet sheet in workbook.Worksheets)
                {
                    exportWorkbook.Worksheets.AddCopy(sheet);
                }

                if (checkpictures == true)
                {
                    // Copy worksheets from the second workbook
                    foreach (Worksheet sheet in workbookPicture.Worksheets)
                    {
                        exportWorkbook.Worksheets.AddCopy(sheet);
                    }
                }
                //exportWorkbook.ConverterSetting.SheetFitToPage = true;
                string filename = ncr.NcrNumber;
                string defaultDownloadFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string downloadsFolder = Path.Combine(defaultDownloadFolder, "Downloads");
                string pdfFilePath = Path.Combine(downloadsFolder, $"{filename}.pdf");

                exportWorkbook.SaveToFile(pdfFilePath, FileFormat.PDF);


                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = $"{filename}.pdf",
                    Inline = false, // Set to false to force download
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());

                // Return the file
                return PhysicalFile(pdfFilePath, "application/pdf");

            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating PDF: {ex.Message}");
            }


        }
        #endregion


        private bool NcrExists(int id)
        {
            return _context.Ncrs.Any(e => e.NcrId == id);
        }
    }
}
