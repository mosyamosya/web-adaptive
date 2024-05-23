using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAdaptive.Models;
using WebAdaptive.Services.DataRandomizerService;

namespace WebAdaptive.Controllers.VersionedDataController.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v1.2")]
    [Route("V{version:apiVersion}/[controller]")]
    [Authorize]
    public class DataRandomizerController : ControllerBase
    {
        private readonly IDataRandomizerService _dataRandomizerService;
        public DataRandomizerController(IDataRandomizerService dataRandomizerService)
        {
            _dataRandomizerService = dataRandomizerService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRandomData()
        {
            try
            {
                var randomText = _dataRandomizerService.GetTextValue();
                var response = new ResponseModel<object>
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Data = randomText
                };
                return Ok(response);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }

        }
    }
}