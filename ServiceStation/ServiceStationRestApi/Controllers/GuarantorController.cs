using Microsoft.AspNetCore.Mvc;
using ServiceStationContracts.BindingModels;
using ServiceStationContracts.BusinessLogicsContracts;
using ServiceStationContracts.SearchModels;
using ServiceStationContracts.ViewModels;

namespace ServiceStationRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GuarantorController : Controller
    {
        private readonly IGuarantorLogic _glogic;
        private readonly ILogger _logger;
        public GuarantorController(IGuarantorLogic glogic, ILogger<GuarantorController> logger)
        {
            _glogic = glogic;
            _logger = logger;
        }

        [HttpPost]
        public void Register(GuarantorBindingModel model)
        {
            try
            {
                _glogic.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка регистрации");
                throw;
            }
        }

        [HttpGet]
        public GuarantorViewModel? Login(string guarantorNumber, string password)
        {
            try
            {
                return _glogic.ReadElement(new GuarantorSearchModel
                {
                    GuarantorNumber = guarantorNumber.TrimStart(' '),
                    GuarantorPassword = password
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка входа в систему");
                throw;
            }
        }

        [HttpPost]
        public void UpdateGuarantor(GuarantorBindingModel model)
        {
            try
            {
                _glogic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных");
                throw;
            }
        }
    }
}
