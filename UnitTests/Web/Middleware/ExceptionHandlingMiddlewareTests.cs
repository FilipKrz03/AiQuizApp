using Domain.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Web.Middleware;

namespace UnitTests.Web.Middleware
{
    public class ExceptionHandlingMiddlewareTests
    {
        private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;
        private readonly ExceptionHandlingMiddleware _middleware;
        private readonly HttpContext _context;

        public ExceptionHandlingMiddlewareTests()
        {
            _loggerMock = new();
            _middleware = new(_loggerMock.Object);
            _context = new DefaultHttpContext();
        }

        [Fact]
        public async Task Middleware_Should_CatchResourceNotFoundException_WhenThrown()
        {
            var responseStream = new MemoryStream();
            _context.Response.Body = responseStream;

            var next = new RequestDelegate(_ => throw new ResourceNotFoundException(Guid.NewGuid()));

            await _middleware.InvokeAsync(_context, next);

            responseStream.Position = 0;

            _context.Response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task Middleware_Should_CatchAnyOtherUnkownException_WhenThrown()
        {
            var responseStream = new MemoryStream();
            _context.Response.Body = responseStream;

            var next = new RequestDelegate(_ => throw new AccessViolationException()); // Unkown

            await _middleware.InvokeAsync(_context, next);

            responseStream.Position = 0;

            _context.Response.StatusCode
                .Should()
                .Be(StatusCodes.Status500InternalServerError);
        }
    }
}
