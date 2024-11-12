using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Application.Models.Dto
{
    [ExcludeFromCodeCoverageAttribute]
    public class SignatoryDto
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public byte[]? SignatureImage { get; set; }
    }
}
