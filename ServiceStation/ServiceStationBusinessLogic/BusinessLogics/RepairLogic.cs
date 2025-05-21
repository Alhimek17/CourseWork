using Microsoft.Extensions.Logging;
using ServiceStationContracts.BindingModels;
using ServiceStationContracts.BusinessLogicsContracts;
using ServiceStationContracts.SearchModels;
using ServiceStationContracts.StoragesContracts;
using ServiceStationContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationBusinessLogic.BusinessLogics
{
    public class RepairLogic : IRepairLogic
    {
        private readonly ILogger _logger;

        private readonly IRepairStorage _repairStorage;

        public RepairLogic(ILogger<RepairLogic> logger, IRepairStorage repairStorage)
        {
            _logger = logger;
            _repairStorage = repairStorage;
        }

        public bool Create(RepairBindingModel model)
        {
            CheckModel(model);

            if (_repairStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }

        public bool Delete(RepairBindingModel model)
        {
            CheckModel(model, false);

            _logger.LogInformation("Delete. Id:{Id}", model.Id);

            if (_repairStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }

        public RepairViewModel? ReadElement(RepairSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("ReadElement. RepairName:{RepairName}.Id:{Id}", model.RepairName, model.Id);

            var element = _repairStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }

        public List<RepairViewModel>? ReadList(RepairSearchModel? model)
        {
            _logger.LogInformation("ReadList. RepairName:{RepairName}.Id:{Id}", model?.RepairName, model?.Id);

            var list = model == null ? _repairStorage.GetFullList() : _repairStorage.GetFilteredList(model);

            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }

            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }

        public bool Update(RepairBindingModel model)
        {
            if (_repairStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }

		public bool AddSparePartToRepair(RepairSearchModel model, int[] spareparts)
		{
			if (model == null)
			{
				throw new ArgumentNullException(nameof(model));
			}
			_logger.LogInformation("AddSparePartToRepair. RepairName:{RepairName}. Id:{Id}", model.RepairName, model.Id);
			var element = _repairStorage.GetElement(model);

			if (element == null)
			{
				_logger.LogWarning("AddSparePartToRepair element not found");
				return false;
			}

			_logger.LogInformation("AddSparePartToRepair find. Id:{Id}", element.Id);

            element.RepairSpareParts.Clear();
            foreach (int sparepart in spareparts)
            {
                element.RepairSpareParts.Add(sparepart, new SparePartViewModel() { Id = sparepart });
            }

            _repairStorage.Update(new RepairBindingModel()
			{
				Id = element.Id,
				RepairName = element.RepairName,
				RepairStartDate = element.RepairStartDate,
				RepairPrice = element.RepairPrice,
				GuarantorId = element.GuarantorId,
				DefectId = element.DefectId,
				RepairSpareParts = element.RepairSpareParts,
			});

			return true;
		}

		private void CheckModel(RepairBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!withParams)
            {
                return;
            }

            if (string.IsNullOrEmpty(model.RepairName))
            {
                throw new ArgumentNullException("Нет названия ремонта", nameof(model.RepairName));
            }

            if (model.RepairPrice <= 0)
            {
                throw new ArgumentNullException("Цена ремонта должна быть больше 0", nameof(model.RepairPrice));
            }

            _logger.LogInformation("Repair. RepairName:{RepairName}.RepairPrice:{ Price }. Id: {Id}", model.RepairName, model.RepairPrice, model.Id);

            var element = _repairStorage.GetElement(new RepairSearchModel
            {
                RepairName = model.RepairName
            });

            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Ремонт с таким названием уже есть");
            }
        }
    }
}
