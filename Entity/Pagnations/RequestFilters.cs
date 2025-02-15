using System.ComponentModel.DataAnnotations;

namespace Api_1.Entity.Pagnations;

public record RequestFilters
{
    [Range(1, int.MaxValue, ErrorMessage = "PageNumber Value must be at least 1.")]
    public int PageNumber { get; init; } = 1;
    [Range(1, int.MaxValue, ErrorMessage = "PageSize Value must be at least 1.")]
    public int PageSize { get; init; } = 5;
    public string? SearchValue { get; init; }
    public string? SortColumn { get; init; } = "JoinDate";
    public string? SortDirection { get; init; } = "ASC";
}