using Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Domain.ValueObjects
{
    public class AdvanceNumberTests
    {
        [Theory]
        [InlineData(11)]
        [InlineData(122)]
        [InlineData(-2)]
        public void AdvanceNumber_Should_ReturnNull_WhenProvidedNumberIsOutOfAllowedRange(int number)
        {
            var advanceNumber = AdvanceNumber.Create(number);

            advanceNumber
                .Should()
                .BeNull();
        }

        [Theory]
        [InlineData(10)]
        [InlineData(5)]
        [InlineData(1)]
        public void AdvanceNumber_Should_ReturnAdavanceNumberObjectWithProperValue_WhenProvidedNumberIsInAllowedRange(int number)
        {
            var advanceNumber = AdvanceNumber.Create(number);

            advanceNumber
                .Should()
                .NotBeNull();

            advanceNumber
                .Should()
                .BeOfType<AdvanceNumber>();

            advanceNumber!.Number
                .Should()
                .Be(number);
        }
    }
}
