using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Application.Entities
{
    [ExcludeFromCodeCoverageAttribute]
    public class TravelDocument
    {
        [Key]
        public Guid Id { get; set; }
        public int? IssuingAuthorityId { get; set; }
        public Guid PetId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid ApplicationId { get; set; }        
        public byte[]? QRCode { get; set; }
        public string DocumentReferenceNumber { get; set; } = string.Empty;
        public bool? IsLifeTIme { get; set; }
        public DateTime? ValidityStartDate { get; set; }
        public DateTime? ValidityEndDate { get; set; }
        public int? StatusId { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string DocumentSignedBy { get; set; } = string.Empty;
        
    }
}