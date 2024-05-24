using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteManager.Models;
using NoteManager.tools;
using System.Security.Claims;

namespace NoteManager.Controllers
{
    [Route("Images")]

    public class ImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [Route("note-image/{Id}")]
        public async Task<IActionResult> NoteImage(int Id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            var note = await _context.Notes.FirstOrDefaultAsync(m => (m.Id == Id) &&(m.OwnerId==userId));
            if (note == null)
            {
                return NotFound();
            }
            var filePath = FileService.Instance.toAbsolutePath(note.ImagePath);

            return PhysicalFile(filePath, "image/jpeg");
        }

    }
}
