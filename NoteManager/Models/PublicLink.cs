using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoteManager.Models;

[Table("Public_Links")]
public partial class PublicLink
{
    [Key]
    public int Id { get; set; }

    public int Note { get; set; }
    [ForeignKey("Note")]
    public Note NoteObj { get; set; }
}
