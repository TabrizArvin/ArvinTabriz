using ArvinTabriz.Models;
using ArvinTabriz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArvinTabriz.Pages.Admin;

[Authorize]
public class IndexModel(IContactMessageStore contactMessageStore) : PageModel
{
    private const int PageSize = 10;

    public IReadOnlyList<ContactSubmission> Messages { get; private set; } = [];
    public int TotalCount { get; private set; }
    public int NewCount { get; private set; }
    public int InProgressCount { get; private set; }
    public int ClosedCount { get; private set; }
    public int TotalFilteredCount { get; private set; }
    public int PageCount => Math.Max(1, (int)Math.Ceiling(TotalFilteredCount / (double)PageSize));

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public ContactMessageStatus? Status { get; set; }

    [BindProperty(SupportsGet = true, Name = "page")]
    public int PageNumber { get; set; } = 1;

    public async Task OnGetAsync()
    {
        var allMessages = await contactMessageStore.GetAllAsync();
        TotalCount = allMessages.Count;
        NewCount = allMessages.Count(message => message.Status == ContactMessageStatus.New);
        InProgressCount = allMessages.Count(message => message.Status == ContactMessageStatus.InProgress);
        ClosedCount = allMessages.Count(message => message.Status == ContactMessageStatus.Closed);

        var query = allMessages.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(Search))
        {
            query = query.Where(message =>
                message.Name.Contains(Search, StringComparison.OrdinalIgnoreCase)
                || message.Phone.Contains(Search, StringComparison.OrdinalIgnoreCase)
                || (message.Email?.Contains(Search, StringComparison.OrdinalIgnoreCase) ?? false)
                || message.Message.Contains(Search, StringComparison.OrdinalIgnoreCase));
        }

        if (Status.HasValue)
        {
            query = query.Where(message => message.Status == Status.Value);
        }

        var filteredMessages = query.ToList();
        TotalFilteredCount = filteredMessages.Count;
        PageNumber = Math.Clamp(PageNumber, 1, PageCount);
        Messages = filteredMessages
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }

    public async Task<IActionResult> OnPostStatusAsync(Guid id, ContactMessageStatus status)
    {
        if (!ModelState.IsValid || !Enum.IsDefined(status))
        {
            return BadRequest();
        }

        await contactMessageStore.UpdateStatusAsync(id, status);
        return RedirectToPage(new { Search, Status, page = PageNumber });
    }

    public string StatusLabel(ContactMessageStatus status) => status switch
    {
        ContactMessageStatus.New => "جدید",
        ContactMessageStatus.InProgress => "در حال پیگیری",
        ContactMessageStatus.Closed => "بسته‌شده",
        _ => status.ToString()
    };
}
