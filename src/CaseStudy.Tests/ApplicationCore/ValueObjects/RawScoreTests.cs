using ApplicationCore.ValueObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CaseStudy.Tests.ApplicationCore.ValueObjects
{
    public class RawScoreTests
    {
        [Fact]
        public void ToStringOverride()
        {
            // Arrange Data.
            var inputScore = new Score()
            {
                Create = 0,
                Collaborate = 4,
                Compete = 0,
                Control = 34
            };

            var model = new RawScore(inputScore);

            // Act.
            var result = model.ToString();

            // Assert Verification.
            Assert.NotNull(result);
            Assert.Equal(result, $"Collaborate: {model.Collaborate.Value}, Create: {model.Create.Value}, Compete: {model.Compete.Value}, Control: {model.Control.Value}");
            Assert.Equal(0, inputScore.Compete);
        }
    }
}
