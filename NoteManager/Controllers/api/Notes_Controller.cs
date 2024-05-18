using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteManager.Models;
using NoteManager.Models.Responses;

namespace NoteManager.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class Notes_Controller : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public Notes_Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Notes_
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteResponseModel>>> GetNotes()
        {
            return await _context.Notes.Select((note) => new NoteResponseModel (note)).ToListAsync();
        }

        // GET: api/Notes_/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NoteResponseModel>> GetNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            return new NoteResponseModel(note);
        }

        // PUT: api/Notes_/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote(int id, Note note)
        {
            if (id != note.Id)
            {
                return BadRequest();
            }

            _context.Entry(note).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Notes_
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NoteResponseModel>> PostNote(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { id = note.Id }, new NoteResponseModel(note));
        }

        // DELETE: api/Notes_/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}
