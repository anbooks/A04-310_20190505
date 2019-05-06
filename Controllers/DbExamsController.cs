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
    public class DbExamsController : FonourControllerBase
    {
        private readonly NEUContext _context;

        public DbExamsController(NEUContext context)
        {
            _context = context;
        }

        // GET: DbExams
        public async Task<IActionResult> Index(int?id)
        {
            //var nEUContext = from m _context.DbExam
            //                 select m;
            var arr_p= from m in _context.DbExam
                       where (m.PaId == id )
                       orderby (m.Pax1_ID)
                       select m;

            return View(await arr_p.ToListAsync());
        }

        // GET: DbExams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var arr_p = from m in _context.DbExam
                        where (m.PaId == id&&m.RW=="错")
                        select m;

            return View(await arr_p.ToListAsync());
        }

        // GET: DbExams/Create

        public async Task<IActionResult> Create(int? id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem1 = new DbEm();
            int Eid = ViewBag.UserId;
            dbem1 = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);

            if (!dbem1.PoId.Equals(3))
            {
                return Redirect("~/Login/Create");
            }
            //var nEUContext = from m _context.DbExam
            //                 select m;
            var arr_p = from m in _context.DbExam
                        where (m.PaId == id && m.RW == "错")
                        select m;

            return View(await arr_p.ToListAsync());
        }
        // GET: DbExams/Edit/58
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbExam = await _context.DbExam.SingleOrDefaultAsync(m => m.ExamId == id);
            if (dbExam == null)
            {
                return NotFound();
            }
            ViewData["PaId"] = new SelectList(_context.DbPa, "PaId", "PaId", dbExam.PaId);
            return View(dbExam);
        }

        // POST: DbExams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExamId,QuId,Question,OptionA,OptionB,OptionC,OptionD,RightAnswer,Type,PaId")] DbExam dbExam)
        {
            if (id != dbExam.ExamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbExam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DbExamExists(dbExam.ExamId))
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
            ViewData["PaId"] = new SelectList(_context.DbPa, "PaId", "PaId", dbExam.PaId);
            return View(dbExam);
        }

        // GET: DbExams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbExam = await _context.DbExam
                .Include(d => d.Pa)
                .SingleOrDefaultAsync(m => m.ExamId == id);
            if (dbExam == null)
            {
                return NotFound();
            }

            return View(dbExam);
        }

        // POST: DbExams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dbExam = await _context.DbExam.SingleOrDefaultAsync(m => m.ExamId == id);
            _context.DbExam.Remove(dbExam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DbExamExists(int id)
        {
            return _context.DbExam.Any(e => e.ExamId == id);
        }
    }
}
