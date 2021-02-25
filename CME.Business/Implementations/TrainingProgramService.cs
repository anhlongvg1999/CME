using CME.Business.Interfaces;
using CME.Business.Models;
using CME.Entities;
using Microsoft.EntityFrameworkCore;
using SERP.Filenet.DB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tsoft.Framework.Common;
using TSoft.Framework.DB;
using System.Linq;

namespace CME.Business.Implementations
{
    public class TrainingProgramService : ITrainingProgramService
    {
        private const string CachePrefix = "trainingProgram-";
        private readonly DataContext _dataContext;
        //private readonly ICacheService _cacheService;

        //TODO: CACHE
        public TrainingProgramService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Pagination<TrainingProgramViewModel>> GetAllAsync(TrainingProgramQueryModel queryModel)
        {
            var query = from trp in _dataContext.TrainingPrograms.AsNoTracking().Include(x => x.Organization).Include(x => x.TrainingProgram_Users)
                        select new TrainingProgramViewModel
                        {
                            Id = trp.Id,
                            Name = trp.Name,
                            Code = trp.Code,
                            FromDate = trp.FromDate,
                            ToDate = trp.ToDate,
                            OrganizationId = trp.OrganizationId,
                            Organization = trp.Organization,
                            Address = trp.Address,
                            Note = trp.Note,
                            Status = trp.Status,
                            MetaDataObject = trp.MetaDataObject,
                            LastModifiedOnDate = trp.LastModifiedOnDate,
                            TrainingProgram_Users = trp.TrainingProgram_Users
                        };
            return await query.GetPagedAsync(queryModel.CurrentPage.Value, queryModel.PageSize.Value, queryModel.Sort);
        }

        public async Task<TrainingProgram> GetById(Guid id)
        {
            var model = await _dataContext.TrainingPrograms.AsNoTracking().Include(x => x.Organization).FirstOrDefaultAsync(x => x.Id == id);
            return model;
        }

        public async Task<TrainingProgram> SaveAsync(TrainingProgram model, ICollection<TrainingProgram_UserRequestModel> trainingProgram_UserRequestModel)
        {
            model.Organization = null;
            model.TrainingProgram_Users = new List<TrainingProgram_User>();

            if (model.Id == null || model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                // TODO: USER_ID
                //model.CreatedByUserId = userId;
                model.CreatedOnDate = DateTime.Now;
                model.LastModifiedOnDate = DateTime.Now;

                await _dataContext.TrainingPrograms.AddAsync(model);
            }
            else
            {
                model.LastModifiedOnDate = DateTime.Now;
                // TODO: USER_ID
                // model.LastModifiedByUserId = actorId;
                _dataContext.TrainingPrograms.Update(model);

                var deleteTrainingProgram_Users = _dataContext.TrainingProgram_User.Where(x => x.TrainingProgramId == model.Id);
                _dataContext.TrainingProgram_User.RemoveRange(deleteTrainingProgram_Users);
            }

            if (trainingProgram_UserRequestModel != null && trainingProgram_UserRequestModel.Count > 0)
            {
                List<TrainingProgram_User> trainingProgram_Users = new List<TrainingProgram_User>();
                foreach (var item in trainingProgram_UserRequestModel)
                {
                    trainingProgram_Users.Add(new TrainingProgram_User
                    {
                        Id = Guid.NewGuid(),
                        TrainingProgramId = model.Id,
                        TrainingSubjectName = item.TrainingSubjectName,
                        UserId = item.UserId,
                        Amount = item.Amount,
                        //x.CreatedByUserId = userId;
                        CreatedOnDate = DateTime.Now,
                        //x.LastModifiedByUserId = actorId;
                        LastModifiedOnDate = DateTime.Now
                    });
                }
                await _dataContext.TrainingProgram_User.AddRangeAsync(trainingProgram_Users);
            }
            await _dataContext.SaveChangesAsync();

            InvalidCache(model.Id);

            return model;
        }

        public async Task<bool> DeleteManyAsync(Guid[] deleteIds)
        {
            foreach (var id in deleteIds)
            {
                var model = await GetById(id);

                if (model == null)
                {
                    throw new ArgumentException($"Id {id} không tồn tại");
                }

                var deleteTrainingProgram = new TrainingProgram() { Id = id };
                _dataContext.TrainingPrograms.Attach(deleteTrainingProgram);
                _dataContext.TrainingPrograms.Remove(deleteTrainingProgram);
            }
            await _dataContext.SaveChangesAsync();
            return true;
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
