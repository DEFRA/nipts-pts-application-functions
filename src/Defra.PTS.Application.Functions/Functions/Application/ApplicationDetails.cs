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
using applicationModel = Defra.PTS.Application.Models;
using applicationEntity = Defra.PTS.Application.Entities;

using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Defra.PTS.Application.Functions.Functions.Application;

/// <summary>
/// Get user applications
/// </summary>
/// <remarks>
/// Get application details
/// </remarks>
/// <param name="applicationService">The application service</param>
/// <param name="log">The log</param>
public class ApplicationDetails(IApplicationService applicationService, ILogger<ApplicationDetails> log)
{
    private readonly IApplicationService _applicationService = applicationService;
    private readonly ILogger<ApplicationDetails> _logger = log;


    /// <summary>
    /// GetApplicationDetails
    /// </summary>
    /// <param name="req"></param>    
    /// <returns></returns>
    /// <exception cref="ApplicationFunctionException"></exception>
    [FunctionName("GetApplicationDetails")]
    [OpenApiOperation(operationId: "GetApplicationDetails", tags: ["GetApplicationDetails"])]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(applicationModel.ApplicationDetail), Description = "GetApplicationDetails")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> GetApplicationDetails(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Applications/GetApplicationDetails")] HttpRequest req)
    {
        try
        {
            var inputData = req?.Body;
            if (inputData != null)
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                applicationEntity.ApplicationDetail applicationDetail = JsonConvert.DeserializeObject<applicationEntity.ApplicationDetail>(requestBody, new JsonSerializerSettings
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
            throw new ApplicationFunctionException("Invalid Application Id input, is NUll or Empty");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Stack: {StackTrace} \n Exception Message: {Message}", ex.StackTrace, ex.Message);

            return new BadRequestObjectResult("Failed to retrieve application details");
        }
    }
}

