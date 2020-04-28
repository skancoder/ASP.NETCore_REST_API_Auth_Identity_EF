using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TweetBookAPI.Constants.V1;
using TweetBookAPI.Domain;

namespace TweetBookAPI.Controllers.V1
{
    //[Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private List<Post> _posts;
        public PostsController()
        {
            _posts = new List<Post>();
            for(var i = 0; i < 5; i++)
            {
                _posts.Add(new Post { Id = Guid.NewGuid().ToString() });
            }
            
        }

        // GET: api/v1/Posts
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_posts);
        }
    }
}
