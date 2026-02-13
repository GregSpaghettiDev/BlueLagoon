using System.Data;
using System.Linq.Dynamic.Core;

namespace BlueLagoon.Shared.DevTools.Pagination;

public static class Extensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationParameters paginationParameters, bool withOrdering = false)
        where T : class
        => withOrdering
                ? queryable
                    .OrderBy(paginationParameters.OrderByColumn + " " + paginationParameters.SortBy)
                    .Skip(((int)paginationParameters.PageNumber - 1) * (int)paginationParameters.PageSize)
                    .Take((int)paginationParameters.PageSize)
                : queryable
                    .Skip(((int)paginationParameters.PageNumber - 1) * (int)paginationParameters.PageSize)
                    .Take((int)paginationParameters.PageSize);
}
