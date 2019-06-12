using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class DbPaxesController : Controller
    {
        private readonly NEUContext _context;
        private int tureOrFalseNum = 10;//判断题数量
        private int singleCho = 20;//单选题数量
        private int multipleCho = 10;//多选题数量
        private static DateTime testStart;
        private static DateTime testEnd;
        private static int signTime;



        public DbPaxesController(NEUContext context)
        {

            _context = context;
        }

        private bool QuestionExists(Int32 id, List<int> value)
        {
            foreach (var item in value)
            {
                if (item == id) return true;

                else
                {
                    continue;
                }
            }
            return false;

        }



        public static Dictionary<int, int> pax_list = new Dictionary<int, int>();
        public static Dictionary<int, string> ans_list = new Dictionary<int, string>();
        // GET: DbPaxes
        public async Task<IActionResult> Index()
        {
            pax_list.Clear();
            ans_list.Clear();

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            if (ViewBag.UserId == null)
            {
                return RedirectToAction("Create", "DbEms");
            }

            //var selId_3 = HttpContext.Session.GetInt32("selId_3");
            //var selId_2 = HttpContext.Session.GetInt32("selId_2");
            var selId_1 = HttpContext.Session.GetInt32("selId_1");

            //生成试卷号


            IQueryable<DbQu> qu;
            //根据专业抽题
            //var select3 = HttpContext.Session.GetString("select3");
            //var select2 = HttpContext.Session.GetString("select2");
            var select1 = HttpContext.Session.GetString("select1");
            //if (select3 != null)
            //{
            //    var sel = await _context.DbSu3.SingleOrDefaultAsync(d => d.SucName.Equals(select3));
            //    qu = from d in _context.DbQu where (d.SucId == sel.SucId) select d;

            //}
            //else if (select2 != null)
            //{
            //    var sel = await _context.DbSu2.SingleOrDefaultAsync(d => d.SubName.Equals(select2));
            //    qu = from d in _context.DbQu where (d.SubId == sel.SubId) select d;

            //}
            //else 
            if (select1 != null)
            {
                var sel = await _context.DbSu1.SingleOrDefaultAsync(d => d.SuaName.Equals(select1));
                qu = from d in _context.DbQu where (d.SuaId == sel.SuaId) select d;
                if(qu.Count()==0)
                {
                    HttpContext.Session.SetInt32("Sign", 1);
                    return RedirectToAction("Start", "DbEms");
                }

            }
            else
            {
                HttpContext.Session.SetInt32("Sign", 0);
                qu = from d in _context.DbQu select d;
            }


            //从抽出的该专业的题中抽取不同难度的单选题和多选题
            int j = 1;

            var single_Choice = from d in qu where (d.Type.Equals("单选")) select d;
            //int m = single_Choice.Count();
            var multiple_Choice = from d in qu where (d.Type.Equals("多选")) select d;
            var trueOrFalse = from d in qu where (d.Type.Equals("判断")) select d;

            List<int> keys = new List<int>();
            List<int> value = new List<int>();


            var hardT = from d in trueOrFalse where (d.Difficulty.Equals("难")) select d;
            for (int i = 0; i < tureOrFalseNum * 0.2;)
            {


                DbPax dbPax = new DbPax();
                Random s = new Random();
                var cc = hardT.ToList();
                int b = s.Next(0, hardT.Count());
                if (!QuestionExists(cc[b].QuId, value))//试卷中不存在新抽取的题，将其加入试卷
                {
                    j++;

                    pax_list.Add(j - 1, cc[b].QuId);
                    keys.Add(j - 1);
                    value.Add(cc[b].QuId);

                    ++i;
                }

            }

            var mediumT = from d in trueOrFalse where (d.Difficulty.Equals("中")) select d;
            for (int i = 0; i < tureOrFalseNum * 0.5;)
            {
                DbExam dbExam = new DbExam();
                DbPax dbPax = new DbPax();

                Random s = new Random();
                var cc = mediumT.ToList();

                int b = s.Next(0, mediumT.Count());
                if (!QuestionExists(cc[b].QuId, value))
                {

                    j++;
                    pax_list.Add(j - 1, cc[b].QuId);
                    keys.Add(j - 1);
                    value.Add(cc[b].QuId);

                    ++i;
                }

            }

            var simpleT = from d in trueOrFalse where (d.Difficulty.Equals("易")) select d;
            for (int i = 0; i < tureOrFalseNum * 0.3;)
            {

                DbExam dbExam = new DbExam();

                Random s = new Random();
                var cc = simpleT.ToList();
                DbPax dbPax = new DbPax();
                int b = s.Next(0, simpleT.Count());
                if (!QuestionExists(cc[b].QuId, value))
                {
                    j++;

                    pax_list.Add(j - 1, cc[b].QuId);
                    keys.Add(j - 1);
                    value.Add(cc[b].QuId);

                    ++i;
                }
            }

            var hardS = from d in single_Choice where (d.Difficulty.Equals("难")) select d;
            for (int i = 0; i < singleCho * 0.2;)
            {


                DbPax dbPax = new DbPax();
                Random s = new Random();
                var cc = hardS.ToList();
                int b = s.Next(0, hardS.Count());
                if (!QuestionExists(cc[b].QuId, value))//试卷中不存在新抽取的题，将其加入试卷
                {
                    j++;

                    pax_list.Add(j - 1, cc[b].QuId);
                    keys.Add(j - 1);
                    value.Add(cc[b].QuId);

                    ++i;
                }

            }

            var mediumS = from d in single_Choice where (d.Difficulty.Equals("中")) select d;
            for (int i = 0; i < singleCho * 0.5;)
            {
                DbExam dbExam = new DbExam();
                DbPax dbPax = new DbPax();

                Random s = new Random();
                var cc = mediumS.ToList();

                int b = s.Next(0, mediumS.Count());
                if (!QuestionExists(cc[b].QuId, value))
                {

                    j++;
                    pax_list.Add(j - 1, cc[b].QuId);
                    keys.Add(j - 1);
                    value.Add(cc[b].QuId);

                    ++i;
                }

            }

            var simpleS = from d in single_Choice where (d.Difficulty.Equals("易")) select d;
            for (int i = 0; i < singleCho * 0.3;)
            {

                DbExam dbExam = new DbExam();

                Random s = new Random();
                var cc = simpleS.ToList();
                DbPax dbPax = new DbPax();
                int b = s.Next(0, simpleS.Count());
                if (!QuestionExists(cc[b].QuId, value))
                {
                    j++;

                    pax_list.Add(j - 1, cc[b].QuId);
                    keys.Add(j - 1);
                    value.Add(cc[b].QuId);

                    ++i;
                }
            }

            var hardM = from d in multiple_Choice where (d.Difficulty.Equals("难")) select d;
            for (int i = 0; i < multipleCho * 0.2;)
            {

                DbPax dbPax = new DbPax();

                Random s = new Random();
                var cc = hardM.ToList();

                int b = s.Next(0, hardM.Count());
                if (!QuestionExists(cc[b].QuId, value))
                {
                    j++;

                    pax_list.Add(j - 1, cc[b].QuId);
                    keys.Add(j - 1);
                    value.Add(cc[b].QuId);

                    ++i;
                }
            }
            for (int i = 0; i < multipleCho * 0.5;)
            {
                DbExam dbExam = new DbExam();
                DbPax dbPax = new DbPax();
                var mediumM = from d in multiple_Choice where (d.Difficulty.Equals("中")) select d;
                Random s = new Random();
                var cc = mediumM.ToList();

                int b = s.Next(0, mediumM.Count());
                if (!QuestionExists(cc[b].QuId, value))
                {
                    j++;

                    pax_list.Add(j - 1, cc[b].QuId);
                    keys.Add(j - 1);
                    value.Add(cc[b].QuId);

                    ++i;
                }
            }
            for (int i = 0; i < multipleCho * 0.3;)
            {
                DbExam dbExam = new DbExam();
                DbPax dbPax = new DbPax();
                var simpleM = from d in multiple_Choice where (d.Difficulty.Equals("易")) select d;
                Random s = new Random();
                var cc = simpleM.ToList();

                int b = s.Next(0, simpleM.Count());
                if (!QuestionExists(cc[b].QuId, value))
                {
                    j++;

                    pax_list.Add(j - 1, cc[b].QuId);
                    keys.Add(j - 1);
                    value.Add(cc[b].QuId);

                    ++i;
                }
            }


            DbPa dbpa = new DbPa();

            dbpa.EmId = ViewBag.UserId;
            dbpa.SuaId = selId_1;
            //dbpa.SubId = selId_2;
            //dbpa.SucId = selId_3;
            _context.DbPa.Add(dbpa);
            await _context.SaveChangesAsync();
            int PaperId = dbpa.PaId;
            HttpContext.Session.SetInt32("paperId", PaperId);

            foreach (var item in keys)
            {
                DbPax dbPax = new DbPax();
                dbPax.Pax1_ID = item;
                dbPax.QuId = pax_list[item];
                dbPax.PaId = PaperId;
                _context.Add(dbPax);

                //将生成的试卷中的题目存入DbExam表中
                DbExam dbExam = new DbExam();
                var ti = await _context.DbQu.FirstOrDefaultAsync(b => b.QuId == pax_list[item]);
                dbExam.OptionA = ti.OptionA;
                dbExam.OptionB = ti.OptionB;
                dbExam.OptionC = ti.OptionC;
                dbExam.OptionD = ti.OptionD;
                dbExam.QuId = ti.QuId;
                dbExam.Question = ti.Question;
                dbExam.RightAnswer = ti.RightAnswer;
                dbExam.Type = ti.Type;
                dbExam.PaId = PaperId;
                dbExam.Pax1_ID = item;

                _context.Add(dbExam);
            }
            await _context.SaveChangesAsync();

            testStart = DateTime.Now;

            var paContext = from d in _context.DbPax where (d.PaId == PaperId) select d;
            var nEUContext = paContext.OrderBy(d => d.Pax1_ID).Include(d => d.Pa).Include(d => d.Qu);
            return View(await nEUContext.ToListAsync());
        }

        /// <summary>
        /// 返回题目列表
        /// </summary>
        /// <returns></returns>
        // GET: DbPaxes/Details/5
        public async Task<IActionResult> Details(int id, string Option)
        {

            ViewBag.paperId = HttpContext.Session.GetInt32("paperId");
            int paId = ViewBag.paperId;
            var dbPax = from d in _context.DbPax.OrderBy(d => d.Pax1_ID) where (d.PaId == paId) select d;

            if (dbPax == null)
            {
                return NotFound();
            }
            var dbpax = dbPax.Include(d => d.Qu);

            List<QuestionAns> quAns_list = new List<QuestionAns>();
            foreach (var item in dbpax)
            {
                QuestionAns quAns = new QuestionAns();
                quAns.Pax = item;
                int i = item.Pax1_ID;
                if (ans_list.ContainsKey(i))
                {
                    quAns.Answer = ans_list[i];
                }
                else
                {
                    quAns.Answer = null;
                }

                quAns_list.Add(quAns);

            }


            return View(quAns_list);
        }


        public bool Answer(string id, string option)
        {
            if ((id != null) && (option != null))
            {
                int i = Convert.ToInt16(id);
                if (ans_list.ContainsKey(i))
                {
                    ans_list[i] = option;

                }
                ans_list.Add(i, option);
                return true;

            }
            else
            {
                return true;
            }


        }


        // GET: DbPaxes/Create
        public async Task<IActionResult> Create(int? id)
        {
            var paperId = HttpContext.Session.GetInt32("paperId");
            var Sign = HttpContext.Session.GetInt32("ErrorSign");

            if (Sign == 1)
            {
                ViewData["ErrorSign"] = 1;
            }

            if (id == null)
            {
                return NotFound();
            }

            var pax = _context.DbPax.Include(m => m.Qu);
            var dbPax = pax.SingleOrDefault(m => (m.PaId == paperId) && (m.Pax1_ID == id));

            if (dbPax == null)
            {
                return NotFound();
            }
            int i = dbPax.Pax1_ID;
            if (ans_list.ContainsKey(i))
            {
                ViewData["ans"] = ans_list[i];
            }
            //var dbQu = await _context.DbQu.SingleOrDefaultAsync(m => m.QuId == dbPax.QuId);
            //ViewData["PaId"] = new SelectList(_context.DbPa, "PaId", "PaId");
            //ViewData["QuId"] = new SelectList(_context.DbQu, "QuId", "Difficulty");
            return View(dbPax);
        }


        List<string> keys = new List<string>();
        // POST: DbPaxes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, string edit, string searchString)
        {
            var Eid = HttpContext.Session.GetInt32("UserId");
            var paperId = HttpContext.Session.GetInt32("paperId");
            DbPax dbPax = new DbPax();
            dbPax = await _context.DbPax.SingleOrDefaultAsync(m => (m.PaId == paperId) && (m.Pax1_ID == id));



            string stores = Request.Form["checkBox"];
            if (searchString != null)
            {

                DbEm dbem = new DbEm();
                dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
                DbTick dbtick = new DbTick();
                dbtick.Ticking = searchString;
                dbtick.Handle = "未处理";
                dbtick.Update = "未更新";
                dbtick.QuId = dbPax.QuId;
                dbtick.EmName = dbem.EmName;
                _context.Add(dbtick);
                await _context.SaveChangesAsync();
            }
            if (stores == null)
            {
                HttpContext.Session.SetInt32("ErrorSign", 1);
                //ModelState.AddModelError("", "请作答，选项不能为空");//不显示
                return RedirectToAction("Create", "DbPaxes", id);
            }
            else
            {
                HttpContext.Session.SetInt32("ErrorSign", 0);
            }


            if (id != dbPax.Pax1_ID)
            {
                return NotFound();
            }

            if (edit.Equals("上一题"))
            {
                if (dbPax.Pax1_ID != 1)
                {
                    if (ans_list.TryGetValue(id, out string value))
                    {
                        ans_list[id] = stores;
                        --id;
                    }
                    else
                    {
                        ans_list.Add(id, stores);
                        --id;
                    }
                    return RedirectToRoute(new { Controller = "DbPaxes", Action = "Edit", id });
                }
                else { return View(); }
            }
            else
            {
                if (dbPax.Pax1_ID < 40)
                {
                    if (ans_list.TryGetValue(id, out string value))
                    {
                        ans_list[id] = stores;
                        ++id;
                    }
                    else
                    {
                        ans_list.Add(id, stores);
                        ++id;
                    }

                    return RedirectToRoute(new { Controller = "DbPaxes", Action = "Edit", id });
                }
                else if (dbPax.Pax1_ID == 40)
                {
                    if (ans_list.TryGetValue(id, out string value))
                    {

                        ans_list[id] = stores;
                    }
                    else
                    {
                        ans_list.Add(id, stores);
                    }
                    testEnd = DateTime.Now;
                    var paper_list = from d in _context.DbPax where (d.PaId == paperId) select d;
                    foreach (var item in ans_list)
                    {
                        var ti = await paper_list.SingleOrDefaultAsync(d => d.Pax1_ID == item.Key);
                        DbTe te = new DbTe();
                        te.PaId = (int)paperId;
                        te.PaxId = ti.PaxId;
                        te.EmAnswer = item.Value;
                        _context.Add(te);

                        DbExam ex = new DbExam();
                        ex = await _context.DbExam.SingleOrDefaultAsync(d => (d.PaId == paperId) && (d.Pax1_ID == item.Key));
                        if (ex == null)
                        {
                            return NotFound();
                        }
                        ex.EmAnswer = item.Value;
                        _context.Update(ex);

                    }
                    await _context.SaveChangesAsync();

                    return RedirectToRoute(new { Controller = "DbPaxes", Action = "Delete", paperId });

                }
                else return NotFound();
            }

        }

        // GET: DbPaxes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
           
            var paId = HttpContext.Session.GetInt32("paperId");
            var Sign = HttpContext.Session.GetInt32("ErrorSign");
            if(Sign==1)
            {
                ViewData["ErrorSign"] = 1;
            }

            if (id == 0)
            {
                return NotFound();
            }
            //if (signTime == 1)
            //{
            //    var hour = HttpContext.Session.GetInt32("hour");
            //    var minutes = HttpContext.Session.GetInt32("minutes");
            //    var second = HttpContext.Session.GetInt32("second");
            //    ViewData["signTime"] = signTime;
            //    ViewData["hour"] = (int)hour;
            //    ViewData["minutes"] = (int)minutes;
            //    ViewData["second"] = (int)second;

            //}
            //else
            //{
            //    ViewData["signTime"] = signTime;
            //}

            DbPax pax = new DbPax();
            pax = _context.DbPax.SingleOrDefault(m => (m.PaId == paId) && (m.Pax1_ID == id));
            var paperQu = _context.DbQu.SingleOrDefault(m => m.QuId == pax.QuId);
            if (paperQu == null)
            {
                return NotFound();
            }
            else
            {
                if (id <= 40)
                {
                    if (paperQu.Type.Equals("多选"))
                    {
                        return RedirectToRoute(new { Controller = "DbPaxes", Action = "Create", id });
                    }
                    else
                    {
                        int i = pax.Pax1_ID;
                        if (ans_list.ContainsKey(i))
                        {
                            ViewData["ans"] = ans_list[i];
                        }
                        return View(pax);
                    }
                }

                else
                {
                    testEnd = DateTime.Now;

                    return RedirectToRoute(new { Controller = "DbPaxes", Action = "Delete", paId });
                }

            }



            //ViewData["PaId"] = new SelectList(_context.DbPa, "PaId", "PaId", dbPax.PaId);
            //ViewData["QuId"] = new SelectList(_context.DbQu, "QuId", "Difficulty", dbPax.QuId);
        }

        // POST: DbPaxes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string edit, string Option, string searchString, string submit)
        {

            var paperId = HttpContext.Session.GetInt32("paperId");
            var Eid = HttpContext.Session.GetInt32("UserId");
            DbPax dbPax = new DbPax();
            dbPax = await _context.DbPax.SingleOrDefaultAsync(m => (m.PaId == paperId) && (m.Pax1_ID == id));

            if (searchString != null)
            {

                DbEm dbem = new DbEm();

                dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
                DbTick dbtick = new DbTick();
                dbtick.Ticking = searchString;
                dbtick.Handle = "未处理";
                dbtick.Update = "未更新";
                dbtick.QuId = dbPax.QuId;
                dbtick.EmName = dbem.EmName;
                _context.Add(dbtick);
                await _context.SaveChangesAsync();
            }
            if (Option == null)
            {
                //ModelState.AddModelError("", "请作答，选项不能为空");//不显示
                //return View();
                
                HttpContext.Session.SetInt32("ErrorSign",1);
                return RedirectToAction("Edit", "DbPaxes");
            }
            else
            {
                HttpContext.Session.SetInt32("ErrorSign", 0);
            }


            if (edit.Equals("上一题"))
            {
                if (dbPax.Pax1_ID != 1)
                {
                    if (ans_list.TryGetValue(id, out string value))
                    {
                        ans_list[id] = Option;
                        --id;
                    }
                    else
                    {
                        ans_list.Add(id, Option);
                        --id;
                    }
                    //if (ans_list[id] != null)
                    //{
                    //    ans_list.Add(id, Option);
                    //    --id;
                    //}
                    //else
                    //{
                    //    ans_list[id] = Option;
                    //}


                    return RedirectToRoute(new { Controller = "DbPaxes", Action = "Edit", id });
                }
                else return View();
            }
            else
            {
                if (dbPax.Pax1_ID <= 40)
                {
                    if (ans_list.TryGetValue(id, out string value))
                    {
                        ans_list[id] = Option;
                        ++id;
                    }
                    else
                    {

                        ans_list.Add(id, Option);
                        ++id;
                    }


                    return RedirectToRoute(new { Controller = "DbPaxes", Action = "Edit", id });
                }
                testEnd = DateTime.Now;


                return RedirectToRoute(new { Controller = "DbPaxes", Action = "Delete", dbPax.PaId });

            }
        }
        /// <summary>
        /// 提交用户答案，计算考试成绩
        /// </summary>
        /// <returns></returns>
        // GET: DbPaxes/Delete/5
        public async Task<IActionResult> Delete()
        {
            var paId = HttpContext.Session.GetInt32("paperId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            var paper = from d in _context.DbPax where (d.PaId == paId) select d;
            var dbpax = await paper.ToListAsync();
            int countT=0, countS = 0, countN = 0;
            foreach (var item in dbpax)
            {
                DbQu dbqu = await _context.DbQu.SingleOrDefaultAsync(d => d.QuId == item.QuId);
                DbTe dbte = await _context.DbTe.SingleOrDefaultAsync(d => d.PaxId == item.PaxId);

                if (dbte == null)
                {
                    DbTe dbTe = new DbTe();
                    dbTe.PaId = (int)paId;
                    dbTe.PaxId = item.PaxId;
                    await _context.DbTe.AddAsync(dbTe);
                    await _context.SaveChangesAsync();
                    dbte = dbTe;

                }
                DbExam dbexam = await _context.DbExam.SingleOrDefaultAsync(m => (m.PaId == paId) && (m.Pax1_ID == item.Pax1_ID));

                if (dbqu.Type.Equals("判断"))
                {
                    if (dbqu.RightAnswer.Equals(dbte.EmAnswer))
                    {

                        dbte.RW = "对";
                        dbexam.RW = "对";
                        ++countT;
                    }
                    else
                    {
                        dbte.RW = "错";
                        dbexam.RW = "错";
                    }
                }
                else if (dbqu.Type.Equals("单选"))
                {
                    if (dbqu.RightAnswer.Equals(dbte.EmAnswer))
                    {

                        dbte.RW = "对";
                        dbexam.RW = "对";
                        ++countS;
                    }
                    else
                    {
                        dbte.RW = "错";
                        dbexam.RW = "错";
                    }
                }
                else 
                {
                    if ((dbqu.RightAnswer).Equals(dbte.EmAnswer))
                    {
                        dbte.RW = "对";
                        dbexam.RW = "对";
                        ++countN;
                    }
                    else
                    {
                        dbte.RW = "错";
                        dbexam.RW = "错";
                    }
                }
            }
            int score = (countS + countN * 2+countT)*2;
            ViewBag.htmlStr = score.ToString();
            int useId = ViewBag.UserId;
            var employee = await _context.DbEm.SingleOrDefaultAsync(m => m.EmId == useId);

            DbSc sco = new DbSc();
            sco.EmId = useId;
            sco.Score = score;
            sco.BuId = employee.BuId;
            sco.TestStart = testStart;
            sco.TestEnd = testEnd;
            sco.PaId = paId;
            sco.Correct = countN + countS+countT;
            _context.Add(sco);
            await _context.SaveChangesAsync();


            return View();

            /*if (dbPax == null)
            {
                return NotFound();
            }

            return View(dbPax);*/
        }


        public void Time(string text)
        {
           
            if(text!=null)
            {
                signTime = 1;
                var time = text.Split(":");
                HttpContext.Session.SetString("hour", time[0]);
                HttpContext.Session.SetString("minutes", time[1]);
                HttpContext.Session.SetString("second", time[2]);
            }
            else
            {
                signTime = 0;
            }
            
            
        }
        



        private bool DbPaxExists(int id)
        {
            return _context.DbPax.Any(e => e.PaxId == id);
        }
    }
}
