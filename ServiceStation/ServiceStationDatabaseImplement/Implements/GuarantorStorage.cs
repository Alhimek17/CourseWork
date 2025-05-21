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
    public class GuarantorStorage : IGuarantorStorage
    {
        public List<GuarantorViewModel> GetFullList()
        {
            using var context = new ServiceStationDatabase();

            return context.Guarantors
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<GuarantorViewModel> GetFilteredList(GuarantorSearchModel model)
        {
            if (string.IsNullOrEmpty(model.GuarantorFIO)) return new();
            using var context = new ServiceStationDatabase();

            return context.Guarantors
                .Include(x => x.SpareParts)
                .Include(x => x.Repairs)
                .Include(x => x.Works)
                .Where(x => x.GuarantorFIO.Contains(model.GuarantorFIO))
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public GuarantorViewModel? GetElement(GuarantorSearchModel model)
        {
            using var context = new ServiceStationDatabase();
            if (!model.Id.HasValue && !string.IsNullOrEmpty(model.GuarantorNumber) && string.IsNullOrEmpty(model.GuarantorPassword)) return null;

            if (!string.IsNullOrEmpty(model.GuarantorNumber) && !string.IsNullOrEmpty(model.GuarantorPassword))
            {
                return context.Guarantors
                    .Include(x => x.SpareParts)
                    .Include(x => x.Repairs)
                    .Include(x => x.Works)
                    .FirstOrDefault(x => x.GuarantorNumber.Contains(model.GuarantorNumber) && x.GuarantorPassword.Contains(model.GuarantorPassword))?
                    .GetViewModel;
            }
            if (!string.IsNullOrEmpty(model.GuarantorNumber))
            {
                return context.Guarantors
                    .Include(x => x.SpareParts)
                    .Include(x => x.Repairs)
                    .Include(x => x.Works)
                    .FirstOrDefault(x => x.GuarantorNumber.Contains(model.GuarantorNumber))?
                    .GetViewModel;
            }
            return context.Guarantors
                    .Include(x => x.SpareParts)
                    .Include(x => x.Repairs)
                    .Include(x => x.Works)
                    .FirstOrDefault(x => x.Id == model.Id)?
                    .GetViewModel;
        }

        public GuarantorViewModel? Insert(GuarantorBindingModel model)
        {
            var newGuarantor = Guarantor.Create(model);
            if (newGuarantor == null) return null;

            using var context = new ServiceStationDatabase();
            context.Guarantors.Add(newGuarantor);
            context.SaveChanges();

            return newGuarantor.GetViewModel;
        }

        public GuarantorViewModel? Update(GuarantorBindingModel model)
        {
            using var context = new ServiceStationDatabase();

            var guarantor = context.Guarantors.FirstOrDefault(x => x.Id == model.Id);

            if (guarantor == null) return null;

            guarantor.Update(model);
            context.SaveChanges();

            return guarantor.GetViewModel;
        }

        public GuarantorViewModel? Delete(GuarantorBindingModel model)
        {
            using var context = new ServiceStationDatabase();
            var element = context.Guarantors.FirstOrDefault(x => x.Id == model.Id);

            if (element != null)
            {
                context.Guarantors.Remove(element);
                context.SaveChanges();

                return element.GetViewModel;
            }
            return null;
        }
    }
}
