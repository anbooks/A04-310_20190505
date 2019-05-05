using System;
using System.Collections.Generic;
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
    public class Login1Controller : FonourControllerBase
    {
        private readonly NEUContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        public Login1Controller(NEUContext context,IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: Login1
        public async Task<IActionResult> Index(string _input2)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);

            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (dbem.PoId.Equals(2))
            {
                return Redirect("~/Login/Create");
            }
      
            if (_input2.Equals("系统管理"))
            {
                return RedirectToAction("Create", "Jump");
            }
            else if (_input2.Equals("题库管理"))
            {
                return RedirectToAction("Index", "Jump");
            }
            else if (_input2.Equals("部门成绩"))
            {
                return RedirectToAction("Details", "Jump");
            }
          
            else
            {
                return RedirectToAction("Index", "DbEms");
            }
            
        }

        // GET: Login1/Details/5

        public IActionResult Details()
        {
           
            return View();
        }

        // POST: Login1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details([Bind("BuId,BuName")] DbBu dbBu)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);

            if (dbem.PoId.Equals(2))
            {
                return Redirect("~/Login/Create");
            }
            if (ModelState.IsValid)
            {
                _context.Add(dbBu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

     
        // GET: Login1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Login1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuId,BuName")] DbBu dbBu)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);

            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (dbem.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
            if (ModelState.IsValid)
            {
                _context.Add(dbBu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dbBu);
        }
        public IActionResult Delete()
        {
            return View();
        }

        // POST: Login1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("BuId,BuName")] DbBu dbBu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dbBu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dbBu);
        }

        // GET: Login1/Edit/5
        public async Task<IActionResult> Edit()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);

            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (dbem.PoId.Equals(1))
            {
                return Redirect("~/Login1/Delete");
            }
            if (dbem.PoId.Equals(3))
            {
                return Redirect("~/Login1/Details");
            }
            if (dbem.PoId.Equals(4))
            {
                return Redirect("~/Login1/Create");
            }

            if (dbem.PoId.Equals(2))
            {
                return RedirectToAction("Create", "DbEms");
            }
          
            return View();
        }

        // POST: Login1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BuId,BuName")] DbBu dbBu)
        {
            if (id != dbBu.BuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbBu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DbBuExists(dbBu.BuId))
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
            return View(dbBu);
        }

        // GET: Login1/Delete/5
       

        private bool DbBuExists(int id)
        {
            return _context.DbBu.Any(e => e.BuId == id);
        }
    }
}
