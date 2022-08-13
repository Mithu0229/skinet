using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiValidationErrorReponse : ApiResponse//for error
    {
        public ApiValidationErrorReponse() : base(400)
        {
        }
        public IEnumerable<string> Errors { get; set; }
    }
}