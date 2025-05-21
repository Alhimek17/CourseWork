using ServiceStationContracts.BindingModels;
using ServiceStationContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.BusinessLogicsContracts
{
    public interface IExecutorReportLogic
    {
        List<ReportWorksViewModel> GetWorks(List<int> Ids);
        List<ReportCarsViewModel> GetCars(PdfReportBindingModel model);

        void SaveWorkByCarsWordFile(ReportExecutorBindingModel model);

        void SaveWorkByCarsToExcelFile(ReportExecutorBindingModel model);

        void SaveTechWorkAndRepairsByCarsToPdfFile(PdfReportBindingModel model);
    }
}
