using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NoteManager.Models.Responses
{
    public class NoteResponseModel
    {
        public NoteResponseModel(Note note)
        {
            Id = note.Id;
            Title = note.Title;
            Content = note.Content;
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
