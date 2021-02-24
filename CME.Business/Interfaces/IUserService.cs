using CME.Business.Models;
using CME.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.DB;

namespace CME.Business.Interfaces
{
    public interface IUserService
    {
        Task<Pagination<UserViewModel>> GetAllAsync(UserQueryModel queryModel);

        Task<User> GetById(Guid id);

        Task<User> SaveAsync(User user, IFormFile avatarFile);

        Task<bool> DeleteManyAsync(Guid[] deleteIds);
    }
}
