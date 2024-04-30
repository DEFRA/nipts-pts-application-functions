using Defra.PTS.Application.Api.Services.Interface;
using Defra.PTS.Application.Models.Constants;

namespace Defra.PTS.Application.Api.Services.Implementation
{
    public class TravelDocumentServiceHelper : ITravelDocumentServiceHelper
    {
        string? result = null;
        public string GenerateUniqueAlphaNumericCode(int length)
        {
            while (result == null || ConditionSatisfied(result))
            {
                result = GetUniqueCode(length);                
            }
            result = "GB826" + result;
            return result;
        }

        public static string GetUniqueCode(int length)
        {
            const string chars = ApplicationConstant.AlphaNumericBase;            
            Guid guid = Guid.NewGuid();
            byte[] bytes = guid.ToByteArray();

            char[] codeArray = new char[length];

            for (int i = 0; i < length; i++)            {

                codeArray[i] = chars[bytes[i] % chars.Length];
            }
            return new string(codeArray);
        }

        
        static bool ConditionSatisfied(string result)
        {
            if (result.StartsWith("AD") || result.Any(c => c >= 'G' && c <= 'Z'))
                return true;
            else
                return false;
        }
    }
    

}
