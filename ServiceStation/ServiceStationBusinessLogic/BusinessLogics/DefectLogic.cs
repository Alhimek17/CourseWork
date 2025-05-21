using Microsoft.Extensions.Logging;
using ServiceStationContracts.BindingModels;
using ServiceStationContracts.BusinessLogicsContracts;
using ServiceStationContracts.SearchModels;
using ServiceStationContracts.StoragesContracts;
using ServiceStationContracts.ViewModels;
using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationBusinessLogic.BusinessLogics
{
    public class DefectLogic : IDefectLogic
    {
        private readonly ILogger _logger;
        private readonly IDefectStorage _defectStorage;

        public DefectLogic(ILogger<DefectLogic> logger, IDefectStorage defectStorage)
        {
            _logger = logger;
            _defectStorage = defectStorage;
        }

        public List<DefectViewModel>? ReadList(DefectSearchModel? model)
        {
            _logger.LogInformation("ReadList. DefectType:{DefectType}. Id:{Id}", model?.DefectType, model?.Id);
            var list = model == null ? _defectStorage.GetFullList() : _defectStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }
        public DefectViewModel? ReadElement(DefectSearchModel? model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. DefectType:{DefectType}. Id:{Id}", model.DefectType, model?.Id);
            var element = _defectStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element is not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }

        public bool Create(DefectBindingModel model)
        {
            CheckModel(model);
            if (_defectStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }
        public bool Update(DefectBindingModel model)
        {
            if (_defectStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }
        public bool Delete(DefectBindingModel model)
        {
            CheckModel(model, false);
            if (_defectStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }

        public bool AddCarToDefect(DefectSearchModel model, int[] cars)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("AddCarToDefect. DefectName:{DefectName}. Id:{Id}", model.DefectType, model.Id);
            var element = _defectStorage.GetElement(model);

            if(element == null)
            {
                _logger.LogWarning("AddCarToDefect element not found");
                return false;
            }

            _logger.LogInformation("AddCarToDefect find. Id:{Id}", element.Id);

            element.DefectCars.Clear();
            foreach(int car in cars)
            {
                element.DefectCars.Add(car, new CarViewModel() { Id = car });
            }

            _defectStorage.Update(new DefectBindingModel()
            {
                Id = element.Id,
                DefectPrice = element.DefectPrice,
                DefectType = element.DefectType,
                ExecutorId = element.ExecutorId,
                RepairId = element.RepairId,
                DefectCars = element.DefectCars,
            });
            
            return true;
        }

        private void CheckModel(DefectBindingModel model, bool withParams = true)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.DefectType))
            {
                throw new ArgumentNullException("Нет названия неисправности", nameof(model.DefectType));
            }
            if(model.DefectPrice <= 0)
            {
                throw new ArgumentNullException("Цена дефекта должна быть больше нуля", nameof(model.DefectPrice));
            }
            _logger.LogInformation("Defect. DefectType:{DefectType}. DefectPrice:{DefectPrice}. Id:{Id}", model.DefectType, model.DefectPrice, model.Id);
            var element = _defectStorage.GetElement(new DefectSearchModel
            {
                DefectType = model.DefectType
            });
            if(element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Такая неисправность уже есть");
            }
        }
    }
}
