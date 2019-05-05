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
    public class DbScsController : FonourControllerBase
    {
        private readonly NEUContext _context;
        public SelectList Name { get; set; }
        public string Ename { get; set; }

        public ActionResult Count()
        {
            // var co= await _context.Test.SingleOrDefaultAsync(m => m.ShenpiFlag == 0);
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View();
        }
        public DbScsController(NEUContext context)
        {
            _context = context;
        }
        // GET: DbScs
        public async Task<IActionResult> Index()
        {
           
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            int id = ViewBag.UserId;
            if (id == 0)
            {
                return NotFound();
            }
            var dbsc = from d in _context.DbSc.Include(d=>d.Pa).Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc) where (d.EmId == id) select d;
            //  var nEUContext = _context.DbSc.Include(d=>d.Bu).Include(d => d.Em);
            return View(await dbsc.ToListAsync());
        }

        // GET: DbScs/Details/5             
        public async Task<IActionResult> Details()
        {
        
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
                return Redirect("~/Login/Create");
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
                var sc = from d in _context.DbSc.Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc) where (d.EmId == item.EmId) select d;
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

            List<DbSc> dbsc = new List<DbSc>();
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
                    var bsc = from d in _context.DbSc.Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc) where (d.EmId == item.EmId) select d;
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
                var me = from d in _context.DbSc.Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc) where (d.EmId == m.EmId) select d;
                foreach (var ii in me)
                {

                    dbsc.Add(ii);
                }
                dbsc = me.ToList();
            }
            else if ((memName != null) && (date != null))
            {
                var m = await _context.DbEm.SingleOrDefaultAsync(d => d.EmName.Equals(memName));
                var me = from d in _context.DbSc.Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc) where (d.EmId == m.EmId) select d;
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


        /// <summary>
        /// 部门成绩信息查询 成员的详细成绩信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: DbScs/Create
        public async Task<IActionResult> Create(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var em = await _context.DbEm.SingleOrDefaultAsync(m => m.EmId == id);
            ViewBag.name = em.EmName;
            var dbsc = from d in _context.DbSc.Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc).Include(d => d.Em) where (d.EmId == id) select d;

            return View(await dbsc.ToListAsync());
        }

        // POST: DbScs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// 没用
        /// </summary>
        /// <param name="date"></param>
        /// <param name="memName"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string date, string memName)
        {
            List<DbSc> dbsc = new List<DbSc>();
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
                    var bsc = from d in _context.DbSc.Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc) where (d.EmId == item.EmId) select d;
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
                var me = from d in _context.DbSc.Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc) where (d.EmId == m.EmId) select d;
                foreach (var ii in me)
                {

                    dbsc.Add(ii);
                }
                dbsc = me.ToList();
            }
            else if ((memName != null) && (date != null))
            {
                var m = await _context.DbEm.SingleOrDefaultAsync(d => d.EmName.Equals(memName));
                var me = from d in _context.DbSc.Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc) where (d.EmId == m.EmId) select d;
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
        /// <summary>
        /// 没用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: DbScs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbSc = await _context.DbSc.Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc).SingleOrDefaultAsync(m => m.ScId == id);
            if (dbSc == null)
            {
                return NotFound();
            }
            ViewData["BuId"] = new SelectList(_context.DbBu, "BuId", "BuName", dbSc.BuId);
            ViewData["EmId"] = new SelectList(_context.DbEm, "EmId", "EmName", dbSc.EmId);
            return View(dbSc);
        }

        // POST: DbScs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// 通过下拉列表查询成绩信息
        /// </summary>
        /// <param name="date"></param>
        /// <param name="memName"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string date, string memName)
        {
            List<DbSc> dbsc = new List<DbSc>();
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
                    var bsc = from d in _context.DbSc.Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc) where (d.EmId == item.EmId) select d;
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
                var me = from d in _context.DbSc.Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc) where (d.EmId == m.EmId) select d;
                foreach (var ii in me)
                {

                    dbsc.Add(ii);
                }
                dbsc = me.ToList();
            }
            else if ((memName != null) && (date != null))
            {
                var m = await _context.DbEm.SingleOrDefaultAsync(d => d.EmName.Equals(memName));
                var me = from d in _context.DbSc.Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc) where (d.EmId == m.EmId) select d;
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

        // GET: DbScs/Delete/5
        public async Task<IActionResult> Delete()
        {
            string quNum = HttpContext.Session.GetString("quNum");
            var sta = from d in _context.DbSta.Include(d => d.Em.Bu) where (d.QueryNum.Equals(quNum)) select d;

            return View(await sta.ToListAsync());
        }

        // POST: DbScs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dbSc = await _context.DbSc.Include(d => d.Pa.Sua).Include(d => d.Pa.Sub).Include(d => d.Pa.Suc).SingleOrDefaultAsync(m => m.ScId == id);
            _context.DbSc.Remove(dbSc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DbScExists(int id)
        {
            return _context.DbSc.Any(e => e.ScId == id);
        }
    }
}