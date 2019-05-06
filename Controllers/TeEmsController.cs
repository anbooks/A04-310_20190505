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
    public class TeEmsController : Controller
    {
        private readonly NEUContext _context;

        public TeEmsController(NEUContext context)
        {
            _context = context;
        }

        // GET: TeEms
        public async Task<IActionResult> Index(int?id)
        {
            ViewBag.Teid = id;
            var test = await _context.Test.SingleOrDefaultAsync(m => m.TestId == id);
            HttpContext.Session.SetInt32("Testid", test.TestId);
          
            var nEUContext = _context.TeEm.Include(m => m.Em).Where(m=>m.Teid==id);
            return View(await nEUContext.ToListAsync());
        }

        // GET: TeEms/Details/5
        public async Task<IActionResult> Details()
        {
            ViewBag.Testid = HttpContext.Session.GetInt32("Testid");

            var nEUContext = _context.DbEm;
            return View(await nEUContext.ToListAsync());
        }
        public async Task<IActionResult> Yonghu(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Testid = HttpContext.Session.GetInt32("Testid");
            int TestId = ViewBag.Testid;
            TeEm te = new TeEm();
            te.TeEmId = id;
            te.Teid = TestId;
            te.TeFlag = 0;
            _context.Add(te);


            await _context.SaveChangesAsync();
            return Redirect("~/TeEms/Details");
        }
        // GET: TeEms/Create
        public IActionResult Create()
        {
            ViewData["TeEmId"] = new SelectList(_context.DbEm, "EmId", "Password");
            ViewData["Teid"] = new SelectList(_context.Test, "TestId", "TestName");
            return View();
        }

        // POST: TeEms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TeEmId,Teid,TeFlag")] TeEm teEm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teEm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeEmId"] = new SelectList(_context.DbEm, "EmId", "Password", teEm.TeEmId);
            ViewData["Teid"] = new SelectList(_context.Test, "TestId", "TestName", teEm.Teid);
            return View(teEm);
        }

        // GET: TeEms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teEm = await _context.TeEm.FindAsync(id);
            if (teEm == null)
            {
                return NotFound();
            }
            ViewData["TeEmId"] = new SelectList(_context.DbEm, "EmId", "Password", teEm.TeEmId);
            ViewData["Teid"] = new SelectList(_context.Test, "TestId", "TestName", teEm.Teid);
            return View(teEm);
        }

        // POST: TeEms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeEmId,Teid,TeFlag")] TeEm teEm)
        {
            if (id != teEm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teEm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeEmExists(teEm.Id))
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
            ViewData["TeEmId"] = new SelectList(_context.DbEm, "EmId", "Password", teEm.TeEmId);
            ViewData["Teid"] = new SelectList(_context.Test, "TestId", "TestName", teEm.Teid);
            return View(teEm);
        }

        // GET: TeEms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teEm = await _context.TeEm
                .Include(t => t.Em)
                .Include(t => t.Te)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teEm == null)
            {
                return NotFound();
            }

            return View(teEm);
        }

        // POST: TeEms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teEm = await _context.TeEm.FindAsync(id);
            _context.TeEm.Remove(teEm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeEmExists(int id)
        {
            return _context.TeEm.Any(e => e.Id == id);
        }
    }
}
