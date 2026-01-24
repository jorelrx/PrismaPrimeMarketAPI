namespace PrismaPrimeMarket.Application.Common.Models;

public class PagedResponse<T> : Response<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PagedResponse(T data, string message, int pageNumber, int pageSize, int totalRecords, string? path = null)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = totalRecords > 0 ? (int)Math.Ceiling(totalRecords / (double)pageSize) : 0;
        Data = data;
        Succeeded = true;
        Message = message;
        Type = ResponseType.Retrieved;
        Errors = null;
        Path = path;
        Timestamp = DateTime.UtcNow;
    }

    public static PagedResponse<T> Create(T data, int pageNumber, int pageSize, int totalRecords, string? customMessage = null, string? path = null)
    {
        return new PagedResponse<T>(
            data,
            customMessage ?? ResponseMessages.ListRetrieved,
            pageNumber,
            pageSize,
            totalRecords,
            path
        );
    }
}
