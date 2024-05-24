using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NoteManager.Models;
using NoteManager.Models.Requests;
using NoteManager.tools;

namespace NoteManager.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Notes
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            return View(await _context.Notes.Where((note)=>note.OwnerId== userId).ToListAsync());
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( NoteRequest noteReq)
        {
            Note note = new Note();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            note.OwnerId = userId;
            if (ModelState.IsValid)
            {
                note.Title=noteReq.Title;
                note.Content=noteReq.Content;
                if(noteReq.AttachedImage!=null)
                    note.ImagePath = FileService.Instance.SaveFile(@"uploads\notes-images", noteReq.AttachedImage);
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(note);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            return View(new NoteRequest() { Id=note.Id, Content=note.Content, Title=note.Title});
        }

        // POST: Notes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NoteRequest noteReq)
        {
            Note note = await _context.Notes.FindAsync(id);

            if (note ==null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            note.OwnerId = userId;

            if (ModelState.IsValid)
            {
                try
                {
                    if (noteReq.AttachedImage != null)
                    {

                        string? oldPath = note.ImagePath;
                        note.ImagePath = FileService.Instance.SaveFile(@"uploads\notes-images", noteReq.AttachedImage);
                        FileService.Instance.DeleteFileAsync(oldPath);
                    }

                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
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
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            string? imgPath=null;
            if (note != null)
            {
                imgPath = note.ImagePath;
                _context.Notes.Remove(note);
            }

            await _context.SaveChangesAsync();
            if(imgPath != null)
                FileService.Instance.DeleteFileAsync(imgPath);

            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}
