using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Application.Models
{
    [ExcludeFromCodeCoverageAttribute]
    public class Application
    {
        public Guid Id { get; set; }
        public Guid PetId { get; set; }
        public Guid UserId { get; set; }
        public Guid OwnerId { get; set; }
        public string? OwnerNewName { get; set; }
        public string? OwnerNewTelephone { get; set; }
        public Guid? OwnerAddressId { get; set; }
        public string? Status { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public bool IsDeclarationSigned { get; set; }
        public bool IsConsentAgreed { get; set; }
        public bool IsPrivacyPolicyAgreed { get; set; }
        public DateTime DateOfApplication { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}
