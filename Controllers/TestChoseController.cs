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
    public class TestChoseController : Controller
    {
        private readonly NEUContext _context;

        public TestChoseController(NEUContext context)
        {
            _context = context;
        }

        // GET: TestChose
        public async Task<IActionResult> Index()
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
            ViewBag.Teid = HttpContext.Session.GetInt32("Teid");
            int Teid = ViewBag.Teid;
       
          
            
            var nEUContext = _context.DbQu.Include(d => d.Bu).Include(d => d.Em).Include(d => d.Sua).Include(d => d.Sub).Include(d => d.Suc).Include(d => d.Sud).Include(d => d.Sue);
            return View(await nEUContext.ToListAsync());
        }

        // GET: TestChose/Details/5
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
            ViewBag.Teid = HttpContext.Session.GetInt32("Teid");
            int Teid = ViewBag.Teid;
            var testa = await _context.Test.SingleOrDefaultAsync(d => d.TestId == Teid);
            var testb = await _context.DbQu.SingleOrDefaultAsync(d => d.QuId == id);
            var test1 = from d in _context.TestQu where (d.TestId == Teid&&d.Qu.Type==("单选")) select d;
            var test2 = from d in _context.TestQu where (d.TestId == Teid && d.Qu.Type == ("多选")) select d;
            var test3 = from d in _context.TestQu where (d.TestId == Teid && d.Qu.Type == ("判断")) select d;
            int counta = test1.Count();
            int countb = test2.Count();
            int countc = test3.Count();
            if (testb.Type == "单选")
            {
                if (counta >= testa.Dan)
                {
                    return RedirectToAction("Index", "TestQus",Teid);
                }
                TestQu testQu = new TestQu();
                testQu.QuId = Convert.ToInt32(id);
                testQu.TestId = Teid;
                await _context.AddAsync(testQu);
                await _context.SaveChangesAsync();
            }
            if (testb.Type == "多选")
            {
                if (countb >= testa.Duo)
                {
                    return RedirectToAction("Index", "TestQus");
                }
                TestQu testQu = new TestQu();
                testQu.QuId = Convert.ToInt32(id);
                testQu.TestId = Teid;
                await _context.AddAsync(testQu);
                await _context.SaveChangesAsync();
            }
            if (testb.Type == "判断")
            {
                if (countc >= testa.Pan)
                {
                    return RedirectToAction("Index", "TestQus");
                }
                TestQu testQu = new TestQu();
                testQu.QuId = Convert.ToInt32(id);
                testQu.TestId = Teid;
                await _context.AddAsync(testQu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "TestChose");
        }

        // GET: TestChose/Create
        public IActionResult Create()
        {
            ViewData["BuId"] = new SelectList(_context.DbBu, "BuId", "BuName");
            ViewData["EmId"] = new SelectList(_context.DbEm, "EmId", "Password");
            ViewData["SuaId"] = new SelectList(_context.DbSu1, "SuaId", "SuaName");
            ViewData["SubId"] = new SelectList(_context.DbSu2, "SubId", "SubName");
            ViewData["SucId"] = new SelectList(_context.DbSu3, "SucId", "SucName");
            ViewData["SudId"] = new SelectList(_context.DbSu4, "SudId", "SudName");
            ViewData["SueId"] = new SelectList(_context.DbSu5, "SueId", "SueName");
            return View();
        }

        // POST: TestChose/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuId,Type,BuId,Question,OptionA,OptionB,OptionC,OptionD,RightAnswer,Difficulty,SuaId,SubId,SucId,SudId,SueId,EmId")] DbQu dbQu)
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
                _context.Add(dbQu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuId"] = new SelectList(_context.DbBu, "BuId", "BuName", dbQu.BuId);
            ViewData["EmId"] = new SelectList(_context.DbEm, "EmId", "Password", dbQu.EmId);
            ViewData["SuaId"] = new SelectList(_context.DbSu1, "SuaId", "SuaName", dbQu.SuaId);
            ViewData["SubId"] = new SelectList(_context.DbSu2, "SubId", "SubName", dbQu.SubId);
            ViewData["SucId"] = new SelectList(_context.DbSu3, "SucId", "SucName", dbQu.SucId);
            ViewData["SudId"] = new SelectList(_context.DbSu4, "SudId", "SudName", dbQu.SudId);
            ViewData["SueId"] = new SelectList(_context.DbSu5, "SueId", "SueName", dbQu.SueId);
            return View(dbQu);
        }

        // GET: TestChose/Edit/5
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

            var dbQu = await _context.DbQu.SingleOrDefaultAsync(m => m.QuId == id);
            if (dbQu == null)
            {
                return NotFound();
            }
            ViewData["BuId"] = new SelectList(_context.DbBu, "BuId", "BuName", dbQu.BuId);
            ViewData["EmId"] = new SelectList(_context.DbEm, "EmId", "Password", dbQu.EmId);
            ViewData["SuaId"] = new SelectList(_context.DbSu1, "SuaId", "SuaName", dbQu.SuaId);
            ViewData["SubId"] = new SelectList(_context.DbSu2, "SubId", "SubName", dbQu.SubId);
            ViewData["SucId"] = new SelectList(_context.DbSu3, "SucId", "SucName", dbQu.SucId);
            ViewData["SudId"] = new SelectList(_context.DbSu4, "SudId", "SudName", dbQu.SudId);
            ViewData["SueId"] = new SelectList(_context.DbSu5, "SueId", "SueName", dbQu.SueId);
            return View(dbQu);
        }

        // POST: TestChose/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuId,Type,BuId,Question,OptionA,OptionB,OptionC,OptionD,RightAnswer,Difficulty,SuaId,SubId,SucId,SudId,SueId,EmId")] DbQu dbQu)
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
            if (id != dbQu.QuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbQu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DbQuExists(dbQu.QuId))
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
            ViewData["BuId"] = new SelectList(_context.DbBu, "BuId", "BuName", dbQu.BuId);
            ViewData["EmId"] = new SelectList(_context.DbEm, "EmId", "Password", dbQu.EmId);
            ViewData["SuaId"] = new SelectList(_context.DbSu1, "SuaId", "SuaName", dbQu.SuaId);
            ViewData["SubId"] = new SelectList(_context.DbSu2, "SubId", "SubName", dbQu.SubId);
            ViewData["SucId"] = new SelectList(_context.DbSu3, "SucId", "SucName", dbQu.SucId);
            ViewData["SudId"] = new SelectList(_context.DbSu4, "SudId", "SudName", dbQu.SudId);
            ViewData["SueId"] = new SelectList(_context.DbSu5, "SueId", "SueName", dbQu.SueId);
            return View(dbQu);
        }

        // GET: TestChose/Delete/5
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

            var dbQu = await _context.DbQu
                .Include(d => d.Bu)
                .Include(d => d.Em)
                .Include(d => d.Sua)
                .Include(d => d.Sub)
                .Include(d => d.Suc)
                .Include(d => d.Sud)
                .Include(d => d.Sue)
                .SingleOrDefaultAsync(m => m.QuId == id);
            if (dbQu == null)
            {
                return NotFound();
            }

            return View(dbQu);
        }

        // POST: TestChose/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dbQu = await _context.DbQu.SingleOrDefaultAsync(m => m.QuId == id);
            _context.DbQu.Remove(dbQu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DbQuExists(int id)
        {
            return _context.DbQu.Any(e => e.QuId == id);
        }
    }
}
