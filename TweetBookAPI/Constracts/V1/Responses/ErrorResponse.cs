using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetBookAPI.Constracts.V1.Responses
{
    //this will be used for every model error response instead of using "if(!ModelState.Valid)" in every controller end point.
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
