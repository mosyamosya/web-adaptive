using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAdaptive.Models;
using WebAdaptive.Services.SeriesService;

namespace WebAdaptive.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]

    public class SeriesController : ControllerBase
    {
        private readonly ISeriesService _seriesService;

        public SeriesController(ISeriesService seriesService)
        {
            _seriesService = seriesService ?? throw new ArgumentNullException(nameof(seriesService));
        }

        // GET /series
        [HttpGet]
        public async Task<IActionResult> GetAllSeries()
        {
            try
            {
                var series = await _seriesService.GetAllSeries();
                var response = new ResponseModel<List<SeriesModel>>
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Data = series
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

        // GET /series/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeriesById(int id)
        {
            try
            {
                var series = await _seriesService.GetSeriesById(id);
                if (series == null)
                    return NotFound();

                var response = new ResponseModel<SeriesModel>
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Data = series
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

        // POST /series
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SeriesModel series)
        {
            try
            {
                if (series == null)
                    return BadRequest();

                await _seriesService.AddSeries(series);
                var responseCreated = new ResponseModel<SeriesModel>
                {
                    Message = "Series created successfully.",
                    StatusCode = HttpStatusCode.Created,
                    Data = series
                };
                return CreatedAtAction(nameof(GetSeriesById), new { id = series.Id }, responseCreated);
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

        // PUT /series/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SeriesModel series)
        {
            try
            {
                if (series == null || id != series.Id)
                    return BadRequest();

                await _seriesService.UpdateSeries(id, series);
                var responseNoContent = new ResponseModel<object>
                {
                    Message = "Series updated successfully.",
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

        // DELETE /series/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _seriesService.DeleteSeries(id);
                var responseNoContent = new ResponseModel<object>
                {
                    Message = "Series deleted successfully.",
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
