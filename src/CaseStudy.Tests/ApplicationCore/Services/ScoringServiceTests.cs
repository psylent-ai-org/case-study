using ApplicationCore.ValueObjects;
using CaseStudy.Tests.PUblicApi;
using Xunit;

namespace CaseStudy.Tests.ApplicationCore.Services
{
    public class ScoringServiceTests : BaseTests
    {
        [Fact]
        public void GetScaledScoreFromScale()
        {
            // Arrange Data.
            Score model = new()
            {
                Collaborate = 30,
                Compete = 22,
                Control = 55,
                Create = 30
            };

            // Act.
            var result = scoringService.Scale(model);

            // Assert Verification.
            Assert.NotNull(result);
        }
    }
}
