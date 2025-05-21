using Microsoft.EntityFrameworkCore;
using ServiceStationContracts.BindingModels;
using ServiceStationContracts.SearchModels;
using ServiceStationContracts.StoragesContracts;
using ServiceStationContracts.ViewModels;
using ServiceStationDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationDatabaseImplement.Implements
{
    public class TechnicalWorkStorage : ITechnicalWorkStorage
    {
        public List<TechnicalWorkViewModel> GetFullList()
        {
            using var context = new ServiceStationDatabase();
            return context.TechnicalWorks
                .Include(x => x.Cars)
                .ThenInclude(x => x.Car)
                .ThenInclude(x => x.CarDefects)
                .ThenInclude(x => x.Defect)
                .Include(x => x.Executor)
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<TechnicalWorkViewModel> GetFilteredList(TechnicalWorkSearchModel model)
        {
            if(!model.DateFrom.HasValue && !model.DateTo.HasValue && !model.ExecutorId.HasValue)
            {
                return new();
            }
            using var context = new ServiceStationDatabase();

            if(model.DateTo.HasValue && model.DateTo.HasValue)
            {
                return context.TechnicalWorks
                    .Include(x => x.Cars)
                    .ThenInclude(x => x.Car)
                    .ThenInclude(x => x.CarDefects)
                    .ThenInclude(x => x.Defect)
                    .Include(x => x.Executor)
                    .Where(x => x.DateStartWork >= model.DateFrom && x.DateStartWork <= model.DateTo || x.ExecutorId == model.ExecutorId)
                    .Select(x => x.GetViewModel)
                    .ToList();
            }
            else if (model.ExecutorId.HasValue)
            {
                return context.TechnicalWorks
                    .Include(x => x.Cars)
                    .ThenInclude(x => x.Car)
                    .ThenInclude(x => x.CarDefects)
                    .ThenInclude(x => x.Defect)
                    .Include(x => x.Executor)
                    .Where(x => x.ExecutorId == model.ExecutorId)
                    .Select(x => x.GetViewModel)
                    .ToList();
            }
            return context.TechnicalWorks
                .Include(x => x.Cars)
                .ThenInclude(x => x.Car)
                .ThenInclude(x => x.CarDefects)
                .ThenInclude(x => x.Defect)
                .Include(x => x.Executor)
                .Where(x => x.WorkType.Contains(model.WorkType))
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public TechnicalWorkViewModel? GetElement(TechnicalWorkSearchModel model)
        {
            if (string.IsNullOrEmpty(model.WorkType) && !model.Id.HasValue) return null;
            using var context = new ServiceStationDatabase();

            return context.TechnicalWorks
                .Include(x => x.Cars)
                .ThenInclude(x => x.Car)
                .ThenInclude(x => x.CarDefects)
                .ThenInclude(x => x.Defect)
                .Include(x => x.Executor)
                .FirstOrDefault(x => (!string.IsNullOrEmpty(model.WorkType) && x.WorkType == model.WorkType) || (model.Id.HasValue && x.Id == model.Id))?
                .GetViewModel;
        }

        public TechnicalWorkViewModel? Insert(TechnicalWorkBindingModel model)
        {
            using var context = new ServiceStationDatabase();
            var newWork = TechnicalWork.Create(context, model);
            if(newWork == null) return null;

            context.TechnicalWorks.Add(newWork);
            context.SaveChanges();

            return newWork.GetViewModel;
        }

        public TechnicalWorkViewModel? Update(TechnicalWorkBindingModel model)
        {
            using var context = new ServiceStationDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var elem = context.TechnicalWorks.FirstOrDefault(rec => rec.Id == model.Id);
                if (elem == null) return null;

                elem.Update(model);
                context.SaveChanges();
                if (model.TechnicalWorkCars != null) elem.UpdateCars(context, model);
                transaction.Commit();
                return elem.GetViewModel;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public TechnicalWorkViewModel? Delete(TechnicalWorkBindingModel model)
        {
            using var context = new ServiceStationDatabase();

            var element = context.TechnicalWorks
                .Include(x => x.Cars)
                .FirstOrDefault(rec => rec.Id == model.Id);

            if(element != null)
            {
                context.TechnicalWorks.Remove(element);
                context.SaveChanges();

                return element.GetViewModel;
            }
            return null;
        }
    }
}
