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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServiceStationBusinessLogic.BusinessLogics
{
    public class GuarantorLogic : IGuarantorLogic
    {
        private readonly ILogger _logger;

        private readonly IGuarantorStorage _guarantorStorage;

        public GuarantorLogic(ILogger<GuarantorLogic> logger, IGuarantorStorage guarantorStorage)
        {
            _logger = logger;
            _guarantorStorage = guarantorStorage;
        }

        public bool Create(GuarantorBindingModel model)
        {
            CheckModel(model);

            if (_guarantorStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }

            return true;
        }

        public bool Delete(GuarantorBindingModel model)
        {
            CheckModel(model, false);

            _logger.LogInformation("Delete. Id:{Id}", model.Id);

            if (_guarantorStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }

            return true;
        }

        public GuarantorViewModel? ReadElement(GuarantorSearchModel? model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("ReadElement. GuarantorFIO:{GuarantorFIO}.Id:{Id}", model.GuarantorFIO, model.Id);

            var element = _guarantorStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }

            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }

        public List<GuarantorViewModel>? ReadList(GuarantorSearchModel? model)
        {
            _logger.LogInformation("ReadList. GuarantorFIO:{GuarantorFIO}.Id:{Id}", model?.GuarantorFIO, model?.Id);

            var list = model == null ? _guarantorStorage.GetFullList() : _guarantorStorage.GetFilteredList(model);

            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }

            _logger.LogInformation("ReadList. Count:{Count}", list.Count);

            return list;
        }

        public bool Update(GuarantorBindingModel model)
        {
            CheckModel(model);

            if (_guarantorStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }

            return true;
        }

        private void CheckModel(GuarantorBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!withParams)
            {
                return;
            }

            if (string.IsNullOrEmpty(model.GuarantorFIO))
            {
                throw new ArgumentNullException("Нет ФИО поручителя", nameof(model.GuarantorFIO));
            }

            if (string.IsNullOrEmpty(model.GuarantorPassword) || !Regex.IsMatch(model.GuarantorPassword, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", RegexOptions.IgnoreCase))
            {
                throw new ArgumentNullException("Пароль введен некорректно", nameof(model.GuarantorPassword));
            }

            if (string.IsNullOrEmpty(model.GuarantorNumber) || !Regex.IsMatch(model.GuarantorNumber, @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", RegexOptions.IgnoreCase))
            {
                throw new ArgumentNullException("Нет номера телефона поручителя", nameof(model.GuarantorNumber));
            }

            if (string.IsNullOrEmpty(model.GuarantorEmail) || !Regex.IsMatch(model.GuarantorEmail, @"([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)", RegexOptions.IgnoreCase))
            {
                throw new ArgumentNullException("Почта введена некорректно", nameof(model.GuarantorEmail));
            }

            _logger.LogInformation("Guarantor. GuarantorFIO:{GuarantorFIO}.GuarantorPassword:{GuarantorPassword}.GuarantorNumber:{GuarantorNumber} Id: {Id}", model.GuarantorFIO, model.GuarantorPassword, model.GuarantorNumber, model.Id);

            var element = _guarantorStorage.GetElement(new GuarantorSearchModel
            {
                GuarantorNumber = model.GuarantorNumber
            });

            if (element != null)
            {
                throw new InvalidOperationException("Поручитель с таким номером уже есть");
            }
            if (model.GuarantorNumber[0] == '+')
            {
                model.GuarantorNumber.TrimStart('+');
            }
        }
    }
}
