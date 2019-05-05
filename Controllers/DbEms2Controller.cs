using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WebApplication8.Models;



namespace WebApplication8.Controllers
{
    public class DbEms2Controller : FonourControllerBase
    {      
        private readonly NEUContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private bool EmployeeExists(String  cardid)
        {
            return _context.DbEm.Any(e => e.CardId == (cardid));
        }
        public DbEms2Controller(NEUContext context, IHostingEnvironment hostingEnvironment)
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
                    string  a = (worksheet.Cells[row, 5].Value.ToString());
                    var b= await _context.DbEm.SingleOrDefaultAsync(m => m.CardId == a);
                    if (b==null)
                    {
                        DbEm dbEm = new DbEm();
                        dbEm.EmName = worksheet.Cells[row, 1].Value.ToString();
                        //dbEm.Branch = worksheet.Cells[row, 2].Value.ToString();
                        var aaa = await _context.DbBu.SingleOrDefaultAsync(m => m.BuName == worksheet.Cells[row, 3].Value.ToString());
                        dbEm.BuId = aaa.BuId;
                        var bbb = await _context.DbPo.SingleOrDefaultAsync(m => m.PoName == worksheet.Cells[row, 4].Value.ToString());
                        dbEm.PoId = bbb.PoId;
                        dbEm.CardId = a;
                        dbEm.Password = worksheet.Cells[row, 6].Value.ToString();
                        _context.Add(dbEm);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Index", "DbEms2");
            }
        }
        // GET: DbEms
        public async Task<IActionResult> Index(String NameString, int id = 1)
        {         
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);       
            //var PoName = from d in _context.DbPo where (d.PoId == dbem.EmId) select d;
            if (!dbem.PoId.Equals(4))
            {
               return Redirect("~/Login/Create");
            }
            var movies = from o in _context.DbEm.Include(d => d.Bu).Include(d => d.Po).Include(d => d.Bm)
                         select o;
            if (!String.IsNullOrEmpty(NameString))
            {
                movies = movies.Where(h => h.EmName.Contains(NameString));
            }
            ViewBag.string1 = HttpContext.Session.GetString("string1");
            string string11 = ViewBag.string1;
            if (NameString != null)
            {
                HttpContext.Session.SetString("string1", NameString);
            }
            if (string11 != null)
            {
                ViewBag.string1 = HttpContext.Session.GetString("string1");
                string string2 = ViewBag.string1;
                NameString = string2;
                if (!String.IsNullOrEmpty(NameString))
                {
                    movies = movies.Where(h => h.EmName.Contains(NameString));
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(NameString))
                {
                    movies = movies.Where(h => h.EmName.Contains(NameString));
                }
            }
            var pageOption = new MoPagerOption
            {
                CurrentPage = id,
                PageSize = 20,
                Total = await movies.CountAsync(),
                RouteUrl = "/DbEms2/Index"
            };
           //分页参数
            ViewBag.PagerOption = pageOption;
            return View(await movies.OrderByDescending(b => b.EmId).Skip((pageOption.CurrentPage - 1) * pageOption.PageSize).Take(pageOption.PageSize).ToListAsync());
        }

        // GET: DbEms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
            if (id == null)
            {
                return NotFound();
            }
            var dbEm = await _context.DbEm
                .Include(d => d.Bu)
                .Include(d => d.Po)
                .Include(d => d.Bm)
                .SingleOrDefaultAsync(m => m.EmId == id);
            if (dbEm == null)
            {
                return NotFound();
            }
            return View(dbEm);
        }
        // GET: DbEms/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
            ViewData["BuId"] = new SelectList(_context.DbBu, "BuId", "BuName");
            ViewData["PoId"] = new SelectList(_context.DbPo, "PoId", "PoName");
            ViewData["Branch"] = new SelectList(_context.DbBm, "Id", "Branch");
            return View();
        }
        // POST: DbEms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmId,Password,EmName,Branch,PoId,BuId,CardId,Password1")] DbEm dbEm)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
            if (!ModelState.IsValid)
            {
                _context.Add(dbEm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuId"] = new SelectList(_context.DbBu, "BuId", "BuName", dbEm.BuId);
            ViewData["PoId"] = new SelectList(_context.DbPo, "PoId", "PoName", dbEm.PoId);
            ViewData["Branch"] = new SelectList(_context.DbBm, "Id", "Branch", dbEm.Branch);
            return View(dbEm);
        }



        private string GetFileExt(string fileName)
        {
            string[] array = fileName.Split(".");
            return array[1];
        }


        // GET: DbEms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem = new DbEm();
            int Eid = ViewBag.UserId;
            dbem = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
            if (id == null)
            {
                return NotFound();
            }
            var dbEm = await _context.DbEm.SingleOrDefaultAsync(m => m.EmId == id);
            if (dbEm == null)
            {
                return NotFound();
            }      
            ViewData["BuId"] = new SelectList(_context.DbBu, "BuId", "BuName", dbEm.BuId);
            ViewData["PoId"] = new SelectList(_context.DbPo, "PoId", "PoName", dbEm.PoId);
            ViewData["Branch"] = new SelectList(_context.DbBm, "Id", "Branch", dbEm.Branch);
            return View(dbEm);
        }
        // POST: DbEms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmId,Password,EmName,Branch,PoId,BuId,CardId,Password1")] DbEm dbEm, List<Microsoft.AspNetCore.Http.IFormFile> files)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem1 = new DbEm();
            int Eid = ViewBag.UserId;
            dbem1 = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem1.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
            if (id != dbEm.EmId)
            {
                return NotFound();
            }
            var dbem = await _context.DbEm.SingleOrDefaultAsync(m => m.EmId == id);


            long size = files.Sum(f => f.Length);
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    string fileExt = GetFileExt(formFile.FileName); //文件扩展名，不含“.”
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
                    var filePath = webRootPath + "\\upload\\" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {

                        await formFile.CopyToAsync(stream);
                        dbem.Picture = "/upload/" + newFileName;
                    }
                }
            }



            dbem.EmName = dbEm.EmName;
            dbem.Branch = dbEm.Branch;
            dbem.BuId = dbEm.BuId;
            dbem.PoId = dbEm.PoId;
            dbem.CardId = dbEm.CardId;
            _context.Update(dbem);
            await _context.SaveChangesAsync();






            //long size = files.Sum(f => f.Length);
            //string webRootPath = _hostingEnvironment.WebRootPath;
            //string contentRootPath = _hostingEnvironment.ContentRootPath;
            //foreach (var formFile in files)
            //{
            //    if (formFile.Length > 0)
            //    {

            //        string fileExt = GetFileExt(formFile.FileName); //文件扩展名，不含“.”
            //        long fileSize = formFile.Length; //获得文件大小，以字节为单位
            //        string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
            //        var filePath = webRootPath + "\\upload\\" + newFileName;
            //        using (var stream = new FileStream(filePath, FileMode.Create))
            //        {

            //            await formFile.CopyToAsync(stream);
            //            movie.Picture = "/upload/" + newFileName;
            //        }
            //    }
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(movie);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!MovieExists(movie.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}




            return RedirectToAction(nameof(Index));

        }

        // GET: DbEms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem1 = new DbEm();
            int Eid = ViewBag.UserId;
            dbem1 = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem1.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
            if (id == null)
            {
                return NotFound();
            }
            var dbEm = await _context.DbEm
                .Include(d => d.Bu)
                .Include(d => d.Po)
                .Include(d => d.Bm)
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
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            DbEm dbem1 = new DbEm();
            int Eid = ViewBag.UserId;
            dbem1 = await _context.DbEm.SingleOrDefaultAsync(d => d.EmId == Eid);
            if (!dbem1.PoId.Equals(4))
            {
                return Redirect("~/Login/Create");
            }
            var dbEm = await _context.DbEm.SingleOrDefaultAsync(m => m.EmId == id);
            _context.DbEm.Remove(dbEm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool DbEmExists(int id)
        {
            return _context.DbEm.Any(e => e.EmId == id);
        }
    }
}
