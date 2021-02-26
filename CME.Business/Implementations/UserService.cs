﻿using CME.Business.Interfaces;
using CME.Business.Models;
using Microsoft.EntityFrameworkCore;
using SERP.Filenet.DB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.DB;
using System.Linq;
using Tsoft.Framework.Common;
using CME.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using CME.Entities.Constants;

namespace CME.Business.Implementations
{
    public class UserService : IUserService
    {
        private const string CachePrefix = "user-";
        private readonly DataContext _dataContext;
        private readonly IOrganizationService _organizationService;
        private readonly ITitleService _titleService;
        private readonly IDepartmentService _departmentService;
        private readonly IHostingEnvironment _environment;
        //private readonly ICacheService _cacheService;

        //TODO: CACHE
        public UserService(DataContext dataContext, IOrganizationService organizationService, ITitleService titleService, IDepartmentService departmentService, IHostingEnvironment environment)
        {
            _dataContext = dataContext;
            _organizationService = organizationService;
            _titleService = titleService;
            _departmentService = departmentService;
            _environment = environment;
        }
        public async Task<Pagination<UserViewModel>> GetAllAsync(UserQueryModel queryModel)
        {

            var query = from u in _dataContext.Users.AsNoTracking().Include(x => x.Department).Include(x => x.Organization).Include(x => x.Title)
                        select new UserViewModel
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Fullname = u.Fullname,
                            Code = u.Code,
                            CertificateNumber = u.CertificateNumber,
                            IssueDate = u.IssueDate,
                            BirthDate = u.BirthDate,
                            Email = u.Email,
                            Gender = u.Gender,
                            Address = u.Address,
                            Type = u.Type,
                            AvatarUrl = u.AvatarUrl,
                            TitleId = u.TitleId,
                            Title = u.Title,
                            OrganizationId = u.OrganizationId,
                            Organization = u.Organization,
                            DepartmentId = u.DepartmentId,
                            Department = u.Department,
                            LastModifiedOnDate = u.LastModifiedOnDate
                        };

            if (queryModel.ListTextSearch != null && queryModel.ListTextSearch.Count > 0)
            {
                foreach (var ts in queryModel.ListTextSearch)
                {
                    query = query.Where(q =>
                        q.Fullname.Contains(ts) ||
                        q.Code.Contains(ts) ||
                        q.CertificateNumber.Contains(ts) ||
                        q.Email.Contains(ts)
                    );
                }
            }

            if(queryModel.DepartmentId != null && queryModel.DepartmentId != Guid.Empty)
            {
                query = query.Where(x => x.DepartmentId == queryModel.DepartmentId);
            }

            if (queryModel.OrganizationId != null && queryModel.OrganizationId != Guid.Empty)
            {
                query = query.Where(x => x.OrganizationId == queryModel.OrganizationId);
            }

            if (queryModel.TitleId != null && queryModel.TitleId != Guid.Empty)
            {
                query = query.Where(x => x.TitleId == queryModel.TitleId);
            }

            return await query.GetPagedAsync(queryModel.CurrentPage.Value, queryModel.PageSize.Value, queryModel.Sort);
        }

        public async Task<User> GetById(Guid id)
        {
            var user = await _dataContext.Users
                .AsNoTracking()
                .Include(x => x.Organization)
                .Include(x => x.Title)
                .Include(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<User> SaveAsync(User model, IFormFile avatarFile)
        {
            //TODO: Xem lại phần update user mà k set null
            model.Organization = null;
            model.Title = null;
            model.Department = null;

            if ((!string.IsNullOrEmpty(model.Code) && model.Type == UserType.INTERNAL) || (!string.IsNullOrEmpty(model.IdentificationNumber) && model.Type == UserType.EXTERNAL))
            {
                // Check user type
                if (model.Type == UserType.INTERNAL)
                {
                    if (string.IsNullOrEmpty(model.Code))
                    {
                        throw new ArgumentException("Mã nhân viên không được để trống");
                    }
                    model.Username = model.Code;
                }
                else if (model.Type == UserType.EXTERNAL)
                {
                    if (string.IsNullOrEmpty(model.IdentificationNumber))
                    {
                        throw new ArgumentException("Số CMND không được để trống");
                    }
                    model.Username = model.IdentificationNumber;
                }
                else
                {
                    throw new ArgumentException($"Không tồn tại loại đối tượng: {model.Type}");
                }

                // Check is exist
                if (model.Id == null || model.Id == Guid.Empty)
                {
                    if (await UsernameIsExist(model.Username))
                    {
                        throw new ArgumentException("Tên tài khoản đã tồn tại");
                    }

                }
                else
                {
                    var oldUser = await GetById(model.Id);
                    if (oldUser.Username != model.Username)
                    {
                        if (await UsernameIsExist(model.Username))
                        {
                            throw new ArgumentException("Tên tài khoản đã tồn tại");
                        }
                    }
                }
            }

            // Upload Avatar
            if (avatarFile != null && avatarFile.Length > 0)
            {
                model.AvatarUrl = await UploadAvatarFile(avatarFile);
            }

            //if (model.Password != null)
            //{
            //    var passwordHasher = new PasswordHasher<User>();
            //    model.Password = passwordHasher.HashPassword(model, model.Password);
            //}

            if (model.Id == null || model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                // TODO: USER_ID
                //model.CreatedByUserId = userId;
                model.CreatedOnDate = DateTime.Now;
                model.LastModifiedOnDate = DateTime.Now;
                await _dataContext.Users.AddAsync(model);
            }
            else
            {
                model.LastModifiedOnDate = DateTime.Now;
                // TODO: USER_ID
                //model.LastModifiedByUserId = userId;
                _dataContext.Users.Update(model);
            }

            await _dataContext.SaveChangesAsync();

            InvalidCache(model.Id);

            return model;
        }

        public async Task<bool> DeleteManyAsync(Guid[] deleteIds)
        {
            foreach (var id in deleteIds)
            {
                var user = await GetById(id);

                if (user == null)
                {
                    throw new ArgumentException($"Tài khoản {user.Fullname} không tồn tại");
                }

                var deleteUser = new User() { Id = id };
                _dataContext.Users.Attach(deleteUser);
                _dataContext.Users.Remove(deleteUser);
            }
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UsernameIsExist(string Username)
        {
            var user = await _dataContext.Users.Where(x => x.Username == Username).FirstOrDefaultAsync();
            return user == null ? false : true;
        }

        public async Task<bool> CodeIsExist(string Code)
        {
            var user = await _dataContext.Users.Where(x => x.Code == Code).FirstOrDefaultAsync();
            return user == null ? false : true;
        }

        public async Task<bool> IdentificationNumberIsExist(string IdentificationNumber)
        {
            var user = await _dataContext.Users.Where(x => x.IdentificationNumber == IdentificationNumber).FirstOrDefaultAsync();
            return user == null ? false : true;
        }

        private async Task<string> UploadAvatarFile(IFormFile avatarFile)
        {

            var pathToSave = Path.Combine(_environment.WebRootPath, "avatars");
            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);

            var fileName = Guid.NewGuid() + Path.GetFileName(avatarFile.FileName);
            var filePath = Path.Combine(pathToSave, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await avatarFile.CopyToAsync(fileStream);
            }
            return fileName;
        }

        private void InvalidCache(Guid id)
        {
            //string cacheKey = BuildCacheKey(id);
            //string cacheMasterKey = BuildCacheMasterKey();

            //_cacheService.Remove(cacheKey);
            //_cacheService.Remove(cacheMasterKey);
        }
        private string BuildCacheKey(Guid id)
        {
            return $"{CachePrefix}{id}";
        }

        private string BuildCacheMasterKey()
        {
            return $"{CachePrefix}*";
        }

    }
}
