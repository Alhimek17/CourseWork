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
    public class SparePartLogic : ISparePartLogic
    {
        private readonly ILogger _logger;

        private readonly ISparePartStorage _sparePartStorage;

        public SparePartLogic(ILogger<SparePartLogic> logger, ISparePartStorage sparePartStorage)
        {
            _logger = logger;
            _sparePartStorage = sparePartStorage;
        }

        public bool Create(SparePartBindingModel model)
        {
            CheckModel(model);

            if (_sparePartStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }

            return true;
        }

        public bool Delete(SparePartBindingModel model)
        {
            CheckModel(model, false);

            _logger.LogInformation("Delete. Id:{Id}", model.Id);

            if (_sparePartStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }

            return true;
        }

        public SparePartViewModel? ReadElement(SparePartSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("ReadElement. SparePartName:{SparePartName}.Id:{Id}", model.SparePartName, model.Id);

            var element = _sparePartStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }

            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);

            return element;
        }

        public List<SparePartViewModel>? ReadList(SparePartSearchModel? model)
        {
            _logger.LogInformation("ReadList. SparePartName:{SparePartName}.Id:{Id}", model?.SparePartName, model?.Id);

            var list = model == null ? _sparePartStorage.GetFullList() : _sparePartStorage.GetFilteredList(model);

            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }

            _logger.LogInformation("ReadList. Count:{Count}", list.Count);

            return list;
        }

        public bool Update(SparePartBindingModel model)
        {
            CheckModel(model);

            if (_sparePartStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }

            return true;
        }

        private void CheckModel(SparePartBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!withParams)
            {
                return;
            }

            if (string.IsNullOrEmpty(model.SparePartName))
            {
                throw new ArgumentNullException("Нет названия запчасти", nameof(model.SparePartName));
            }

            if (model.SparePartPrice <= 0)
            {
                throw new ArgumentNullException("Цена запчасти должна быть больше 0", nameof(model.SparePartPrice));
            }

            _logger.LogInformation("SparePart. SparePartName:{SparePartName}.SparePartPrice:{ Price }. Id: {Id}", model.SparePartName, model.SparePartPrice, model.Id);

            var element = _sparePartStorage.GetElement(new SparePartSearchModel
            {
                SparePartName = model.SparePartName
            });

            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Запчасть с таким названием уже есть");
            }
        }
    }
}
