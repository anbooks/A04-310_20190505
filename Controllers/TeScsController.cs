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
    public class TeScsController : Controller
    {
        private readonly NEUContext _context;

        public TeScsController(NEUContext context)
        {
            _context = context;
        }

        // GET: TeScs
        public async Task<IActionResult> Index()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            int id = ViewBag.UserId;
            var nEUContext = _context.TeSc.Include(t => t.Bu).Include(t => t.Em).Include(t => t.Pa).Where(t=>t.EmId==id);
            return View(await nEUContext.ToListAsync());
        }
        public async Task<IActionResult> Tu()
        {
           
            return View();
        }
        // GET: TeScs/Details/5
        public async Task<IActionResult> Details()
        {
            var count = from d in _context.Test where (d.ShenpiFlag == 0) select d;
            int number = count.Count();
            ViewBag.NUM = number;
            List<DbSc> dbsc = new List<DbSc>();
            //List<DbSta> dbsta = new List<DbSta>();
            string num = (Guid.NewGuid().ToString());
            HttpContext.Session.SetString("quNum", num);
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem1 = new DbEm();
            int Eid = ViewBag.UserId;
            dbem1 = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);


            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem1.PoId.Equals(3))
            {
                //return Redirect("~/Login/Create");
            }
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            int id = ViewBag.UserId;
            var dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == id);
            var bran = from d in _context.DbEm where (d.Branch.Equals(dbem.Branch)) select d;//部门所有成员Id列表
            if (bran == null)
            {
                return NotFound();
            }
            foreach (var item in bran)//部门中每一个成员的答题记录
            {
                var sc = from d in _context.TeSc where (d.EmId == item.EmId) select d;
                int t = sc.Count();
                if (sc.Count() == 0)
                {
                    continue;
                }
                float sum = 0;
                int correct = 0;
                foreach (var itemsc in sc)
                {
                    sum += (float)itemsc.Score;
                    correct += itemsc.Correct;

                }
                DbSta sta = new DbSta
                {
                    Quantity = sc.Count(),
                    Average = Math.Round(sum / sc.Count(), 2),
                    Accuracy = Math.Round((float)(correct) / (sc.Count() * 30), 2),
                    EmId = item.EmId,
                    QueryNum = num
                };
                _context.Add(sta);
                await _context.SaveChangesAsync();
            }
            IQueryable<string> nameQuery = from m in bran orderby m.EmName select m.EmName;
            ViewBag.memberName = new SelectList(await nameQuery.ToListAsync());

            var neu = from d in _context.DbSta.Include(d => d.Em.Bu) where (d.QueryNum.Equals(num)) select d;
            //foreach (var item in bran)
            //{
            //    var bsc = from d in _context.DbSc where (d.EmId == item.EmId) select d;
            //    foreach (var ii in bsc)
            //    {
            //        dbsc.Add(ii);
            //    }
            //}
            //if (dbsc == null)
            //{
            //    return NotFound();
            //}
            return View(await neu.ToListAsync());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(string date, string memName)
        {

            List<TeSc> dbsc = new List<TeSc>();
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            int id = ViewBag.UserId;
            var dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == id);
            var bran = from d in _context.DbEm where (d.Branch.Equals(dbem.Branch)) select d;//部门所有成员Id列表
            if (bran == null)
            {
                return NotFound();
            }
            IQueryable<string> nameQuery = from m in bran orderby m.EmName select m.EmName;
            ViewBag.memberName = new SelectList(await nameQuery.ToListAsync());
            if ((memName == null) && (date != null))//只根据日期查询
            {
                foreach (var item in bran)
                {
                    var bsc = from d in _context.TeSc where (d.EmId == item.EmId) select d;
                    var data_bsc = from d in bsc where ((d.TestEnd.Value.ToString("yyyy-MM-dd")).Equals(date)) select d;
                    foreach (var ii in data_bsc)
                    {
                        dbsc.Add(ii);
                    }
                }
            }
            else if ((memName != null) && (date == null))
            {
                var m = await _context.DbEm.SingleOrDefaultAsync(d => d.EmName.Equals(memName));
                var me = from d in _context.TeSc where (d.EmId == m.EmId) select d;
                foreach (var ii in me)
                {

                    dbsc.Add(ii);
                }
                dbsc = me.ToList();
            }
            else if ((memName != null) && (date != null))
            {
                var m = await _context.DbEm.SingleOrDefaultAsync(d => d.EmName.Equals(memName));
                var me = from d in _context.TeSc where (d.EmId == m.EmId) select d;
                var dame = from d in me where (d.TestEnd.Value.ToString("yyyy-MM-dd").Equals(date)) select d;
                foreach (var ii in dame)
                {
                    dbsc.Add(ii);
                }
            }
            else
            {
                return View();
            }

            if (dbsc == null)
            {
                return NotFound();
            }
            return View(dbsc);
        }

        // GET: TeScs/Create
        public async Task<IActionResult> Create(int?id)
        {   
            var nEUContext = _context.TeSc.Include(t => t.Bu).Include(t => t.Em).Include(t => t.Pa).Where(t => t.EmId == id);
            return View(await nEUContext.ToListAsync());
        }
        public async Task<IActionResult> Edit()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            int id = ViewBag.UserId;
            var nEUContext = _context.TeSc.Include(t => t.Bu).Include(t => t.Em).Include(t => t.Pa).Where(t => t.EmId == id);
            return View(await nEUContext.ToListAsync());
        }
        public async Task<IActionResult> Delete ()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            int id = ViewBag.UserId;
            var nEUContext = _context.TeSc.Include(t => t.Bu).Include(t => t.Em).Include(t => t.Pa).Where(t => t.EmId == id);
            return View(await nEUContext.ToListAsync());
        }

        // POST: TeScs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teSc = await _context.TeSc.SingleOrDefaultAsync(m => m.ScId == id);
            _context.TeSc.Remove(teSc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeScExists(int id)
        {
            return _context.TeSc.Any(e => e.ScId == id);
        }
    }
}
