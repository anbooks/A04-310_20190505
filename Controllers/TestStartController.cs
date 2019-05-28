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
    public class TestStartController : Controller
    {
        private readonly NEUContext _context;
        //private int singleCho = 10;//单选题数量
        //private int multipleCho = 0;//多选题数量
        private static DateTime testStart;
        private static DateTime testEnd;




        public TestStartController(NEUContext context)
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
        public async Task<IActionResult> Index(int? id)
        {

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            int uid = ViewBag.UserId;
            if (ViewBag.UserId == null)
            {
                return RedirectToAction("Create", "DbEms");
            }
            var sel = from m in _context.TestAn where ((m.TestId == id) && (m.EmId == uid)) select m;
            int sal = sel.Count();

            var num = await _context.Test.SingleOrDefaultAsync(m => (m.TestId == id));
            if (sal!=0)
            {
                return RedirectToAction("Index", "TestGo");
            }
            var qu = from d in _context.TestQu.Include(d => d.Qu) where (d.TestId == id) select d;
            // from m in _context.TestQu where (m.TestId==id) select m
            //从抽出的该专业的题中抽取不同难度的单选题和多选题   
            int j = 0;

            var single_Choice = from d in qu where (d.Qu.Type.Equals("单选")) select d;
            // int m = single_Choice.Count();
            var multiple_Choice = from d in qu where (d.Qu.Type.Equals("多选")) select d;
            var tureOrFalse_Choice = from d in qu where (d.Qu.Type.Equals("判断")) select d;
            List<int> keys = new List<int>();
            List<int> value = new List<int>();
            var hardS = from d in single_Choice select d;
            for (int i = 0; i < num.Dan;)
            {
                TestAn dbPax = new TestAn();
                Random s = new Random();
                var cc = hardS.ToList();
                int b = s.Next(0, hardS.Count());
                if (!QuestionExists(cc[b].Qu.QuId, value))//试卷中不存在新抽取的题，将其加入试卷
                {
                    j++;
                    pax_list.Add(j, cc[b].Qu.QuId);
                    keys.Add(j);
                    value.Add(cc[b].Qu.QuId);
                    ++i;
                }
            }
            var hardM = from d in multiple_Choice select d;
            for (int i = 0; i < num.Duo;)
            {
                TestAn dbPax = new TestAn();
                Random s = new Random();
                var cc = hardM.ToList();

                int b = s.Next(0, hardM.Count());
                if (!QuestionExists(cc[b].Qu.QuId, value))
                {
                    j++;

                    pax_list.Add(j, cc[b].Qu.QuId);
                    keys.Add(j);
                    value.Add(cc[b].Qu.QuId);

                    ++i;
                }
            }
            var hardT = from d in tureOrFalse_Choice select d;
            for (int i = 0; i < num.Pan;)
            {
                TestAn dbPax = new TestAn();
                Random s = new Random();
                var cc = hardT.ToList();

                int b = s.Next(0, hardT.Count());
                if (!QuestionExists(cc[b].Qu.QuId, value))
                {
                    j++;

                    pax_list.Add(j, cc[b].Qu.QuId);
                    keys.Add(j);
                    value.Add(cc[b].Qu.QuId);

                    ++i;
                }
            }
            var selte = await _context.Test.SingleOrDefaultAsync(d => d.TestId == id);
            int PaperId = selte.TestId;
            HttpContext.Session.SetInt32("paperId", PaperId);

            foreach (var item in keys)
            {
                TestAn dbPax = new TestAn();
                dbPax.TestaId = item;
                dbPax.QuId = pax_list[item];
                dbPax.TestId = PaperId;
                dbPax.EmId = ViewBag.UserId;
                _context.Add(dbPax);

            }
            await _context.SaveChangesAsync();
            testStart = DateTime.Now;

            var paContext = from d in _context.TestAn where (d.TestId == PaperId && d.EmId == uid) select d;
            var nEUContext = paContext.OrderBy(d => d.TestaId).Include(d => d.Test).Include(d => d.Qu);
            return View(await nEUContext.ToListAsync());
        }

        /// <summary>
        /// 返回题目列表
        /// </summary>
        /// <returns></returns>
        // GET: DbPaxes/Details/5
        public async Task<IActionResult> Details()
        {
            ViewBag.paperId = HttpContext.Session.GetInt32("paperId");
            var Eid = HttpContext.Session.GetInt32("UserId");
            int paId = ViewBag.paperId;
            var dbPax = from d in _context.TestAn.OrderBy(d => d.TestaId) where (d.TestId == paId && d.EmId == Eid) select d;

            if (dbPax == null)
            {
                return NotFound();
            }
            var dbpax = dbPax.Include(d => d.Qu);

            List<QuestionTeans> quAns_list = new List<QuestionTeans>();
            foreach (var item in dbpax)
            {
                QuestionTeans quAns = new QuestionTeans();
                quAns.Pax = item;
                int i = item.TestaId;
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


        // GET: DbPaxes/Create
        public async Task<IActionResult> Create(int? id)
        {
            var paperId = HttpContext.Session.GetInt32("paperId");
            var Eid = HttpContext.Session.GetInt32("UserId");
            if (id == null)
            {
                return NotFound();
            }

            var pax = _context.TestAn.Include(m => m.Qu);
            var dbPax = pax.SingleOrDefault(m => (m.TestId == paperId) && (m.TestaId == id) && (m.EmId == Eid));

            if (dbPax == null)
            {
                return NotFound();
            }
            int i = dbPax.TestaId;
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
        public async Task<IActionResult> Create(int id, string edit)
        {
            var Eid = HttpContext.Session.GetInt32("UserId");
            var paperId = HttpContext.Session.GetInt32("paperId");
            TestAn dbPax = new TestAn();
            dbPax = await _context.TestAn.SingleOrDefaultAsync(m => (m.TestId == paperId) && (m.TestaId == id) && (m.EmId == Eid));
            string stores = Request.Form["checkBox"];
            var num = await _context.Test.SingleOrDefaultAsync(m => (m.TestId == paperId));
            int cou = Convert.ToInt32(num.Dan + num.Duo+num.Pan);
            ViewBag.htmlStr = cou.ToString();
            if (stores == null)
            {
                ModelState.AddModelError("", "请作答，选项不能为空");//不显示
                return RedirectToAction("Create", "TestStart", id);
            }

            if (id != dbPax.TestaId)
            {
                return NotFound();
            }

            if (edit.Equals("上一题"))
            {
                if (dbPax.TestaId != 1)
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
                    return RedirectToRoute(new { Controller = "TestStart", Action = "Edit", id });
                }
                else { return View(); }
            }
            else
            {
                if (dbPax.TestaId < (num.Dan + num.Duo))
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

                    return RedirectToRoute(new { Controller = "TestStart", Action = "Edit", id });
                }
                else if (dbPax.TestaId == (num.Dan + num.Duo))
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
                    var paper_list = from d in _context.TestAn where (d.TestId == paperId && d.EmId == Eid) select d;

                    foreach (var item in ans_list)
                    {
                        var ti = await paper_list.SingleOrDefaultAsync(d => d.TestaId == item.Key && d.EmId == Eid);
                        TeTe te = new TeTe();
                        te.PaId = (int)paperId;
                        te.PaxId = ti.TequId;
                        te.EmAnswer = item.Value;
                        _context.Add(te);
                        TestAn ex = new TestAn();
                        ex = await _context.TestAn.SingleOrDefaultAsync(d => (d.TestId == paperId) && (d.TestaId == item.Key) && (d.EmId == Eid));
                        if (ex == null)
                        {
                            return NotFound();
                        }
                        ex.TeAn = item.Value;
                        _context.Update(ex);
                    }
                    await _context.SaveChangesAsync();

                    return RedirectToRoute(new { Controller = "TestStart", Action = "Delete", paperId });

                }
                else return NotFound();
            }

        }

        // GET: DbPaxes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var paId = HttpContext.Session.GetInt32("paperId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            int uid = ViewBag.UserId;
            if (id == 0)
            {
                return NotFound();
            }

            TestAn pax = new TestAn();
            pax = _context.TestAn.SingleOrDefault(m => (m.TestId == paId) && (m.TestaId == id) && (m.EmId == uid));
            var paperQu = _context.DbQu.SingleOrDefault(m => m.QuId == pax.QuId);
            if (paperQu == null)
            {
                return NotFound();
            }
            else
            {
                if (id <= 30)
                {
                    if (paperQu.Type.Equals("多选"))
                    {
                        return RedirectToRoute(new { Controller = "TestStart", Action = "Create", id });
                    }
                    else
                    {
                        int i = pax.TestaId;
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

                    return RedirectToRoute(new { Controller = "TestStart", Action = "Delete", paId });
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
        public async Task<IActionResult> Edit(int id, string edit, string Option)
        {

            var paperId = HttpContext.Session.GetInt32("paperId");
            var Eid = HttpContext.Session.GetInt32("UserId");
            TestAn dbPax = new TestAn();
            dbPax = await _context.TestAn.SingleOrDefaultAsync(m => (m.TestId == paperId) && (m.TestaId == id) && (m.EmId == Eid));
            var num = await _context.Test.SingleOrDefaultAsync(m => (m.TestId == paperId));
            if (Option == null)
            {
                ModelState.AddModelError("", "请作答，选项不能为空");//不显示
                                                           //return View();
                return RedirectToAction("Edit", "TestStart", id);
            }


            if (edit.Equals("上一题"))
            {
                if (dbPax.TestaId != 1)
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

                    return RedirectToRoute(new { Controller = "TestStart", Action = "Edit", id });
                }
                else return View();
            }
            else
            {
                if (dbPax.TestaId <= num.Dan)
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
                    if (dbPax.TestaId == num.Dan)
                    {
                        return RedirectToRoute(new { Controller = "TestStart", Action = "Create", id });
                    }
                    testEnd = DateTime.Now;

                }
                return RedirectToRoute(new { Controller = "TestStart", Action = "Edit", id });

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
            int uid = ViewBag.UserId;
            var paper = from d in _context.TestAn where (d.TestId == paId) select d;
            var dbpax = await paper.ToListAsync();
            int countS = 0, countN = 0,countP = 0;
            foreach (var item in dbpax)
            {
                TestAn dbexam = await _context.TestAn.Include(m => m.Qu).SingleOrDefaultAsync(m => (m.TestId == paId) && (m.TestaId == item.TestaId) && (m.EmId == uid));

                if (dbexam.Qu.Type.Equals("单选"))
                {
                    if (dbexam.Qu.RightAnswer.Equals(dbexam.TeAn))
                    {
                        dbexam.RW = "对";
                        ++countS;
                    }
                    else
                    {
                        dbexam.RW = "错";
                    }
                }
                if (dbexam.Qu.Type.Equals("多选"))
                {
                    if (dbexam.Qu.RightAnswer.Equals(dbexam.TeAn))
                    {
                        dbexam.RW = "对";
                        ++countS;
                    }
                    else
                    {
                        dbexam.RW = "错";
                    }
                }
                else
                {
                    if ((dbexam.Qu.RightAnswer).Equals(dbexam.TeAn))
                    {
                        dbexam.RW = "对";
                        ++countP;
                    }
                    else
                    {
                        dbexam.RW = "错";
                    }
                }
            }
            var sc = await _context.Test.SingleOrDefaultAsync(m => m.TestId == paId);
            int dan = Convert.ToInt32(sc.DanScore);
            int duo = Convert.ToInt32(sc.DuoScore);
            int pan = Convert.ToInt32(sc.PanScore);
            float score = countS * duo + countN * dan+countP*pan;
            ViewBag.htmlStr = score.ToString();
            int useId = ViewBag.UserId;
            var employee = await _context.DbEm.SingleOrDefaultAsync(m => m.EmId == useId);

            TeSc sco = new TeSc();
            sco.EmId = useId;
            sco.Score = score;
            sco.BuId = employee.BuId;
            sco.TestStart = testStart;
            sco.TestEnd = testEnd;
            sco.PaId = paId;
            sco.Correct = countN + countS;
            _context.Add(sco);
            await _context.SaveChangesAsync();


            return View();

            /*if (dbPax == null)
            {
                return NotFound();
            }

            return View(dbPax);*/
        }





        private bool TestStartExists(int id)
        {
            return _context.TestQu.Any(e => e.TequId == id);
        }
    }
}

