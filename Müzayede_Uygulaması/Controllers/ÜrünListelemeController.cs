using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Müzayede_Uygulaması.Data;
using Müzayede_Uygulaması.Models;

namespace Müzayede_Uygulaması.Controllers
{
    public class ÜrünListelemeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public ÜrünListelemeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }



        // GET: ÜrünListeleme
        public async Task<IActionResult> Index(int sayfa, string Ara)
        {
            var applicationDbContext = _context.ÜrünListelemes.Include(ü => ü.User);
            if (!string.IsNullOrEmpty(Ara))
            {
                applicationDbContext = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<ÜrünListeleme, IdentityUser?>)applicationDbContext.Where(x => x.Başlık.Contains(Ara));
            }
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ÜrünListeleme/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ÜrünListelemes == null)
            {
                return NotFound();
            }
            ViewBag.user = userManager.GetUserId(User);
            var ürünListeleme = await _context.ÜrünListelemes
                .Include(ü => ü.User)
                .Include(ü => ü.Teklifs)
                .Include(ü => ü.Yorums)
                .ThenInclude(ü => ü.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ürünListeleme == null)
            {
                return NotFound();
            }

            return View(ürünListeleme);
        }
        //[HttpPost]
        //public async Task<IActionResult> Details([Bind("Id,Fiyat,IdentityUserId,ÜrünListelemeId")] Teklif model)
        //{


        //    var teklif = new Teklif
        //    {

        //        Fiyat = model.Fiyat,
        //        ÜrünListelemeId = model.Id,
        //        IdentityUserId = userManager.GetUserId(User)
        //    };
        //    _context.Teklifs.Add(teklif);
        //   await _context.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}
        // GET: ÜrünListeleme/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.user = userManager.GetUserId(User);
            //ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ÜrünListeleme/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ÜrünListelemeViewModel listelemeViewModel)
        {
            if (ModelState.IsValid)
            {
                string resimYolu = null;

                if (listelemeViewModel.Resim != null)
                {
                    var filePath = Path.Combine("wwwroot/images", listelemeViewModel.Resim.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await listelemeViewModel.Resim.CopyToAsync(stream);
                    }

                    resimYolu = "/images/" + listelemeViewModel.Resim.FileName;
                }

                var ürünListeleme = new ÜrünListeleme
                {
                    Başlık = listelemeViewModel.Başlık,
                    Açıklama = listelemeViewModel.Açıklama,
                    Fiyat = listelemeViewModel.Fiyat,
                    ResimYolu = resimYolu,
                    IdentityUserId = userManager.GetUserId(User)
                };

                _context.ÜrünListelemes.Add(ürünListeleme);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", listelemeViewModel.IdentityUserId);
            return View(listelemeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> TeklifVer([Bind("Id", "Fiyat", "IdentityUserId", "ÜrünListelemeId")] Teklif teklif)
        {

            if (ModelState.IsValid)
            {
                if(teklif.Fiyat != null)
                {
                    _context.Teklifs.Add(teklif);
                    await _context.SaveChangesAsync();
                }
                
            }

            var liste = await _context.ÜrünListelemes
                .Include(ü => ü.User)
                .Include(ü => ü.Teklifs)
                .Include(ü => ü.Yorums)
                .ThenInclude(ü => ü.User)
                .FirstOrDefaultAsync(m => m.Id == teklif.ÜrünListelemeId);



            return View("Details", liste);


        }


        public async Task<IActionResult> TeklifiKapat(int id)
        {
            //var urun = await _context.ÜrünListelemes
            //    .Include(ü => ü.User)
            //    .Include(ü => ü.Teklifs)
            //    .Include(ü => ü.Yorums)
            //    .ThenInclude(ü => ü.User)
            //    .FirstOrDefaultAsync(m => m.Id == id);

            var urun = await _context.ÜrünListelemes
        .Include(ü => ü.User)
        .Include(ü => ü.Teklifs)
        .ThenInclude(t => t.User)
        .Include(ü => ü.Yorums)
        .ThenInclude(y => y.User)
        .FirstOrDefaultAsync(m => m.Id == id);

            if (urun == null)
            {
                return NotFound();
            }
            urun.Satıldımı = true;
            _context.ÜrünListelemes.Update(urun);
            await _context.SaveChangesAsync();


            if (urun.Satıldımı == true)
            {
                var yuksekTeklif = urun.Teklifs.OrderByDescending(x => x.Fiyat).FirstOrDefault();

                if (yuksekTeklif != null)
                {
                    var user = userManager.GetUserId(User);

                    ViewBag.KazananUserName = yuksekTeklif.User.UserName;
                    ViewBag.KazananTeklif = yuksekTeklif.Fiyat;
                    ViewBag.KazananUserId = yuksekTeklif.IdentityUserId;
                    ViewBag.Kazandımı = yuksekTeklif.IdentityUserId == user;
                }
                else
                {
                    ViewBag.KazananUserName = "Kullanıcı bulunamadı";
                    ViewBag.KazananTeklif = 0;
                    ViewBag.KazananUserId = null;
                    ViewBag.Kazandım = false;
                }
            }

            return View("Details", urun);
        }

        public async  Task<IActionResult> YorumEkle([Bind("Id","İçerik", "IdentityUserId", "ÜrünListelemeId")] Yorum yorum)
        {
            if (ModelState.IsValid)
            {
                if(yorum.İçerik != null)
                {
                    _context.Yorums.Add(yorum);
                    await _context.SaveChangesAsync();

                }
 
            }
            var urun = await _context.ÜrünListelemes
        .Include(ü => ü.User)
        .Include(ü => ü.Teklifs)
        .ThenInclude(t => t.User)
        .Include(ü => ü.Yorums)
        .ThenInclude(y => y.User)
        .FirstOrDefaultAsync(m => m.Id == yorum.ÜrünListelemeId);
            return View("Details",urun);
        }

        public async Task<IActionResult> Listem(int id)
        {
           var liste=await _context.ÜrünListelemes.Include(l=>l.User).ToListAsync();   
            return View(liste);
        } 
        public  async Task<IActionResult> Tekliflerim()
        {
            var liste = await _context.Teklifs.Include(l => l.ÜrünListeleme).ThenInclude(l=>l.User).ToListAsync();
            return View(liste);
        }
        // GET: ÜrünListeleme/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ÜrünListelemes == null)
            {
                return NotFound();
            }

            var ürünListeleme = await _context.ÜrünListelemes.FindAsync(id);
            if (ürünListeleme == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", ürünListeleme.IdentityUserId);
            return View(ürünListeleme);
        }

        // POST: ÜrünListeleme/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Başlık,Açıklama,Fiyat,ResimYolu,Satıldımı,IdentityUserId")] ÜrünListeleme ürünListeleme)
        {
            if (id != ürünListeleme.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ürünListeleme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ÜrünListelemeExists(ürünListeleme.Id))
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
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", ürünListeleme.IdentityUserId);
            return View(ürünListeleme);
        }

        // GET: ÜrünListeleme/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ÜrünListelemes == null)
            {
                return NotFound();
            }

            var ürünListeleme = await _context.ÜrünListelemes
                .Include(ü => ü.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ürünListeleme == null)
            {
                return NotFound();
            }

            return View(ürünListeleme);
        }

        // POST: ÜrünListeleme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ÜrünListelemes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ÜrünListelemes'  is null.");
            }
            var ürünListeleme = await _context.ÜrünListelemes.FindAsync(id);
            if (ürünListeleme != null)
            {
                _context.ÜrünListelemes.Remove(ürünListeleme);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ÜrünListelemeExists(int id)
        {
            return (_context.ÜrünListelemes?.Any(e => e.Id == id)).GetValueOrDefault();
        }




    }
}
