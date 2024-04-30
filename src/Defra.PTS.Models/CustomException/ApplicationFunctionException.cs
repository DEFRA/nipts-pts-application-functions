using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Application.Models.CustomException
{
    [ExcludeFromCodeCoverageAttribute]
    public class ApplicationFunctionException : Exception
    {
        public ApplicationFunctionException() { }

        public ApplicationFunctionException(string message) : base(message) { }

        public ApplicationFunctionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
