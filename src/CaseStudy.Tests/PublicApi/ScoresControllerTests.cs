using ApplicationCore.Enums;
using ApplicationCore.TransferObjects;
using ApplicationCore.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CaseStudy.Tests.PUblicApi
{
    public class ScoresControllerTests : BaseTests
    {
        #region ProcessScoreHandler

        [Fact]
        public void ProcessScoreHandlerSuccess()
        {
            // Arrange Data.
            var model = new ProcessScoreRequest()
            {
                Data = new Score()
                {
                    Collaborate = 30,
                    Compete = 22,
                    Control = 55,
                    Create = 30
                }
            };

            // Act.
            IActionResult result = scoresController.ProcessScoreHandler(model);

            // Assert Verification.
            OkObjectResult objResult = (OkObjectResult)result;
            ProcessedScoreResponse processedScoreResponse = (ProcessedScoreResponse)objResult.Value;
            Assert.NotNull(result);
            Assert.Equal(typeof(OkObjectResult), result.GetType());
            Assert.NotNull(processedScoreResponse.Data.Raw);
            Assert.NotNull(processedScoreResponse.Data.Scaled);
            Assert.NotNull(processedScoreResponse.Data.Ranked);
            Assert.Equal(Culture.Compete, processedScoreResponse.Data.Ranked.First);
            Assert.Equal(Culture.Compete, processedScoreResponse.Data.Ranked.Second);
            Assert.Equal(Culture.Create, processedScoreResponse.Data.Ranked.Third);
            Assert.Equal(Culture.Collaborate, processedScoreResponse.Data.Ranked.Fourth);
        }

        #endregion

        #region CheckRulesHandler

        [Fact]
        public void CheckAllLowScoresFailedChecks()
        {
            // Arrange Data.
            var model = new EvaluateScoreRequest()
            {
                Data = new Score()
                {
                    Collaborate = 9,
                    Compete = 6,
                    Control = 8,
                    Create = 18
                }
            };

            // Act.
            var result = scoresController.CheckRulesHandler(model);

            // Assert Verification.
            Assert.NotNull(result);
            OkObjectResult objResult = (OkObjectResult)result;
            EvaluatedScoreResponse evaluationScoreResponse = (EvaluatedScoreResponse)objResult.Value;
            Assert.NotNull(evaluationScoreResponse.Results);
            Assert.Contains(evaluationScoreResponse.Results, x => x.Name == RuleType.AllLowScore && x.Result == EvaluationResult.FailedChecks);
        }

        [Fact]
        public void CheckRulesHandlerSuccess()
        {
            // Arrange Data.
            var model = new EvaluateScoreRequest()
            {
                Data = new Score()
                {
                    Collaborate = 80,
                    Compete = 80,
                    Control = 80,
                    Create = 80
                }
            };

            // Act.
            var result = scoresController.CheckRulesHandler(model);

            // Assert Verification.
            Assert.NotNull(result);
        }

        [Fact]
        public void CheckAllZerosApplied()
        {
            // Arrange Data.
            var model = new EvaluateScoreRequest()
            {
                Data = new Score()
                {
                    Collaborate = 0,
                    Compete = 0,
                    Control = 0,
                    Create = 0
                }
            };

            // Act.
            var result = scoresController.CheckRulesHandler(model);

            // Assert Verification.
            Assert.NotNull(result);
            OkObjectResult objResult = (OkObjectResult)result;
            EvaluatedScoreResponse evaluationScoreResponse = (EvaluatedScoreResponse)objResult.Value;
            Assert.NotNull(evaluationScoreResponse.Results);
            Assert.Contains(evaluationScoreResponse.Results, x => x.Name == RuleType.AllLowScore && x.Result != EvaluationResult.FailedChecks);
        }

        [Fact]
        public void AllLowScoreApplied()
        {
            // Arrange Data.
            var model = new EvaluateScoreRequest()
            {
                Data = new Score()
                {
                    Collaborate = 0,
                    Compete = 6,
                    Control = 38,
                    Create = 7
                }
            };

            // Act.
            var result = scoresController.CheckRulesHandler(model);

            // Assert Verification.
            Assert.NotNull(result);
            OkObjectResult objResult = (OkObjectResult)result;
            EvaluatedScoreResponse evaluationScoreResponse = (EvaluatedScoreResponse)objResult.Value;
            Assert.NotNull(evaluationScoreResponse.Results);
            Assert.Contains(evaluationScoreResponse.Results, x => x.Name == RuleType.AllLowScore && x.Result == EvaluationResult.Applied);
        }

        #endregion
    }
}
