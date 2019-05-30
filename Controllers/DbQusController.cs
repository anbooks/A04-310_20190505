using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class DbQusController : FonourControllerBase
    {
        private readonly NEUContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DbQusController(NEUContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Do()
        {
            HttpContext.Session.Remove("string1");

            return Redirect("~/DbQus/Index");
        }


        [HttpPost]
        public async Task<IActionResult> Import(IFormFile excelfile)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = $"{Guid.NewGuid()}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));       
                using (FileStream fs = new FileStream(file.ToString(), FileMode.Create))
                {
                    excelfile.CopyTo(fs);
                    fs.Flush();
                }
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;

                    for (int row = 2; row <= rowCount; row++)
                {                   
                        DbQu dbQu = new DbQu();
                        dbQu.Type = worksheet.Cells[row,1].Value.ToString();
                        var aaa = await _context.DbBu.SingleOrDefaultAsync(m => m.BuName == worksheet.Cells[row, 2].Value.ToString());
                        dbQu.BuId = aaa.BuId;
                        dbQu.Question = worksheet.Cells[row, 3].Value.ToString();
                        dbQu.OptionA = worksheet.Cells[row, 4].Value.ToString();
                        dbQu.OptionB = worksheet.Cells[row, 5].Value.ToString();
                        if (dbQu.Type!="判断")
                        {
                        dbQu.OptionC = worksheet.Cells[row, 6].Value.ToString();
                        dbQu.OptionD = worksheet.Cells[row, 7].Value.ToString();
                         }
                        dbQu.OptionC = " ";
                        dbQu.OptionD = " ";
                        dbQu.RightAnswer = worksheet.Cells[row, 8].Value.ToString();
                        dbQu.Difficulty = worksheet.Cells[row, 9].Value.ToString();
                        var bbb = await _context.DbSu1.SingleOrDefaultAsync(m => m.SuaName == worksheet.Cells[row, 10].Value.ToString());
                        dbQu.SuaId = bbb.SuaId;
                        var ccc = await _context.DbSu2.SingleOrDefaultAsync(m => m.SubName == worksheet.Cells[row, 11].Value.ToString());
                        dbQu.SubId = ccc.SubId;
                        var ddd = await _context.DbSu3.SingleOrDefaultAsync(m => m.SucName == worksheet.Cells[row, 12].Value.ToString());
                        dbQu.SucId = ddd.SucId;

                        ViewBag.UserId = HttpContext.Session.GetInt32("UserId");                   
                        dbQu.EmId = ViewBag.UserId;;
                        _context.Add(dbQu);
                        await _context.SaveChangesAsync();
                    }
                    return RedirectToAction("Index", "DbQus");
                }                       
        }

        public async Task<IActionResult> First(string text)
        {
            if (text == null)
            {
                return NotFound();
            }
            //HttpContext.Session.SetString("select1", text);
            var a = await _context.DbSu1.SingleOrDefaultAsync(m => m.SuaName.Equals(text));
            HttpContext.Session.SetInt32("selId_1", a.SuaId);
            var b = from d in _context.DbSu2 where (d.SuaId == a.SuaId) select d;
   
            IQueryable<string> nameQuery = from m in b orderby m.SubName select m.SubName;
            ViewBag.second = new SelectList(await nameQuery.ToListAsync());

            SubNameViewModel sub = new SubNameViewModel
            {
                Sel2_na = new SelectList(await nameQuery.ToListAsync())
            };
            //var jsonResult = Json(b.ToList());
            return PartialView("First", sub);
        }

        public async Task<IActionResult> Second(string text)
        {
            if (text == null)
            {
                return NotFound();
            }
            //HttpContext.Session.SetString("select2", text);
            var a = await _context.DbSu2.SingleOrDefaultAsync(m => m.SubName.Equals(text));
            HttpContext.Session.SetInt32("selId_2", a.SubId);
            var b = from d in _context.DbSu3 where (d.SubId == a.SubId) select d;
            IQueryable<string> nameQuery = from m in b orderby m.SucName select m.SucName;


            SubNameViewModel sub = new SubNameViewModel
            {
                Sel2_na = new SelectList(await nameQuery.ToListAsync())
            };
            //var jsonResult = Json(b.ToList());
            return PartialView("Second", sub);

        }
        // GET: DbQus
        public async Task<IActionResult> Index(String searchString, int id = 1)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            int poid = 0;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            ViewBag.string1 = HttpContext.Session.GetString("string1");
            string string11 = ViewBag.string1;

            IQueryable<string> diff = from m in _context.DbQu
                                            orderby m.Question
                                            select m.Question;
            
           
               var  movies = from m in _context.DbQu.Include(d => d.Em).Include(d => d.Sua).Include(d => d.Sub).Include(d => d.Suc)
                             where (m.EmId == Eid)
                             select m;
          
            if ((dbem.CardId == "paper"))  
            { 
              movies = from m in _context.DbQu.Include(d => d.Em).Include(d => d.Sua).Include(d => d.Sub).Include(d => d.Suc)
                         select m;
            }

            if (searchString != null)
            {
                HttpContext.Session.SetString("string1", searchString);

            }
            if (string11 != null)
            {
                searchString = string11;
                if (!String.IsNullOrEmpty(searchString))
                {
                    movies = movies.Where(s => s.Question.Contains(searchString));
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    movies = movies.Where(s => s.Question.Contains(searchString));
                }
            }
            var movieGenreVM = new QuesDifficultuModel();
            movieGenreVM.Difficulty = new SelectList(await diff.Distinct().ToListAsync());
            var pageOption = new MoPagerOption
            {
                CurrentPage = id,
                PageSize = 20,
                Total = await movies.CountAsync(),
                RouteUrl = "/DbQus/Index",
                
            };
            var text = new Text
            {      
                po = dbem.PoId
            };

           
            movieGenreVM.movies = await movies.OrderByDescending(b => b.QuId).Skip((pageOption.CurrentPage - 1) * pageOption.PageSize).Take(pageOption.PageSize).ToListAsync();
            //分页参数
             ViewBag.PagerOption = pageOption;
           // return View(movieGenreVM);
            return View(movieGenreVM);
        }

        // GET: DbQus/Details/5
        public async Task<IActionResult> Details(int? id)
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

            var dbQu = await _context.DbQu
                .Include(d => d.Bu)
                .Include(d => d.Suc)
                .SingleOrDefaultAsync(m => m.QuId == id);
            if (dbQu == null)
            {
                return NotFound();
            }

            return View(dbQu);
        }

        // GET: DbQus/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            IQueryable<string> pro = from m in _context.DbSu1 orderby m.SuaName select m.SuaName;
            ViewBag.first = new SelectList(await pro.ToListAsync());

            ViewData["BuId"] = new SelectList(_context.DbBu, "BuId", "BuName");
            ViewData["SucId"] = new SelectList(_context.DbSu3, "SucId", "SucName");
          
            return View();
        }

        // POST: DbQus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string sel_1, string sel_2, string sel_3,[Bind("QuId,Type,BuId,Question,OptionA,OptionB,OptionC,OptionD,RightAnswer,Difficulty,SucId,EmId")] DbQu dbQu,string Type)
        {

            string test = Type;    //获取前台的下拉框的值
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            //var sel3 = await _context.DbSu3.SingleOrDefaultAsync(m => m.SucName.Equals(sel_3));
            //var sel2 = await _context.DbSu2.SingleOrDefaultAsync(m => m.SubName.Equals(sel_2));
            var sel1 = await _context.DbSu1.SingleOrDefaultAsync(m => m.SuaName.Equals(sel_1));
            //if (sel1 == null|| sel2 == null||sel3 == null)
                if (sel1 == null )
                {
                ModelState.AddModelError("", "请填写，部门不能为空");//不显示
                return RedirectToAction("Create", "DbQus");
            }
            if (ModelState.IsValid)
            {
                if (Type == "判断")
                {
                    dbQu.OptionA = "对";
                    dbQu.OptionB = "错";
                    dbQu.OptionC = "";
                    dbQu.OptionD = "";
                }
               
                    dbQu.SuaId = sel1.SuaId;
                    //dbQu.SubId = sel2.SubId;
                    //dbQu.SucId = sel3.SucId;
                    dbQu.EmId = Eid;
               



                _context.Add(dbQu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuId"] = new SelectList(_context.DbBu, "BuId", "BuName", dbQu.BuId);
            ViewData["SucId"] = new SelectList(_context.DbSu3, "SucId", "SucName", dbQu.SucId);
            return View(dbQu);
        }
        public async Task<IActionResult> PanDuan()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            IQueryable<string> pro = from m in _context.DbSu1 orderby m.SuaName select m.SuaName;
            ViewBag.first = new SelectList(await pro.ToListAsync());

            ViewData["BuId"] = new SelectList(_context.DbBu, "BuId", "BuName");
            ViewData["SucId"] = new SelectList(_context.DbSu3, "SucId", "SucName");
            return View();
        }

        // POST: DbQus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PanDuan(string sel_1, string sel_2, string sel_3, [Bind("QuId,Type,BuId,Question,RightAnswer,Difficulty,SucId,EmId")] DbQu dbQu)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            var sel3 = await _context.DbSu3.SingleOrDefaultAsync(m => m.SucName.Equals(sel_3));
            var sel2 = await _context.DbSu2.SingleOrDefaultAsync(m => m.SubName.Equals(sel_2));
            var sel1 = await _context.DbSu1.SingleOrDefaultAsync(m => m.SuaName.Equals(sel_1));
            if (sel1 == null || sel2 == null || sel3 == null)
            {
                ModelState.AddModelError("", "请填写，专业不能为空");//不显示
                return RedirectToAction("Create", "DbQus");
            }
            if (ModelState.IsValid)
            {
                dbQu.SuaId = sel1.SuaId;
                dbQu.SubId = sel2.SubId;
                dbQu.SucId = sel3.SucId;
                dbQu.EmId = Eid;
                _context.Add(dbQu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuId"] = new SelectList(_context.DbBu, "BuId", "BuName", dbQu.BuId);
            ViewData["SucId"] = new SelectList(_context.DbSu3, "SucId", "SucName", dbQu.SucId);
            return View(dbQu);
        }
        // GET: DbQus/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, [Bind("QuId,Type,BuId,Question,OptionA,OptionB,OptionC,OptionD,RightAnswer,Difficulty,EmId")] DbQu dbQu,[Bind("QuId,AlQuestion,AlOptionA,AlOptionB,AlOptionC,AlOptionD,AlRightAnswer,AlDifficulty,EmName,AlterTime,AlEdit")] DbLog dbLog)
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
            var dbqu = await _context.DbQu.SingleOrDefaultAsync(m => m.QuId == id);
            //var dblogb=from d  in _context.DbLog where (d.QuId == id) select d;
            //int p = dblogb.Count();
            //if (p>3)
            //{
            //    var dbLog1 = from d in _context.DbLog where (d.QuId.Equals(id)) select d;
            //    var altertime = dbLog1.Min(n => n.AlterTime);
            //    var dbLoga = await _context.DbLog
            //        .SingleOrDefaultAsync(m => m.QuId == id && m.AlterTime == altertime);
            //    _context.Remove(dbLoga);
            //    await _context.SaveChangesAsync();
            //}
            if (dbQu.Type != dbqu.Type || dbQu.BuId != dbqu.BuId || dbQu.Question != dbqu.Question || dbQu.OptionA != dbqu.OptionA || dbQu.OptionB != dbqu.OptionB || dbQu.OptionC != dbqu.OptionC || dbQu.OptionD != dbqu.OptionD || dbQu.Difficulty != dbqu.Difficulty||dbQu.RightAnswer!=dbqu.RightAnswer)
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
                _context.Add(dbLog);
            }
            dbqu.Type = dbQu.Type;
            dbqu.BuId = dbQu.BuId;
            dbqu.Question = dbQu.Question;
            dbqu.OptionA = dbQu.OptionA;
            dbqu.OptionB = dbQu.OptionB;
            dbqu.OptionC = dbQu.OptionC;
            dbqu.OptionD = dbQu.OptionD;
            dbqu.RightAnswer = dbQu.RightAnswer;
           
            dbqu.Difficulty = dbQu.Difficulty;
            dbqu.EmId = Eid;        
           
            _context.Update(dbqu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: DbQus/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            var dbQu = await _context.DbQu
                .Include(d => d.Bu)
                .SingleOrDefaultAsync(m => m.QuId == id);
            if (dbQu == null)
            {
                return NotFound();
            }

            return View(dbQu);
        }

        // POST: DbQus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, [Bind("QuId,AlQuestion,AlOptionA,AlOptionB,AlOptionC,AlOptionD,AlRightAnswer,AlDifficulty,EmName,AlterTime")] DbLog dbLog)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem.PoId.Equals(1))
            {
                return Redirect("~/Login/Create");
            }
            var dbQu = await _context.DbQu.SingleOrDefaultAsync(m => m.QuId == id);
            var dbPax = from d in _context.DbPax where (d.QuId==id) select d;
                dbLog.QuId = dbQu.QuId;
                dbLog.AlDifficulty = dbQu.Difficulty;
                dbLog.AlQuestion = dbQu.Question;
                dbLog.AlOptionA = dbQu.OptionA;
                dbLog.AlOptionB = dbQu.OptionB;
                dbLog.AlOptionC = dbQu.OptionC;
                dbLog.AlOptionD = dbQu.OptionD;
                dbLog.AlRightAnswer = dbQu.RightAnswer;
                dbLog.AlterTime = DateTime.Now;
                dbLog.EmName = dbem.EmName;
                dbLog.AlEdit = "删除";
                _context.Add(dbLog);
                foreach (var item in dbPax)
                {
                    _context.DbPax.Remove(item);
                }
                _context.DbQu.Remove(dbQu);
                await _context.SaveChangesAsync();         
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
      
        private bool DbQuExists(int id)
        {
            return _context.DbQu.Any(e => e.QuId == id);
        }
    }
}
