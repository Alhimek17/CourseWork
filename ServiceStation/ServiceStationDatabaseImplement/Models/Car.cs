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
    public class Car : ICarModel
    {
        public int Id { get; set; }

        [Required]
        public string CarNumber { get; set; } = string.Empty;

        [Required]
        public string CarBrand { get; set; } = string.Empty;

        [Required]
        public int ExecutorId { get; set; }

        public virtual Executor Executor { get; set; }

        [ForeignKey("CarId")]
        public virtual List<CarDefect> CarDefects { get; set; } = new();

        [ForeignKey("CarId")]
        public virtual List<CarTechnicalWork> CarTechnicalWorks { get; set; } = new();

        public static Car? Create(CarBindingModel model)
        {
            if (model == null) return null;
            return new Car()
            {
                Id = model.Id,
                CarNumber = model.CarNumber,
                CarBrand = model.CarBrand,
                ExecutorId = model.ExecutorId
            };
        }

        public void Update(CarBindingModel model)
        {
            if (model == null) return;
            CarNumber = model.CarNumber;
            CarBrand = model.CarBrand;
            ExecutorId = model.ExecutorId;
        }

        public CarViewModel GetViewModel => new()
        {
            Id = Id,
            CarNumber = CarNumber,
            CarBrand = CarBrand,
            ExecutorId = ExecutorId
        };
    }
}
