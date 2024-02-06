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
    public class StatusUpdateController : LookupsController
    {
        private readonly HaverNiagaraContext _context;

        public StatusUpdateController(HaverNiagaraContext context)
        {
            _context = context;
        }

        // GET: StatusUpdate
        public IActionResult Index()
        {
            return Redirect(ViewData["returnURL"].ToString());
        }

        // GET: StatusUpdate/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StatusUpdate/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusUpdateName")] StatusUpdate statusUpdate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(statusUpdate);
                    await _context.SaveChangesAsync();
                    return Redirect(ViewData["returnURL"].ToString());
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(statusUpdate);
        }

        // GET: StatusUpdate/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StatusUpdates == null)
            {
                return NotFound();
            }

            var statusUpdate = await _context.StatusUpdates.FindAsync(id);
            if (statusUpdate == null)
            {
                return NotFound();
            }
            return View(statusUpdate);
        }

        // POST: StatusUpdate/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var statusUpdateToUpdate = await _context.StatusUpdates.FirstOrDefaultAsync(su => su.StatusUpdateId == id);
            if (statusUpdateToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<StatusUpdate>(statusUpdateToUpdate, "",
                    su => su.StatusUpdateName))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return Redirect(ViewData["returnURL"].ToString());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusUpdateExists(statusUpdateToUpdate.StatusUpdateId))
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
            return View(statusUpdateToUpdate);
        }

        // GET: StatusUpdate/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StatusUpdates == null)
            {
                return NotFound();
            }

            var statusUpdate = await _context.StatusUpdates
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.StatusUpdateId == id);
            if (statusUpdate == null)
            {
                return NotFound();
            }

            return View(statusUpdate);
        }

        // POST: StatusUpdate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StatusUpdates == null)
            {
                return Problem("There are no Status to delete.");
            }
            var statusUpdate = await _context.StatusUpdates.FindAsync(id);
            
            try
            {
                if (statusUpdate != null)
                {
                    _context.StatusUpdates.Remove(statusUpdate);
                }
                await _context.SaveChangesAsync();
                return Redirect(ViewData["returnURL"].ToString());
            }            
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError("", "Unable to Delete Status. Remember, you cannot delete a Status that is used in the system.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(statusUpdate);
        }

        private bool StatusUpdateExists(int id)
        {
          return _context.StatusUpdates.Any(e => e.StatusUpdateId == id);
        }
    }
}
