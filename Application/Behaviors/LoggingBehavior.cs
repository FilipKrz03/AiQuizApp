using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Behaviors
{
	public class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger) 
		: IPipelineBehavior<TRequest, TResponse>
		where TRequest : notnull
	{
		private readonly ILogger<TRequest> _logger = logger;

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{

			var requestName = request.GetType().Name;
			var requestGuid = Guid.NewGuid().ToString();

			var requestNameWithGuid = $"{requestName} [{requestGuid}]";

			_logger.LogInformation($"[START] {requestNameWithGuid}");
			TResponse response;

			try
			{
				response = await next();
			}
			finally
			{
				_logger.LogInformation(
					$"[END] {requestNameWithGuid}");
			}

			return response;
		}
	}
}
