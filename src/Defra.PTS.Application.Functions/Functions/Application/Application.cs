using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
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
using Newtonsoft.Json;
using model = Defra.PTS.Application.Models;
using entity = Defra.PTS.Application.Entities;
using Defra.PTS.Application.Models.Dto;

namespace Defra.PTS.Application.Functions.Functions.Application
{
    public class Application
    {
        private readonly IApplicationService _applicationService;
        private readonly ITravelDocumentService _travelDocumentService;
        public Application(IApplicationService applicationService, ITravelDocumentService travelDocumentService)
        {
            _applicationService = applicationService;
            _travelDocumentService = travelDocumentService;
        }


        /// <summary>
        /// Create Application
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationFunctionException"></exception>
        [FunctionName("CreateApplication")]
        [OpenApiOperation(operationId: "CreateApplication", tags: new[] { "Create Application" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(model.Application), Description = "Create Application")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> CreateApplication(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "application")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var inputData = req?.Body;
                if (inputData == null)
                {
                    throw new ApplicationFunctionException("Invalid Application input, is NUll or Empty");
                }
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                entity.Application application = JsonConvert.DeserializeObject<entity.Application>(requestBody, new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Error, 
                    NullValueHandling = NullValueHandling.Ignore
                });

                var response = await _applicationService.CreateApplication(application);
                await _travelDocumentService.CreateTravelDocument(response);
                var responseDto = new ApplicationDto() { Id = response.Id, ReferenceNumber = response.ReferenceNumber };

                return new OkObjectResult(responseDto);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error Stack: ", ex.StackTrace);
                log.LogError(ex, "Exception Message: ", ex.Message);

                throw;
            }
        }
    }
}

