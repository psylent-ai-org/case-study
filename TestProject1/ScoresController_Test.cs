using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using ApplicationCore.TransferObjects;
using ApplicationCore.ValueObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PublicApi.Controllers;
using System;
using Xunit;

namespace ScoresController_Test
{
    public class ScoresController_Test
    {
        [Theory]
        [InlineData(10,20,30,40)]
        [InlineData(40,30,20,10)]
        [InlineData(40,00,20,00)]
        [InlineData(00,00,00,00)]
        public void CompleteFlow_Test(int collaborate, int create, int compete, int control)
        {
            //setup
            IScoring scoring = new ScoringService();
            ScoresController scoresController = new ScoresController(scoring);
            IActionResult serviceOutput = null;
            var requestOfProcessScore = new ProcessScoreRequest
            {
                Data = new Score
                {
                    Collaborate = collaborate,
                    Create = create,
                    Compete = compete,
                    Control = control
                }
            };

            //execute
            serviceOutput = scoresController.ProcessScoreHandler(requestOfProcessScore);
            var processedScore = (serviceOutput as OkObjectResult).Value as ProcessedScoreResponse;

            var requestOfCheckRules = new EvaluateScoreRequest
            {
                Data = new Score
                {
                    Collaborate = (int)processedScore.Data.Raw.Collaborate.Value,
                    Create = (int)processedScore.Data.Raw.Create.Value,
                    Compete = (int)processedScore.Data.Raw.Compete.Value,
                    Control = (int)processedScore.Data.Raw.Control.Value,
                }
            };

            serviceOutput = scoresController.CheckRulesHandler(requestOfCheckRules);
            var evaluatedScore = (serviceOutput as OkObjectResult).Value as EvaluatedScoreResponse;

            //Assert
            //TODO: assert raw,scaled & rank
            if (collaborate == 0 && create == 0 && compete == 0 && control == 0)
            {
                Assert.Equal(evaluatedScore.Results[0].Result, EvaluationResult.Applied);
            }
            else
            {
                Assert.NotEqual(evaluatedScore.Results[0].Result, EvaluationResult.Applied);
                Assert.Equal(evaluatedScore.Results[0].Result, EvaluationResult.FailedChecks);
            }
            if (collaborate < 10 && create < 10 && compete < 10 && control <= 10)
            {
                Assert.Equal(evaluatedScore.Results[1].Result, EvaluationResult.Applied);
            }
            else
            {
                Assert.NotEqual(evaluatedScore.Results[1].Result, EvaluationResult.Applied);
                Assert.Equal(evaluatedScore.Results[1].Result, EvaluationResult.FailedChecks);
            }

        }

        [Theory]
        [InlineData(10, 20, 30, 40)]
        [InlineData(40, 30, 20, 10)]
        [InlineData(40, 00, 20, 00)]
        [InlineData(00, 00, 00, 00)]
        public void ProcessScoreHandler_Test(int collaborate, int create, int compete, int control)
        {
            //setup
            var scoring = new Mock<IScoring>();            
            ScaledScore scaledScore = new ScaledScore
            {
                Collaborate = new CultureScore { Culture = Culture.Collaborate, Value = 0 },
                Create = new CultureScore { Culture = Culture.Create, Value = 0 },
                Compete = new CultureScore { Culture = Culture.Compete, Value = 0 },
                Control = new CultureScore { Culture = Culture.Control, Value = 0 }                
            };
            //below todo
            RankedCulture rankedCulture= new RankedCulture
            {
                //First = arr[3].Culture,
                //Second = arr[2].Culture,
                //Third = arr[1].Culture,
                //Fourth = arr[0].Culture,
            };

            scoring.Setup(x => x.Scale(It.IsAny<RawScore>())).Returns(scaledScore);
            scoring.Setup(x => x.Rank(It.IsAny<ScaledScore>())).Returns(rankedCulture);
            ScoresController scoresController = new ScoresController(scoring.Object);

            IActionResult serviceOutput = null;
            var requestOfProcessScore = new ProcessScoreRequest
            {
                Data = new Score
                {
                    Collaborate = collaborate,
                    Create = create,
                    Compete = compete,
                    Control = control
                }
            };

            //execute
            serviceOutput = scoresController.ProcessScoreHandler(requestOfProcessScore);
            var processedScore = (serviceOutput as OkObjectResult).Value as ProcessedScoreResponse;
            scoring.Verify(x => x.Scale(It.IsAny<RawScore>()), Times.Once);
            scoring.Verify(x => x.Rank(It.IsAny<ScaledScore>()), Times.Once);
        }

        [Theory]
        [InlineData(10, 20, 30, 40)]
        [InlineData(40, 30, 20, 10)]
        [InlineData(40, 00, 20, 00)]
        [InlineData(00, 00, 00, 00)]
        public void CheckRulesHandler_Test(int collaborate, int create, int compete, int control)
        {
            //completed
            //setup
            var scoring = new Mock<IScoring>();
            ScoresController scoresController = new ScoresController(scoring.Object);
            IActionResult serviceOutput = null;
            var requestOfCheckRules = new EvaluateScoreRequest
            {
                Data = new Score
                {
                    Collaborate = collaborate,
                    Create = create,
                    Compete = compete,
                    Control = control
                }
            };

            //execute
            serviceOutput = scoresController.CheckRulesHandler(requestOfCheckRules);
            var evaluatedScore = (serviceOutput as OkObjectResult).Value as EvaluatedScoreResponse;
            
            //Assert
            if (collaborate == 0 && create == 0 && compete == 0 && control == 0)
            {
                Assert.Equal(evaluatedScore.Results[0].Result, EvaluationResult.Applied);
            }
            else
            {
                Assert.NotEqual(evaluatedScore.Results[0].Result, EvaluationResult.Applied);
                Assert.Equal(evaluatedScore.Results[0].Result, EvaluationResult.FailedChecks);
            }
            if (collaborate < 10 && create < 10 && compete < 10 && control <= 10)
            {
                Assert.Equal(evaluatedScore.Results[1].Result, EvaluationResult.Applied);
            }
            else
            {
                Assert.NotEqual(evaluatedScore.Results[1].Result, EvaluationResult.Applied);
                Assert.Equal(evaluatedScore.Results[1].Result, EvaluationResult.FailedChecks);
            }
        }
    }
}
