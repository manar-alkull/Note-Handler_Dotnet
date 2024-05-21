using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NoteManager.Models;
using NoteManager.Models.ViewModels;

namespace NoteManager.Controllers
{
    //[Route("PublicLinks")]
    [Authorize]
    public class PublicLinksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicLinksController(ApplicationDbContext context)
        {
            _context = context;
        }

        string creataLink(int noteId)
        {
            //string linkStr = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/";
            string linkStr = $"{this.Request.Scheme}://{this.Request.Host}/PublicLinks/NoteDetails/";
            linkStr += noteId + "";
            return linkStr;
        }


        // GET: PublicLinks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PublicLink.Include(p => p.NoteObj);
            
            var x=await applicationDbContext.Include(p=>p.NoteObj).Select((l) => new PublicLinkIndexModel {linkId=l.Id, link = "", noteTitle=l.NoteObj.Title }).ToListAsync();
            foreach (var item in x)
            {
                item.link = creataLink((int)item.linkId);
            }
            return View(x); 

        }

        // GET: PublicLinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicLink = await _context.PublicLink
                .Include(p => p.NoteObj)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publicLink == null)
            {
                return NotFound();
            }

            ViewData["linkStr"] =creataLink((int)id);

            return View();
        }

        // GET: PublicLinks/Create/id
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Note Id</param>
        /// <returns></returns>
        public async Task<IActionResult> Create(int? id)
        {
            if(id == null)
                return BadRequest("note id is required");
            var link = new PublicLink() { Note = (int)id };
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<PublicLink> x = _context.Add(link);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = link.Id });
        }



        // GET: PublicLinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicLinks = await _context.PublicLink
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publicLinks == null)
            {
                return NotFound();
            }

            return View(publicLinks);
        }


        // POST: PublicLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publicLink = await _context.PublicLink.FindAsync(id);
            if (publicLink != null)
            {
                _context.PublicLink.Remove(publicLink);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: PublicLinks/NoteDetails/5
        [AllowAnonymous]
        public async Task<IActionResult> NoteDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicLink = await _context.PublicLink
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publicLink == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == publicLink.Note);
            if (note == null)
            {
                return NotFound();
            }

            return View("NoteDetails", note);
        }



        private bool PublicLinkExists(int id)
        {
            return _context.PublicLink.Any(e => e.Id == id);
        }
    }
}
