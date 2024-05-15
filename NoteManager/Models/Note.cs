using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoteManager.Models;

public partial class Note
{
    [Key]
    public int Id { get; set; }

    [Unicode(false)]
    public string? Title { get; set; }

    [Unicode(false)]
    public string? Content { get; set; }

    [Column(TypeName = "image")]
    public byte[]? AttachedImage { get; set; }

    [Column("OwnerID")]
    public string OwnerId { get; set; } = "";
}
