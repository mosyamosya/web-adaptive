using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAdaptive.Models;
using WebAdaptive.Services.DataRandomizerService;

namespace WebAdaptive.Controllers.VersionedDataController.V3
{
    [ApiController]
    [ApiVersion("3.0")]
    [ApiExplorerSettings(GroupName = "v1.3")]
    [Route("V{version:apiVersion}/[controller]")]
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
                byte[] fileBytes = _dataRandomizerService.GenerateExcelFile();
                string contentType = "application/octet-stream";
                return File(fileBytes, contentType, "new_generated.xlsx");
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }

        }
    }
}
