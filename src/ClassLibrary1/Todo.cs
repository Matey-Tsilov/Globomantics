using System.Reflection.Metadata;

namespace Globomantics.Domain;

public abstract record Todo
(
    Guid Id,
    string Title,
    DateTimeOffset CreatedDate,
    User CreatedBy,
    bool isComplated = false,
    bool isDeleted = false
)
{
    public Todo? Parent { get; init; }
}
