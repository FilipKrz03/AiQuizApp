using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
	public static class ClaimsExtensions
	{
		public static string GetId(this IEnumerable<Claim> claims) =>
			claims.Where(e => e.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value ??
				throw new InvalidAccesTokenException();
	}
}
