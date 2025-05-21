using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using ServiceStationContracts.BindingModels;
using ServiceStationContracts.BusinessLogicsContracts;
using ServiceStationContracts.SearchModels;
using ServiceStationContracts.ViewModels;
using ServiceStationDatabaseImplement.Models;

namespace ServiceStationRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICarLogic _clogic;
        private readonly IDefectLogic _dlogic;
        private readonly ITechnicalWorkLogic _tlogic;
        private readonly ISparePartLogic _splogic;
        private readonly IRepairLogic _rlogic;
        private readonly IWorkLogic _wlogic;

        public MainController(ILogger<MainController> logger, ICarLogic clogic, IDefectLogic dlogic, ITechnicalWorkLogic tlogic, ISparePartLogic splogic, IRepairLogic rlogic, IWorkLogic wlogic)
        {
            _logger = logger;
            _clogic = clogic;
            _dlogic = dlogic;
            _tlogic = tlogic;
            _splogic = splogic;
            _rlogic = rlogic;
            _wlogic = wlogic;
        }

        [HttpGet]
        public List<CarViewModel>? GetCarList(int executorId)
        {
            try
            {
                return _clogic.ReadList(new CarSearchModel
                {
                    ExecutorId = executorId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка машин");
                throw;
            }
        }

        [HttpGet]
        public List<DefectViewModel>? GetDefects()
        {
            try
            {
                return _dlogic.ReadList(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка неисправностей");
                throw;
            }
        }

        [HttpGet]
        public List<DefectViewModel>? GetDefectList(int executorId)
        {
            try
            {
                return _dlogic.ReadList(new DefectSearchModel
                {
                    ExecutorId = executorId
                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка неисправностей");
                throw;
            }
        }
        [HttpGet]
        public List<TechnicalWorkViewModel>? GetTechnicalWorkList(int executorId)
        {
            try
            {
                return _tlogic.ReadList(new TechnicalWorkSearchModel
                {
                    ExecutorId = executorId
                });
            }
            catch( Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка ТО");
                throw;
            }
        }
        [HttpPost]
        public void CreateCar(CarBindingModel model)
        {
            try
            {
                _clogic.Create(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания машины");
                throw;
            }
        }
        [HttpPost]
        public void CreateDefect(DefectBindingModel model)
        {
            try
            {
                _dlogic.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания неисправности");
                throw;
            }
        }
        [HttpPost]
        public void CreateTechnicalWork(TechnicalWorkBindingModel model)
        {
            try
            {
                _tlogic.Create(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания ТО");
                throw;
            }
        }
        [HttpPost]
        public void DeleteCar(CarBindingModel model)
        {
            try
            {
                _clogic.Delete(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления машины");
                throw;
            }
        }
        [HttpPost]
        public void DeleteDefect(DefectBindingModel model)
        {
            try
            {
                _dlogic.Delete(model);
            }
            catch( Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления неисправности");
                throw;
            }
        }
        [HttpPost]
        public void DeleteTechnicalWork(TechnicalWorkBindingModel model)
        {
            try
            {
                _tlogic.Delete(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления ТО");
                throw;
            }
        }
        [HttpPost]
        public void UpdateCar(CarBindingModel model)
        {
            try
            {
                _clogic.Update(model);
            }
            catch( Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления машины");
                throw;
            }
        }
        [HttpPost]
        public void UpdateDefect(DefectBindingModel model)
        {
            try
            {
                _dlogic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления неисправности");
                throw;
            }
        }
        [HttpPost]
        public void UpdateTechnicalWork(TechnicalWorkBindingModel model)
        {
            try
            {
                _tlogic.Update(model);
            }
            catch( Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления ТО");
                throw;
            }
        }
        [HttpPost]
        public void AddCarToDefect(Tuple<DefectSearchModel, int[]> model)
        {
            try
            {
                _dlogic.AddCarToDefect(model.Item1, model.Item2);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка добавления машины в неисправность.");
                throw;
            }
        }
        [HttpPost]
        public void AddCarToTechnicalWork(Tuple<TechnicalWorkSearchModel, int[]> model)
        {
            try
            {
                _tlogic.AddCarToTechnicalWork(model.Item1, model.Item2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка добавления машины в ТО.");
                throw;
            }
        }
        [HttpGet]
        public CarViewModel? GetCar(int carId)
        {
            try
            {
                return _clogic.ReadElement(new CarSearchModel { Id = carId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения машины по id={Id}", carId);
                throw;
            }
        }
        [HttpGet]
        public Tuple<DefectViewModel, List<Tuple<string, string>>>? GetDefect(int defectId)
        {
            try
            {
                var elem = _dlogic.ReadElement(new DefectSearchModel { Id = defectId });
                if (elem == null) return null;
                return Tuple.Create(elem, elem.DefectCars.Select(x => Tuple.Create(x.Value.CarNumber, x.Value.CarBrand)).ToList());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения машины по id={Id}", defectId);
                throw;
            }
        }
        [HttpGet]
        public Tuple<TechnicalWorkViewModel, List<Tuple<string,string>>>? GetTechnicalWork(int technicalWorkId)
        {
            try
            {
                var elem = _tlogic.ReadElement(new TechnicalWorkSearchModel { Id = technicalWorkId });
                if (elem == null) return null;
                return Tuple.Create(elem, elem.TechnicalWorkCars.Select(x => Tuple.Create(x.Value.CarNumber, x.Value.CarBrand)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения машины по id={Id}", technicalWorkId);
                throw;
            }
        }

        [HttpGet]
        public List<SparePartViewModel>? GetSparePartList(int guarantorId)
        {
            try
            {
                return _splogic.ReadList(new SparePartSearchModel
                {
                    GuarantorId = guarantorId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка запчастей");
                throw;
            }
        }

        [HttpPost]
        public void CreateSparePart(SparePartBindingModel model)
        {
            try
            {
                _splogic.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания запчасти");
                throw;
            }
        }

        [HttpPost]
        public void UpdateSparePart(SparePartBindingModel model)
        {
            try
            {
                _splogic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления запчасти");
                throw;
            }
        }

        [HttpPost]
        public void DeleteSparePart(SparePartBindingModel model)
        {
            try
            {
                _splogic.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления запчасти");
                throw;
            }
        }

        [HttpGet]
        public List<RepairViewModel>? GetRepairList(int guarantorId)
        {
            try
            {
                return _rlogic.ReadList(new RepairSearchModel
                {
                    GuarantorId = guarantorId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка ремонтов");
                throw;
            }
        }

        [HttpPost]
        public void CreateRepair(RepairBindingModel model)
        {
            try
            {
                _rlogic.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания ремонта");
                throw;
            }
        }

        [HttpPost]
        public void UpdateRepair(RepairBindingModel model)
        {
            try
            {
                _rlogic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления ремонта");
                throw;
            }
        }

        [HttpPost]
        public void DeleteRepair(RepairBindingModel model)
        {
            try
            {
                _rlogic.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления ремонта");
                throw;
            }
        }

        [HttpGet]
        public List<WorkViewModel>? GetWorks()
        {
            try
            {
                return _wlogic.ReadList(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка работ");
                throw;
            }
        }

        [HttpGet]
        public List<WorkViewModel>? GetWorkList(int guarantorId)
        {
            try
            {
                return _wlogic.ReadList(new WorkSearchModel
                {
                    GuarantorId = guarantorId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка работ");
                throw;
            }
        }

        [HttpPost]
        public void CreateWork(WorkBindingModel model)
        {
            try
            {
                _wlogic.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания работы");
                throw;
            }
        }

        [HttpPost]
        public void UpdateWork(WorkBindingModel model)
        {
            try
            {
                _wlogic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления работы");
                throw;
            }
        }

        [HttpPost]
        public void UpdateWorkStatus(WorkBindingModel model)
        {
            try
            {
                _wlogic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления статуса работы");
                throw;
            }
        }

        [HttpPost]
        public void UpdateWorkStatusExecution(WorkBindingModel model)
        {
            try
            {
                _wlogic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления статуса работы");
                throw;
            }
        }

        [HttpPost]
        public void DeleteWork(WorkBindingModel model)
        {
            try
            {
                _wlogic.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления работы");
                throw;
            }
        }

        [HttpPost]
        public void AddSparePartToRepair(Tuple<RepairSearchModel, int[]> model)
        {
            try
            {
                _rlogic.AddSparePartToRepair(model.Item1, model.Item2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка добавления запчасти в ремонт.");
                throw;
            }
        }

        [HttpPost]
        public void AddSparePartToWork(Tuple<WorkSearchModel, int[]> model)
        {
            try
            {
                _wlogic.AddSparePartToWork(model.Item1, model.Item2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка добавления запчасти в работу.");
                throw;
            }
        }

        [HttpGet]
        public Tuple<RepairViewModel, List<Tuple<string, string>>>? GetRepair(int repairId)
        {
            try
            {
                var elem = _rlogic.ReadElement(new RepairSearchModel { Id = repairId });
                if (elem == null) return null;
                return Tuple.Create(elem, elem.RepairSpareParts.Select(x => Tuple.Create(x.Value.SparePartName, x.Value.SparePartPrice.ToString("F2"))).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения запчасти по id={Id}", repairId);
                throw;
            }
        }

        [HttpGet]
        public SparePartViewModel? GetSparePart(int sparepartId)
        {
            try
            {
                return _splogic.ReadElement(new SparePartSearchModel { Id = sparepartId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения запчасти по id={Id}", sparepartId);
                throw;
            }
        }

        [HttpGet]
        public Tuple<WorkViewModel, List<Tuple<string, string>>>? GetWork(int workId)
        {
            try
            {
                var elem = _wlogic.ReadElement(new WorkSearchModel { Id = workId });
                if (elem == null) return null;
                return Tuple.Create(elem, elem.WorkSpareParts.Select(x => Tuple.Create(x.Value.SparePartName, x.Value.SparePartPrice.ToString("F2"))).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения запчасти по id={Id}", workId);
                throw;
            }
        }
    }
}
