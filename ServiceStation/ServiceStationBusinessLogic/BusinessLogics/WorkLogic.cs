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
    public class WorkLogic : IWorkLogic
    {
        private readonly ILogger _logger;

        private readonly IWorkStorage _workStorage;

        public WorkLogic(ILogger<WorkLogic> logger, IWorkStorage workStorage)
        {
            _logger = logger;
            _workStorage = workStorage;
        }

        public bool Create(WorkBindingModel model)
        {
            CheckModel(model);

            if (_workStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }

            return true;
        }
        public bool Delete(WorkBindingModel model)
        {
            CheckModel(model, false);

            _logger.LogInformation("Delete. Id:{Id}", model.Id);

            if (_workStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }

            return true;
        }
        public WorkViewModel? ReadElement(WorkSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("ReadElement. WorkName:{WorkName}.Id:{Id}", model.WorkName, model.Id);

            var element = _workStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }

            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);

            return element;
        }

        public List<WorkViewModel>? ReadList(WorkSearchModel? model)
        {
            _logger.LogInformation("ReadList. WorkName:{WorkName}.Id:{Id}", model?.WorkName, model?.Id);

            var list = model == null ? _workStorage.GetFullList() : _workStorage.GetFilteredList(model);

            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }

            _logger.LogInformation("ReadList. Count:{Count}", list.Count);

            return list;
        }

        public bool Update(WorkBindingModel model)
        {
            //CheckModel(model);

            if (_workStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }

            return true;
        }

		public bool AddSparePartToWork(WorkSearchModel model, int[] spareparts)
		{
			if (model == null)
			{
				throw new ArgumentNullException(nameof(model));
			}
			_logger.LogInformation("AddSparePartToWork. WorkName:{workName}. Id:{Id}", model.WorkName, model.Id);
			var element = _workStorage.GetElement(model);

			if (element == null)
			{
				_logger.LogWarning("AddSparePartToWork element not found");
				return false;
			}

			_logger.LogInformation("AddSparePartToWork find. Id:{Id}", element.Id);

			foreach (int sparepart in spareparts)
			{
				if (!element.WorkSpareParts.Keys.Contains(sparepart))
				{
					element.WorkSpareParts.Add(sparepart, new SparePartViewModel() { Id = sparepart });
				}
			}

			_workStorage.Update(new WorkBindingModel
			{
				Id = element.Id,
				WorkName = element.WorkName,
                Status = element.Status,
				WorkPrice = element.WorkPrice,
				GuarantorId = element.GuarantorId,
				TechnicalWorkId = element.TechnicalWorkId,
				WorkSpareParts = element.WorkSpareParts,
			});

			return true;
		}

		private void CheckModel(WorkBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!withParams)
            {
                return;
            }

            if (string.IsNullOrEmpty(model.WorkName))
            {
                throw new ArgumentNullException("Нет названия работы", nameof(model.WorkName));
            }

            if (model.WorkPrice <= 0)
            {
                throw new ArgumentNullException("Цена работы должна быть больше 0", nameof(model.WorkPrice));
            }

            _logger.LogInformation("Work. WorkName:{WorkName}.WorkPrice:{ Price }. Id: {Id}", model.WorkName, model.WorkPrice, model.Id);

            var element = _workStorage.GetElement(new WorkSearchModel
            {
                WorkName = model.WorkName
            });

            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Работа с таким названием уже есть");
            }
        }
    }
}
