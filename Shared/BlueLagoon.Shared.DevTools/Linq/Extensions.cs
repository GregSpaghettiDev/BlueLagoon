using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlueLagoon.Shared.DevTools.Base.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlueLagoon.Shared.DevTools.Linq;

public static class Extensions
{
    public static Task<T> ReturnSingleOrDefaultAsync<T>(this IQueryable<T> queryable, bool withTracking)
       where T : class, IBaseEntity
       => withTracking
               ? queryable.SingleOrDefaultAsync()
               : queryable.AsNoTracking().SingleOrDefaultAsync();

    public static Task<TProjection> ReturnSingleOrDefaultAsync<T, TProjection>(this IQueryable<T> queryable, bool withTracking, IConfigurationProvider mapperConfiguration)
        where T : class, IBaseEntity
        where TProjection : class
        => withTracking
                ? queryable
                    .ProjectTo<TProjection>(mapperConfiguration)
                    .SingleOrDefaultAsync()
                : queryable
                    .AsNoTracking()
                    .ProjectTo<TProjection>(mapperConfiguration)
                    .SingleOrDefaultAsync();

    public static Task<T> ReturnSingleOrDefaultAsync<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> expression, bool withTracking)
        where T : class, IBaseEntity
    {
        if (withTracking == false)
            queryable = queryable.AsNoTracking();

        return queryable.SingleOrDefaultAsync(expression);
    }

    public static Task<TProjection> ReturnSingleOrDefaultAsync<T, TProjection>(this IQueryable<T> queryable, Expression<Func<T, bool>> expression, bool withTracking, IConfigurationProvider mapperConfiguration, object projectionParameters = null)
        where T : class, IBaseEntity
        where TProjection : class
    {
        if (withTracking == false)
            queryable = queryable.AsNoTracking();

        queryable = queryable.Where(expression);

        return projectionParameters is { }
                        ? queryable.ProjectTo<TProjection>(mapperConfiguration, projectionParameters).SingleOrDefaultAsync()
                        : queryable.ProjectTo<TProjection>(mapperConfiguration).SingleOrDefaultAsync();
    }

    public static Task<List<T>> ReturnListAsync<T>(this IQueryable<T> queryable, bool withTracking)
        where T : class, IBaseEntity
        => withTracking
                ? queryable.ToListAsync()
                : queryable.AsNoTracking().ToListAsync();

    public static Task<List<T>> ReturnListAsync<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> expression, bool withTracking)
        where T : class, IBaseEntity
    {
        if (withTracking == false)
            queryable = queryable.AsNoTracking();

        return queryable.Where(expression).ToListAsync();
    }

    public static Task<List<TProjection>> ReturnListAsync<T, TProjection>(this IQueryable<T> queryable, Expression<Func<T, bool>> expression, bool withTracking, IConfigurationProvider mapperConfiguration)
        where T : class, IBaseEntity
        where TProjection : class
    {
        if (withTracking == false)
            queryable = queryable.AsNoTracking();

        return queryable
                .Where(expression)
                .ProjectTo<TProjection>(mapperConfiguration)
                .ToListAsync();
    }

    public static Task<List<TProjection>> ReturnListAsync<T, TProjection>(this IQueryable<T> queryable, bool withTracking, IConfigurationProvider mapperConfiguration)
        where T : class, IBaseEntity
        where TProjection : class
    {
        if (withTracking == false)
            queryable = queryable.AsNoTracking();

        return queryable
                .ProjectTo<TProjection>(mapperConfiguration)
                .ToListAsync();
    }
}