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
    public class DbLogsController : Controller
    {
        private readonly NEUContext _context;

        public DbLogsController(NEUContext context)
        {
            _context = context;
        }

        // GET: DbLogs
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
            return View(await _context.DbLog.ToListAsync());
        }

        // GET: DbLogs/Details/5
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
            var dbLog = from d in _context.DbLog where (d.QuId.Equals(id)) select d;
            var altertime = dbLog.Max(p=>p.AlterTime);
            var dbLoga = await _context.DbLog
                .SingleOrDefaultAsync(m => m.QuId == id&&m.AlterTime== altertime);
            if (dbLoga == null)
            {
                return NotFound();
            }
            return View(dbLoga);
        }
        // GET: DbLogs/Delete/5
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
            var dbLog = await _context.DbLog
                .SingleOrDefaultAsync(m => m.AlterId == id);
            if (dbLog == null)
            {
                return NotFound();
            }
            return View(dbLog);
        }

        // POST: DbLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            var dbLog = await _context.DbLog.SingleOrDefaultAsync(m => m.AlterId == id);
            _context.DbLog.Remove(dbLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DbLogExists(int id)
        {
            return _context.DbLog.Any(e => e.AlterId == id);
        }
    }
}
