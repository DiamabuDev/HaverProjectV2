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
    public class FollowUpTypeController : LookupsController
    {
        private readonly HaverNiagaraContext _context;

        public FollowUpTypeController(HaverNiagaraContext context)
        {
            _context = context;
        }

        // GET: FollowUpType
        public IActionResult Index()
        {
            return Redirect(ViewData["returnURL"].ToString());
        }

        // GET: FollowUpType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FollowUpType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FollowUpTypeName")] FollowUpType followUpType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(followUpType);
                    await _context.SaveChangesAsync();
                    return Redirect(ViewData["returnURL"].ToString());
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(followUpType);
        }

        // GET: FollowUpType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FollowUpTypes == null)
            {
                return NotFound();
            }

            var followUpType = await _context.FollowUpTypes.FindAsync(id);
            if (followUpType == null)
            {
                return NotFound();
            }
            return View(followUpType);
        }

        // POST: FollowUpType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var followUpTypeToUpdate = await _context.FollowUpTypes.FirstOrDefaultAsync(ft => ft.FollowUpTypeId == id);
            if (followUpTypeToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<FollowUpType>(followUpTypeToUpdate, "",
                    dt => dt.FollowUpTypeName))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return Redirect(ViewData["returnURL"].ToString());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FollowUpTypeExists(followUpTypeToUpdate.FollowUpTypeId))
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
            return View(followUpTypeToUpdate);
        }

        // GET: FollowUpType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FollowUpTypes == null)
            {
                return NotFound();
            }

            var followUpType = await _context.FollowUpTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.FollowUpTypeId == id);
            if (followUpType == null)
            {
                return NotFound();
            }

            return View(followUpType);
        }

        // POST: FollowUpType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FollowUpTypes == null)
            {
                return Problem("There are no Follow Up Types to delete.");
            }
            var followUpType = await _context.FollowUpTypes.FindAsync(id);
            try
            {
                if (followUpType != null)
                {
                    _context.FollowUpTypes.Remove(followUpType);
                }
                await _context.SaveChangesAsync();
                return Redirect(ViewData["returnURL"].ToString());
            }   
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError("", "Unable to Delete Follow Up Type. Remember, you cannot delete a Function Type that is used in the system.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }

            return View(followUpType);
        }

        private bool FollowUpTypeExists(int id)
        {
          return _context.FollowUpTypes.Any(e => e.FollowUpTypeId == id);
        }
    }
}
