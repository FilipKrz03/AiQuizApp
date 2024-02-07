using Application.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Application.Common
{
    public class ResourceParamethersTests
    {
        [Fact]
        public void PageSize_ShouldBeNotGraeterThan30EvenIfUserProvidedGreaterNumber()
        {
            ResourceParamethers resourceParamethers = new() { PageSize = 100 };

            resourceParamethers.PageSize
                .Should()
                .Be(30);
        }

        [Fact]
        public void SortOrder_ShouldHaveDefaultAscValue()
        {
            ResourceParamethers resourceParamethers = new();

            resourceParamethers.SortOrder
                .Should()
                .Be("asc");
        }
    }
}
