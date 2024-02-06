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

namespace HaverDevProject.Controllers
{
    public class ProcessApplicableController : LookupsController
    {
        private readonly HaverNiagaraContext _context;

        public ProcessApplicableController(HaverNiagaraContext context)
        {
            _context = context;
        }

        // GET: ProcessApplicable
        public IActionResult Index()
        {
            return Redirect(ViewData["returnURL"].ToString());
        }

        // GET: ProcessApplicable/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProcessApplicable/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProAppName")] ProcessApplicable processApplicable)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(processApplicable);
                    await _context.SaveChangesAsync();
                    return Redirect(ViewData["returnURL"].ToString());
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(processApplicable);
        }

        // GET: ProcessApplicable/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProcessApplicables == null)
            {
                return NotFound();
            }

            var processApplicable = await _context.ProcessApplicables.FindAsync(id);
            if (processApplicable == null)
            {
                return NotFound();
            }
            return View(processApplicable);
        }

        // POST: ProcessApplicable/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var processApplicableToUpdate = await _context.ProcessApplicables.FirstOrDefaultAsync(pa => pa.ProAppId == id);

            if (processApplicableToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<ProcessApplicable>(processApplicableToUpdate, "",
                 pa => pa.ProAppName))
            {
                try
                {                    
                    await _context.SaveChangesAsync();
                    return Redirect(ViewData["returnURL"].ToString());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessApplicableExists(processApplicableToUpdate.ProAppId))
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
            return View(processApplicableToUpdate);
        }

        // GET: ProcessApplicable/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProcessApplicables == null)
            {
                return NotFound();
            }

            var processApplicable = await _context.ProcessApplicables
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ProAppId == id);
            if (processApplicable == null)
            {
                return NotFound();
            }

            return View(processApplicable);
        }

        // POST: ProcessApplicable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProcessApplicables == null)
            {
                return Problem("There are no Process Applicables to delete.");
            }
            var processApplicable = await _context.ProcessApplicables.FindAsync(id);
            
            try
            {
                if (processApplicable != null)
                {
                    _context.ProcessApplicables.Remove(processApplicable);
                }

                await _context.SaveChangesAsync();
                return Redirect(ViewData["returnURL"].ToString());
            }

            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError("", "Unable to Delete Process Applicable. Remember, you cannot delete a Process Applicable that is used in the system.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }

            return View(processApplicable);
        }

        private bool ProcessApplicableExists(int id)
        {
          return _context.ProcessApplicables.Any(e => e.ProAppId == id);
        }
    }
}
