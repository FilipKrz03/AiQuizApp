using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHostedService<BaseQuizesManager>();
            services.AddTransient<IQuizesCreator, QuizesCreator>();
            services.AddScoped<IAiQuestionsConverter, AiQuestionsConverter>();
  
            return services;
        }
    }
}
