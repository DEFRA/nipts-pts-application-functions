using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Application.Models.CustomException;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using model = Defra.PTS.Application.Models;
using entity = Defra.PTS.Application.Entities;

using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Defra.PTS.Application.Functions.Functions.Application;

/// <summary>
/// Get user applications
/// </summary>
public class ApplicationDetails
{
    private readonly IApplicationService _applicationService;
    private readonly ILogger<ApplicationDetails> _logger;

    /// <summary>
    /// Get application details
    /// </summary>
    /// <param name="applicationService">The application service</param>
    /// <param name="log">The log</param>
    public ApplicationDetails(IApplicationService applicationService, ILogger<ApplicationDetails> log)
    {
        _applicationService = applicationService;
        _logger = log;
    }


    /// <summary>
    /// GetApplicationDetails
    /// </summary>
    /// <param name="req"></param>    
    /// <returns></returns>
    /// <exception cref="ApplicationFunctionException"></exception>
    [FunctionName("GetApplicationDetails")]
    [OpenApiOperation(operationId: "GetApplicationDetails", tags: new[] { "GetApplicationDetails" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(model.ApplicationDetail), Description = "GetApplicationDetails")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> GetApplicationDetails(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Applications/GetApplicationDetails")] HttpRequest req)
    {
        try
        {
            var inputData = req?.Body;
            if (inputData == null)
            {
                throw new ApplicationFunctionException("Invalid Application Id input, is NUll or Empty");
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            entity.ApplicationDetail applicationDetail = JsonConvert.DeserializeObject<entity.ApplicationDetail>(requestBody, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error,
                NullValueHandling = NullValueHandling.Ignore
            });

            var response = await _applicationService.GetApplicationDetails(applicationDetail.ApplicationId);
            if (response == null)
            {
                return new NotFoundObjectResult($"No application found for id {applicationDetail.ApplicationId}");
            }

            return new OkObjectResult(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Stack: ", ex.StackTrace);
            _logger.LogError(ex, "Exception Message: ", ex.Message);

            throw;
        }
    }
}

