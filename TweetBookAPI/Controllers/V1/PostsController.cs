using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostsController(IPostService postService)
        {
            _postService = postService;
            
        }

        // GET: api/v1/posts
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _postService.GetPostsAsync());
        }
        // GET: api/v1/posts/{postId}
        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute]Guid postId)
        {
            var post =await _postService.GetPostByIdAsync(postId);
            if (post == null)
                return NotFound();

            return Ok(post);
        }
        

        // POST: api/v1/posts
        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)//Always Separate Domain Objects from Contracts
        {
            var post = new Post { Name = postRequest.Name};

            await _postService.CreatePostAsync(post);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            var response = new PostResponse { Id = post.Id };
            return Created(locationUri,response);
        }
        // PUT: api/v1/posts/{postId}
        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute]Guid postId,[FromBody] UpdatePostRequest request)
        {

            var post = new Post
            {
                Id = postId,
                Name = request.Name
            };
            var updated =await _postService.UpdatePostAsync(post);
            if (updated)
                return Ok(post);

            return NotFound();
        }
        // DELETE: api/v1/posts/{postId}
        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute]Guid postId)
        {
            var deleted =await _postService.DeletePostAsync(postId);
            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}
