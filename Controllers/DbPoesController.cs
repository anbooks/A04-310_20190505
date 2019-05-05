using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication8.Controllers
{
    public class DbPoesController : FonourControllerBase
    {
        private readonly NEUContext _context;

        public DbPoesController(NEUContext context)
        {
            _context = context;
        }

        // GET: DbPoes
        public async Task<IActionResult> Index()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
            return View(await _context.DbPo.ToListAsync());
        }

        // GET: DbPoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
            if (id == null)
            {
                return NotFound();
            }

            var dbPo = await _context.DbPo
                .SingleOrDefaultAsync(m => m.PoId == id);
            if (dbPo == null)
            {
                return NotFound();
            }

            return View(dbPo);
        }

        // GET: DbPoes/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
            return View();
        }

        // POST: DbPoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PoId,PoName")] DbPo dbPo)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
            if (ModelState.IsValid)
            {
                _context.Add(dbPo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dbPo);
        }

        // GET: DbPoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
                if (id == null)
            {
                return NotFound();
            }

            var dbPo = await _context.DbPo.SingleOrDefaultAsync(m => m.PoId == id);
            if (dbPo == null)
            {
                return NotFound();
            }
            return View(dbPo);
        }

        // POST: DbPoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PoId,PoName")] DbPo dbPo)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
            if (id != dbPo.PoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbPo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DbPoExists(dbPo.PoId))
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
            return View(dbPo);
        }

        // GET: DbPoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
            if (id == null)
            {
                return NotFound();
            }

            var dbPo = await _context.DbPo
                .SingleOrDefaultAsync(m => m.PoId == id);
            if (dbPo == null)
            {
                return NotFound();
            }

            return View(dbPo);
        }

        // POST: DbPoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
            var dbPo = await _context.DbPo.SingleOrDefaultAsync(m => m.PoId == id);
            _context.DbPo.Remove(dbPo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DbPoExists(int id)
        {
            return _context.DbPo.Any(e => e.PoId == id);
        }
    }
}
