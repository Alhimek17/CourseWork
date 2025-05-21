using Microsoft.EntityFrameworkCore.Migrations;
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
    public class Defect : IDefectModel
    {
        public int Id { get; set; }

        [Required]
        public string DefectType { get; set; } = string.Empty;

        [Required]
        public double DefectPrice { get; set; }

        [Required]
        public int ExecutorId { get; set; }

        public int? RepairId { get; set; }

        public virtual Executor Executor { get; set; }
        public virtual Repair? Repair { get; set; }


        private Dictionary<int, ICarModel>? _defectCars = null;
        [NotMapped]
        public Dictionary<int, ICarModel> DefectCars
        {
            get
            {
                if (_defectCars == null)
                {
                    _defectCars = Cars.ToDictionary(rec => rec.CarId, rec => rec.Car as ICarModel);
                }
                return _defectCars;
            }
        }
        [ForeignKey("DefectId")]
        public virtual List<CarDefect> Cars { get; set; } = new();

        public static Defect? Create(ServiceStationDatabase context, DefectBindingModel model)
        {
            if (model == null) return null;
            return new Defect()
            {
                Id = model.Id,
                DefectType = model.DefectType,
                DefectPrice = model.DefectPrice,
                ExecutorId = model.ExecutorId,
                Cars = model.DefectCars.Select(x => new CarDefect
                {
                    Car = context.Cars.First(y => y.Id == x.Key)
                }).ToList()
            };
        }

        public void Update(DefectBindingModel model)
        {
            if(model == null) return;
            if (model.RepairId.HasValue)
            {
                RepairId = model.RepairId;
                return;
            }
            DefectType = model.DefectType;
            DefectPrice = model.DefectPrice;
        }

        public DefectViewModel GetViewModel => new()
        {
            Id = Id,
            DefectType = DefectType,
            DefectPrice = DefectPrice,
            ExecutorId = ExecutorId,
            DefectCars = DefectCars,
            RepairId = RepairId
        };

        public void UpdateCars(ServiceStationDatabase context, DefectBindingModel model)
        {
            var carDefects = context.CarDefects.Where(rec => rec.DefectId == model.Id).ToList();
            if(carDefects != null && carDefects.Count > 0)
            {
                context.CarDefects.RemoveRange(carDefects.Where(rec => !model.DefectCars.ContainsKey(rec.CarId)));
                context.SaveChanges();

                foreach(var updateCar in carDefects)
                {
                    model.DefectCars.Remove(updateCar.CarId);
                }
                context.SaveChanges();
            }

            var defect = context.Defects.First(x => x.Id == Id);
            foreach(var cd in model.DefectCars)
            {
                context.CarDefects.Add(new CarDefect
                {
                    Defect = defect,
                    Car = context.Cars.First(x => x.Id == cd.Key)
                });
                context.SaveChanges();
            }
            _defectCars = null;
        }
    }
}
