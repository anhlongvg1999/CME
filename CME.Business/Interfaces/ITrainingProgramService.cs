using CME.Business.Models;
using CME.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.DB;

namespace CME.Business.Interfaces
{
    public interface ITrainingProgramService
    {
        Task<Pagination<TrainingProgramViewModel>> GetAllAsync(TrainingProgramQueryModel queryModel);

        Task<TrainingProgram> GetById(Guid id);

        Task<TrainingProgram> SaveAsync(TrainingProgram model, ICollection<TrainingProgram_UserRequestModel> trainingProgram_Users);

        Task<bool> DeleteManyAsync(Guid[] deleteIds);
    }
}
