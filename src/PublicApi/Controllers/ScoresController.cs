using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using ApplicationCore.TransferObjects;
using ApplicationCore.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ScoresController: ControllerBase
{
    private readonly IScoring _scoring;

    public ScoresController(IScoring scoring)
    {
        _scoring = scoring;
    }

    [HttpPost("process")]
    public IActionResult ProcessScoreHandler([FromBody] ProcessScoreRequest request)
    {
        var raw = new RawScore(request.Data);
        var scaled = _scoring.Scale(raw);
        var ranked = _scoring.Rank(scaled);
        var response = new ProcessedScoreResponse { 
            Data = new ProcessedScore
            {
                Raw = raw,
                Scaled = scaled,
                Ranked = ranked
            }
        };
        return Ok(response);
    }

    [HttpPost("rules-evaluator/check")]
    public IActionResult CheckRulesHandler([FromBody] EvaluateScoreRequest request)
    {
        var rawScore = new RawScore(request.Data.Value);
        var allZeroResult = Evaluator.CheckAllZeroRule(rawScore);
        var allLowResult = Evaluator.CheckAllLowScoreRule(rawScore);
        var response = new EvaluatedScoreResponse
        {
            Results = new List<EvaluatedResult>
            {
                new() { Result = allZeroResult, Name = RuleType.AllZeros },
                new() { Result = allLowResult, Name = RuleType.AllLowScore }
            }
        };
        return Ok(response);
    }
}