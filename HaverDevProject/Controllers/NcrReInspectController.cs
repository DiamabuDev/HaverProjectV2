using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaverDevProject.Data;
using HaverDevProject.Models;
using Microsoft.EntityFrameworkCore.Storage;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HaverDevProject.CustomControllers;
using HaverDevProject.Utilities;
using HaverDevProject.ViewModels;

namespace HaverDevProject.Controllers
{
    public class NcrReInspectController : ElephantController
    {
        private readonly HaverNiagaraContext _context;

        public NcrReInspectController(HaverNiagaraContext context)
        {
            _context = context;
        }

        // GET: NcrReInspect
        public async Task<IActionResult> Index(string SearchCode, int? page, int? pageSizeID, string actionButton,
            DateTime StartDate, DateTime EndDate, string sortDirection = "desc", string sortField = "Created", string filter = "Active")
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

            string[] sortOptions = new[] { "Created", "Acceptable", "Inspected By", "NCR #" };

            var ncrReInspect = _context.NcrReInspects
                .Include(n => n.Ncr)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa)
                .AsNoTracking();

            //Filterig values            
            if (!System.String.IsNullOrEmpty(filter))
            {
                if (filter == "Active")
                {
                    ncrReInspect = ncrReInspect.Where(n => n.Ncr.NcrStatus == true);
                }
                else //(filter == "Closed")
                {

                    ncrReInspect = ncrReInspect.Where(n => n.Ncr.NcrStatus == false);
                }
            }

            if (!System.String.IsNullOrEmpty(SearchCode))
            {
                ncrReInspect = ncrReInspect.Where(s => s.Ncr.NcrNumber.ToUpper().Contains(SearchCode.ToUpper()));
            }
            if (StartDate == EndDate)
            {
                ncrReInspect = ncrReInspect.Where(n => n.Ncr.NcrQa.NcrQacreationDate == StartDate);
            }
            else
            {
                ncrReInspect = ncrReInspect.Where(n => n.Ncr.NcrQa.NcrQacreationDate >= StartDate &&
                         n.Ncr.NcrQa.NcrQacreationDate <= EndDate);
            }

            //Sorting columns
            if (!System.String.IsNullOrEmpty(actionButton)) //Form Submitted!
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

            if (sortField == "Created")
            {
                if (sortDirection == "desc") //desc by default
                {
                    ncrReInspect = ncrReInspect
                        .OrderBy(p => p.Ncr.NcrQa.NcrQacreationDate);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrReInspect = ncrReInspect
                        .OrderByDescending(p => p.Ncr.NcrQa.NcrQacreationDate);

                    ViewData["filterApplied:Created"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Acceptable")
            {
                if (sortDirection == "asc")
                {
                    ncrReInspect = ncrReInspect
                        .OrderBy(p => p.NcrReInspectAcceptable);
                    ViewData["filterApplied:NcrReInspectAcceptable"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrReInspect = ncrReInspect
                        .OrderByDescending(p => p.NcrReInspectAcceptable);
                    ViewData["filterApplied:NcrReInspectAcceptable"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else if (sortField == "Inspected By")
            {
                if (sortDirection == "asc")
                {
                    ncrReInspect = ncrReInspect
                        .OrderBy(p => p.NcrReInspectUserId);
                    ViewData["filterApplied:NcrReInspectUserId"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrReInspect = ncrReInspect
                        .OrderByDescending(p => p.NcrReInspectUserId);
                    ViewData["filterApplied:NcrReInspectUserId"] = "<i class='bi bi-sort-down'></i>";
                }
            }
            else
            {
                if (sortDirection == "asc")
                {
                    ncrReInspect = ncrReInspect
                        .OrderBy(p => p.Ncr.NcrNumber);
                    ViewData["filterApplied:Ncr"] = "<i class='bi bi-sort-up'></i>";
                }
                else
                {
                    ncrReInspect = ncrReInspect
                        .OrderByDescending(p => p.Ncr.NcrNumber);
                    ViewData["filterApplied:Ncr"] = "<i class='bi bi-sort-down'></i>";
                }
            }

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<NcrReInspect>.CreateAsync(ncrReInspect.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: NcrReInspect/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NcrReInspects == null)
            {
                return NotFound();
            }

            var ncrReInspect = await _context.NcrReInspects
                .Include(n => n.Ncr)
                .Include(n => n.Ncr).ThenInclude(n => n.NcrQa)
                .FirstOrDefaultAsync(m => m.NcrReInspectId == id);
            if (ncrReInspect == null)
            {
                return NotFound();
            }

            return View(ncrReInspect);
        }

        // GET: NcrReInspect/Create
        public IActionResult Create()
        {
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber");
            return View();
        }

        // POST: NcrReInspect/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NcrReInspectId,NcrReInspectAcceptable,NcrReInspectNewNcrNumber,NcrReInspectUserId,NcrId")] NcrReInspect ncrReInspect)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(ncrReInspect);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
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

            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrReInspect.NcrId);
            return View(ncrReInspect);
        } //,RowVersion

        // GET: NcrReInspect/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NcrReInspects == null)
            {
                return NotFound();
            }

            var ncrReInspect = await _context.NcrReInspects.FindAsync(id);
            if (ncrReInspect == null)
            {
                return NotFound();
            }
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrReInspect.NcrId);
            return View(ncrReInspect);
        }

        // POST: NcrReInspect/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, List<IFormFile> Photos)
        {
            var ncrReInspectToUpdate = await _context.NcrReInspects
                .Include(r => r.Ncr)
                .FirstOrDefaultAsync(r => r.NcrReInspectId == id);

            if (ncrReInspectToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<NcrReInspect>(ncrReInspectToUpdate, "",
                r => r.NcrReInspectAcceptable, r => r.NcrReInspectNewNcrNumber, r => r.NcrReInspectUserId, r => r.NcrId))
            {
                try
                {
                    await AddReInspectPictures(ncrReInspectToUpdate, Photos);

                    await _context.SaveChangesAsync();

                    var ncrToUpdate = await _context.Ncrs.FindAsync(ncrReInspectToUpdate.NcrId);
                    if (ncrToUpdate != null)
                    {
                        ncrToUpdate.NcrStatus = false;
                        _context.Entry(ncrToUpdate).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NcrReInspectExists(ncrReInspectToUpdate.NcrReInspectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            else
            {
                //Debugging Approach: Not for production code.
                //This code will list validation errors at the top of the View.
                //Use it to diagnose when there seems to be a Validation Error
                //that is going unreported.  Remove this code when you are
                //finished debugging.
                var booBoos = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors });

                foreach (var booBoo in booBoos)
                {
                    string key = booBoo.Key;
                    foreach (var error in booBoo.Errors)
                    {
                        var errorMessage = error?.ErrorMessage;
                        ModelState.AddModelError("", "For " + key + ": " + errorMessage);
                    }
                }
            }
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrReInspectToUpdate.NcrId);
            return View(ncrReInspectToUpdate);
        }

        // GET: NcrReInspect/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NcrReInspects == null)
            {
                return NotFound();
            }

            var ncrReInspect = await _context.NcrReInspects
                .Include(n => n.Ncr)
                .FirstOrDefaultAsync(m => m.NcrReInspectId == id);
            if (ncrReInspect == null)
            {
                return NotFound();
            }

            return View(ncrReInspect);
        }

        // POST: NcrReInspect/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NcrReInspects == null)
            {
                return Problem("There are no Re-Inspections to delete.");
            }
            var ncrReInspect = await _context.NcrReInspects.FindAsync(id);

            try
            {
                if (ncrReInspect != null)
                {
                    _context.NcrReInspects.Remove(ncrReInspect);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(ncrReInspect);
        }

        private async Task AddReInspectPictures(NcrReInspect ncrReInspect, List<IFormFile> pictures)
        {
            if (pictures != null && pictures.Any())
            {
                ncrReInspect.NcrReInspectPhotos = new List<NcrReInspectPhoto>();

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

                            ncrReInspect.NcrReInspectPhotos.Add(new NcrReInspectPhoto
                            {
                                NcrReInspectPhotoContent = ResizeImage.shrinkImageWebp(pictureArray, 500, 600),
                                NcrReInspectPhotoMimeType = "image/webp"
                            });
                        }
                    }
                }
            }
        }

        private bool NcrReInspectExists(int id)
        {
            return _context.NcrReInspects.Any(e => e.NcrReInspectId == id);
        }
    }
}
