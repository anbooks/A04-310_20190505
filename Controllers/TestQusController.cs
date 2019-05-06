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
    public class TestQusController : Controller
    {
        private readonly NEUContext _context;

        public TestQusController(NEUContext context)
        {
            _context = context;
        }

        // GET: TestQus   .SingleOrDefaultAsync(m=>m.TestId==id)
        public async Task<IActionResult> Index(int?id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            // int Teid = id; 
            var test = await _context.Test.SingleOrDefaultAsync(m => m.TestId == id);
            HttpContext.Session.SetInt32("Teid", test.TestId);
           
            //int Teid = ViewBag.Teid;
            var nEUContext = _context.TestQu.Include(t => t.Qu).Include(t => t.Test).Where(m=>m.TestId==id);

            return View(await nEUContext.ToListAsync());
        }

        // GET: TestQus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            if (id == null)
            {
                return NotFound();
            }

            var testQu = await _context.TestQu
                .Include(t => t.Qu)
                .Include(t => t.Test)
                .SingleOrDefaultAsync(m => m.TequId == id);
            if (testQu == null)
            {
                return NotFound();
            }

            return View(testQu);
        }

        // GET: TestQus/Create
        public IActionResult Create()
        {
            ViewData["QuId"] = new SelectList(_context.DbQu, "QuId", "Difficulty");
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestName");
            return View();
        }

        // POST: TestQus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TequId,TestId,QuId,TestaId")] TestQu testQu)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            if (ModelState.IsValid)
            {
                _context.Add(testQu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuId"] = new SelectList(_context.DbQu, "QuId", "Difficulty", testQu.QuId);
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestName", testQu.TestId);
            return View(testQu);
        }

        // GET: TestQus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            if (id == null)
            {
                return NotFound();
            }

            var testQu = await _context.TestQu.SingleOrDefaultAsync(m => m.TequId == id);
            if (testQu == null)
            {
                return NotFound();
            }
            ViewData["QuId"] = new SelectList(_context.DbQu, "QuId", "Difficulty", testQu.QuId);
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestName", testQu.TestId);
            return View(testQu);
        }

        // POST: TestQus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TequId,TestId,QuId,TestaId")] TestQu testQu)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            if (id != testQu.TequId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testQu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestQuExists(testQu.TequId))
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
            ViewData["QuId"] = new SelectList(_context.DbQu, "QuId", "Difficulty", testQu.QuId);
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestName", testQu.TestId);
            return View(testQu);
        }

        // GET: TestQus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            if (id == null)
            {
                return NotFound();
            }

            var testQu = await _context.TestQu
                .Include(t => t.Qu)
                .Include(t => t.Test)
                .SingleOrDefaultAsync(m => m.TequId == id);
            if (testQu == null)
            {
                return NotFound();
            }

            return View(testQu);
        }

        // POST: TestQus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testQu = await _context.TestQu.SingleOrDefaultAsync(m => m.TequId == id);
            _context.TestQu.Remove(testQu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestQuExists(int id)
        {
            return _context.TestQu.Any(e => e.TequId == id);
        }
    }
}
