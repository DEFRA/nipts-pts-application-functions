using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
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

namespace Defra.PTS.Application.Functions.Application
{
    /// <summary>
    /// Controller for handling signatory-related operations.
    /// </summary>
    public class SignatoryController
    {
        private readonly ISignatoryService _signatoryService;
        private const string InvalidRequestBodyMessage = "Invalid request body, is NULL or Empty";
        private const string ExceptionOccurredMessage = "An exception occurred";

        /// <summary>
        /// Initializes a new instance of the <see cref="SignatoryController"/> class.
        /// </summary>
        /// <param name="signatoryService">The signatory service.</param>
        public SignatoryController(ISignatoryService signatoryService)
        {
            _signatoryService = signatoryService;
        }

        /// <summary>
        /// Retrieves the latest signatory.
        /// </summary>
        /// <returns>The latest signatory information.</returns>
        [FunctionName("GetLatestSignatory")]
        [OpenApiOperation(operationId: "GetLatestSignatory", tags: new[] { "GetLatestSignatory" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SignatoryDto), Description = "OK")]
        public async Task<IActionResult> GetLatestSignatory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "signatories/latest")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var signatoryDto = await _signatoryService.GetLatestSignatory();
                return new OkObjectResult(signatoryDto);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ExceptionOccurredMessage);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }



        /// <summary>
        /// Retrieves a signatory by ID.
        /// </summary>
        /// <returns>The signatory information.</returns>
        [FunctionName("GetSignatoryById")]
        [OpenApiOperation(operationId: "GetSignatoryById", tags: new[] { "GetSignatoryById" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(SignatoryRequestIdDto), Description = "Signatory request data")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SignatoryDto), Description = "OK")]
        public async Task<IActionResult> GetSignatoryById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "signatories/getbyid")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var input = (req?.Body) ?? throw new InvalidDataException(InvalidRequestBodyMessage);
                string requestBody = await new StreamReader(input).ReadToEndAsync();

                var signatoryRequest = JsonSerializer.Deserialize<SignatoryRequestIdDto>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (signatoryRequest == null || signatoryRequest.Id == Guid.Empty)
                {
                    throw new JsonException("Cannot deserialize request body or Id is missing");
                }

                var signatoryDto = await _signatoryService.GetSignatoryById(signatoryRequest.Id);

                return new OkObjectResult(signatoryDto);
            }
            catch (InvalidDataException ex)
            {
                log.LogError(ex, ExceptionOccurredMessage);
                return new BadRequestObjectResult(InvalidRequestBodyMessage);
            }
            catch (JsonException ex)
            {
                log.LogError(ex, ExceptionOccurredMessage);
                return new BadRequestObjectResult("Cannot deserialize request body or Id is missing");
            }
            catch (Exception ex)
            {
                log.LogError(ex, ExceptionOccurredMessage);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves a signatory by name.
        /// </summary>
        /// <returns>The signatory information.</returns>
        [FunctionName("GetSignatoryByName")]
        [OpenApiOperation(operationId: "GetSignatoryByName", tags: new[] { "GetSignatoryByName" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(SignatoryRequestNameDto), Description = "Signatory request data")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SignatoryDto), Description = "OK")]
        public async Task<IActionResult> GetSignatoryByName(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "signatories/byname")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var input = (req?.Body) ?? throw new InvalidDataException(InvalidRequestBodyMessage);
                string requestBody = await new StreamReader(input).ReadToEndAsync();

                var signatoryRequest = JsonSerializer.Deserialize<SignatoryRequestNameDto>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (signatoryRequest == null || string.IsNullOrWhiteSpace(signatoryRequest.Name))
                {
                    throw new JsonException("Cannot deserialize request body or Name is missing");
                }

                var signatoryDto = await _signatoryService.GetSignatoryByName(signatoryRequest.Name);

                return new OkObjectResult(signatoryDto);
            }
            catch (InvalidDataException ex)
            {
                log.LogError(ex, ExceptionOccurredMessage);
                return new BadRequestObjectResult(InvalidRequestBodyMessage);
            }
            catch (JsonException ex)
            {
                log.LogError(ex, ExceptionOccurredMessage);
                return new BadRequestObjectResult("Cannot deserialize request body or Name is missing");
            }
            catch (Exception ex)
            {
                log.LogError(ex, ExceptionOccurredMessage);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
