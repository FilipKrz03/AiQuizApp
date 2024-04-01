using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Behaviors
{
	public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) 
		: IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			if (_validators.Any())
			{
				var context = new ValidationContext<TRequest>(request);

				var failtures = _validators
					.Select(v => v.Validate(context))
					.SelectMany(result => result.Errors)
					.Where(f => f != null).ToList();

				if (failtures.Count != 0)
				{
					throw new ValidationException(failtures);
				}
			}

			return await next();
		}
	}
}
