using ArvinTabriz.Models;
using ArvinTabriz.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArvinTabriz.Pages;

public class IndexModel : PageModel
{
    private readonly IContactMessageStore _contactMessageStore;

    public IndexModel(IContactMessageStore contactMessageStore)
    {
        _contactMessageStore = contactMessageStore;
    }

    [BindProperty]
    public ContactSubmission Contact { get; set; } = new();

    public bool IsSubmitted { get; private set; }

    public async Task<IActionResult> OnPostContactAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _contactMessageStore.AddAsync(Contact);
        return RedirectToPage("/Index", null, new { submitted = true }, "contact");
    }

    public void OnGet(bool submitted = false)
    {
        IsSubmitted = submitted;
    }
}
