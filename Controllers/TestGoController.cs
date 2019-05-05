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
    public class TestGoController : Controller
    {
        private readonly NEUContext _context;

        public TestGoController(NEUContext context)
        {
            _context = context;
        }
        public ActionResult Count()
        {
           // var co= await _context.Test.SingleOrDefaultAsync(m => m.ShenpiFlag == 0);
            int cou = 0;
            ViewBag.count = cou;
            return View();
        }
        // GET: TestGo
        public async Task<IActionResult> Index()
        {
            //var test = await _context.Test.SingleOrDefaultAsync(m => m.ShenpiFlag ==1 );
            return View(await _context.Test.Where(m => m.ShenpiFlag == 1).ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View(await _context.Test.Where(m => m.ShenpiFlag == 0).ToListAsync());
        }
        // GET: TestGo/Details/5
        public async Task<IActionResult> Details()
        {
            return View(await _context.Test.ToListAsync());
        }
        // GET: TestGo/Create
       
        // GET: TestGo/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var test = await _context.Test.SingleOrDefaultAsync(m => m.TestId == id);
            test.ShenpiFlag = 1;
            _context.Update(test);
            await _context.SaveChangesAsync();
            return Redirect("~/TestGo/Create");
        }

        // GET: TestGo/Delete/5
        public async Task<IActionResult> Delete()
        {
            return View(await _context.Test.ToListAsync());
        }

        // POST: TestGo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var test = await _context.Test.SingleOrDefaultAsync(m => m.TestId == id);
            _context.Test.Remove(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestExists(int id)
        {
            return _context.Test.Any(e => e.TestId == id);
        }
    }
}
