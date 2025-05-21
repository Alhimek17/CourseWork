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
    public class SparePartStorage : ISparePartStorage
    {
        public List<SparePartViewModel> GetFullList()
        {
            using var context = new ServiceStationDatabase();
            return context.SpareParts
                .Include(x => x.SparePartRepairs)
                .ThenInclude(x => x.Repair)
                .Include(x => x.SparePartWorks)
                .ThenInclude(x => x.Work)
                .Include(x => x.Guarantor)
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<SparePartViewModel> GetFilteredList(SparePartSearchModel model)
        {
            if (string.IsNullOrEmpty(model.SparePartName) && !model.GuarantorId.HasValue) return new();
            using var context = new ServiceStationDatabase();

            if (model.GuarantorId.HasValue)
            {
                return context.SpareParts
                    .Include(x => x.SparePartRepairs)
                    .ThenInclude(x => x.Repair)
                    .Include(x => x.SparePartWorks)
                    .ThenInclude(x => x.Work)
                    .Include(x => x.Guarantor)
                    .Where(x => x.GuarantorId == model.GuarantorId)
                    .Select(x => x.GetViewModel)
                    .ToList();
            }

            return context.SpareParts
                .Include(x => x.SparePartRepairs)
                .ThenInclude(x => x.Repair)
                .Include(x => x.SparePartWorks)
                .ThenInclude(x => x.Work)
                .Include(x => x.Guarantor)
                .Where(x => x.SparePartName.Contains(model.SparePartName))
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public SparePartViewModel? GetElement(SparePartSearchModel model)
        {
            if (string.IsNullOrEmpty(model.SparePartName) && !model.Id.HasValue) return null;
            using var context = new ServiceStationDatabase();

            return context.SpareParts
                .Include(x => x.SparePartRepairs)
                .ThenInclude(x => x.Repair)
                .Include(x => x.SparePartWorks)
                .ThenInclude(x => x.Work)
                .Include(x => x.Guarantor)
                .FirstOrDefault(x => (!string.IsNullOrEmpty(model.SparePartName) && model.SparePartName == x.SparePartName) || (model.Id.HasValue && x.Id == model.Id))?
                .GetViewModel;
        }

        public SparePartViewModel? Insert(SparePartBindingModel model)
        {
            using var context = new ServiceStationDatabase();

            var newSparePart = SparePart.Create(model);
            if (newSparePart == null) return null;

            context.SpareParts.Add(newSparePart);
            context.SaveChanges();

            return newSparePart.GetViewModel;
        }

        public SparePartViewModel? Update(SparePartBindingModel model)
        {
            using var context = new ServiceStationDatabase();

            var sparePart = context.SpareParts.FirstOrDefault(x => x.Id == model.Id);

            if (sparePart == null) return null;

            sparePart.Update(model);
            context.SaveChanges();

            return sparePart.GetViewModel;
        }

        public SparePartViewModel? Delete(SparePartBindingModel model)
        {
            using var context = new ServiceStationDatabase();

            var element = context.SpareParts.FirstOrDefault(x => x.Id == model.Id);

            if (element != null)
            {
                context.SpareParts.Remove(element);
                context.SaveChanges();

                return element.GetViewModel;
            }

            return null;
        }
    }
}
