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
    public class CarStorage : ICarStorage
    {
        public List<CarViewModel> GetFullList()
        {
            using var context = new ServiceStationDatabase();
            return context.Cars
                .Include(x => x.CarDefects)
                .ThenInclude(x => x.Defect)
                .Include(x => x.CarTechnicalWorks)
                .ThenInclude(x => x.TechnicalWork)
                .Include(x => x.Executor)
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<CarViewModel> GetFilteredList(CarSearchModel model)
        {
            if (string.IsNullOrEmpty(model.CarNumber) && !model.ExecutorId.HasValue) return new();
            using var context = new ServiceStationDatabase();

            if (model.ExecutorId.HasValue)
            {
                return context.Cars
                    .Include(x => x.CarDefects)
                    .ThenInclude(x => x.Defect)
                    .Include(x => x.CarTechnicalWorks)
                    .ThenInclude(x => x.TechnicalWork)
                    .Include(x => x.Executor)
                    .Where(x => x.ExecutorId == model.ExecutorId)
                    .Select(x => x.GetViewModel)
                    .ToList();
            }

            return context.Cars
                .Include(x => x.CarDefects)
                .ThenInclude(x => x.Defect)
                .Include(x => x.CarTechnicalWorks)
                .ThenInclude(x => x.TechnicalWork)
                .Include(x => x.Executor)
                .Where(x => x.CarNumber.Contains(model.CarNumber))
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public CarViewModel? GetElement(CarSearchModel model)
        {
            if(string.IsNullOrEmpty(model.CarNumber) && !model.Id.HasValue) return null;
            using var context = new ServiceStationDatabase();

            return context.Cars
                .Include(x => x.CarDefects)
                .ThenInclude(x => x.Defect)
                .Include(x => x.CarTechnicalWorks)
                .ThenInclude(x => x.TechnicalWork)
                .Include(x => x.Executor)
                .FirstOrDefault(x => (!string.IsNullOrEmpty(model.CarNumber) && model.CarNumber == x.CarNumber) || (model.Id.HasValue && x.Id == model.Id))?
                .GetViewModel;
        }

        public CarViewModel? Insert(CarBindingModel model)
        {
            using var context = new ServiceStationDatabase();

            var newCar = Car.Create(model);
            if(newCar == null) return null;

            context.Cars.Add(newCar);
            context.SaveChanges();

            return newCar.GetViewModel;
        }

        public CarViewModel? Update(CarBindingModel model)
        {
            using var context = new ServiceStationDatabase();

            var car = context.Cars.FirstOrDefault(x => x.Id == model.Id);

            if(car == null) return null;

            car.Update(model);
            context.SaveChanges();

            return car.GetViewModel;
        }

        public CarViewModel? Delete(CarBindingModel model)
        {
            using var context = new ServiceStationDatabase();

            var element = context.Cars.FirstOrDefault(x => x.Id == model.Id);

            if(element != null)
            {
                context.Cars.Remove(element);
                context.SaveChanges();

                return element.GetViewModel;
            }

            return null;
        }
    }
}
