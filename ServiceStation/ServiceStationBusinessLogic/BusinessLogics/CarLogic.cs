
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
    public class CarLogic : ICarLogic
    {
        private readonly ILogger _logger;
        private readonly ICarStorage _carStorage;

        public CarLogic(ILogger<CarLogic> logger, ICarStorage carStorage)
        {
            _logger = logger;
            _carStorage = carStorage;
        }

        public List<CarViewModel>? ReadList(CarSearchModel? model)
        {
            _logger.LogInformation("ReadList. Car:{CarNumber}. Id:{Id}", model?.CarNumber, model?.Id);
            var list = model == null ? _carStorage.GetFullList() : _carStorage.GetFilteredList(model);
            if(list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }
        public CarViewModel? ReadElement(CarSearchModel? model)
        {
            if(model == null) throw new ArgumentNullException(nameof(model));
            _logger.LogInformation("ReadElement. CarNumber:{CarNumber}. Id:{Id}", model?.CarNumber, model?.Id);
            var element = _carStorage.GetElement(model);
            if(element == null)
            {
                _logger.LogWarning("ReadElement element is not found");
                return null;
            }
            _logger.LogInformation("ReadElement fing. Id:{Id}", element.Id);
            return element;
        }

        public bool Create(CarBindingModel model)
        {
            CheckModel(model);
            if(_carStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }
        public bool Update(CarBindingModel model)
        {
            CheckModel(model);
            if(_carStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }
        public bool Delete(CarBindingModel model)
        {
            CheckModel(model, false);
            if(_carStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }

        private void CheckModel(CarBindingModel model, bool withParams = true)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.CarBrand))
            {
                throw new ArgumentNullException("Нет названия бренда машины", nameof(model.CarBrand));
            }
            if (string.IsNullOrEmpty(model.CarNumber))
            {
                throw new ArgumentNullException("Нет номера машины", nameof(model.CarNumber));
            }
            _logger.LogInformation("Car. CarBrand:{CarBrand}. CarNumber:{CarNumber}. Id:{Id}", model.CarBrand, model.CarNumber, model.Id);
            var element = _carStorage.GetElement(new CarSearchModel
            {
                CarNumber = model.CarNumber
            });
            if(element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Машина с таким номером уже есть");
            }
        }
    }
}
