using ClosedXML.Excel;
using CME.Business.Interfaces;
using CME.Business.Models;
using CME.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SERP.Filenet.DB;
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
        private DataContext _dataContext;
        public ImportController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost("import-user")]
        [ProducesResponseType(typeof(TitleViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ImportUser([FromForm] ImportUserRequestModel requestModel)
        {
            //using (var transaction = _dataContext.Database.BeginTransaction())
            //{
                try
                {
                    var oldTitles = _dataContext.Titles.ToList();
                    _dataContext.RemoveRange(oldTitles);

                    var oldDepartments = _dataContext.Departments.ToList();
                    _dataContext.RemoveRange(oldDepartments);
                    // Xóa tất cả user trong db
                    var oldUsers = _dataContext.Users.ToList();
                    _dataContext.RemoveRange(oldUsers);
                    _dataContext.SaveChanges();

                    // Đoc excel
                    var workbook = new XLWorkbook(requestModel.File.OpenReadStream());
                    IXLWorksheet worksheet = null;

                    if (workbook.Worksheets.TryGetWorksheet("2019-2020- 2021 (Bỏ hộ lý)", out worksheet))
                    {
                        var totalRows = worksheet.RowsUsed().Count();

                        for (int i = 3; i <= totalRows; i++)
                        {
                            //Họ và tên
                            string fullname = worksheet.Cell(i, 2).Value.ToString();
                            string certificationNumber = worksheet.Cell(i, 3).Value.ToString();
                            string issueDateStr = worksheet.Cell(i, 5).Value.ToString();
                            DateTime issueDate;
                            try
                            {
                                issueDate = DateTime.FromOADate(double.Parse(issueDateStr.ToString()));
                            }
                            catch
                            {
                                issueDate = new DateTime();
                            }
                        var words = fullname.Trim().Split(' ');

                        var user = new User();
                            user.Id = Guid.NewGuid();
                            user.Fullname = fullname;
                            user.Username = "vvvv";
                            user.Firstname= words[words.Length - 1];
                            user.CertificateNumber = certificationNumber;
                            user.IssueDate = issueDate;
                            user.OrganizationId = Guid.Parse("00000000-0000-0000-0000-000000000001");

                            var passwordHasher = new PasswordHasher<User>();
                            user.Password = passwordHasher.HashPassword(user, "123456a@");
                            string nameTitle = worksheet.Cell(i, 6).Value.ToString();
                            string nameDepartment = worksheet.Cell(i, 7).Value.ToString();
                            var nameKhoaPhong= _dataContext.Departments.Where(x => nameDepartment.Contains(x.Name)).ToList();
                            var nameChucDanh = _dataContext.Titles.Where(x => nameTitle.Contains(x.Name)).ToList();
                            if(nameChucDanh.Count() >0)
                            {

                            }
                            else
                            {
                                var title = new Title();
                                title.Id = Guid.NewGuid();
                                title.Name = nameTitle;
                                await _dataContext.Titles.AddAsync(title);
                            _dataContext.SaveChanges();
                        }
                            if (nameKhoaPhong.Count() > 0)
                            {

                            }
                            else
                            {
                                var department = new Department();
                                department.Id = Guid.NewGuid();
                                department.Name = nameDepartment;
                                department.OrganizationId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                                await _dataContext.Departments.AddAsync(department);
                            _dataContext.SaveChanges();
                        }
                            //Check name title in title array
                            //If not exist: Add title to array
                            //Else: user.TitleId = title.Id


                            await _dataContext.AddAsync(user);
                        }
                    }
                       
                    _dataContext.SaveChanges();
                    return null;
                    //transaction.Commit();
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    Console.WriteLine("Error occurred.");
                return null;
            }
            //}
            //return null;
            //return await ExecuteFunction(async () =>
            //{
            //    var model = AutoMapperUtils.AutoMap<TitleRequestModel, Title>(requestModel);
            //    return await _titleService.SaveAsync(model);
            //});
        }

    }
}
