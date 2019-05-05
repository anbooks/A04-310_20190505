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
    public class DbBmsController : Controller
    {
        private readonly NEUContext _context;

        public DbBmsController(NEUContext context)
        {
            _context = context;
        }

        // GET: DbBms
        public async Task<IActionResult> Index()
        {
            return View(await _context.DbBm.ToListAsync());
        }

        // GET: DbBms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbBm = await _context.DbBm
                .SingleOrDefaultAsync(m => m.Id == id);
            if (dbBm == null)
            {
                return NotFound();
            }

            return View(dbBm);
        }

        // GET: DbBms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DbBms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Branch")] DbBm dbBm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dbBm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dbBm);
        }

        // GET: DbBms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbBm = await _context.DbBm.SingleOrDefaultAsync(m => m.Id == id);
            if (dbBm == null)
            {
                return NotFound();
            }
            return View(dbBm);
        }

        // POST: DbBms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Branch")] DbBm dbBm)
        {
            if (id != dbBm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbBm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DbBmExists(dbBm.Id))
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
            return View(dbBm);
        }

        // GET: DbBms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbBm = await _context.DbBm
                .SingleOrDefaultAsync(m => m.Id == id);
            if (dbBm == null)
            {
                return NotFound();
            }

            return View(dbBm);
        }

        // POST: DbBms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dbBm = await _context.DbBm.SingleOrDefaultAsync(m => m.Id == id);
            _context.DbBm.Remove(dbBm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DbBmExists(int id)
        {
            return _context.DbBm.Any(e => e.Id == id);
        }
    }
}
