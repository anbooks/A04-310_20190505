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
    public class TestAnsController : Controller
    {
        private readonly NEUContext _context;

        public TestAnsController(NEUContext context)
        {
            _context = context;
        }

        // GET: TestAns
        public async Task<IActionResult> Index(int?id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(1))
            {
                //return Redirect("~/Login/Create");
            }
            var nEUContext = _context.TestAn.Include(t => t.Em).Include(t => t.Qu).Include(t => t.Test).Where(t=>t.RW=="错"&&t.TestId==id&&t.EmId==Eid);
            return View(await nEUContext.ToListAsync());
        }

        // GET: TestAns/Details/5
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

            var testAn = await _context.TestAn
                .Include(t => t.Em)
                .Include(t => t.Qu)
                .Include(t => t.Test)
                .SingleOrDefaultAsync(m => m.TeAnId == id);
            if (testAn == null)
            {
                return NotFound();
            }

            return View(testAn);
        }

        // GET: TestAns/Create
        public IActionResult Create()
        {

            ViewData["EmId"] = new SelectList(_context.DbEm, "EmId", "Password");
            ViewData["QuId"] = new SelectList(_context.DbQu, "QuId", "Difficulty");
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestName");
            return View();
        }

        // POST: TestAns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeAnId,QuId,TestId,EmId,TeAn,TestaId")] TestAn testAn)
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
                _context.Add(testAn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmId"] = new SelectList(_context.DbEm, "EmId", "Password", testAn.EmId);
            ViewData["QuId"] = new SelectList(_context.DbQu, "QuId", "Difficulty", testAn.QuId);
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestName", testAn.TestId);
            return View(testAn);
        }

        // GET: TestAns/Edit/5
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

            var testAn = await _context.TestAn.SingleOrDefaultAsync(m => m.TeAnId == id);
            if (testAn == null)
            {
                return NotFound();
            }
            ViewData["EmId"] = new SelectList(_context.DbEm, "EmId", "Password", testAn.EmId);
            ViewData["QuId"] = new SelectList(_context.DbQu, "QuId", "Difficulty", testAn.QuId);
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestName", testAn.TestId);
            return View(testAn);
        }

        // POST: TestAns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeAnId,QuId,TestId,EmId,TeAn,TestaId")] TestAn testAn)
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
            if (id != testAn.TeAnId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testAn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestAnExists(testAn.TeAnId))
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
            ViewData["EmId"] = new SelectList(_context.DbEm, "EmId", "Password", testAn.EmId);
            ViewData["QuId"] = new SelectList(_context.DbQu, "QuId", "Difficulty", testAn.QuId);
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestName", testAn.TestId);
            return View(testAn);
        }

        // GET: TestAns/Delete/5
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

            var testAn = await _context.TestAn
                .Include(t => t.Em)
                .Include(t => t.Qu)
                .Include(t => t.Test)
                .SingleOrDefaultAsync(m => m.TeAnId == id);
            if (testAn == null)
            {
                return NotFound();
            }

            return View(testAn);
        }

        // POST: TestAns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testAn = await _context.TestAn.SingleOrDefaultAsync(m => m.TeAnId == id);
            _context.TestAn.Remove(testAn);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestAnExists(int id)
        {
            return _context.TestAn.Any(e => e.TeAnId == id);
        }
    }
}
