using Defra.PTS.Application.Entities;
using Defra.PTS.Application.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity = Defra.PTS.Application.Entities;

namespace Defra.PTS.Application.Api.Services.Interface
{
    public interface IApplicationService
    {
        Task<VwApplication?> GetApplicationDetails(Guid applicationId);
        Task<IEnumerable<ApplicationSummaryDto>> GetApplicationsForUser(Guid userId);

        entity.Application GetApplication(Guid id);

        Task<entity.Application> CreateApplication(entity.Application application);
        Task<bool> PerformHealthCheckLogic();
    }
}
