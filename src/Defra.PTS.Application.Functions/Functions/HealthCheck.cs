using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Defra.PTS.Application.Api.Services.Interface;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace Defra.PTS.Functions.Functions
{
    /// <summary>
    /// Health check endpoint
    /// </summary>
    /// <remarks>
    /// Health check dependancies
    /// </remarks>
    /// <param name="applicationService"></param>
    public class HealthCheck(IApplicationService applicationService)
    {
        private readonly IApplicationService _applicationService = applicationService;

        /// <summary>
        /// Check service health
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("HealthCheck")]
        [OpenApiOperation(operationId: "Run", tags: ["name"])]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
#pragma warning disable IDE0060 // Remove unused parameter
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequest req
#pragma warning restore IDE0060 // Remove unused parameter
            , ILogger log)
        {
            log.LogInformation("Health Check Trigger.");

            // Perform health check logic here
            bool isHealthy = await _applicationService.PerformHealthCheckLogic();

            if (isHealthy)
            {
                return new OkResult();
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
            }
        }
    }
}
