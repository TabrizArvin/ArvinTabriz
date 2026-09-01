using ArvinTabriz.Models;
using ArvinTabriz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArvinTabriz.Pages.Admin;

[Authorize]
public class IndexModel(IContactMessageStore contactMessageStore) : PageModel
{
    public IReadOnlyList<ContactSubmission> Messages { get; private set; } = [];
    public int TotalCount => Messages.Count;
    public int NewCount => Messages.Count(message => message.Status == ContactMessageStatus.New);
    public int InProgressCount => Messages.Count(message => message.Status == ContactMessageStatus.InProgress);
    public int ClosedCount => Messages.Count(message => message.Status == ContactMessageStatus.Closed);

    public async Task OnGetAsync() => Messages = await contactMessageStore.GetAllAsync();

    public async Task<IActionResult> OnPostStatusAsync(Guid id, ContactMessageStatus status)
    {
        if (!ModelState.IsValid || !Enum.IsDefined(status))
        {
            return BadRequest();
        }

        await contactMessageStore.UpdateStatusAsync(id, status);
        return RedirectToPage();
    }

    public string StatusLabel(ContactMessageStatus status) => status switch
    {
        ContactMessageStatus.New => "جدید",
        ContactMessageStatus.InProgress => "در حال پیگیری",
        ContactMessageStatus.Closed => "بسته‌شده",
        _ => status.ToString()
    };
}
