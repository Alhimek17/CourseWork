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
    public class TechnicalWorkLogic : ITechnicalWorkLogic
    {
        private ILogger _logger;
        private ITechnicalWorkStorage _technicalWorkStorage;

        public TechnicalWorkLogic(ILogger<TechnicalWorkLogic> logger, ITechnicalWorkStorage technicalWorkStorage)
        {
            _logger = logger;
            _technicalWorkStorage = technicalWorkStorage;
        }

        public List<TechnicalWorkViewModel>? ReadList(TechnicalWorkSearchModel? model)
        {
            _logger.LogInformation("ReadList. WorkType:{WorkType}. Id:{Id}", model?.WorkType, model?.Id);
            var list = model == null ? _technicalWorkStorage.GetFullList() : _technicalWorkStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }
        public TechnicalWorkViewModel? ReadElement(TechnicalWorkSearchModel? model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. WorkType:{WorkType}. Id:{Id}", model.WorkType, model?.Id);
            var element = _technicalWorkStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element is not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }

        public bool Create(TechnicalWorkBindingModel model)
        {
            CheckModel(model);
            if (_technicalWorkStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }
        public bool Update(TechnicalWorkBindingModel model)
        {
            CheckModel(model);
            if (_technicalWorkStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }
        public bool Delete(TechnicalWorkBindingModel model)
        {
            CheckModel(model, false);
            if (_technicalWorkStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }

        public bool AddCarToTechnicalWork(TechnicalWorkSearchModel model, int[] cars)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("AddCarToTechnicalWork. WorkType:{worktype}. Id:{Id}", model.WorkType, model.Id);
            var element = _technicalWorkStorage.GetElement(model);

            if(element == null)
            {
                _logger.LogWarning("AddCarToTechnicalWork element not found");
                return false;
            }

            _logger.LogInformation("AddCarToTechnicalWork find. Id:{Id}", element.Id);

            foreach(int car in cars)
            {
                if (!element.TechnicalWorkCars.Keys.Contains(car))
                {
                    element.TechnicalWorkCars.Add(car, new CarViewModel() { Id = car });
                }
            }

            _technicalWorkStorage.Update(new TechnicalWorkBindingModel
            {
                Id = element.Id,
                WorkType = element.WorkType,
                WorkPrice = element.WorkPrice,
                DateStartWork = element.DateStartWork,
                TechnicalWorkCars = element.TechnicalWorkCars,
                ExecutorId = element.ExecutorId,
            });

            return true;
        }

        private void CheckModel(TechnicalWorkBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.WorkType))
            {
                throw new ArgumentNullException("Нет названия ТО", nameof(model.WorkType));
            }
            if(model.WorkPrice <= 0)
            {
                throw new ArgumentNullException("Цена ТО должна быть больше 0", nameof(model.WorkPrice));
            }
            _logger.LogInformation("TechnicalWork. WorkType:{WorkType}. WorkPrice:{WorkPrice}. Id:{Id}", model.WorkType, model.WorkType, model.Id);
            var element = _technicalWorkStorage.GetElement(new TechnicalWorkSearchModel
            {
                WorkType = model.WorkType
            });
            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("ТО с таким названием уже есть");
            }
        }
    }
}
