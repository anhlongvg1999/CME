using CME.Business.Interfaces;
using CME.Business.Models;
using CME.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tsoft.Framework.Common;
using TSoft.Framework.ApiUtils.Controllers;

namespace CME.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadCSVController : ApiControllerBase
    {
        private readonly IReadCSVService _readCSVService;
        public ReadCSVController(IReadCSVService readCSVService)
        {
            _readCSVService = readCSVService;
        }
        [HttpPost("")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReadData([FromBody] List<DataCSV> requestModel)
        {
            var a = requestModel.Select(x=>x.Lot).ToArray();
            return await ExecuteFunction(async () =>
            {
                var result = await _readCSVService.GetDataCSV(requestModel);
                return result;
            });
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PathCSVViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid id)
        {
            return await ExecuteFunction(async () =>
            {
                var result = await _readCSVService.GetById(id);
                return AutoMapperUtils.AutoMap<FileCSV, PathCSVViewModel>(result);
            });
        }
        [HttpGet("")]
        [ProducesResponseType(typeof(List<DataChart>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDataChart([FromQuery] DataChartQueryModel requestModel)
        {
            return await ExecuteFunction(async () =>
            {
                var result = await _readCSVService.GetDataChart(requestModel);
                return result;
            });
        }
    }
}
