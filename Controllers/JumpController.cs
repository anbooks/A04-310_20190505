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
    public class JumpController : FonourControllerBase
    {
        private readonly NEUContext _context;

        public JumpController(NEUContext context)
        {
            _context = context;
        }

        // GET: Jump
        public async Task<IActionResult> Index()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.Include(d=>d.Po).SingleOrDefaultAsync(d => d.EmId == Eid);
            if (dbem.Po.PoName == "试卷管理员")
            {

                return Redirect("~/DbQus/Index");
            }
            if (dbem.Po.PoName == "系统管理员")
            {
                return Redirect("~/DbEms2/Index");
            }
            if (dbem.Po.PoName == "职工")
            {
                return Redirect("~/DbEms/Index");
            }
            if (dbem.Po.PoName == "上级主管")
            {
                return Redirect("~/TeScs/Details");
            }
            return View(await _context.DbBu.ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                int id = ViewBag.UserId;
                if (id == 0)
                {
                    return NotFound();
                }
                var dbsc = from d in _context.DbSc.Include(d => d.Pa).Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc) where (d.EmId == id) select d;
                //var nEUContext = _context.DbSc.Include(d => d.Bu).Include(d => d.Em);
                return View(await dbsc.ToListAsync());
            
        }
        public async Task<IActionResult> Details()
        {
              ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
              int id = ViewBag.UserId;
              if (id == 0)
              {
               return NotFound();
              }
              var dbsc = from d in _context.DbSc.Include(d => d.Pa).Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc) where (d.EmId == id) select d;
        //var nEUContext = _context.DbSc.Include(d => d.Bu).Include(d => d.Em);
              return View(await dbsc.ToListAsync());
        }
        public async Task<IActionResult> Delete()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            int id = ViewBag.UserId;
            if (id == 0)
            {
                return NotFound();
            }
            var dbsc = from d in _context.DbSc.Include(d => d.Pa).Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc) where (d.EmId == id) select d;
            //var nEUContext = _context.DbSc.Include(d => d.Bu).Include(d => d.Em);
            return View(await dbsc.ToListAsync());
        }
        public async Task<IActionResult> Edit()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            return View();
        }
       

        // GET: Jump/Details/5


        // POST: Jump/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
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
