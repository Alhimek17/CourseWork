using Newtonsoft.Json;
using ServiceStationDataModels.Enums;
using ServiceStationDataModels.Models;
using System.ComponentModel;


namespace ServiceStationContracts.ViewModels
{
    public class WorkViewModel : IWorkModel
    {
        public int Id { get; set; }

        [DisplayName("Наименование работы")]
        public string WorkName { get; set; } = string.Empty;

        [DisplayName("Статус работы")]
        public WorkStatus Status { get; set; }

        [DisplayName("Стоимость работы")]
        public double WorkPrice { get; set; }

        public int GuarantorId { get; set; }

        public int? TechnicalWorkId { get; set; }

        public Dictionary<int, ISparePartModel> WorkSpareParts { get; set; } = new();

		public WorkViewModel() { }

		[JsonConstructor]
		public WorkViewModel(Dictionary<int, SparePartViewModel> WorkSpareParts)
		{
			this.WorkSpareParts = WorkSpareParts.ToDictionary(x => x.Key, x => x.Value as ISparePartModel);
		}
	}
}
