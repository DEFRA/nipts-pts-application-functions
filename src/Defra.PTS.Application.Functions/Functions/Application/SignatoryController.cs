using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Application.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Threading.Tasks;

using System;

/// <summary>
/// Controller for handling signatory-related operations.
/// </summary>
public class SignatoryController
{
    private readonly ISignatoryService _signatoryService;
    private readonly ILogger<SignatoryController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignatoryController"/> class.
    /// </summary>
    /// <param name="signatoryService">The signatory service.</param>
    /// <param name="logger">The logger instance.</param>
    public SignatoryController(ISignatoryService signatoryService, ILogger<SignatoryController> logger)
    {
        _signatoryService = signatoryService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves the latest signatory.
    /// </summary>
    /// <param name="req">The HTTP request.</param>
    /// <returns>The latest signatory information.</returns>
    [FunctionName(nameof(GetLatestSignatory))]
    [OpenApiOperation(operationId: nameof(GetLatestSignatory), tags: ["Signatories"])]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SignatoryDto), Description = "OK")]
    public async Task<IActionResult> GetLatestSignatory(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Signatories/Latest")] HttpRequest req)
    {
        _logger.LogInformation("HTTP trigger function processed a request.", nameof(GetLatestSignatory));

        var signatoryDto = await _signatoryService.GetLatestSignatory();
        if (signatoryDto == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(signatoryDto);
    }

    /// <summary>
    /// Retrieves a signatory by ID.
    /// </summary>
    /// <param name="req">The HTTP request.</param>
    /// <param name="signatoryId">The signatory ID.</param>
    /// <returns>The signatory information.</returns>
    [FunctionName(nameof(GetSignatoryById))]
    [OpenApiOperation(operationId: nameof(GetSignatoryById), tags: ["Signatories"])]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SignatoryDto), Description = "OK")]
    public async Task<IActionResult> GetSignatoryById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Signatories/{signatoryId}")] HttpRequest req, Guid signatoryId)
    {
        _logger.LogInformation($"Retrieving signatory with ID: {signatoryId}");

        var signatoryDto = await _signatoryService.GetSignatoryById(signatoryId);
        if (signatoryDto == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(signatoryDto);
    }

    /// <summary>
    /// Retrieves a signatory by name.
    /// </summary>
    /// <param name="req">The HTTP request.</param>
    /// <param name="name">The signatory name.</param>
    /// <returns>The signatory information.</returns>
    [FunctionName(nameof(GetSignatoryByName))]
    [OpenApiOperation(operationId: nameof(GetSignatoryByName), tags: ["Signatories"])]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SignatoryDto), Description = "OK")]
    public async Task<IActionResult> GetSignatoryByName(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Signatories/ByName/{name}")] HttpRequest req, string name)
    {
        _logger.LogInformation($"Retrieving signatory with Name: {name}");

        var signatoryDto = await _signatoryService.GetSignatoryByName(name);
        if (signatoryDto == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(signatoryDto);
    }
}