using System.Text.Json;
using ArvinTabriz.Models;

namespace ArvinTabriz.Services;

/// <summary>Stores website contact requests in a small JSON file without requiring a database server.</summary>
public sealed class FileContactMessageStore : IContactMessageStore
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

    public FileContactMessageStore(IWebHostEnvironment environment)
    {
        _filePath = Path.Combine(environment.ContentRootPath, "App_Data", "contact-messages.json");
    }

    public async Task AddAsync(ContactSubmission submission, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            var messages = await ReadAsync(cancellationToken);
            messages.Add(submission);
            await SaveAsync(messages, cancellationToken);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<IReadOnlyList<ContactSubmission>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            return (await ReadAsync(cancellationToken)).OrderByDescending(message => message.SubmittedAtUtc).ToList();
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<bool> UpdateStatusAsync(Guid id, ContactMessageStatus status, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            var messages = await ReadAsync(cancellationToken);
            var message = messages.FirstOrDefault(item => item.Id == id);
            if (message is null)
            {
                return false;
            }

            message.Status = status;
            await SaveAsync(messages, cancellationToken);
            return true;
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task<List<ContactSubmission>> ReadAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_filePath))
        {
            return [];
        }

        await using var stream = File.OpenRead(_filePath);
        return await JsonSerializer.DeserializeAsync<List<ContactSubmission>>(stream, _jsonOptions, cancellationToken) ?? [];
    }

    private async Task SaveAsync(List<ContactSubmission> messages, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        var temporaryPath = $"{_filePath}.tmp";
        await using (var stream = File.Create(temporaryPath))
        {
            await JsonSerializer.SerializeAsync(stream, messages, _jsonOptions, cancellationToken);
        }

        File.Move(temporaryPath, _filePath, true);
    }
}
