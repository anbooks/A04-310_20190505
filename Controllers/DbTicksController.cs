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
    public class DbTicksController : FonourControllerBase
    {
        private readonly NEUContext _context;

        public DbTicksController(NEUContext context)
        {
            _context = context;
        }
        public IActionResult Do()
        {
            HttpContext.Session.Remove("string1");

            return Redirect("~/DbTicks/Create");
        }
        // GET: DbTicks
        public async Task<IActionResult> Index()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            var dbTick = from d in _context.DbTick.Include(e => e.Qu) where (d.Handle == "未处理") select d;
            //var nEUContext = _context.DbSc.Include(d => d.Bu).Include(d => d.Em);
          //  return View(await dbTick.ToListAsync());

            return View(dbTick);
        }

        // GET: DbTicks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbTick = await _context.DbTick
                .SingleOrDefaultAsync(m => m.TickId == id);
            if (dbTick == null)
            {
                return NotFound();
            }

            return View(dbTick);
        }

        // GET: DbTicks/Create
        public async Task<IActionResult> Create(string searchString, int id = 1)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            ViewBag.string1 = HttpContext.Session.GetString("string1");
            string string11 = ViewBag.string1;
            var dbTick = from d in _context.DbTick where (d.Handle == "已处理") select d;

            if (searchString != null)
            {
                HttpContext.Session.SetString("string1", searchString);

            }
            if (string11 != null)
            {
                ViewBag.string1 = HttpContext.Session.GetString("string1");
                string string2 = ViewBag.string1;
                searchString = string2;
                if (!String.IsNullOrEmpty(searchString))
                {
                    dbTick = dbTick.Where(s => s.Update.Contains(searchString));
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    dbTick = dbTick.Where(s => s.Update.Contains(searchString));
                }
            }
        
            var pageOption = new MoPagerOption
            {
                CurrentPage = id,
                PageSize = 20,
                Total = await dbTick.CountAsync(),
                RouteUrl = "/DbTicks/Create"
            };


          
            //分页参数
            ViewBag.PagerOption = pageOption;
            // return View(movieGenreVM);
            return View(await dbTick.OrderByDescending(b => b.TickId).Skip((pageOption.CurrentPage - 1) * pageOption.PageSize).Take(pageOption.PageSize).ToListAsync());
            //var nEUContext = _context.DbSc.Include(d => d.Bu).Include(d => d.Em);
          
        }

        // GET: DbTicks/Edit/5
        public async Task<IActionResult> Edit(int? id,int ida)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            if (id == null)
            {
                return NotFound();
            }
          
            HttpContext.Session.SetInt32("ida", ida);
            var dbTick=await _context.DbTick.SingleOrDefaultAsync(m => m.TickId == ida);
            var dbQu = await _context.DbQu.SingleOrDefaultAsync(m => m.QuId == id);
            if (dbQu == null)
            {
                return NotFound();
            }
            ViewData["BuId"] = new SelectList(_context.DbBu, "BuId", "BuName", dbQu.BuId);
            ViewData["SucId"] = new SelectList(_context.DbSu3, "SucId", "SucName", dbQu.SucId);
            return View(dbQu);
        }

        // POST: DbQus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuId,Type,BuId,Question,OptionA,OptionB,OptionC,OptionD,RightAnswer,Difficulty,SucId,EmId")] DbQu dbQu, [Bind("QuId,AlQuestion,AlOptionA,AlOptionB,AlOptionC,AlOptionD,AlRightAnswer,AlDifficulty,EmName,AlterTime,AlEdit")] DbLog dbLog)
        {

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);

            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            if (id != dbQu.QuId)
            {
                return NotFound();
            }
          
            ViewBag.ida = HttpContext.Session.GetInt32("ida");
            
            int ida = ViewBag.ida;
            DbTick dbtick=new DbTick();
            var dbqu = await _context.DbQu.SingleOrDefaultAsync(m => m.QuId == id);
            dbtick= await _context.DbTick.SingleOrDefaultAsync(m => m.TickId == ida);
    
         //   Type,BuId,Question,OptionA,OptionB,OptionC,OptionD,RightAnswer,Difficulty,SucId,
            if (dbQu.Type!=dbqu.Type||dbQu.BuId!=dbqu.BuId || dbQu.Question != dbqu.Question || dbQu.OptionA != dbqu.OptionA || dbQu.OptionB != dbqu.OptionB || dbQu.OptionC != dbqu.OptionC || dbQu.OptionD != dbqu.OptionD || dbQu.Difficulty != dbqu.Difficulty||dbQu.RightAnswer!=dbqu.RightAnswer)
            {
                dbLog.QuId = dbqu.QuId;
                dbLog.AlDifficulty = dbqu.Difficulty;
                dbLog.AlQuestion = dbqu.Question;
                dbLog.AlOptionA = dbqu.OptionA;
                dbLog.AlOptionB = dbqu.OptionB;
                dbLog.AlOptionC = dbqu.OptionC;
                dbLog.AlOptionD = dbqu.OptionD;
                dbLog.AlRightAnswer = dbqu.RightAnswer;
                dbLog.AlterTime = DateTime.Now;
                dbLog.EmName = dbem.EmName;
                dbLog.AlEdit = "修改";                
                dbtick.Update = "已更新";
                _context.Add(dbLog);
            }
            dbtick.Handle = "已处理";
            dbtick.Update = "未更新";

            dbqu.Type = dbQu.Type;
            dbqu.BuId = dbQu.BuId;
            dbqu.Question = dbQu.Question;
            dbqu.OptionA = dbQu.OptionA;
            dbqu.OptionB = dbQu.OptionB;
            dbqu.OptionC = dbQu.OptionC;
            dbqu.OptionD = dbQu.OptionD;
            dbqu.RightAnswer = dbQu.RightAnswer;
            dbqu.SucId = dbQu.SucId;
            dbqu.Difficulty = dbQu.Difficulty;
            dbqu.EmId = Eid;
            _context.Update(dbtick);
           
            _context.Update(dbqu);
            await _context.SaveChangesAsync();
            //HttpContext.Session.Remove("ida");
            return RedirectToAction(nameof(Index));
        }


        // GET: DbTicks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbTick = await _context.DbTick
                .SingleOrDefaultAsync(m => m.TickId == id);
            if (dbTick == null)
            {
                return NotFound();
            }

            return View(dbTick);
        }

        // POST: DbTicks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dbTick = await _context.DbTick.SingleOrDefaultAsync(m => m.TickId == id);
            _context.DbTick.Remove(dbTick);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DbTickExists(int id)
        {
            return _context.DbTick.Any(e => e.TickId == id);
        }
    }
}
