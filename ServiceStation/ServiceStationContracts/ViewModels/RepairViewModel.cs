using Newtonsoft.Json;
using ServiceStationDataModels.Enums;
using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.ViewModels
{
    public class RepairViewModel : IRepairModel
    {
        public int Id { get; set; }

        [DisplayName("Наименование ремонта")]
        public string RepairName { get; set; } = string.Empty;

        [DisplayName("Начало ремонта")]
        public DateTime? RepairStartDate { get; set; } = DateTime.Now;

        [DisplayName("Стоимость ремонта")]
        public double RepairPrice { get; set; }

        public int GuarantorId { get; set; }

        public int? DefectId { get; set; }

        public Dictionary<int, ISparePartModel> RepairSpareParts { get; set; } = new();

		public RepairViewModel() { }

		[JsonConstructor]
		public RepairViewModel(Dictionary<int, SparePartViewModel> RepairSpareParts)
		{
			this.RepairSpareParts = RepairSpareParts.ToDictionary(x => x.Key, x => x.Value as ISparePartModel);
		}
	}
}
