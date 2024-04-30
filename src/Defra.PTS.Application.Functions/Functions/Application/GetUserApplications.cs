using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Application.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Defra.PTS.Application.Functions.Functions.Application;

/// <summary>
/// Get user applications
/// </summary>
public class GetUserApplications
{
    private readonly IApplicationService _applicationService;
    private readonly ILogger<GetUserApplications> _logger;

    /// <summary>
    /// Get user applications
    /// </summary>
    /// <param name="applicationService">The application service</param>
    /// <param name="log">The log</param>
    public GetUserApplications(IApplicationService applicationService, ILogger<GetUserApplications> log)
    {
        _applicationService = applicationService;
        _logger = log;
    }

    [FunctionName(nameof(GetUserApplications))]
    [OpenApiOperation(operationId: nameof(GetUserApplications), tags: new[] { "Applications" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **UserId** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<ApplicationSummaryDto>), Description = "OK")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType:typeof(string), Description = "BAD REQUEST")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Applications/UserApplications/{userId}")] HttpRequest req, string userId)
    {
        _logger.LogInformation("HTTP trigger function processed a request.",nameof(GetUserApplications));

        if (!Guid.TryParse(userId, out Guid userGuid))
        {
            return new BadRequestObjectResult("You must provide a valid value for userId");
        }

        var result = await _applicationService.GetApplicationsForUser(userGuid);

        return new OkObjectResult(result);
    }
}

