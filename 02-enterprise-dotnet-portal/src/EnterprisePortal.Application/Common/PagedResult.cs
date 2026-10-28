namespace EnterprisePortal.Application.Common;

public record PagedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int PageSize)
{
    public int TotalPages => (int)Math.Ceiling(Total / (double)PageSize);
}
public record Result<T>(bool Ok, T? Value, string? Error)
{
    public static Result<T> Success(T v) => new(true, v, null);
    public static Result<T> Fail(string e) => new(false, default, e);
}
