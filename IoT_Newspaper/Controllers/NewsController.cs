using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IoT_Newspaper.Data;
using IoT_Newspaper.Model;
using Microsoft.AspNetCore.Authorization;

namespace IoT_Newspaper.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: News
        public async Task<IActionResult> Index(Guid? Id)
        { 
            ViewBag.Section_Id = Id;
            var applicationDbContext = _context.News.Include(n => n.Section).Where(s => s.Section_Id == Id);
            var news = applicationDbContext.ToList();

            return View(news);
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.Section)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public IActionResult Create(Guid? Id)
        {
            ViewData["Section_Id"] = new SelectList(_context.Section, "Id", "Name", Id);
            var news = new News();
            news.Date = DateTime.Now;
            if(Id != null)
                news.Section_Id = Id.Value;

            return View(news);
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Section_Id,Title,Description,Url,Date,Categories,Image,Permanent,Disabled")] News news)
        {
            if (ModelState.IsValid)
            {
                news.Id = Guid.NewGuid();
                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),new { Id = news.Section_Id});
            }
            ViewData["Section_Id"] = new SelectList(_context.Section.OrderBy(s => s.Order), "Id", "Name", news.Section_Id);
            return View(news);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.SingleOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["Section_Id"] = new SelectList(_context.Section.OrderBy(s => s.Order), "Id", "Name", news.Section_Id);
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Section_Id,Title,Description,Url,Date,Categories,Image,Permanent,Disabled")] News news)
        {
            if (id != news.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { news.Section_Id });
            }
            ViewData["Section_Id"] = new SelectList(_context.Section, "Id", "Name", news.Section_Id);
            return View(news);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.Section)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var news = await _context.News.SingleOrDefaultAsync(m => m.Id == id);
            var section = news.Section_Id;
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { section });
        }

        private bool NewsExists(Guid id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }
}
