using Domain.Entities;
using Infrastructure.DbContexts;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
	public class UserRepository(QuizApplicationDbContext context) : IUserRepository
	{
		private readonly QuizApplicationDbContext _context = context;

		// User repository need to be seperate repository because it is not entity owned by application (it is pre-defined by identity packacge)
		public async Task<bool> UserExistAsync(string id)
		{
			return await _context.Users
				.Where(u => u.Id == id)
				.AnyAsync();
		}
	}
}
