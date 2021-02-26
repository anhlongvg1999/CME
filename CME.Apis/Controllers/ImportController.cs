using ClosedXML.Excel;
using CME.Business.Interfaces;
using CME.Business.Models;
using CME.Entities;
using CME.Entities.Constants;
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
            return await ExecuteFunction(async () =>
            {
                using (var transaction = _dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        // Đoc excel
                        var workbook = new XLWorkbook(requestModel.File.OpenReadStream());
                        IXLWorksheet worksheet;

                        if (workbook.Worksheets.TryGetWorksheet(requestModel.SheetName, out worksheet))
                        {
                            // Xóa tất cả user trong db
                            var oldUsers = _dataContext.Users.ToList();
                            _dataContext.RemoveRange(oldUsers);
                            await _dataContext.SaveChangesAsync();

                            var oldTitles = _dataContext.Titles.ToList();
                            _dataContext.RemoveRange(oldTitles);
                            await _dataContext.SaveChangesAsync();


                            var oldDepartments = _dataContext.Departments.ToList();
                            _dataContext.RemoveRange(oldDepartments);
                            await _dataContext.SaveChangesAsync();


                            var newTitles = new List<Title>();
                            var newDepartments = new List<Department>();

                            var totalRows = worksheet.RowsUsed().Count();

                            for (int i = 3; i <= totalRows; i++)
                            {
                                //Họ và tên
                                string fullname = worksheet.Cell(i, 2).Value.ToString().Trim();
                                string certificationNumber = worksheet.Cell(i, 3).Value.ToString().Trim();
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
                                user.Firstname = words[words.Length - 1];
                                user.CertificateNumber = certificationNumber;
                                user.IssueDate = issueDate;
                                user.OrganizationId = Guid.Parse(Default.OrganizationId);
                                user.Type = UserType.INTERNAL;

                                var passwordHasher = new PasswordHasher<User>();
                                user.Password = passwordHasher.HashPassword(user, Default.Password);
                                string nameTitle = worksheet.Cell(i, 6).Value.ToString().Trim();
                                string nameDepartment = worksheet.Cell(i, 7).Value.ToString().Trim();

                                var department = newDepartments.Where(x => nameDepartment.Contains(x.Name)).FirstOrDefault();
                                var title = newTitles.Where(x => nameTitle.Contains(x.Name)).FirstOrDefault();

                                if (title == null)
                                {
                                    title = new Title();
                                    title.Id = Guid.NewGuid();
                                    title.Name = nameTitle;

                                    newTitles.Add(title);
                                    await _dataContext.Titles.AddAsync(title);
                                }

                                if (department == null)
                                {

                                    department = new Department();
                                    department.Id = Guid.NewGuid();
                                    department.Name = nameDepartment;
                                    department.OrganizationId = Guid.Parse(Default.OrganizationId);

                                    newDepartments.Add(department);
                                    await _dataContext.Departments.AddAsync(department);
                                }

                                user.TitleId = title.Id;
                                user.DepartmentId = department.Id;
                                await _dataContext.AddAsync(user);
                            }
                        }
                        else
                        {
                            throw new ArgumentException($"Sheet {requestModel.SheetName} không tồn tại");
                        }

                        await _dataContext.SaveChangesAsync();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ArgumentException(ex.Message);
                    }
                }
            });
        }

    }
}
