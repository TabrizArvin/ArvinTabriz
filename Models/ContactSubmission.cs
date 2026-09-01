using System.ComponentModel.DataAnnotations;

namespace ArvinTabriz.Models;

public class ContactSubmission
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required(ErrorMessage = "نام و نام خانوادگی را وارد کنید.")]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "شماره تماس را وارد کنید.")]
    [StringLength(30)]
    public string Phone { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "آدرس ایمیل معتبر نیست.")]
    [StringLength(160)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "شرح کوتاهی از نیازتان بنویسید.")]
    [StringLength(2000)]
    public string Message { get; set; } = string.Empty;

    public DateTime SubmittedAtUtc { get; init; } = DateTime.UtcNow;
    public ContactMessageStatus Status { get; set; } = ContactMessageStatus.New;
}

public enum ContactMessageStatus
{
    New,
    InProgress,
    Closed
}
