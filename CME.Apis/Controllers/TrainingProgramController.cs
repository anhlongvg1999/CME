using CME.Business.Interfaces;
using CME.Business.Models;
using CME.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Tsoft.Framework.Common;
using TSoft.Framework.ApiUtils.Controllers;
using TSoft.Framework.DB;

namespace CME.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingProgramController : ApiControllerBase
    {
        private readonly ITrainingProgramService _trainingProgramService;

        public TrainingProgramController(ITrainingProgramService trainingProgramService)
        {
            _trainingProgramService = trainingProgramService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(Pagination<TrainingProgramViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int currentPage = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sort = "",
            [FromQuery] string queryString = "{ }")
        {
            return await ExecuteFunction(async () =>
            {
                var filterObject = JsonSerializer.Deserialize<TrainingProgramQueryModel>(queryString);
                filterObject.CurrentPage = currentPage;
                filterObject.PageSize = pageSize;
                filterObject.Sort = sort;

                return await _trainingProgramService.GetAllAsync(filterObject);
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TrainingProgramViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid id)
        {
            return await ExecuteFunction(async () =>
            {
                var result = await _trainingProgramService.GetById(id);
                return AutoMapperUtils.AutoMap<TrainingProgram, TrainingProgramViewModel>(result); ;
            });
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(TrainingProgramViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(TrainingProgramRequestModel requestModel)
        {
            return await ExecuteFunction(async () =>
            {
                var model = AutoMapperUtils.AutoMap<TrainingProgramRequestModel, TrainingProgram>(requestModel);
                return await _trainingProgramService.SaveAsync(model, requestModel.TrainingProgram_Users);
            });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TrainingProgramViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] TrainingProgramRequestModel requestModel)
        {
            return await ExecuteFunction(async () =>
            {
                var model = await _trainingProgramService.GetById(id);

                if (model == null)
                {
                    throw new ArgumentException($"Id {id} không tồn tại");
                }

                model = AutoMapperUtils.AutoMap<TrainingProgramRequestModel, TrainingProgram>(requestModel, model);
                return await _trainingProgramService.SaveAsync(model, requestModel.TrainingProgram_Users);
            });
        }

        [HttpPost("delete/many")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteMany([FromBody] Guid[] deleteIds)
        {
            return await ExecuteFunction(async () =>
            {
                return await _trainingProgramService.DeleteManyAsync(deleteIds);
            });
        }
    }
}
