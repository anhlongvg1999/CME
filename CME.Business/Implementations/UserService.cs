using CME.Business.Interfaces;
using CME.Business.Models;
using CME.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SERP.Filenet.DB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.DB;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CME.Business.Implementations
{
    public class UserService : IUserService
    {
        private const string CachePrefix = "user-";
        private readonly DataContext _dataContext;
        private readonly IHostingEnvironment _environment;
        public UserService(DataContext dataContext, IHostingEnvironment environment)
        {
            _dataContext = dataContext;
            _environment = environment;
        }
        public async Task<bool> DeleteManyAsync(Guid[] deleteIds)
        {
            foreach (var id in deleteIds)
            {
                var user = await GetById(id);

                if (user == null)
                {
                    throw new ArgumentException($"Tài khoản {user.Username} không tồn tại");
                }

                var deleteUser = new User() { Id = id };
                _dataContext.Users.Attach(deleteUser);
                _dataContext.Users.Remove(deleteUser);
            }
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<Pagination<UserViewModel>> GetAllAsync(UserQueryModel queryModel)
        {
            var query = from u in _dataContext.Users.AsNoTracking()
                        select new UserViewModel
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Firstname = u.Firstname,
                            BirthDate = u.BirthDate,
                            Email = u.Email,
                            Gender = u.Gender,
                            Address = u.Address,
                            AvatarUrl = u.AvatarUrl 
                        };
            var result = await query.GetPagedAsync(queryModel.CurrentPage.Value, queryModel.PageSize.Value, queryModel.Sort);
            return result;
        }

        public async Task<User> GetById(Guid id)
        {
            var user = await _dataContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<User> GetByUsername(string username)
        {
            var user = await _dataContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username);
            return user;
        }

        public async Task<User> SaveAsync(User user)
        {
            
            if(user.Id == null || user.Id == Guid.Empty)
            {
                if(await UsernameIsExist(user.Username))
                {
                    throw new ArgumentException("Tên tài khoản đã tồn tại");
                }    
            }
            else 
            {
                var oldUser = await GetById(user.Id);
                if (oldUser.Username != user.Username)
                {
                    if (await UsernameIsExist(user.Username))
                    {
                        throw new ArgumentException("Tên tài khoản đã tồn tại");
                    }
                }
            }
            if (user.Id == null || user.Id == Guid.Empty)
            {
                user.Id = Guid.NewGuid();
                user.CreatedOnDate = DateTime.Now;
                user.LastModifiedOnDate = DateTime.Now;
                await _dataContext.Users.AddAsync(user);
            }
            else
            {
                user.LastModifiedOnDate = DateTime.Now;
                _dataContext.Users.Update(user);
            }
            await _dataContext.SaveChangesAsync();
            return await GetById(user.Id);
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

        public async Task<bool> UsernameIsExist(string Username)
        {
            var user = await _dataContext.Users.Where(x => x.Username == Username).FirstOrDefaultAsync();
            return user == null ? false : true;
        }
    }
}
