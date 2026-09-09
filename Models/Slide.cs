using System.ComponentModel.DataAnnotations;

namespace ArvinTabriz.Models;

public sealed class Slide
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Content { get; set; }

    [StringLength(500)]
    public string? ImagePath { get; set; }

    [StringLength(500)]
    public string? LinkUrl { get; set; }

    public int DisplayOrder { get; set; }
    public bool IsPublished { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}
