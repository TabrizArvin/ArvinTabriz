using ArvinTabriz.Models;

namespace ArvinTabriz.Services;

public interface IContactMessageStore
{
    Task AddAsync(ContactSubmission submission, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ContactSubmission>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateStatusAsync(Guid id, ContactMessageStatus status, CancellationToken cancellationToken = default);
}
