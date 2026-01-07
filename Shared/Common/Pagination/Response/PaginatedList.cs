using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.EntityFramework.Base;
using Common.Pagination.Request;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Common.Pagination.Response;


public class PaginatedList<TDto> where TDto : class
{
    public PaginatedList()
    {
    }

    public int PageNumber { get; set; }

    public int TotalPages { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public IEnumerable<TDto> Items { get; private set; }

    public bool HasPrevious
    {
        get
        {
            return PageNumber > 1;
        }
    }

    public bool HasNext
    {
        get
        {
            return PageNumber < TotalPages;
        }
    }

    public static async Task<PaginatedList<TDto>> GetPaginatedPageAsync<TEntity>(IQueryable<TEntity> queryable, PaginationParameters paginationParameters, IConfigurationProvider configurationProvider, object projectionParameters = null) where TEntity : BaseEntity
    {
        var count = await queryable.CountAsync();

        paginationParameters = PaginationParameters.CreatePaginationParametersIfNotExists(paginationParameters);

        queryable = queryable
                        .OrderBy(paginationParameters.OrderByColumn + " " + paginationParameters.SortBy)
                        .Skip(((int)paginationParameters.PageNumber - 1) * (int)paginationParameters.PageSize)
                        .Take((int)paginationParameters.PageSize);

        var result =
            projectionParameters is { } ?
                await queryable.ProjectTo<TDto>(configurationProvider, projectionParameters).ToListAsync() :
                await queryable.ProjectTo<TDto>(configurationProvider).ToListAsync();

        return new PaginatedList<TDto>
        {
            PageSize = (int)paginationParameters.PageSize,
            PageNumber = (int)paginationParameters.PageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)paginationParameters.PageSize),
            TotalCount = count,
            Items = result
        };
    }

    public static async Task<PaginatedList<TDto>> GetPaginatedPageFromTwoQueryables<TEntity1, TEntity2>(IQueryable<TEntity1> queryable1, IQueryable<TEntity2> queryable2, PaginationParameters paginationParameters, IConfigurationProvider configurationProvider, object projectionParameters1 = null, object projectionParameters2 = null)
        where TEntity1 : BaseEntity
        where TEntity2 : BaseEntity
    {
        var count1 = await queryable1.CountAsync();
        var count2 = await queryable2.CountAsync();

        paginationParameters = PaginationParameters.CreatePaginationParametersIfNotExists(paginationParameters);

        var result1 =
            projectionParameters1 is { } ?
                queryable1.ProjectTo<TDto>(configurationProvider, projectionParameters1) :
                queryable1.ProjectTo<TDto>(configurationProvider);

        var result2 =
            projectionParameters2 is { } ?
                queryable2.ProjectTo<TDto>(configurationProvider, projectionParameters2) :
                queryable2.ProjectTo<TDto>(configurationProvider);

        var finalResult = result1.Union(result2);

        finalResult = finalResult
                        .OrderBy(paginationParameters.OrderByColumn + " " + paginationParameters.SortBy)
                        .Skip(((int)paginationParameters.PageNumber - 1) * (int)paginationParameters.PageSize)
                        .Take((int)paginationParameters.PageSize);

        return new PaginatedList<TDto>
        {
            PageSize = (int)paginationParameters.PageSize,
            PageNumber = (int)paginationParameters.PageNumber,
            TotalPages = (int)Math.Ceiling((count1 + count2) / (double)paginationParameters.PageSize),
            TotalCount = count1 + count2,
            Items = await finalResult.ToListAsync()
        };
    }

    public static async Task<TPaginatedType> GetPaginatedPageAsTypeDerivedFromPaginatedListAsync<TEntity, TPaginatedType>(IQueryable<TEntity> queryable, PaginationParameters paginationParameters, IConfigurationProvider configurationProvider, object projectionParameters = null)
        where TEntity : BaseEntity
        where TPaginatedType : PaginatedList<TDto>, new()
    {
        var count = await queryable.CountAsync();

        paginationParameters = PaginationParameters.CreatePaginationParametersIfNotExists(paginationParameters);

        queryable = queryable
                        .OrderBy(paginationParameters.OrderByColumn + " " + paginationParameters.SortBy)
                        .Skip(((int)paginationParameters.PageNumber - 1) * (int)paginationParameters.PageSize)
                        .Take((int)paginationParameters.PageSize);

        var result =
            projectionParameters is { } ?
                await queryable.ProjectTo<TDto>(configurationProvider, projectionParameters).ToListAsync() :
                await queryable.ProjectTo<TDto>(configurationProvider).ToListAsync();

        return new TPaginatedType
        {
            PageSize = (int)paginationParameters.PageSize,
            PageNumber = (int)paginationParameters.PageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)paginationParameters.PageSize),
            TotalCount = count,
            Items = result
        };
    }

    public static async Task<PaginatedList<TDto>> GetPaginatedPageWithOrderingAfterProjectionAsync<TEntity>(IQueryable<TEntity> queryable, PaginationParameters paginationParameters, IConfigurationProvider configurationProvider, object projectionParameters = null) where TEntity : BaseEntity
    {
        var count = await queryable.CountAsync();

        paginationParameters = PaginationParameters.CreatePaginationParametersIfNotExists(paginationParameters);

        var result =
            projectionParameters is { }
                ? await queryable
                            .ProjectTo<TDto>(configurationProvider, projectionParameters)
                            .OrderBy(paginationParameters.OrderByColumn + " " + paginationParameters.SortBy)
                            .Skip(((int)paginationParameters.PageNumber - 1) * (int)paginationParameters.PageSize)
                            .Take((int)paginationParameters.PageSize)
                            .ToListAsync()
                : await queryable
                            .ProjectTo<TDto>(configurationProvider)
                            .OrderBy(paginationParameters.OrderByColumn + " " + paginationParameters.SortBy)
                            .Skip(((int)paginationParameters.PageNumber - 1) * (int)paginationParameters.PageSize)
                            .Take((int)paginationParameters.PageSize)
                            .ToListAsync();

        return new PaginatedList<TDto>
        {
            PageSize = (int)paginationParameters.PageSize,
            PageNumber = (int)paginationParameters.PageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)paginationParameters.PageSize),
            TotalCount = count,
            Items = result
        };
    }

    public static PaginatedList<TDto> GetPaginatePageFromList(IEnumerable<TDto> collection, int count, PaginationParameters paginationParameters)
    {
        return new PaginatedList<TDto>
        {
            PageSize = (int)paginationParameters.PageSize,
            PageNumber = (int)paginationParameters.PageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)paginationParameters.PageSize),
            TotalCount = count,
            Items = collection
        };
    }

    public static PaginatedList<TDto> GetEmptyPaginatedList()
        => new()
        {
            PageSize = default,
            PageNumber = default,
            TotalPages = default,
            TotalCount = default,
            Items = new List<TDto>()
        };
}