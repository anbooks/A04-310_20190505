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
    public class DbPas1Controller : FonourControllerBase
    {
        private readonly NEUContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
       
        public DbPas1Controller(NEUContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost]
        public async Task<IActionResult> Import(IFormFile excelfile)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = $"{Guid.NewGuid()}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            try
            {
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
                        dbQu.Type = worksheet.Cells[row,2].Value.ToString();
                        var aaa = await _context.DbBu.SingleOrDefaultAsync(m => m.BuName== worksheet.Cells[row, 3].Value.ToString());
                        dbQu.BuId = aaa.BuId; 
                        dbQu.Question = worksheet.Cells[row, 4].Value.ToString();
                        dbQu.OptionA = worksheet.Cells[row, 5].Value.ToString();
                        dbQu.OptionB = worksheet.Cells[row, 6].Value.ToString();
                        dbQu.OptionC = worksheet.Cells[row, 7].Value.ToString();
                        dbQu.OptionD = worksheet.Cells[row, 8].Value.ToString();
                        dbQu.RightAnswer = worksheet.Cells[row, 9].Value.ToString();
                        dbQu.Difficulty = worksheet.Cells[row, 10].Value.ToString();
                        var bbb = await _context.DbSu1.SingleOrDefaultAsync(m => m.SuaName == worksheet.Cells[row, 11].Value.ToString());
                        dbQu.SuaId = bbb.SuaId;
                        var ccc = await _context.DbSu2.SingleOrDefaultAsync(m => m.SubName == worksheet.Cells[row, 12].Value.ToString());
                        dbQu.SubId = ccc.SubId;
                        var ddd = await _context.DbSu3.SingleOrDefaultAsync(m => m.SucName == worksheet.Cells[row, 13].Value.ToString());
                        dbQu.SucId = ddd.SucId;                           
                        var eee = await _context.DbEm.SingleOrDefaultAsync(m => m.EmName == worksheet.Cells[row, 14].Value.ToString());
                        dbQu.EmId = eee.EmId;
                        _context.Add(dbQu);
                        await _context.SaveChangesAsync();


                    }
                  
                   
                    return Content(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        // GET: DbPas
        public async Task<IActionResult> Index()
        {
            var nEUContext = _context.DbPa.Include(d => d.Em);
            return View(await nEUContext.ToListAsync());
        }

        // GET: DbPas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbPa = await _context.DbPa
                .Include(d => d.Em)
                .SingleOrDefaultAsync(m => m.PaId == id);
            if (dbPa == null)
            {
                return NotFound();
            }

            return View(dbPa);
        }

        // GET: DbPas/Create
        public IActionResult Create()
        {
            ViewData["EmId"] = new SelectList(_context.DbEm, "EmId", "EmId");
            return View();
        }

        // POST: DbPas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaId,EmId")] DbPa dbPa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dbPa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmId"] = new SelectList(_context.DbEm, "EmId", "EmId", dbPa.EmId);
            return View(dbPa);
        }

        // GET: DbPas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbPa = await _context.DbPa.SingleOrDefaultAsync(m => m.PaId == id);
            if (dbPa == null)
            {
                return NotFound();
            }
            ViewData["EmId"] = new SelectList(_context.DbEm, "EmId", "EmId", dbPa.EmId);
            return View(dbPa);
        }

        // POST: DbPas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaId,EmId")] DbPa dbPa)
        {
            if (id != dbPa.PaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbPa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DbPaExists(dbPa.PaId))
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
            ViewData["EmId"] = new SelectList(_context.DbEm, "EmId", "EmId", dbPa.EmId);
            return View(dbPa);
        }

        // GET: DbPas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbPa = await _context.DbPa
                .Include(d => d.Em)
                .SingleOrDefaultAsync(m => m.PaId == id);
            if (dbPa == null)
            {
                return NotFound();
            }

            return View(dbPa);
        }

        // POST: DbPas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dbPa = await _context.DbPa.SingleOrDefaultAsync(m => m.PaId == id);
            _context.DbPa.Remove(dbPa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DbPaExists(int id)
        {
            return _context.DbPa.Any(e => e.PaId == id);
        }
    }
}
