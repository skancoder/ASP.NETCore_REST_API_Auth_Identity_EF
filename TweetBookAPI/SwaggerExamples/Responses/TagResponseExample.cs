using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBookAPI.Constracts.V1.Responses;

namespace TweetBookAPI.SwaggerExamples.Responses
{
    public class TagResponseExample : IExamplesProvider<TagResponse>
    {

        //to make this work we need to add "[ProducesResponseType(typeof(TagResponse),statusCode:201)],..." for the controller method
        public TagResponse GetExamples()
        {
            return new TagResponse
            {
                Name = "sample Tag name from Tag Response Example"
            };
        }
    }
}
