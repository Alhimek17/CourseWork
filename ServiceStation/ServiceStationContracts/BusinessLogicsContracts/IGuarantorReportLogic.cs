using ServiceStationContracts.BindingModels;
using ServiceStationContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.BusinessLogicsContracts
{
    public interface IGuarantorReportLogic
    {
        List<ReportDefectsViewModel> GetDefects(List<int> Ids);
        List<ReportSparePartsViewModel> GetSpareParts(PdfReportBindingModel model);

        void SaveDefectsToWordFile(ReportGuarantorBindingModel model);

        void SaveDefectsToExcelFile(ReportGuarantorBindingModel model);

        void SaveSparePartsToPdfFile(PdfReportBindingModel model);
    }
}
