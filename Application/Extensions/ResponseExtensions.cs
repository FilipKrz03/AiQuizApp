using Application.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
	public static class ResponseExtensions
	{
		public static void AddPaginationHeader<T>(this HttpResponse resposne, PagedList<T> result) =>	
			resposne.Headers.Append("X-Pagination", result.CreatePaginationMetadataAsString());
	}
}
