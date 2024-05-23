using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAdaptive.Models;
using WebAdaptive.Services.DataRandomizerService;

namespace WebAdaptive.Controllers.VersionedDataController.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1.1")]
    [Route("V{version:apiVersion}/[controller]")]
    [Obsolete("This version is deprecated.")]
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
                var intValue = _dataRandomizerService.GetIntegerValue();
                var response = new ResponseModel<object>
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Data = intValue
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
