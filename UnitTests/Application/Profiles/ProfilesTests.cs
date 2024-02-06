using Application.Profiles;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Profiles
{
    public class ProfilesTests
    {
        [Fact]
        public void ProfilesConfigurations_Should_BeValid()
        {
            // All profiles need to be tested together because they are depending on each other

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AnswerProfile>();
                cfg.AddProfile<QuestionProfile>();
                cfg.AddProfile<QuizProfile>();
            });

            configuration.AssertConfigurationIsValid();
        }
    }
}
