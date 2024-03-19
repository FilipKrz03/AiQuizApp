using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Application.Common
{
	public class PagedList<T> : List<T>
	{
		public int PageSize { get; set; }
		public int PageNumber { get; set; }

		public int TotalCount { get; set; }
		public int TotalPages { get; set; }

		public bool HasPrevious => PageNumber > 1;
		public bool HasNext => PageNumber < TotalPages;


		public PagedList(List<T> items, int pageSize, int pageNumber, int totalCount)
		{
			PageSize = pageSize;
			PageNumber = pageNumber;
			TotalCount = totalCount;
			TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
			AddRange(items);
		}

		private PaginationMetadata<T> CreatePaginationMetadata()
		{
			return new PaginationMetadata<T>(this);
		}

		public string CreatePaginationMetadataAsString()
		{
			return JsonSerializer.Serialize(CreatePaginationMetadata());
		}

		public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageSize, int pageNumber)
		{
			int totalCount = await source.CountAsync();

			var items = await source
				.Skip(pageSize * (pageNumber - 1))
				.Take(pageSize)
				.ToListAsync();

			return new(items, pageSize, pageNumber, totalCount);
		}
	}
}
