using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Application.Api.Services.Interface
{
    public interface IApplicationServiceHelper
    {
        string GenerateUniqueAlphaNumericCode(int length);
    }
}
