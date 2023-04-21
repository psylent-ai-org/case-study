using ApplicationCore.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CaseStudy.Tests.ApplicationCore.ValueObjects
{
    public class CultureScoreTests
    {
        [Fact]
        public void CompareToWithNull()
        {
            // Arrange Data.
            var cultureScore = new CultureScore();

            // Act.
            var result = cultureScore.CompareTo(null);

            // Assert Verification.
            Assert.Equal(1,  result);
        }
    }
}
