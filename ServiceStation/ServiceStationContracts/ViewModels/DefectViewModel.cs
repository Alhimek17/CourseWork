using ServiceStationDataModels.Enums;
using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ServiceStationContracts.ViewModels
{
    public class DefectViewModel : IDefectModel
    {
        public int Id { get; set; }   

        [DisplayName("Тип неисправности")]
        public string DefectType { get; set; } = string.Empty;

        [DisplayName("Цена неисправности")]
        public double DefectPrice { get; set; }

        public int ExecutorId { get; set; }

        public int? RepairId { get; set; }

        public Dictionary<int, ICarModel> DefectCars { get; set; } = new();

        public DefectViewModel() { }

        [JsonConstructor]
        public DefectViewModel(Dictionary<int, CarViewModel> DefectCars)
        {
            this.DefectCars = DefectCars.ToDictionary(x => x.Key, x => x.Value as ICarModel);
        }
    }
}
