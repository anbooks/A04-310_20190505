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
    public class LoginController : FonourControllerBase
    {
        private readonly NEUContext _context;

        public LoginController(NEUContext context)
        {
            _context = context;
        }

        // GET: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string _input1)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
           
            if (_input1.Equals("后台管理"))
            {
                return RedirectToAction("Create", "Login1");
            }
           
            else
            {
                return RedirectToAction("Index", "DbEms");
            }

            //var nEUContext = _context.DbEm.Include(d => d.Bu).Include(d => d.Po);
            //return View(await nEUContext.ToListAsync());
        }
        // GET: Login/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbBu = await _context.DbBu
                .SingleOrDefaultAsync(m => m.BuId == id);
            if (dbBu == null)
            {
                return NotFound();
            }

            return View(dbBu);
        }

        // GET: Login/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuId,BuName")] DbBu dbBu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dbBu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dbBu);
        }

        // GET: Login/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbBu = await _context.DbBu.SingleOrDefaultAsync(m => m.BuId == id);
            if (dbBu == null)
            {
                return NotFound();
            }
            return View(dbBu);
        }

        // POST: Login/Edit/5
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
       

        // POST: Login/Delete/5
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
