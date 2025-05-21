using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.BindingModels
{
    public class CarBindingModel : ICarModel
    {
        public int Id {  get; set; }

        public string CarNumber { get; set; } = string.Empty;

        public string CarBrand { get; set; } = string.Empty;

        public int ExecutorId { get; set; }

        public Dictionary<int, IDefectModel> CarDefects { get; set; } = new();

        public Dictionary<int, ITechnicalWorkModel> CarTechnicalWorks { get; set; } = new();
    }
}
