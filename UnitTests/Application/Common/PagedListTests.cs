using Application.Common;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using MockQueryable.Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Application.Common
{
    public class PagedListTests
    {
        [Theory]
        [InlineData(10, 25, 3)]
        [InlineData(15, 30, 2)]
        [InlineData(15, 31, 3)]
        public void PagedList_Should_ProperlyCalculateTotalPagesCount(int pageSize, int totalCount, int expectedTotalPages)
        {
            List<string> items = [];

            PagedList<string> pagedList = new(items, pageSize, 1, totalCount);

            pagedList.TotalPages
                .Should()
                .Be(expectedTotalPages);
        }

        [Fact]
        public async Task CreateAsync_Should_ProperlyCreatePagedList()
        {
            List<Quiz> items = new()
            {
                new Quiz(Guid.NewGuid() , "Quiz1", "" , AdvanceNumber.Create(5)!) ,
                new Quiz(Guid.NewGuid() , "Quiz2", "" , AdvanceNumber.Create(5)!) ,
                new Quiz(Guid.NewGuid() , "Quiz3", "" , AdvanceNumber.Create(5)!) ,
                new Quiz(Guid.NewGuid() , "Quiz4", "" , AdvanceNumber.Create(5)!)
            };

            var result = await PagedList<Quiz>.CreateAsync(items.BuildMock(), 2, 2);

            result[0].Title
                .Should()
                .Be("Quiz3");

            result[1].Title
                .Should()
                .Be("Quiz4");

            result.TotalCount
                .Should()
                .Be(4);

            result.PageNumber
                .Should()
                .Be(2);

            result.HasNext
                .Should()
                .Be(false);

            result.HasPrevious
                .Should()
                .Be(true);

            result.TotalPages
                .Should()
                .Be(2);
        }
    }
}
