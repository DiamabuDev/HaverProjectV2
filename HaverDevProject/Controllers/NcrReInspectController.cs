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

namespace HaverDevProject.Controllers
{
    public class NcrReInspectController : Controller
    {
        private readonly HaverNiagaraContext _context;

        public NcrReInspectController(HaverNiagaraContext context)
        {
            _context = context;
        }

        // GET: NcrReInspect
        public async Task<IActionResult> Index()
        {
            var haverNiagaraContext = _context.NcrReInspects.Include(n => n.Ncr);
            return View(await haverNiagaraContext.ToListAsync());
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
        public async Task<IActionResult> Edit(int id)
        {
            var ncrReInspectToUpdate = await _context.NcrReInspects.FirstOrDefaultAsync(r => r.NcrReInspectId == id);

            if (ncrReInspectToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<NcrReInspect>(ncrReInspectToUpdate, "",
                r => r.NcrReInspectAcceptable, r => r.NcrReInspectNewNcrNumber, r => r.NcrReInspectUserId, r => r.NcrId))
            {
                try
                {
                    await _context.SaveChangesAsync();
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

        private bool NcrReInspectExists(int id)
        {
            return _context.NcrReInspects.Any(e => e.NcrReInspectId == id);
        }
    }
}
