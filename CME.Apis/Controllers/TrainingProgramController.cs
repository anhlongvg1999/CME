﻿using CME.Business.Interfaces;
using CME.Business.Models;
using CME.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] Guid? trainingFormId,
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
                filterObject.FromDate = fromDate;
                filterObject.ToDate = toDate;
                filterObject.TrainingFormId = trainingFormId;

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
                var model = AutoMapperUtils.AutoMap<TrainingProgramRequestModel, TrainingProgram, TrainingProgram_UserRequestModel, TrainingProgram_User>(requestModel);
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

                model = AutoMapperUtils.AutoMap<TrainingProgramRequestModel, TrainingProgram, TrainingProgram_UserRequestModel, TrainingProgram_User>(requestModel, model);
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

        [HttpPost("checkin")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> Checkin([FromBody] TrainingProgram_User_CheckinRequestModel request)
        {
            return await ExecuteFunction(async () =>
            {
                return await _trainingProgramService.Checkin(request.TrainingProgramId, request.UserId, request.Active);
            });
        }

        [HttpGet("{id}/export-certifications")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]

        public async Task<IActionResult> ExportCertifications(Guid id)
        {
            var model = await _trainingProgramService.GetById(id);
            var listFiles = await _trainingProgramService.ExportCertifications(id, model);

            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < listFiles.Count; i++)
                    {
                        ziparchive.CreateEntryFromFile(listFiles[i].FilePath, listFiles[i].FileName);
                    }
                }
                return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
            }
        }
    }
}
