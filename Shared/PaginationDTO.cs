namespace Blazor.Shared
{
    public record PaginationDTO
    {
        public int Page            { get; init; } = 1;
        public int QuantityPerPage { get; init; } = 8;
    }
}
