using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class DbPaxes1Controller : FonourControllerBase
    {
        private readonly NEUContext _context;

        public DbPaxes1Controller(NEUContext context)
        {
            _context = context;
        }

        // GET: DbPaxes
        public async Task<IActionResult> Index()
        {
            var nEUContext = _context.DbPax.Include(d => d.Pa).Include(d => d.Qu);
            return View(await nEUContext.ToListAsync());
        }

        // GET: DbPaxes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbPax = await _context.DbPax
                .Include(d => d.Pa)
                .Include(d => d.Qu)
                .SingleOrDefaultAsync(m => m.PaxId == id);
            if (dbPax == null)
            {
                return NotFound();
            }

            return View(dbPax);
        }

        // GET: DbPaxes/Create
        public IActionResult Create()
        {
            ViewData["PaId"] = new SelectList(_context.DbPa, "PaId", "PaId");
            ViewData["QuId"] = new SelectList(_context.DbQu, "QuId", "QuId");
            return View();
        }

        // POST: DbPaxes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaxId,QuId,PaId,Pax1_ID")] DbPax dbPax)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dbPax);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PaId"] = new SelectList(_context.DbPa, "PaId", "PaId", dbPax.PaId);
            ViewData["QuId"] = new SelectList(_context.DbQu, "QuId", "QuId", dbPax.QuId);
            return View(dbPax);
        }

        // GET: DbPaxes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbPax = await _context.DbPax.SingleOrDefaultAsync(m => m.PaxId == id);
            if (dbPax == null)
            {
                return NotFound();
            }
            ViewData["PaId"] = new SelectList(_context.DbPa, "PaId", "PaId", dbPax.PaId);
            ViewData["QuId"] = new SelectList(_context.DbQu, "QuId", "QuId", dbPax.QuId);
            return View(dbPax);
        }

        // POST: DbPaxes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaxId,QuId,PaId,Pax1_ID")] DbPax dbPax)
        {
            if (id != dbPax.PaxId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbPax);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DbPaxExists(dbPax.PaxId))
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
            ViewData["PaId"] = new SelectList(_context.DbPa, "PaId", "PaId", dbPax.PaId);
            ViewData["QuId"] = new SelectList(_context.DbQu, "QuId", "QuId", dbPax.QuId);
            return View(dbPax);
        }

        // GET: DbPaxes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbPax = await _context.DbPax
                .Include(d => d.Pa)
                .Include(d => d.Qu)
                .SingleOrDefaultAsync(m => m.PaxId == id);
            if (dbPax == null)
            {
                return NotFound();
            }

            return View(dbPax);
        }

        // POST: DbPaxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dbPax = await _context.DbPax.SingleOrDefaultAsync(m => m.PaxId == id);
            _context.DbPax.Remove(dbPax);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DbPaxExists(int id)
        {
            return _context.DbPax.Any(e => e.PaxId == id);
        }
    }
}
