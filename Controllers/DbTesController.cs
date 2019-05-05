using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Models;
using Microsoft.AspNetCore.Http;

namespace WebApplication8.Controllers
{
    public class DbTesController : FonourControllerBase
    {
        private readonly NEUContext _context;

        public DbTesController(NEUContext context)
        {
            _context = context;
        }

        // GET: DbTes4545455
        public async Task<IActionResult> Index()
        {
            ViewBag.paperId= HttpContext.Session.GetInt32("paperId");
            int paId = ViewBag.paperId;
            var pa = from d in _context.DbExam where (d.PaId == paId) select d;
            //int p = pa.Count();
            var paMis = from d in pa where (d.RW.Equals("错"))select d;
            //int n = paMis.Count();
            paMis = paMis.OrderBy(d => d.Pax1_ID);
            var nn = paMis;
            
            return View(await nn.ToListAsync());
        }

        // GET: DbTes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var dbTe = await _context.DbTe
                .Include(d => d.Pa)
                .Include(d => d.Pax)
                .SingleOrDefaultAsync(m => m.PaxId == id);
            if (dbTe == null)
            {
                return NotFound();
            }
            if (dbTe.Pax.Qu.Type=="多选")
            {
                return RedirectToRoute(new { Controller = "DbTes", Action = "Edit", id });
            }
            

            return View(dbTe);
        }



        // GET: DbTes/Create
        public async Task<IActionResult> Create(int?id)
        {
          
            var pa = from d in _context.DbExam where (d.PaId == id) select d;
            //int p = pa.Count();
 

            return View(await pa.ToListAsync());
        }

        // GET: DbTes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbTe = await _context.DbTe
                .Include(d => d.Pa)
                .Include(d => d.Pax)
                .SingleOrDefaultAsync(m => m.PaxId == id);
            if (dbTe == null)
            {
                return NotFound();
            }
            return View(dbTe);
        }

        // POST: DbTes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeId,EmAnswer,PaId,PaxId,RW")] DbTe dbTe)
        {
            if (id != dbTe.TeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbTe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DbTeExists(dbTe.TeId))
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
            ViewData["PaId"] = new SelectList(_context.DbPa, "PaId", "PaId", dbTe.PaId);
            ViewData["PaxId"] = new SelectList(_context.DbPax, "PaxId", "PaxId", dbTe.PaxId);
            return View(dbTe);
        }

        // GET: DbTes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbTe = await _context.DbTe
                .Include(d => d.Pa)
                .Include(d => d.Pax)
                .SingleOrDefaultAsync(m => m.TeId == id);
            if (dbTe == null)
            {
                return NotFound();
            }

            return View(dbTe);
        }

        // POST: DbTes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dbTe = await _context.DbTe.SingleOrDefaultAsync(m => m.TeId == id);
            _context.DbTe.Remove(dbTe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DbTeExists(int id)
        {
            return _context.DbTe.Any(e => e.TeId == id);
        }
    }
}
