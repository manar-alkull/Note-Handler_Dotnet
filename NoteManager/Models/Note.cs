﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NoteManager.tools;

namespace NoteManager.Models;

public class Note:ISoftDeletable
{
    [Key]
    public int Id { get; set; }

    [Unicode(false)]
    public string? Title { get; set; }

    [Unicode(false)]
    public string? Content { get; set; }
    public string? ImagePath { get; set; }

    [Column("OwnerID")]
    public string OwnerId { get; set; } = "";


    public bool IsDeleted { get; set; }
}
