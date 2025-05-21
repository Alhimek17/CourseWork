using Newtonsoft.Json;
using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.ViewModels
{
    public class TechnicalWorkViewModel : ITechnicalWorkModel
    {
        public int Id { get; set; }
        [DisplayName("Тип ТО")]
        public string WorkType { get; set; } = string.Empty;
        [DisplayName("Дата последнего ТО")]
        public DateTime? DateStartWork { get; set; } = DateTime.Now;
        [DisplayName("Цена ТО")]
        public double WorkPrice { get; set; }

        public int ExecutorId { get; set; }

        public Dictionary<int, ICarModel> TechnicalWorkCars { get; set; } = new();

        public TechnicalWorkViewModel() { }

        [JsonConstructor]
        public TechnicalWorkViewModel(Dictionary<int, CarViewModel> TechnicalWorkCars)
        {
            this.TechnicalWorkCars = TechnicalWorkCars.ToDictionary(x => x.Key, x => x.Value as ICarModel);
        }
    }
}
