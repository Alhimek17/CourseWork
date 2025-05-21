using Microsoft.EntityFrameworkCore;
using ServiceStationContracts.BindingModels;
using ServiceStationContracts.SearchModels;
using ServiceStationContracts.StoragesContracts;
using ServiceStationContracts.ViewModels;
using ServiceStationDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ServiceStationDatabaseImplement.Implements
{
    public class DefectStorage : IDefectStorage
    {
        public List<DefectViewModel> GetFullList()
        {
            using var context = new ServiceStationDatabase();
            var defects = context.Defects
                .Include(x => x.Cars)
                .ThenInclude(x => x.Car)
                .ThenInclude(x => x.CarTechnicalWorks)
                .ThenInclude(x => x.TechnicalWork)
                .Include(x => x.Executor)
                .Select(x => x.GetViewModel)
                .ToList();
            return defects;
        }

        public List<DefectViewModel> GetFilteredList(DefectSearchModel model)
        {
            if (string.IsNullOrEmpty(model.DefectType) && !model.ExecutorId.HasValue) return new();
            using var context = new ServiceStationDatabase();

            if (model.ExecutorId.HasValue)
            {
                return context.Defects
                    .Include(x => x.Cars)
                    .ThenInclude(x => x.Car)
                    .ThenInclude(x => x.CarTechnicalWorks)
                    .ThenInclude(x => x.TechnicalWork)
                    .Include(x => x.Repair)
                    .Include(x => x.Executor)
                    .Where(x => x.ExecutorId == model.ExecutorId)
                    .ToList()
                    .Select(x => x.GetViewModel)
                    .ToList();
            }
            return context.Defects
                    .Include(x => x.Cars)
                    .ThenInclude(x => x.Car)
                    .ThenInclude(x => x.CarTechnicalWorks)
                    .ThenInclude(x => x.TechnicalWork)
                    .Include(x => x.Repair)
                    .Include(x => x.Executor)
                    .Where(x => x.DefectType == model.DefectType)
                    .Select(x => x.GetViewModel)
                    .ToList();
        }

        public DefectViewModel? GetElement(DefectSearchModel model)
        {
            if (string.IsNullOrEmpty(model.DefectType) && !model.Id.HasValue) return null;

            using var context = new ServiceStationDatabase();

            return context.Defects
                    .Include(x => x.Cars)
                    .ThenInclude(x => x.Car)
                    .ThenInclude(x => x.CarTechnicalWorks)
                    .ThenInclude(x => x.TechnicalWork)
                    .Include(x => x.Repair)
                    .Include(x => x.Executor)
                    .FirstOrDefault(x => (!string.IsNullOrEmpty(model.DefectType) && x.DefectType == model.DefectType) || (model.Id.HasValue && x.Id == model.Id))?
                    .GetViewModel;
        }

        public DefectViewModel? Insert(DefectBindingModel model)
        {
            using var context = new ServiceStationDatabase();
            var newDefect = Defect.Create(context, model);
            if(newDefect == null) return null;
            context.Add(newDefect);
            context.SaveChanges();
            return newDefect.GetViewModel;
        }

        public DefectViewModel? Update(DefectBindingModel model)
        {
            using var context = new ServiceStationDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var elem = context.Defects.FirstOrDefault(rec => rec.Id == model.Id);
                if (elem == null) return null;
                elem.Update(model);
                context.SaveChanges();
                if (model.DefectCars.Count != 0) elem.UpdateCars(context, model);
                transaction.Commit();
                return elem.GetViewModel;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public DefectViewModel? Delete(DefectBindingModel model)
        {
            using var context = new ServiceStationDatabase();

            var element = context.Defects
                .Include(x => x.Cars)
                .FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Defects.Remove(element);
                context.SaveChanges();

                return element.GetViewModel;
            }
            return null;
        }
    }
}
