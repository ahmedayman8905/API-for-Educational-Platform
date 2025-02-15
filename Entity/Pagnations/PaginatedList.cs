﻿using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Api_1.Entity.Pagnations;

public class PaginatedList<T>(List<T> items, int pageNumber, int count, int pageSize)
{
    public List<T> Items { get; private set; } = items;
    public int PageNumber { get; private set; } = pageNumber;
    public int TotalPages { get; private set; } = (int)Math.Ceiling(count / (double)pageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;


    public static async Task<PaginatedList<T>> CreateAsync(IEnumerable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {

        // if use IQueryable then use this code
        //var count = await source.CountAsync(cancellationToken);
        //var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        // if use IEnumerable then use this code
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginatedList<T>(items, pageNumber, count, pageSize);
    }
}