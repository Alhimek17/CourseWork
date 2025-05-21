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
    public class ExecutorReportLogic : IExecutorReportLogic
    {

        private readonly ITechnicalWorkStorage _techWorkStorage;
        private readonly IRepairStorage _repairStorage;
        private readonly IWorkStorage _workStorage;
        private readonly ICarStorage _carStorage;
        private readonly IDefectStorage _defectStorage;
        private readonly AbstractSaveToExcelExecutor _saveToExcel;
        private readonly AbstractSaveToWordExecutor _saveToWord;
        private readonly AbstractSaveToPdfExecutor _saveToPdf;

        public ExecutorReportLogic(ITechnicalWorkStorage technicalWorkStorage, IRepairStorage repairStorage, IWorkStorage workStorage, ICarStorage carStorage, IDefectStorage defectStorage, 
            AbstractSaveToExcelExecutor saveToExcel, AbstractSaveToWordExecutor saveToWord, AbstractSaveToPdfExecutor saveToPdf)
        {
            _techWorkStorage = technicalWorkStorage;
            _repairStorage = repairStorage;
            _workStorage = workStorage;
            _carStorage = carStorage;
            _defectStorage = defectStorage;
            _saveToExcel = saveToExcel;
            _saveToWord = saveToWord;
            _saveToPdf = saveToPdf;
        }

        public List<ReportWorksViewModel> GetWorks(List<int> Ids)
        {
            if(Ids == null) return new List<ReportWorksViewModel>();

            List<ReportWorksViewModel> allList = new List<ReportWorksViewModel>();

            

            var works = _workStorage.GetFullList();
            List<CarViewModel> cars = new List<CarViewModel>();
            foreach (var carId in Ids)
            {
                var car = _carStorage.GetElement(new CarSearchModel
                {
                    Id = carId,
                });
                if(car != null)
                {
                    cars.Add(car);
                }
            }

            foreach(var car in cars)
            {
                double price = 0;
                var rec = new ReportWorksViewModel
                {
                    CarNumber = car.CarNumber,
                    WorksInfo = new List<(string, double)>()
                };
                foreach(var work in works)
                {
                    if (work.TechnicalWorkId.HasValue)
                    {
                        var techWork = _techWorkStorage.GetElement(new TechnicalWorkSearchModel
                        {
                            Id = work.TechnicalWorkId,
                        });
                        foreach (var techCars in techWork.TechnicalWorkCars.Values)
                        {
                            if (techCars.Id == car.Id)
                            {
                                rec.WorksInfo.Add(new(work.WorkName, work.WorkPrice));
                                price += work.WorkPrice;
                            }
                        }
                    }
                    
                }
                rec.FullPrice = price;
                allList.Add(rec);
            }
            return allList;
        }
        public List<ReportCarsViewModel> GetCars(PdfReportBindingModel model)
        {
            List<ReportCarsViewModel> allList = new List<ReportCarsViewModel>();

            List<TechnicalWorkViewModel> techWorkList = _techWorkStorage.GetFilteredList(new TechnicalWorkSearchModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
                ExecutorId = model.ExecutorId,
            });

            List<DefectViewModel> defectList = _defectStorage.GetFullList();
            List<(DefectViewModel, int)> defects = new();

            foreach (var techWork in techWorkList)
            {
                foreach(var car in techWork.TechnicalWorkCars.Values)
                {
                    allList.Add(new ReportCarsViewModel
                    {
                        CarNumber = car.CarNumber,
                        CarBrand = car.CarBrand,
                        WorkType = techWork.WorkType,
                        TechnicalWorkDate = techWork.DateStartWork,
                        TechnicalWorkPrice = techWork.WorkPrice,
                    });
                    
                    foreach (var defect in defectList)
                    {
                        if (defect.DefectCars.ContainsKey(car.Id))
                        {
                            int contains = 0;
                            foreach (var defectt in defects)
                            {
                                var carr = _carStorage.GetElement(new CarSearchModel { Id = defectt.Item2 });
                                if (defectt.Item2 == car.Id && defectt.Item1.DefectType == defect.DefectType)
                                {
                                    contains++;
                                }
                            }
                            if(contains == 0)
                            {
                                defects.Add(new(defect, car.Id));
                            }
                        }
                    }
                }
            }
            

            foreach (var defect in defects)
            {
                if (defect.Item1.RepairId.HasValue)
                {
                    var repair = _repairStorage.GetElement(new RepairSearchModel { Id = defect.Item1.RepairId });
                    var car = _carStorage.GetElement(new CarSearchModel { Id = defect.Item2 });
                    allList.Add(new ReportCarsViewModel
                    {
                        CarNumber = car.CarNumber,
                        CarBrand = car.CarBrand,
                        RepairName = repair.RepairName,
                        RepairStartDate = repair.RepairStartDate,
                        RepairPrice = repair.RepairPrice,
                    });
                }
            }

            return allList;
        }

        public void SaveWorkByCarsWordFile(ReportExecutorBindingModel model)
        {
            _saveToWord.CreateDoc(new WordInfoExecutor
            {
                FileStream = model.FileStream,
                Title = "Список работ",
                WorksByCar = GetWorks(model.Ids!)
            });
        }

        public void SaveTechWorkAndRepairsByCarsToPdfFile(PdfReportBindingModel model)
        {
            if(model.DateFrom == null)
            {
                throw new ArgumentException("Дата начала не задана");
            }
            if(model.DateTo == null)
            {
                throw new ArgumentException("Дата окончания не задана");
            }

            _saveToPdf.CreateDoc(new PdfInfoExecutor
            {
                FileName = model.FileName,
                Title = "Список машин",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                Cars = GetCars(model)
            });
        }

        public void SaveWorkByCarsToExcelFile(ReportExecutorBindingModel model)
        {
            _saveToExcel.CreateReport(new ExcelInfoExecutor
            {
                FileStream = model.FileStream,
                Title = "Список работ",
                WorksByCar = GetWorks(model.Ids!)
            });
        }
    }
}
