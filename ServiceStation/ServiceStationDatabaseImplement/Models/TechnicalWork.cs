using ServiceStationContracts.BindingModels;
using ServiceStationContracts.ViewModels;
using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationDatabaseImplement.Models
{
    public class TechnicalWork : ITechnicalWorkModel
    {
        public int Id { get; set; }

        [Required]
        public string WorkType { get; set; } = string.Empty;

        public DateTime? DateStartWork { get; set; } = DateTime.Now;

        [Required]
        public double WorkPrice { get; set; }

        [Required]
        public int ExecutorId { get; set; }

        public virtual Executor Executor { get; set; }

        private Dictionary<int, ICarModel>? _technicalWorkCars = null;

        [NotMapped]
        public Dictionary<int, ICarModel> TechnicalWorkCars
        {
            get
            {
                if (_technicalWorkCars == null)
                {
                    _technicalWorkCars = Cars.ToDictionary(rec => rec.CarId, rec => rec.Car as ICarModel);
                }
                return _technicalWorkCars;
            }
        }
        [ForeignKey("TechnicalWorkId")]
        public virtual List<CarTechnicalWork> Cars { get; set; } = new();
        [ForeignKey("TechnicalWorkId")]
        public virtual List<Work> Works { get; set; } = new();

        public static TechnicalWork? Create(ServiceStationDatabase context, TechnicalWorkBindingModel model)
        {
            if (model == null) return null;
            return new TechnicalWork()
            {
                Id = model.Id,
                WorkType = model.WorkType,
                DateStartWork = model.DateStartWork,
                WorkPrice = model.WorkPrice,
                ExecutorId = model.ExecutorId,
                Cars = model.TechnicalWorkCars.Select(x => new CarTechnicalWork
                {
                    Car = context.Cars.First(y => y.Id == x.Key)
                }).ToList()
            };
        }

        public void Update(TechnicalWorkBindingModel model)
        {
            WorkType = model.WorkType;
            WorkPrice = model.WorkPrice;
            ExecutorId = model.ExecutorId;
        }
        public TechnicalWorkViewModel GetViewModel => new()
        {
            Id = Id,
            WorkType = WorkType,
            DateStartWork = DateStartWork,
            WorkPrice = WorkPrice,
            ExecutorId = ExecutorId,
            TechnicalWorkCars = TechnicalWorkCars
        };

        public void UpdateCars(ServiceStationDatabase context, TechnicalWorkBindingModel model)
        {
            var carTechnicalWorks = context.CarTechnicalWorks.Where(rec => rec.TechnicalWorkId == model.Id).ToList();
            if (carTechnicalWorks != null && carTechnicalWorks.Count > 0)
            {
                context.CarTechnicalWorks.RemoveRange(carTechnicalWorks.Where(rec => !model.TechnicalWorkCars.ContainsKey(rec.CarId)));
                context.SaveChanges();

                foreach (var updateCar in carTechnicalWorks)
                {
                    model.TechnicalWorkCars.Remove(updateCar.CarId);
                }
                context.SaveChanges();
            }

            var technicalWork = context.TechnicalWorks.First(x => x.Id == Id);
            foreach (var cd in model.TechnicalWorkCars)
            {
                context.CarTechnicalWorks.Add(new CarTechnicalWork
                {
                    TechnicalWork = technicalWork,
                    Car = context.Cars.First(x => x.Id == cd.Key)
                });
                context.SaveChanges();
            }
            _technicalWorkCars = null;
        }
    }
}
