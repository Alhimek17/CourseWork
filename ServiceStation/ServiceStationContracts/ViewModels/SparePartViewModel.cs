using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.ViewModels
{
    public class SparePartViewModel : ISparePartModel
    {
        public int Id { get; set; }

        [DisplayName("Наименование запчасти")]
        public string SparePartName { get; set; } = string.Empty;

        [DisplayName("Стоимость запчасти")]
        public double SparePartPrice { get; set; }

        public int GuarantorId { get; set; }
    }
}
