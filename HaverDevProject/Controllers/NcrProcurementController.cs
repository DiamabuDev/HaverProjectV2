using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaverDevProject.Data;
using HaverDevProject.Models;
using HaverDevProject.ViewModels;
using HaverDevProject.CustomControllers;

namespace HaverDevProject.Controllers
{
    public class NcrProcurementController : ElephantController
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

        //// GET: NcrProcurement/Create
        //public IActionResult Create()
        //{
        //    ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber");
        //    return View();
        //}

        //// POST: NcrProcurement/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("NcrProcurementId,NcrProcSupplierReturnReq,NcrProcExpectedDate,NcrProcDisposedAllowed,NcrProcSAPReturnCompleted,NcrProcCreditExpected,NcrProcSupplierBilled,NcrId")] NcrProcurement ncrProcurement)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(ncrProcurement);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrProcurement.NcrId);
        //    return View(ncrProcurement);
        //}

        // GET: NcrProcurement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ncrProc = await _context.NcrProcurements
                .Include(n => n.Ncr)
                .FirstOrDefaultAsync(n => n.NcrId == id);

            if (ncrProc == null)
            {
                return NotFound();
            }

            var ncrProcDTO = new NcrProcurementDTO
            {
                NcrNumber = ncrProc.Ncr.NcrNumber,
                NcrStatus = ncrProc.Ncr.NcrStatus,
                NcrProcurementId = ncrProc.NcrProcurementId,
                NcrProcSupplierReturnReq = ncrProc.NcrProcSupplierReturnReq,
                NcrProcExpectedDate = ncrProc.NcrProcExpectedDate,
                NcrProcDisposedAllowed = ncrProc.NcrProcDisposedAllowed,
                NcrProcSAPReturnCompleted = ncrProc.NcrProcSAPReturnCompleted,
                NcrProcCreditExpected = ncrProc.NcrProcCreditExpected,
                NcrProcSupplierBilled = ncrProc.NcrProcSupplierBilled,
                NcrProcFlagStatus = ncrProc.NcrProcFlagStatus,
                NcrProcUserId = ncrProc.NcrProcUserId,
                NcrId = ncrProc.NcrId,
                Ncr = ncrProc.Ncr,
            };

            ViewData["NcrId"] = new SelectList(_context.Ncrs, "NcrId", "NcrNumber", ncrProc.NcrId);
            return View(ncrProcDTO);
        }

        // POST: NcrProcurement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NcrProcurementDTO ncrProcDTO)
        {
            ncrProcDTO.NcrId = id;

            if(id != ncrProcDTO.NcrId)
            {
                return NotFound();
            }

            //var ncrProcToUpdate = await _context.NcrProcurements.FirstOrDefaultAsync(n => n.NcrProcurementId == id);

            if (ModelState.IsValid)
            {
                try
                {
                    var ncrProc = await _context.NcrProcurements
                        .FirstOrDefaultAsync(n => n.NcrId == id);

                    ncrProc.NcrProcSupplierReturnReq = ncrProcDTO.NcrProcSupplierReturnReq;
                    ncrProc.NcrProcExpectedDate = ncrProcDTO.NcrProcExpectedDate;
                    ncrProc.NcrProcDisposedAllowed = ncrProcDTO.NcrProcDisposedAllowed;
                    ncrProc.NcrProcSAPReturnCompleted = ncrProcDTO.NcrProcSAPReturnCompleted;
                    ncrProc.NcrProcCreditExpected = ncrProcDTO.NcrProcCreditExpected;
                    ncrProc.NcrProcSupplierBilled = ncrProcDTO.NcrProcSupplierBilled;
                    ncrProc.NcrProcFlagStatus = ncrProcDTO.NcrProcFlagStatus;
                    ncrProc.NcrProcUserId = ncrProcDTO.NcrProcUserId;
                    ncrProc.NcrId = ncrProcDTO.NcrId;

                    _context.Update(ncrProc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NcrProcurementExists(ncrProcDTO.NcrProcurementId))
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
            return View(ncrProcDTO);
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
