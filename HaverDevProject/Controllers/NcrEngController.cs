using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaverDevProject.Data;
using HaverDevProject.Models;
using NcrEng = HaverDevProject.Models.NcrEng;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore.Storage;
using HaverDevProject.CustomControllers;

namespace HaverDevProject.Controllers
{
    public class NcrEngController : ElephantController
    {
        private readonly HaverNiagaraContext _context;

        public NcrEngController(HaverNiagaraContext context)
        {
            _context = context;
        }

        // GET: NcrEng
        public async Task<IActionResult> Index()
        {
            var haverNiagaraContext = _context.NcrEngs
                .Include(n => n.EngDispositionType)
                .Include(n => n.Ncr)
                .Include(n => n.Drawing);
            return View(await haverNiagaraContext.ToListAsync());
        }

        // GET: NcrEng/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NcrEngs == null)
            {
                return NotFound();
            }

            var ncrEng = await _context.NcrEngs
                .Include(n => n.EngDispositionType)
                .Include(n => n.Ncr)
                .Include(n =>n.Drawing)
                .FirstOrDefaultAsync(m => m.NcrEngId == id);
            if (ncrEng == null)
            {
                return NotFound();
            }

            return View(ncrEng);
        }

        // GET: NcrEng/Create
        public IActionResult Create()
        {
            NcrEng ncrEng = new NcrEng();
            //PopulateDropDownLists(ncrEng);
            return View(ncrEng);
            //ViewData["EngDispositionTypeId"] = new SelectList(_context.EngDispositionTypes, "EngDispositionTypeId", "EngDispositionTypeName");
            //ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber");
            //return View();
        }

        // POST: NcrEng/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //NcrEngCustomerNotification,NcrEngDispositionDescription,NcrEngUserId,EngDispositionTypeId,DrawingRequireUpdating,DrawingOriginalRevNumber,DrawingUpdatedRevNumber,DrawingRevDate,NcrId
        public async Task<IActionResult> Create([Bind("NcrEngCustomerNotification,NcrEngDispositionDescription,NcrEngUserId,EngDispositionTypeId,DrawingRequireUpdating,DrawingOriginalRevNumber,DrawingUpdatedRevNumber,DrawingRevDate,NcrId")] NcrEng ncrEng)
        {
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(ncrEng);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Details", new { ncrEng.NcrEngId });
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

                return View(ncrEng);
            }

            //if (ModelState.IsValid)
            //{
            //    _context.Add(ncrEng);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["EngDispositionTypeId"] = new SelectList(_context.EngDispositionTypes, "EngDispositionTypeId", "EngDispositionTypeName", ncrEng.EngDispositionTypeId);
            //ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrEng.NcrId);
            //return View(ncrEng);
        }

        // GET: NcrEng/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NcrEngs == null)
            {
                return NotFound();
            }

            var ncrEng = await _context.NcrEngs.FindAsync(id);
            if (ncrEng == null)
            {
                return NotFound();
            }
            ViewData["EngDispositionTypeId"] = new SelectList(_context.EngDispositionTypes, "EngDispositionTypeId", "EngDispositionTypeName", ncrEng.EngDispositionTypeId);
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrEng.NcrId);
            return View(ncrEng);
        }

        // POST: NcrEng/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NcrEngId,NcrEngCustomerNotification,NcrEngDispositionDescription,NcrEngUserId,EngDispositionTypeId,NcrId")] NcrEng ncrEng)
        {
            if (id != ncrEng.NcrEngId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ncrEng);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NcrEngExists(ncrEng.NcrEngId))
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
            ViewData["EngDispositionTypeId"] = new SelectList(_context.EngDispositionTypes, "EngDispositionTypeId", "EngDispositionTypeName", ncrEng.EngDispositionTypeId);
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrEng.NcrId);
            return View(ncrEng);
        }

        // GET: NcrEng/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NcrEngs == null)
            {
                return NotFound();
            }

            var ncrEng = await _context.NcrEngs
                .Include(n => n.EngDispositionType)
                .Include(n => n.Ncr)
                .FirstOrDefaultAsync(m => m.NcrEngId == id);
            if (ncrEng == null)
            {
                return NotFound();
            }

            return View(ncrEng);
        }

        // POST: NcrEng/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NcrEngs == null)
            {
                return Problem("Entity set 'HaverNiagaraContext.NcrEngs'  is null.");
            }
            var ncrEng = await _context.NcrEngs.FindAsync(id);
            if (ncrEng != null)
            {
                _context.NcrEngs.Remove(ncrEng);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private SelectList EngDispositionTypeList(int? selectedId)
        //{
        //    return new SelectList(_context
        //        .NcrEngs
        //        .OrderBy(m => m.EngDispositionTypeId), "ID", "Name", selectedId);
        //}
        //private void PopulateDropDownLists(NcrEng ncrEng = null)
        //{
        //    ViewData["CustomerID"] = EngDispositionTypeList(ncrEng?.EngDispositionTypeId);
        //}

        private bool NcrEngExists(int id)
        {
          return _context.NcrEngs.Any(e => e.NcrEngId == id);
        }
    }
}
