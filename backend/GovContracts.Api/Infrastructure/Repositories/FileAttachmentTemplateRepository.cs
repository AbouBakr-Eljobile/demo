using System.Text.Json;
using GovContracts.Api.Application.Interfaces;
using GovContracts.Api.Domain.Entities;
using Microsoft.AspNetCore.Hosting;

namespace GovContracts.Api.Infrastructure.Repositories;

public class FileAttachmentTemplateRepository : IAttachmentTemplateRepository
{
    private readonly string _filePath;
    private readonly object _lock = new();

    public FileAttachmentTemplateRepository(IWebHostEnvironment environment)
    {
        var dataDirectory = Path.Combine(environment.ContentRootPath, "data");
        Directory.CreateDirectory(dataDirectory);
        _filePath = Path.Combine(dataDirectory, "attachment-templates.json");
    }

    public Task<IReadOnlyCollection<AttachmentTemplate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var items = LoadAll();
            var snapshot = items
                .OrderByDescending(item => item.CreatedAtUtc)
                .Select(Clone)
                .ToList()
                .AsReadOnly();
            return Task.FromResult((IReadOnlyCollection<AttachmentTemplate>)snapshot);
        }
    }

    public Task<AttachmentTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var items = LoadAll();
            var template = items.FirstOrDefault(item => item.Id == id);
            return Task.FromResult(template is null ? null : Clone(template));
        }
    }

    public Task<AttachmentTemplate> CreateAsync(AttachmentTemplate template, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var items = LoadAll();
            var created = Clone(template);
            items.Add(created);
            SaveAll(items);
            return Task.FromResult(Clone(created));
        }
    }

    public Task<AttachmentTemplate?> UpdateAsync(AttachmentTemplate template, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var items = LoadAll();
            var index = items.FindIndex(item => item.Id == template.Id);
            if (index < 0)
            {
                return Task.FromResult<AttachmentTemplate?>(null);
            }

            items[index] = Clone(template);
            SaveAll(items);
            return Task.FromResult<AttachmentTemplate?>(Clone(items[index]));
        }
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var items = LoadAll();
            var removed = items.RemoveAll(item => item.Id == id) > 0;
            if (removed)
            {
                SaveAll(items);
            }

            return Task.FromResult(removed);
        }
    }

    private List<AttachmentTemplate> LoadAll()
    {
        if (!File.Exists(_filePath))
        {
            return [];
        }

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<AttachmentTemplate>>(json) ?? [];
    }

    private void SaveAll(List<AttachmentTemplate> items)
    {
        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(_filePath, json);
    }

    private static AttachmentTemplate Clone(AttachmentTemplate template) => new()
    {
        Id = template.Id,
        Name = template.Name,
        IsRequired = template.IsRequired,
        CreatedAtUtc = template.CreatedAtUtc
    };
}
