using Domain.Entities;
using Infrastructure.DbContexts;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<QuizApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IRepository<Quiz>, Repository<Quiz>>();
            services.AddTransient<IRepository<Answer>, Repository<Answer>>();
            services.AddTransient<IRepository<Question>, Repository<Question>>();
            services.AddTransient<IRepository<UserOwnQuiz>, Repository<UserOwnQuiz>>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRepository<AlgorithmTask>, Repository<AlgorithmTask>>();
            services.AddTransient<IRepository<AlgorithmAnswer>, Repository<AlgorithmAnswer>>();
            services.AddTransient<IRepository<UserOwnAlgorithmTask> , Repository<UserOwnAlgorithmTask>>();

            return services;
        }
    }
}
