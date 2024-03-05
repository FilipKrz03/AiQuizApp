using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
	public class PaginationMetadata<T>
	{
		public int PageNumber { get; init; }
		public int TotalCount { get; init; }
		public int PageSize { get; init; }
		public bool HasPrevious { get; init; }
		public bool HasNext { get; init; }
		public int TotalPages { get; init; }

		public PaginationMetadata(PagedList<T> pagedList)
		{
			PageNumber = pagedList.PageNumber;
			TotalCount = pagedList.TotalCount;
			PageSize = pagedList.PageSize;
			HasPrevious = pagedList.HasPrevious;
			HasNext = pagedList.HasNext;
			TotalPages = pagedList.TotalPages;
		}
	}
}
