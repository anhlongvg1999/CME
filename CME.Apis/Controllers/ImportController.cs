using ClosedXML.Excel;
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
    public class ImportController : ApiControllerBase
    {

        public ImportController()
        {
            
        }

        [HttpPost("import-user")]
        [ProducesResponseType(typeof(TitleViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ImportUser([FromForm] ImportUserRequestModel requestModel)
        {
            var workbook = new XLWorkbook(requestModel.File.OpenReadStream());
            IXLWorksheet worksheet = null; 

            if(workbook.Worksheets.TryGetWorksheet("2019-2020- 2021 (Bỏ hộ lý)", out worksheet))
            {
                var totalRows = worksheet.RowsUsed().Count();

                for (int i = 3; i <= totalRows; i++)
                {
                    //Họ và tên
                    string fullname = worksheet.Cell(i, 2).Value.ToString();
                    string certificationNumber = worksheet.Cell(i, 3).Value.ToString();
                    string issueDateStr = worksheet.Cell(i, 5).Value.ToString();
                    DateTime issueDate = DateTime.FromOADate(double.Parse(issueDateStr.ToString()));


                    for (int j = 2; j <= 6; j++)
                    {
                        var value = worksheet.Cell(i, j).Value.ToString();
                        if (value.Equals(""))
                        {
                            break;
                        }
                        //if (!int.TryParse(worksheet.Cell(i, j).Value.ToString(), out pointsArray[-2 + i, 42 - j]))
                        //{
                        //    //MessageBox.Show("Error reading points" + i + " " + j);
                        //    break;
                        //}
                    }
                }
            }

            return null;
            //return await ExecuteFunction(async () =>
            //{
            //    var model = AutoMapperUtils.AutoMap<TitleRequestModel, Title>(requestModel);
            //    return await _titleService.SaveAsync(model);
            //});
        }

    }
}
