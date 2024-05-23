using Microsoft.AspNetCore.Mvc;
using WebAdaptive.Models;
using WebAdaptive.Services.CommentService;
using WebAdaptive.Services.ApiService;
using System.Net;
using System.Text.Json;

namespace WebAdaptive.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IApiService _apiService;

        public CommentController(ICommentService commentService, IApiService apiService)
        {
            _commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
        }

        private async Task<string> CorrectTextAsync(string text)
        {
            // Make request to API for text correction
            var correctedText = await _apiService.GetCorrectAsync(text);
            var json = JsonDocument.Parse(correctedText);
            var correctedSentence = json.RootElement.GetProperty("response").GetProperty("corrected").GetString();

            correctedSentence = correctedSentence.Replace("\"", "");
            return correctedSentence;
        }

        // GET /comment
        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            try
            {
                var comments = await _commentService.GetAllComments();
                var response = new ResponseModel<List<CommentModel>>
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Data = comments
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var responseError = new ResponseModel<object>
                {
                    Message = $"Internal server error: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, responseError);
            }
        }

        // GET /comment/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            try
            {
                var comment = await _commentService.GetCommentById(id);
                if (comment == null)
                    return NotFound();

                var response = new ResponseModel<CommentModel>
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Data = comment
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var responseError = new ResponseModel<object>
                {
                    Message = $"Internal server error: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, responseError);
            }
        }

        // POST /comment
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CommentModel comment)
        {
            try
            {
                if (comment == null)
                    return BadRequest();

                // Correct content before adding or updating the comment
                var correctedContent = await CorrectTextAsync(comment.Content);
                comment.Content = correctedContent;

                await _commentService.AddComment(comment);
                var responseCreated = new ResponseModel<CommentModel>
                {
                    Message = "Comment created successfully.",
                    StatusCode = HttpStatusCode.Created,
                    Data = comment
                };
                return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
            }
            catch (Exception ex)
            {
                var responseError = new ResponseModel<object>
                {
                    Message = $"Internal server error: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, responseError);
            }
        }

        // PUT /comment/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CommentModel comment)
        {
            try
            {
                if (comment == null || id != comment.Id)
                    return BadRequest();

                // Correct content before adding or updating the comment
                var correctedContent = await CorrectTextAsync(comment.Content);
                comment.Content = correctedContent;

                await _commentService.UpdateComment(id, comment);
                var responseNoContent = new ResponseModel<object>
                {
                    Message = "Comment updated successfully.",
                    StatusCode = HttpStatusCode.NoContent,
                    Data = null
                };
                return NoContent();
            }
            catch (Exception ex)
            {
                var responseError = new ResponseModel<object>
                {
                    Message = $"Internal server error: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, responseError);
            }
        }


        // DELETE /comment/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _commentService.DeleteComment(id);
                var responseNoContent = new ResponseModel<object>
                {
                    Message = "Comment deleted successfully.",
                    StatusCode = HttpStatusCode.NoContent,
                    Data = null
                };
                return NoContent();
            }
            catch (Exception ex)
            {
                var responseError = new ResponseModel<object>
                {
                    Message = $"Internal server error: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, responseError);
            }
        }
    }
}
