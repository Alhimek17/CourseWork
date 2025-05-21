using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.BindingModels
{
    public class TechnicalWorkBindingModel : ITechnicalWorkModel
    {
        public int Id { get; set; }

        public string WorkType { get; set; } = string.Empty;

        public DateTime? DateStartWork { get; set; } = DateTime.Now;

        public double WorkPrice { get; set; }

        public int ExecutorId { get; set; }

        public Dictionary<int, ICarModel> TechnicalWorkCars { get; set; } = new();
    }
}
