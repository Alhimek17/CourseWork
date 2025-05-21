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

namespace ServiceStationDatabaseImplement.Implements
{
    public class ExecutorStorage : IExecutorStorage
    {
        public List<ExecutorViewModel> GetFullList()
        {
            using var context = new ServiceStationDatabase();

            return context.Executors
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<ExecutorViewModel> GetFilteredList(ExecutorSearchModel model)
        {
            if (string.IsNullOrEmpty(model.ExecutorNumber)) return new();
            using var context = new ServiceStationDatabase();

            return context.Executors
                .Include(x => x.Cars)
                .Include(x => x.Defects)
                .Include(x => x.TechnicalWorks)
                .Where(x => x.ExecutorNumber.Contains(model.ExecutorNumber))
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public ExecutorViewModel? GetElement(ExecutorSearchModel model)
        {
            using var context = new ServiceStationDatabase();
            if (!model.Id.HasValue && string.IsNullOrEmpty(model.ExecutorNumber) && string.IsNullOrEmpty(model.ExecutorPassword)) return null;

            if (!string.IsNullOrEmpty(model.ExecutorNumber) && !string.IsNullOrEmpty(model.ExecutorPassword))
            {
                return context.Executors
                    .Include(x => x.Cars)
                    .Include(x => x.Defects)
                    .Include(x => x.TechnicalWorks)
                    .FirstOrDefault(x => x.ExecutorNumber.Contains(model.ExecutorNumber) && x.ExecutorPassword.Contains(model.ExecutorPassword))?
                    .GetViewModel;
            }
            if (!string.IsNullOrEmpty(model.ExecutorNumber))
            {
                return context.Executors
                    .Include(x => x.Cars)
                    .Include(x => x.Defects)
                    .Include(x => x.TechnicalWorks)
                    .FirstOrDefault(x => x.ExecutorNumber.Contains(model.ExecutorNumber))?
                    .GetViewModel;
            }
            return context.Executors
                    .Include(x => x.Cars)
                    .Include(x => x.Defects)
                    .Include(x => x.TechnicalWorks)
                    .FirstOrDefault(x => x.Id == model.Id)?
                    .GetViewModel;
        }

        public ExecutorViewModel? Insert(ExecutorBindingModel model)
        {
            var newExecutor = Executor.Create(model);
            if(newExecutor == null) return null;

            using var context = new ServiceStationDatabase();
            context.Executors.Add(newExecutor);
            context.SaveChanges();

            return newExecutor.GetViewModel;
        }

        public ExecutorViewModel? Update(ExecutorBindingModel model)
        {
            using var context = new ServiceStationDatabase();

            var executor = context.Executors.FirstOrDefault(x => x.Id == model.Id);

            if(executor == null) return null;

            executor.Update(model);
            context.SaveChanges();

            return executor.GetViewModel;
        }

        public ExecutorViewModel? Delete(ExecutorBindingModel model)
        {
            using var context = new ServiceStationDatabase();
            var element = context.Executors.FirstOrDefault(x => x.Id == model.Id);

            if(element != null)
            {
                context.Executors.Remove(element);
                context.SaveChanges();

                return element.GetViewModel;
            }
            return null;
        }
    }
}
