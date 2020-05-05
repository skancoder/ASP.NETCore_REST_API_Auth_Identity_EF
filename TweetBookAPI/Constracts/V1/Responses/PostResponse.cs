using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBookAPI.Domain;

namespace TweetBookAPI.Constracts.V1.Responses
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public virtual IEnumerable<TagResponse> Tags { get; set; }
    }
}
