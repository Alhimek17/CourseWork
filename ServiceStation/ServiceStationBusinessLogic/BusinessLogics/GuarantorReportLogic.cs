using DocumentFormat.OpenXml.Drawing;
using ServiceStationBusinessLogic.OfficePackage;
using ServiceStationBusinessLogic.OfficePackage.HelperModels;
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
    public class GuarantorReportLogic : IGuarantorReportLogic
    {
        private readonly IDefectStorage _defectStorage;
        private readonly ISparePartStorage _sparepartStorage;
        private readonly IRepairStorage _repairStorage;
        private readonly ITechnicalWorkStorage _techWorkStorage;
        private readonly IWorkStorage _workStorage;
        private readonly AbstractSaveToExcelGuarantor _saveToExcel;
        private readonly AbstractSaveToWordGuarantor _saveToWord;
        private readonly AbstractSaveToPdfGuarantor _saveToPdf;

        public GuarantorReportLogic(IDefectStorage defectStorage, ISparePartStorage sparepartStorage, IRepairStorage repairStorage, ITechnicalWorkStorage techWorkStorage, AbstractSaveToExcelGuarantor saveToExcel, AbstractSaveToPdfGuarantor saveToPdf, AbstractSaveToWordGuarantor saveToWord, IWorkStorage workStorage)
        {
            _defectStorage = defectStorage;
            _sparepartStorage = sparepartStorage;
            _repairStorage = repairStorage;
            _techWorkStorage = techWorkStorage;
            _workStorage = workStorage;
            _saveToExcel = saveToExcel;
            _saveToWord = saveToWord;
            _saveToPdf = saveToPdf;
        }

        public List<ReportDefectsViewModel> GetDefects(List<int> Ids)
        {
            if (Ids == null) return new List<ReportDefectsViewModel>();

            List<ReportDefectsViewModel> allList = new List<ReportDefectsViewModel>();

            var defects = _defectStorage.GetFullList();
            List<SparePartViewModel> spareparts = new List<SparePartViewModel>();
            foreach (var sparepartId in Ids)
            {
                var sparepart = _sparepartStorage.GetElement(new SparePartSearchModel
                {
                    Id = sparepartId,
                });
                if (sparepart != null)
                {
                    spareparts.Add(sparepart);
                }
            }

            foreach (var sparepart in spareparts)
            {
                double price = 0;
                var rec = new ReportDefectsViewModel
                {
                    SparePartName = sparepart.SparePartName,
                    DefectsInfo = new List<(string, double)>()
                };
                foreach (var defect in defects)
                {
                    var repair = new RepairViewModel();
                    if (defect.RepairId.HasValue)
                    {
                        repair = _repairStorage.GetElement(new RepairSearchModel
                        {
                            Id = defect.RepairId,
                        });
                        foreach (var repSpareParts in repair.RepairSpareParts.Values)
                        {
                            if (repSpareParts.Id == sparepart.Id)
                            {
                                rec.DefectsInfo.Add(new(defect.DefectType, defect.DefectPrice));
                                price += defect.DefectPrice;
                            }
                        }
                    }                     
                }
                rec.FullPrice = price;
                allList.Add(rec);
            }
            return allList;
        }

        public List<ReportSparePartsViewModel> GetSpareParts(PdfReportBindingModel model)
        {
            List<ReportSparePartsViewModel> allList = new List<ReportSparePartsViewModel>();

            List<RepairViewModel> repairList = _repairStorage.GetFilteredList(new RepairSearchModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
                GuarantorId = model.GuarantorId,
            });

            List<WorkViewModel> workList = _workStorage.GetFullList();
            List<(WorkViewModel, int)> works = new();

            foreach (var repair in repairList)
            {
                foreach (var sparepart in repair.RepairSpareParts.Values)
                {
                    allList.Add(new ReportSparePartsViewModel
                    {
                        SparePartName = sparepart.SparePartName,
                        SparePartPrice = sparepart.SparePartPrice,
                        RepairName = repair.RepairName,
                        RepairStartDate = repair.RepairStartDate,
                        RepairPrice = repair.RepairPrice,
                    });

                    foreach (var work in workList)
                    {
                        if (work.WorkSpareParts.ContainsKey(sparepart.Id))
                        {
                            int contains = 0;
                            foreach (var workk in works)
                            {
                                var sparepartt = _sparepartStorage.GetElement(new SparePartSearchModel { Id = workk.Item2 });
                                if (workk.Item2 == sparepart.Id && workk.Item1.WorkName == work.WorkName)
                                {
                                    contains++;
                                }
                            }
                            if (contains == 0)
                            {
                                works.Add(new(work, sparepart.Id));
                            }
                        }
                    }
                }
            }

            foreach (var work in works)
            {
                if (work.Item1.TechnicalWorkId.HasValue)
                {
                    var techWork = _techWorkStorage.GetElement(new TechnicalWorkSearchModel { Id = work.Item1.TechnicalWorkId });
                    var sparepart = _sparepartStorage.GetElement(new SparePartSearchModel { Id = work.Item2 });
                    allList.Add(new ReportSparePartsViewModel
                    {
                        SparePartName = sparepart.SparePartName,
                        SparePartPrice = sparepart.SparePartPrice,
                        WorkType = techWork.WorkType,
                        TechnicalWorkDate = techWork.DateStartWork,
                        TechnicalWorkPrice = techWork.WorkPrice,
                    });;
                }
            }
            return allList;
        }

        public void SaveDefectsToWordFile(ReportGuarantorBindingModel model)
        {
            _saveToWord.CreateDoc(new WordInfoGuarantor
            {
                FileStream = model.FileStream,
                Title = "Список неисправностей",
                DefectsBySparePart = GetDefects(model.Ids!)
            });
        }

        public void SaveDefectsToExcelFile(ReportGuarantorBindingModel model)
        {
            _saveToExcel.CreateReport(new ExcelInfoGuarantor
            {
                FileStream = model.FileStream,
                Title = "Список работ",
                DefectsBySparePart = GetDefects(model.Ids!)
            });
        }

        public void SaveSparePartsToPdfFile(PdfReportBindingModel model)
        {
            if (model.DateFrom == null)
            {
                throw new ArgumentException("Дата начала не задана");
            }
            if (model.DateTo == null)
            {
                throw new ArgumentException("Дата окончания не задана");
            }

            _saveToPdf.CreateDoc(new PdfInfoGuarantor
            {
                FileName = model.FileName,
                Title = "Список запчастей",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                SpareParts = GetSpareParts(model)
            });
        }    
    }
}
