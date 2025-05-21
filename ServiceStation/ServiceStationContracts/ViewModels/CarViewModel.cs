using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.ViewModels
{
    public class CarViewModel : ICarModel
    {
        public int Id { get; set; }

        [DisplayName("Номер машины")]
        public string CarNumber { get; set; } = string.Empty;

        [DisplayName("Бренд машины")]
        public string CarBrand { get; set; } = string.Empty;

        public int ExecutorId { get; set; }
    }
}
