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
        [InlineData(00,00,01,00)]
        [InlineData(100,100,100,100)]
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
                        
            //assert
            Assert.True(processedScore.Data.Scaled.Collaborate.Value % 1 == 0);
            Assert.True(processedScore.Data.Scaled.Create.Value % 1 == 0);
            Assert.True(processedScore.Data.Scaled.Compete.Value % 1 == 0);
            Assert.True(processedScore.Data.Scaled.Control.Value % 1 == 0);
            Dictionary<Culture,int> scores = new Dictionary<Culture, int> 
            {
                { Culture.Collaborate, collaborate },
                { Culture.Create, create },
                { Culture.Compete, compete },
                { Culture.Control, control },
            };
            var highestScoreCulture = scores.Where(x => x.Value == scores.Values.Max()).First().Key;
            Assert.True(processedScore.Data.Ranked.First == highestScoreCulture);

            scores = new Dictionary<Culture, int>
            {
                { Culture.Collaborate, (int)processedScore.Data.Scaled.Collaborate.Value },
                { Culture.Create, (int)processedScore.Data.Scaled.Create.Value },
                { Culture.Compete,  (int)processedScore.Data.Scaled.Compete.Value },
                { Culture.Control,  (int)processedScore.Data.Scaled.Control.Value },
            };
            
            if(!scores.Values.All(x=>x==0))
            Assert.True(scores.Where(x=>x.Key == highestScoreCulture).First().Value ==100);

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
        [InlineData(151, 160, 140, 50)]
        public void ProcessScoreHandler_Test(int collaborate, int create, int compete, int control)
        {
            //setup
            var scoring = new Mock<IScoring>();            
            ScaledScore scaledScore = new ScaledScore
            {
                Collaborate = new CultureScore { Culture = Culture.Collaborate, Value = 94 },
                Create = new CultureScore { Culture = Culture.Create, Value = 100 },
                Compete = new CultureScore { Culture = Culture.Compete, Value = 88 },
                Control = new CultureScore { Culture = Culture.Control, Value = 31 }                
            };
            //below todo
            RankedCulture rankedCulture= new RankedCulture
            {
                First = scaledScore.Create.Culture,
                Second = scaledScore.Collaborate.Culture,
                Third = scaledScore.Compete.Culture,
                Fourth = scaledScore.Control.Culture
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
