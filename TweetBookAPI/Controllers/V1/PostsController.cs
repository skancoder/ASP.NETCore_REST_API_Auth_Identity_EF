using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TweetBookAPI.Constracts.V1;
using TweetBookAPI.Constracts.V1.Requests;
using TweetBookAPI.Constracts.V1.Responses;
using TweetBookAPI.Domain;
using TweetBookAPI.Services;

namespace TweetBookAPI.Controllers.V1
{
    //[Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostsController(IPostService postService)
        {
            _postService = postService;
            
        }

        // GET: api/v1/posts/{postId}
        [HttpGet(ApiRoutes.Posts.Get)]
        public IActionResult GetAll([FromRoute]Guid postId)
        {
            var post = _postService.GetPostById(postId);
            if (post == null)
                return NotFound();

            return Ok(post);
        }
        // GET: api/v1/posts
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult Get()
        {
            return Ok(_postService.GetPosts());
        }

        // POST: api/v1/posts
        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create([FromBody] CreatePostRequest postRequest)//Always Separate Domain Objects from Contracts
        {
            var post = new Post { Id = postRequest.Id };

            if (post.Id != Guid.Empty)
                post.Id = Guid.NewGuid();

            _postService.GetPosts().Add(post);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            var response = new PostResponse { Id = post.Id };
            return Created(locationUri,response);
        }
        // PUT: api/v1/posts/{postId}
        [HttpPut(ApiRoutes.Posts.Update)]
        public IActionResult Update([FromRoute]Guid postId,[FromBody] UpdatePostRequest request)
        {

            var post = new Post
            {
                Id = postId,
                Name = request.Name
            };
            var updated = _postService.UpdatePost(post);
            if (updated)
                return Ok(post);

            return NotFound();
        }
        // DELETE: api/v1/posts/{postId}
        [HttpDelete(ApiRoutes.Posts.Delete)]
        public IActionResult Delete([FromRoute]Guid postId)
        {
            var deleted = _postService.DeletePost(postId);
            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}
