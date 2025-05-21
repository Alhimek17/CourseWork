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
    public class ExecutorLogic : IExecutorLogic
    {
        private readonly ILogger _logger;
        private readonly IExecutorStorage _executorStorage;

        public ExecutorLogic(ILogger<IExecutorLogic> logger, IExecutorStorage executorStorage)
        {
            _logger = logger;
            _executorStorage = executorStorage;
        }

        public List<ExecutorViewModel>? ReadList(ExecutorSearchModel? model)
        {
            _logger.LogInformation("ReadList. ExecutorFIO:{ExecutorFIO}. Id:{Id}", model?.ExecutorFIO, model?.Id);
            var list = model == null ? _executorStorage.GetFullList() : _executorStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;

        }
        public ExecutorViewModel? ReadElement(ExecutorSearchModel? model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. ExecutorFIO:{ExecutorFIO}. Id:{Id}", model.ExecutorFIO, model?.Id);
            var element = _executorStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element is not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }

        public bool Create(ExecutorBindingModel model)
        {
            CheckModel(model);
            if (_executorStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }
        public bool Update(ExecutorBindingModel model)
        {
            CheckModel(model);
            if (_executorStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;

        }
        public bool Delete(ExecutorBindingModel model)
        {
            CheckModel(model, false);
            if (_executorStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }

        private void CheckModel(ExecutorBindingModel model, bool withParams = true)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if(!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.ExecutorFIO))
            {
                throw new ArgumentNullException("Нет ФИО исполнителя", nameof(model.ExecutorFIO));
            }
            if (string.IsNullOrEmpty(model.ExecutorPassword) || !Regex.IsMatch(model.ExecutorPassword, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", RegexOptions.IgnoreCase))
            {
                throw new ArgumentNullException("Пароль введен неверно", nameof(model.ExecutorPassword));
            }
            if (string.IsNullOrEmpty(model.ExecutorNumber) || !Regex.IsMatch(model.ExecutorNumber, @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", RegexOptions.IgnoreCase))
            {
                throw new ArgumentNullException("Нет номера исполнителя", nameof(model.ExecutorNumber));
            }
            if (string.IsNullOrEmpty(model.ExecutorEmail) || !Regex.IsMatch(model.ExecutorEmail, @"([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)", RegexOptions.IgnoreCase))
            {
                throw new ArgumentNullException("Почта введена неверно", nameof(model.ExecutorEmail));
            }
            _logger.LogInformation("Executor. ExecutorFIO:{ExecutorFIO}. ExecutorPassword:{ExecutorPassword}. ExecutorNumber:{ExecutorNumber}", model.ExecutorFIO, model.ExecutorPassword, model.ExecutorNumber);
            var element = _executorStorage.GetElement(new ExecutorSearchModel
            {
                ExecutorNumber = model.ExecutorNumber
            });
            if(element != null)
            {
                throw new InvalidOperationException("Исполнитель с таким номером уже есть");
            }
            if (model.ExecutorNumber[0] == '+')
            {
                model.ExecutorNumber.TrimStart('+');
            }
        }
    }
}
