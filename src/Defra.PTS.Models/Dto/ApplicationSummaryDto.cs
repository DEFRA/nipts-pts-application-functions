using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Application.Models.Dto;

[ExcludeFromCodeCoverageAttribute]
public class ApplicationSummaryDto
{
    public Guid ApplicationId { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;

    public string PetName { get; set; } = string.Empty;

    public int PetSpeciesId { get; set; }

    public string OwnerName { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}
