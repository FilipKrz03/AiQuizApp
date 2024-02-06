using Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Domain.ValueObjects
{
    public class AnswerLetterTests
    {
        [Theory]
        [InlineData('g')]
        [InlineData('x')]
        public void AnswerLetter_Should_ReturnNull_WhenCharLetterOutOfAllowedRange(char letter)
        {
            var answerLetter = AnswerLetter.Create(letter);

            answerLetter
                .Should()
                .BeNull();
        }

        [Theory]
        [InlineData('a')]
        [InlineData('d')]
        public void AnswerLetter_Should_ReturnObjectWithProperValue_WhenCharLetterOutOfAllowedRange(char letter)
        {
            var answerLetter = AnswerLetter.Create(letter);

            answerLetter
                .Should()
                .NotBeNull();

            answerLetter
                .Should()
                .BeOfType<AnswerLetter>();

            answerLetter!.Letter
                .Should()
                .Be(letter);
        }
    }
}
