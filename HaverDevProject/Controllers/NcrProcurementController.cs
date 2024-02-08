using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaverDevProject.Data;
using HaverDevProject.Models;

namespace HaverDevProject.Controllers
{
    public class NcrProcurementController : Controller
    {
        private readonly HaverNiagaraContext _context;

        public NcrProcurementController(HaverNiagaraContext context)
        {
            _context = context;
        }

        // GET: NcrProcurement
        public async Task<IActionResult> Index()
        {
            var haverNiagaraContext = _context.NcrProcurements.Include(n => n.Ncr);
            return View(await haverNiagaraContext.ToListAsync());
        }

        // GET: NcrProcurement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NcrProcurements == null)
            {
                return NotFound();
            }

            var ncrProcurement = await _context.NcrProcurements
                .Include(n => n.Ncr)
                .FirstOrDefaultAsync(m => m.NcrProcurementId == id);
            if (ncrProcurement == null)
            {
                return NotFound();
            }

            return View(ncrProcurement);
        }

        // GET: NcrProcurement/Create
        public IActionResult Create()
        {
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber");
            return View();
        }

        // POST: NcrProcurement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NcrProcurementId,NcrProcSupplierReturnReq,NcrProcExpectedDate,NcrProcDisposedAllowed,NcrProcSAPReturnCompleted,NcrProcCreditExpected,NcrProcSupplierBilled,NcrId")] NcrProcurement ncrProcurement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ncrProcurement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrProcurement.NcrId);
            return View(ncrProcurement);
        }

        // GET: NcrProcurement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NcrProcurements == null)
            {
                return NotFound();
            }

            var ncrProcurement = await _context.NcrProcurements.FindAsync(id);
            if (ncrProcurement == null)
            {
                return NotFound();
            }
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrProcurement.NcrId);
            return View(ncrProcurement);
        }

        // POST: NcrProcurement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Byte[] RowVersion)
        {
            var ncrProcToUpdate = await _context.NcrProcurements.FirstOrDefaultAsync(n => n.NcrProcurementId == id);

            //Check that we got the Ncr or exit with a not found error
            if (ncrProcToUpdate == null)
            {
                return NotFound();
            }

            //Put the original RowVersion value in the OriginalValues collection for the entity
            _context.Entry(ncrProcToUpdate).Property("RowVersion").OriginalValue = RowVersion;

            if (await TryUpdateModelAsync<NcrProcurement>(ncrProcToUpdate, "",
                    n => n.NcrProcSupplierReturnReq, n => n.NcrProcExpectedDate, n => n.NcrProcDisposedAllowed,
                    n => n.NcrProcCreditExpected, n => n.NcrProcSupplierBilled, n => n.NcrProcSAPReturnCompleted,
                    n => n.NcrId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return Redirect(ViewData["returnURL"].ToString());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NcrProcurementExists(ncrProcToUpdate.NcrProcurementId))
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
            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrProcToUpdate.NcrId);
            return View(ncrProcToUpdate);
        }

        // GET: NcrProcurement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NcrProcurements == null)
            {
                return NotFound();
            }

            var ncrProcurement = await _context.NcrProcurements
                .Include(n => n.Ncr)
                .FirstOrDefaultAsync(m => m.NcrProcurementId == id);
            if (ncrProcurement == null)
            {
                return NotFound();
            }

            return View(ncrProcurement);
        }

        // POST: NcrProcurement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NcrProcurements == null)
            {
                return Problem("Entity set 'HaverNiagaraContext.NcrProcurements'  is null.");
            }
            var ncrProcurement = await _context.NcrProcurements.FindAsync(id);
            if (ncrProcurement != null)
            {
                _context.NcrProcurements.Remove(ncrProcurement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NcrProcurementExists(int id)
        {
          return _context.NcrProcurements.Any(e => e.NcrProcurementId == id);
        }
    }
}
