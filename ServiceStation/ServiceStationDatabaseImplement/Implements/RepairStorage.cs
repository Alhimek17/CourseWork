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
    public class RepairStorage : IRepairStorage
    {
        public List<RepairViewModel> GetFullList()
        {
            using var context = new ServiceStationDatabase();
            return context.Repairs
                .Include(x => x.SpareParts)
                .ThenInclude(x => x.SparePart)
                .ThenInclude(x => x.SparePartWorks)
                .ThenInclude(x => x.Work)
                .Include(x => x.Guarantor)
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<RepairViewModel> GetFilteredList(RepairSearchModel model)
        {
            if (string.IsNullOrEmpty(model.RepairName) && !model.GuarantorId.HasValue && !model.DateFrom.HasValue && !model.DateTo.HasValue) return new();
            using var context = new ServiceStationDatabase();

            if(model.DateTo.HasValue && model.DateTo.HasValue)
            {
                return context.Repairs
                    .Include(x => x.SpareParts)
                    .ThenInclude(x => x.SparePart)
                    .ThenInclude(x => x.SparePartWorks)
                    .ThenInclude(x => x.Work)
                    .Include(x => x.Guarantor)
                    .Where(x => x.RepairStartDate >= model.DateFrom && x.RepairStartDate <= model.DateTo || x.GuarantorId == model.GuarantorId)
                    .ToList()
                    .Select(x => x.GetViewModel)
                    .ToList();
            }

            if (model.GuarantorId.HasValue)
            {
                return context.Repairs
                    .Include(x => x.SpareParts)
                    .ThenInclude(x => x.SparePart)
                    .ThenInclude(x => x.SparePartWorks)
                    .ThenInclude(x => x.Work)
                    .Include(x => x.Guarantor)
                    .Where(x => x.GuarantorId == model.GuarantorId)
                    .ToList()
                    .Select(x => x.GetViewModel)
                    .ToList();
            }
            return context.Repairs
                    .Include(x => x.SpareParts)
                    .ThenInclude(x => x.SparePart)
                    .ThenInclude(x => x.SparePartWorks)
                    .ThenInclude(x => x.Work)
                    .Include(x => x.Guarantor)
                    .Where(x => x.RepairName == model.RepairName)
                    .ToList()
                    .Select(x => x.GetViewModel)
                    .ToList();
        }

        public RepairViewModel? GetElement(RepairSearchModel model)
        {
            if (string.IsNullOrEmpty(model.RepairName) && !model.Id.HasValue) return null;

            using var context = new ServiceStationDatabase();

            return context.Repairs
                    .Include(x => x.SpareParts)
                    .ThenInclude(x => x.SparePart)
                    .ThenInclude(x => x.SparePartWorks)
                    .ThenInclude(x => x.Work)
                    .Include(x => x.Guarantor)
                    .FirstOrDefault(x => (!string.IsNullOrEmpty(model.RepairName) && x.RepairName == model.RepairName) || (model.Id.HasValue && x.Id == model.Id))?
                    .GetViewModel;
        }

        public RepairViewModel? Insert(RepairBindingModel model)
        {
            using var context = new ServiceStationDatabase();
            var newRepair = Repair.Create(context, model);
            if (newRepair == null) return null;
            context.Add(newRepair);
            context.SaveChanges();
            return newRepair.GetViewModel;
        }

        public RepairViewModel? Update(RepairBindingModel model)
        {
            using var context = new ServiceStationDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var elem = context.Repairs.FirstOrDefault(rec => rec.Id == model.Id);
                if (elem == null) return null;
                elem.Update(model);
                context.SaveChanges();
                if (model.RepairSpareParts != null) elem.UpdateSpareParts(context, model);
                transaction.Commit();
                return elem.GetViewModel;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public RepairViewModel? Delete(RepairBindingModel model)
        {
            using var context = new ServiceStationDatabase();

            var element = context.Repairs
                .Include(x => x.SpareParts)
                .FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Repairs.Remove(element);
                context.SaveChanges();

                return element.GetViewModel;
            }
            return null;
        }
    }
}
