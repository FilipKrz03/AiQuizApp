using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.Reflection;
using Application.Behaviors;
using MediatR;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			ValidatorOptions.Global.LanguageManager.Enabled = false; // To set Fluent Validation default exceptions language to english 

			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
			services.AddHostedService<BaseQuizesManager>();
            services.AddHostedService<BaseAlgorithmsManager>();
            services.AddTransient<IQuizesCreator, QuizesCreator>();
            services.AddScoped<IAiQuestionsConverter, AiQuestionsConverter>();
            services.AddTransient<IAlgorithmsCreator, AlgorithmsCreator>();
            services.AddScoped<IAiAlgorithmsConverter, AiAlgorithmsConverter>();
  
            return services;
        }
    }
}
