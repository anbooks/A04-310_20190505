using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class DbBusController : FonourControllerBase
    {
        private readonly NEUContext _context;

        public DbBusController(NEUContext context)
        {
            _context = context;
        }

        // GET: DbBus
        public async Task<IActionResult> Index()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/DbEms/Create");
            }
            return View(await _context.DbBu.ToListAsync());
        }

        // GET: DbBus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/DbEms/Create");
            }
            if (id == null)
            {
                return NotFound();
            }

            var dbBu = await _context.DbBu
                .SingleOrDefaultAsync(m => m.BuId == id);
            if (dbBu == null)
            {
                return NotFound();
            }

            return View(dbBu);
        }

        // GET: DbBus/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(1))
            {
                //return Redirect("~/DbEms/Create");
            }
            return View();
        }

        // POST: DbBus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuId,BuName")] DbBu dbBu)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/DbEms/Create");
            }
            if (ModelState.IsValid)
            {
                _context.Add(dbBu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dbBu);
        }

        // GET: DbBus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/DbEms/Create");
            }
            if (id == null)
            {
                return NotFound();
            }

            var dbBu = await _context.DbBu.SingleOrDefaultAsync(m => m.BuId == id);
            if (dbBu == null)
            {
                return NotFound();
            }
            return View(dbBu);
        }

        // POST: DbBus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BuId,BuName")] DbBu dbBu)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/DbEms/Create");
            }
            if (id != dbBu.BuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbBu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DbBuExists(dbBu.BuId))
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
            return View(dbBu);
        }

        // GET: DbBus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/DbEms/Create");
            }
            if (id == null)
            {
                return NotFound();
            }

            var dbBu = await _context.DbBu
                .SingleOrDefaultAsync(m => m.BuId == id);
            if (dbBu == null)
            {
                return NotFound();
            }

            return View(dbBu);
        }

        // POST: DbBus/Delete/5
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
                return Redirect("~/DbEms/Create");
            }
            var dbBu = await _context.DbBu.SingleOrDefaultAsync(m => m.BuId == id);
            _context.DbBu.Remove(dbBu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DbBuExists(int id)
        {
            return _context.DbBu.Any(e => e.BuId == id);
        }
    }
}
