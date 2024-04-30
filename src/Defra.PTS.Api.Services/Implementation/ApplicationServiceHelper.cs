using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Application.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Application.Api.Services.Implementation
{
    public class ApplicationServiceHelper : IApplicationServiceHelper
    {
        public string GenerateUniqueAlphaNumericCode(int length)
        {
            const string chars = ApplicationConstant.AlphaNumericBase;

            // Use a GUID to ensure uniqueness
            Guid guid = Guid.NewGuid();
            byte[] bytes = guid.ToByteArray();

            char[] codeArray = new char[length];

            for (int i = 0; i < length; i++)
            {
                // Use bytes from the GUID modulo the length of the character set
                codeArray[i] = chars[bytes[i] % chars.Length];
            }

            return new string(codeArray);
        }
    }
}
