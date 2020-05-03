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
using TweetBookAPI.Domain;
using TweetBookAPI.Extensions;
using TweetBookAPI.Services;

namespace TweetBookAPI.Controllers.V1
{
    //[Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles ="Admin,Poster")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private IPostService _postService;

        public TagsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _postService.GetAllTagsAsync());
        }

        [HttpGet(ApiRoutes.Tags.Get)]
        public async Task<IActionResult> Get([FromRoute]string tagName)
        {
            var tag = await _postService.GetTagByNameAsync(tagName);

            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        [HttpPost(ApiRoutes.Tags.Create)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
        {
            var newTag = new Tag
            {
                Name = request.TagName,
                CreatorId = HttpContext.GetUserId(),
                CreatedOn = DateTime.UtcNow
            };

            var created = await _postService.CreateTagAsync(newTag);
            if (!created)
            {
                return BadRequest(new { error = "Unable to create tag" });
            }

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Tags.Get.Replace("{tagName}", newTag.Name);
            return Created(locationUri, newTag);
        }


        [HttpDelete(ApiRoutes.Tags.Delete)]
        [Authorize(Roles = "Admin")]//only Admin

        //[Authorize(Roles = "Admin,Poster")]//Admin or Poster

        //[Authorize(Roles = "Admin")]//Admin and
        //[Authorize(Roles = "Poster")]//Poster

        public async Task<IActionResult> Delete([FromRoute] string tagName)
        {
            var deleted = await _postService.DeleteTagAsync(tagName);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}