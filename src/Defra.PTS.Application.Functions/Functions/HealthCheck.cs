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
    public class HealthCheck
    {
        private readonly IApplicationService _applicationService;
        public HealthCheck(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [FunctionName("HealthCheck")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequest req
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
