using Microsoft.AspNetCore.Mvc;
using ServiceStationBusinessLogic.MailWorker;
using ServiceStationContracts.BindingModels;
using ServiceStationContracts.BusinessLogicsContracts;

namespace ServiceStationRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly ILogger _logger;
        private readonly IExecutorReportLogic _executorReportLogic;
        private readonly IGuarantorReportLogic _guarantorReportLogic;
        private readonly AbstractMailWorker _mailWorker;

        public ReportController(ILogger<ReportController> logger, IExecutorReportLogic executorReportLogic, AbstractMailWorker abstractMailWorker, IGuarantorReportLogic guarantorReportLogic)
        {
            _logger = logger;
            _executorReportLogic = executorReportLogic;
            _mailWorker = abstractMailWorker;
            _guarantorReportLogic = guarantorReportLogic;
        }
        [HttpPost]
        public void CreateExecutorReportToPdf(PdfReportBindingModel model)
        {
            try
            {
                _executorReportLogic.SaveTechWorkAndRepairsByCarsToPdfFile(new PdfReportBindingModel
                {
                    FileName = model.FileName,
                    DateFrom = model.DateFrom,
                    DateTo = model.DateTo,
                    ExecutorId = model.ExecutorId,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания отчета");
                throw;
            }
        }
        [HttpPost]
        public void SendPdfToMail(MailSendInfoBindingModel model)
        {
            try
            {
                _mailWorker.MailSendAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка отправки письма");
                throw;
            }
        }

        [HttpPost]
        public void CreateGuarantorReportToPdf(PdfReportBindingModel model)
        {
            try
            {
                _guarantorReportLogic.SaveSparePartsToPdfFile(new PdfReportBindingModel
                {
                    FileName = model.FileName,
                    DateFrom = model.DateFrom,
                    DateTo = model.DateTo,
                    GuarantorId = model.GuarantorId,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания отчета");
                throw;
            }
        }
        
    }
}
