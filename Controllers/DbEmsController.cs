using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


/// <summary>
/// 登录，修改初始密码
/// 
/// </summary>
namespace WebApplication8.Controllers
{
    public class DbEmsController : Controller
    {
        private readonly NEUContext _context;

        private bool EmployeeExists(String cardid)
        {
            return _context.DbEm.Any(e => e.CardId == (cardid));
        }

        public DbEmsController(NEUContext context)
        {
            _context = context;
        }
        public IActionResult Loginout()
        {
            HttpContext.Session.Remove("UserId");

            return RedirectToAction("Create", "DbEms");

        }

        // GET: DbEms
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       

        // GET: DbEms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbEm = await _context.DbEm
                .Include(d => d.Bu)
                .Include(d => d.Po)
                .Include(d=>d.Bm)
                .SingleOrDefaultAsync(m => m.EmId == id);
            if (dbEm == null)
            {
                return NotFound();
            }

            return View(dbEm);
        }

        //登录界面
        // GET: DbEms/Create
        public IActionResult Create()
        {
            return View();
        }

        //提交登录界面信息
        // POST: DbEms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Password,CardId")] DbEm dbEm)
        {

            if (EmployeeExists(dbEm.CardId))
            {

               // var Employee = await _context.DbEm.Include(n => n.Po).Include(n => n.Bm).SingleOrDefaultAsync(m => m.CardId == (dbEm.CardId));

                var Employee = await _context.DbEm.Include(n => n.Po).Include(n => n.Bm).SingleOrDefaultAsync(m => m.CardId == (dbEm.CardId));
                //var Employee = await _context.DbEm.Include(n => n.Po).SingleOrDefaultAsync(m => m.CardId == (dbEm.CardId));
                if (Employee == null)
                {

                    return NotFound();
                }
                HttpContext.Session.SetInt32("UserId", Employee.EmId);
                //      HttpContext.Session.SetString("UserId", Employee.Po.PoName);
                if (Employee.Password == dbEm.Password)
                {
                    if (dbEm.Password == "123456")
                    {
                        Employee.Password = "";
                        Employee.EmName = "";
                        Employee.BuId = 0;
                        //Employee.Branch = "";
                        Employee.CardId = "0";
                        Employee.PoId = 0;

                        return RedirectToAction(nameof(Edit), Employee);
                    }
                    if (Employee.Po.PoName == "试卷管理员")
                    {

                        return Redirect("~/DbQus/Index");
                    }
                    if (Employee.Po.PoName == "系统管理员")
                    {
                        return Redirect("~/DbEms2/Index");
                    }
                    if (Employee.Po.PoName == "职工")
                    {
                        return Redirect("~/DbEms/Start");
                    }
                    if (Employee.Po.PoName == "上级主管")
                    {
                        return Redirect("~/TeScs/Details");
                    }
                    return Redirect("~/Login/Create");
                }
            }
            return View();
        }




        //修改密码界面
        // GET: DbEms/Edit/5
        public IActionResult Edit(int? id)
        {

            return View();
        }

        //提交修改密码页面信息，更新数据库
        // POST: DbEms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Password_Old, [Bind("CardId,Password")] DbEm dbEm,string Password,string Password1)
        {

            //ViewBag.UserId = HttpContext.Session.GetString("UserId");
            //dbEm.EmId = ViewBag.UserId;
            //if (dbEm.EmId == 0)
            //{
            //    return NotFound();
            //}
            string test = dbEm.CardId;
            if (!ModelState.IsValid)
            {
                try
                {
                    if (Password == Password1)
                    {
                        var Employee = await _context.DbEm.SingleOrDefaultAsync(m => m.CardId == (dbEm.CardId));   //获得人员对象

                        if (Employee.Password.Equals(Password_Old))    //对象的密码字段与前端输入比较
                        {
                            Employee.Password = dbEm.Password;       //将这个密码输入到对象中 
                                                                     //Employee.EmName = dbEm.EmName;
                                                                     //Employee.Branch = dbEm.Branch;
                                                                     //Employee.BuId
                                                                     //职位

                            _context.Update(Employee);
                            await _context.SaveChangesAsync();
                            //return RedirectToAction(nameof(Index), Employee);
                        }
                        else
                        {
                            ModelState.AddModelError("", "旧密码输入错误");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "更改的密码前后不一致");
                    }


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DbEmExists(dbEm.EmId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }


            }
            ViewData["BuId"] = new SelectList(_context.DbBu, "BuId", "BuName", dbEm.BuId);
            ViewData["PoId"] = new SelectList(_context.DbPo, "PoId", "PoName", dbEm.PoId);
            ViewData["Branch"] = new SelectList(_context.DbBm, "Id", "Branch", dbEm.Branch);
            HttpContext.Session.Remove("UserId");

            return RedirectToAction("Create", "DbEms");
           // return View();
        }


        // GET: DbEms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbEm = await _context.DbEm
                .Include(d => d.Bu)
                .Include(d => d.Po)
                .SingleOrDefaultAsync(m => m.EmId == id);
            if (dbEm == null)
            {
                return NotFound();
            }

            return View(dbEm);
        }

        // POST: DbEms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dbEm = await _context.DbEm.SingleOrDefaultAsync(m => m.EmId == id);
            _context.DbEm.Remove(dbEm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Start()
        {
            IQueryable<string> pro = from m in _context.DbSu1 orderby m.SuaName select m.SuaName;
            ViewBag.first = new SelectList(await pro.ToListAsync());
            var i=HttpContext.Session.GetInt32("Sign");

            if (i==null)
            {               
                return View();
            }
            else
            {
                ViewData["ErrorId"] = i;
                //ModelState.AddModelError("NumError", "该专业下试题数量不足，请选择其他专业");
                
                return View();
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Start(string sel_1, string sel_2, string sel_3)
        {

            //if (!sel_3.Equals("请选择"))
            //{
            //    var a = await _context.DbSu3.SingleOrDefaultAsync(m => m.SucName.Equals(sel_3));
            //    HttpContext.Session.SetInt32("selId_3", a.SucId);
            //    HttpContext.Session.SetString("select3", sel_3);
            //    return RedirectToAction("Index", "DbPaxes");
            //}
            //else if (sel_2 != null)
            //{
            //    HttpContext.Session.SetString("select2", sel_2);
            //    return RedirectToAction("Index", "DbPaxes");
            //}
            //else if (sel_1 != null)
            //{
            //    HttpContext.Session.SetString("select1", sel_1);
            //    return RedirectToAction("Index", "DbPaxes");      //只选择部门后进行模拟题
            //}
            //else
            //{
            //    return View();
            //}

            //if (!sel_3.Equals("请选择"))
            //{
            //    var a = await _context.DbSu3.SingleOrDefaultAsync(m => m.SucName.Equals(sel_3));
            //    HttpContext.Session.SetInt32("selId_3", a.SucId);
            //    HttpContext.Session.SetString("select3", sel_3);
            //    return RedirectToAction("Index", "DbPaxes");
            //}
            //else 
            //if (sel_2 != null)
            //{
            //    HttpContext.Session.SetString("select2", sel_2);
            //    return RedirectToAction("Index", "DbPaxes");
            //}
            //else
            if (sel_1 != null)
            {
                HttpContext.Session.SetString("select1", sel_1);
                return RedirectToAction("Index", "DbPaxes");      //只选择部门后进行模拟题
            }
            else
            {
                return View();
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


        private bool DbEmExists(int id)
        {
            return _context.DbEm.Any(e => e.EmId == id);
        }
    }
}