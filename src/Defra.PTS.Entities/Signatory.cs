using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Application.Entities
{
    [ExcludeFromCodeCoverageAttribute]
    public class Signatory
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public byte[]? SignatureImage { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

}
