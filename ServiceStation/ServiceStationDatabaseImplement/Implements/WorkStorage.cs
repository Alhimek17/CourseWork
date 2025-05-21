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
    public class WorkStorage : IWorkStorage
    {
        public List<WorkViewModel> GetFullList()
        {
            using var context = new ServiceStationDatabase();
            return context.Works
                .Include(x => x.SpareParts)
                .ThenInclude(x => x.SparePart)
                .ThenInclude(x => x.SparePartRepairs)
                .ThenInclude(x => x.Repair)
                //.Include(x => x.TechnicalWork)
                .Include(x => x.Guarantor)
                .ToList()
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<WorkViewModel> GetFilteredList(WorkSearchModel model)
        {
            if (string.IsNullOrEmpty(model.WorkName) && !model.GuarantorId.HasValue)
            {
                return new();
            }
            using var context = new ServiceStationDatabase();

            if (model.GuarantorId.HasValue)
            {
                return context.Works
                    .Include(x => x.SpareParts)
                    .ThenInclude(x => x.SparePart)
                    .ThenInclude(x => x.SparePartRepairs)
                    .ThenInclude(x => x.Repair)
                    .Include(x => x.Guarantor)
                    .Where(x => x.GuarantorId == model.GuarantorId)
                    .Select(x => x.GetViewModel)
                    .ToList();
            }
            else if (model.GuarantorId.HasValue)
            {
                return context.Works
                    .Include(x => x.SpareParts)
                    .ThenInclude(x => x.SparePart)
                    .ThenInclude(x => x.SparePartRepairs)
                    .ThenInclude(x => x.Repair)
                    .Include(x => x.Guarantor)
                    .Where(x => x.GuarantorId == model.GuarantorId)
                    .ToList()
                    .Select(x => x.GetViewModel)
                    .ToList();
            }
            return context.Works
                .Include(x => x.SpareParts)
                .ThenInclude(x => x.SparePart)
                .ThenInclude(x => x.SparePartRepairs)
                .ThenInclude(x => x.Repair)
                .Include(x => x.Guarantor)
                .Where(x => x.WorkName.Contains(model.WorkName))
                .ToList()
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public WorkViewModel? GetElement(WorkSearchModel model)
        {
            if (string.IsNullOrEmpty(model.WorkName) && !model.Id.HasValue) return null;
            using var context = new ServiceStationDatabase();

            var elem = context.Works
                .Include(x => x.SpareParts)
                .ThenInclude(x => x.SparePart)
                .ThenInclude(x => x.SparePartRepairs)
                .ThenInclude(x => x.Repair)
                .Include(x => x.Guarantor)
                .FirstOrDefault(x => (!string.IsNullOrEmpty(model.WorkName) && x.WorkName == model.WorkName) || (model.Id.HasValue && x.Id == model.Id));

            return elem?.GetViewModel;
        }

        public WorkViewModel? Insert(WorkBindingModel model)
        {
            using var context = new ServiceStationDatabase();
            var newWork = Work.Create(context, model);
            if (newWork == null) return null;

            context.Works.Add(newWork);
            context.SaveChanges();

            return newWork.GetViewModel;
        }

        public WorkViewModel? Update(WorkBindingModel model)
        {
            using var context = new ServiceStationDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var elem = context.Works.FirstOrDefault(rec => rec.Id == model.Id);
                if (elem == null) return null;

                elem.Update(model);
                context.SaveChanges();
                if (model.WorkSpareParts.Count != 0) elem.UpdateSpareParts(context, model);
                transaction.Commit();
                return elem.GetViewModel;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public WorkViewModel? Delete(WorkBindingModel model)
        {
            using var context = new ServiceStationDatabase();

            var element = context.Works
                .Include(x => x.SpareParts)
                .FirstOrDefault(rec => rec.Id == model.Id);

            if (element != null)
            {
                context.Works.Remove(element);
                context.SaveChanges();

                return element.GetViewModel;
            }
            return null;
        }
    }
}
