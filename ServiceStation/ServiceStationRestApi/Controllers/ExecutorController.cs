using Microsoft.AspNetCore.Mvc;
using ServiceStationContracts.BindingModels;
using ServiceStationContracts.BusinessLogicsContracts;
using ServiceStationContracts.SearchModels;
using ServiceStationContracts.ViewModels;

namespace ServiceStationRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExecutorController : Controller
    {
        private readonly IExecutorLogic _elogic;
        private readonly ILogger _logger;
        
        public ExecutorController(IExecutorLogic elogic, ILogger<ExecutorController> logger)
        {
            _elogic = elogic;
            _logger = logger;
        }

        [HttpPost]
        public void Register(ExecutorBindingModel model)
        {
            try
            {
                _elogic.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка регистрации");
                throw;
            }
        }
        [HttpGet]
        public ExecutorViewModel? Login(string executorNumber, string password)
        {
            try
            {
                return _elogic.ReadElement(new ExecutorSearchModel
                {
                    ExecutorNumber = executorNumber.TrimStart(' '),
                    ExecutorPassword = password
                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка входа в систему");
                throw;
            }
        }
        [HttpPost]
        public void UpdateExecutor(ExecutorBindingModel model)
        {
            try
            {
                _elogic.Update(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных");
                throw;
            }
        }
    }
}
