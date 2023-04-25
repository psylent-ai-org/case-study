using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text.Json;
using ApplicationCore.TransferObjects;
using ApplicationCore.ValueObjects;
using Xunit;
using System.Text.Json.Serialization;
using ApplicationCore.Enums;
using Microsoft.VisualStudio.TestPlatform.Common;

namespace CaseStudy.IntegrationTests.Controllers;

public sealed class ScoresControllerTests : IDisposable
{
    private readonly WebApplicationFactory<Program> _application;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new();

    public ScoresControllerTests()
    {
        _application = new WebApplicationFactory<Program>();
        _client = _application.CreateClient();

        _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        _jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    }

    [Fact]
    public async Task ProcessScoreHandler_Test()
    {
        var requestData = new ProcessScoreRequest
        {
            Data = new Score
            {
                Control = 50,
                Collaborate = 151,
                Compete = 140,
                Create = 160
            }
        };

        using var response = await _client.PostAsJsonAsync("/scores/process", requestData);
        var responseData = await response.Content.ReadFromJsonAsync<ProcessedScoreResponse>(_jsonSerializerOptions);

        Assert.Equal((uint)50, responseData!.Data!.Raw!.Control.Value);
        Assert.Equal((uint)151, responseData!.Data!.Raw!.Collaborate.Value);
        Assert.Equal((uint)140, responseData!.Data!.Raw!.Compete.Value);
        Assert.Equal((uint)160, responseData!.Data!.Raw!.Create.Value);

        Assert.Equal((uint)31, responseData!.Data!.Scaled!.Control.Value);
        Assert.Equal((uint)94, responseData!.Data!.Scaled!.Collaborate.Value);
        Assert.Equal((uint)88, responseData!.Data!.Scaled!.Compete.Value);
        Assert.Equal((uint)100, responseData!.Data!.Scaled!.Create.Value);

        Assert.Equal(Culture.Create, responseData!.Data!.Ranked.First);
        Assert.Equal(Culture.Collaborate, responseData!.Data!.Ranked.Second);
        Assert.Equal(Culture.Compete, responseData!.Data!.Ranked.Third);
        Assert.Equal(Culture.Control, responseData!.Data!.Ranked.Fourth);
    }

    [Fact]
    public async Task CheckRulesHandler_Test()
    {
        var requestData = new EvaluateScoreRequest
        {
            Data = new Score
            {
                Control = 1,
                Collaborate = 2,
                Compete = 3,
                Create = 4
            }
        };

        using var response = await _client.PostAsJsonAsync("/scores/rules-evaluator/check", requestData);
        var responseData = await response.Content.ReadFromJsonAsync<EvaluatedScoreResponse>(_jsonSerializerOptions);

        Assert.Equal(EvaluationResult.FailedChecks, responseData!.Results[0].Result);
        Assert.Equal(RuleType.AllZeros, responseData!.Results[0].Name);

        Assert.Equal(EvaluationResult.Applied, responseData!.Results[1].Result);
        Assert.Equal(RuleType.AllLowScore, responseData!.Results[1].Name);
    }

    public void Dispose()
    {
        _application.Dispose();
        _client.Dispose();
    }
}
