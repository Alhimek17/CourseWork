using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServiceStationContracts.BindingModels;
using ServiceStationContracts.BusinessLogicsContracts;
using ServiceStationContracts.SearchModels;
using ServiceStationContracts.ViewModels;
using ServiceStationDatabaseImplement.Models;
using ServiceStationExecutorApp.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace ServiceStationExecutorApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IExecutorReportLogic _report;

        public HomeController(ILogger<HomeController> logger, IExecutorReportLogic executorReportLogic)
        {
            _logger = logger;
            _report = executorReportLogic;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            if(APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            return View(APIExecutor.Executor);
        }
        [HttpPost]
        public void Privacy(string email, string fio, string number, string password)
        {
            if (APIExecutor.Executor == null)
            {
                throw new Exception("Авторизироваться не забыли?");
            }
            if (string.IsNullOrEmpty(fio) || string.IsNullOrEmpty(number) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                throw new Exception("Введены не все данные");
            }
            APIExecutor.PostRequest("api/executor/updateexecutor", new ExecutorBindingModel
            {
                ExecutorEmail = email,
                ExecutorNumber = number,
                ExecutorFIO = fio,
                ExecutorPassword = password,
                Id = APIExecutor.Executor.Id
            });
            Response.Redirect("Index");
        }

        public IActionResult Enter()
        {
            return View();
        }
        [HttpPost]
        public void Enter(string executorNumber, string password)
        {
            if(string.IsNullOrEmpty(executorNumber) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Не хватает пароля или номера.");
            }
            APIExecutor.Executor = APIExecutor.GetRequest<ExecutorViewModel>($"api/executor/login?executorNumber={executorNumber}&password={password}");
            if (APIExecutor.Executor == null)
            {
                throw new Exception("Неверный логин/пароль");
            }
            Response.Redirect("Index");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public void Register(string fio, string executorNumber, string password, string email)
        {
            if(string.IsNullOrEmpty(fio) || string.IsNullOrEmpty(executorNumber) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                throw new Exception("Введены не все данные");
            }
            APIExecutor.PostRequest("api/executor/register", new ExecutorBindingModel
            {
                ExecutorNumber = executorNumber,
                ExecutorEmail = email,
                ExecutorFIO = fio,
                ExecutorPassword = password
            });
            Response.Redirect("Enter");
            return;
        }
        public IActionResult ListCars()
        {
            if (APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            return View(APIExecutor.GetRequest<List<CarViewModel>>($"api/main/getcarlist?executorId={APIExecutor.Executor.Id}"));
        }
        public IActionResult ListDefects()
        {
            if (APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            return View(APIExecutor.GetRequest<List<DefectViewModel>>($"api/main/getdefectlist?executorId={APIExecutor.Executor.Id}"));
        }
        public IActionResult ListTechnicalWorks()
        {
            if (APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            return View(APIExecutor.GetRequest<List<TechnicalWorkViewModel>>($"api/main/gettechnicalworklist?executorId={APIExecutor.Executor.Id}"));
        }
        [HttpGet]
        public CarViewModel? GetCar(int carId)
        {
            if (APIExecutor.Executor == null)
            {
                throw new Exception("Необходима авторизация");
            }
            var result = APIExecutor.GetRequest<CarViewModel>($"api/main/getcar?carId={carId}");
            if (result == null) return null;

            return result;
        }
        [HttpGet]
        public Tuple<DefectViewModel, string>? GetDefect(int defectId)
        {
            if(APIExecutor.Executor == null)
            {
                throw new Exception("Необходима авторизация");
            }
            var result = APIExecutor.GetRequest<Tuple<DefectViewModel, List<Tuple<string, string>>>>($"api/main/getdefect?defectId={defectId}");
            if (result == null) return default;
            string table = "";
            for (int i = 0; i < result.Item2.Count; i++)
            {
                var carNumber = result.Item2[i].Item1;
                var carBrand = result.Item2[i].Item2;
                table += "<tr>";
                table += $"<td>{carNumber}</td>";
                table += $"<td>{carBrand}</td>";
                table += "</tr>";
            }

            return Tuple.Create(result.Item1, table);
        }
        [HttpGet]
        public Tuple<TechnicalWorkViewModel, string>? GetTechnicalWork(int technicalWorkId)
        {
            if(APIExecutor.Executor == null)
            {
                throw new Exception("Необходима авторизация");
            }
            var result = APIExecutor.GetRequest<Tuple<TechnicalWorkViewModel, List<Tuple<string, string>>>>($"api/main/gettechnicalwork?technicalworkId={technicalWorkId}");
            if (result == null) return default;
            string table = "";
            for(int i = 0;i<result.Item2.Count; i++)
            {
                var carNumber = result.Item2[i].Item1;
                var carBrand = result.Item2[i].Item2;
                table += "<tr>";
                table += $"<td>{carNumber}</td>";
                table += $"<td>{carBrand}</td>";
                table += "</tr>";
            }
            return Tuple.Create(result.Item1, table);
        }
        public IActionResult DeleteCar()
        {
            if (APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            ViewBag.Cars = APIExecutor.GetRequest<List<CarViewModel>>($"api/main/getcarlist?executorId={APIExecutor.Executor.Id}");
            return View();
        }
        [HttpPost]
        public void DeleteCar(int car)
        {
            if (APIExecutor.Executor == null)
            {
                throw new Exception("Авторизироваться не забыли?");
            }
            APIExecutor.PostRequest("api/main/deletecar", new CarBindingModel
            {
                Id = car
            });
            Response.Redirect("ListCars");
        }
        public IActionResult DeleteDefect()
        {
            if (APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            ViewBag.Defects = APIExecutor.GetRequest<List<DefectViewModel>>($"api/main/getdefectlist?executorId={APIExecutor.Executor.Id}");
            return View();
        }
        [HttpPost]
        public void DeleteDefect(int defect)
        {
            if (APIExecutor.Executor == null)
            {
                throw new Exception("Авторизироваться не забыли?");
            }
            APIExecutor.PostRequest("api/main/deletedefect", new DefectBindingModel { Id = defect });
            Response.Redirect("ListDefects");
        }
        public IActionResult DeleteTechnicalWork()
        {
            if (APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            ViewBag.TechnicalWorks = APIExecutor.GetRequest<List<TechnicalWorkViewModel>>($"api/main/gettechnicalworklist?executorId={APIExecutor.Executor.Id}");
            return View();
        }
        [HttpPost]
        public void DeleteTechnicalWork(int technicalWork)
        {
            if (APIExecutor.Executor == null)
            {
                throw new Exception("Авторизироваться не забыли?");
            }
            APIExecutor.PostRequest("api/main/deletetechnicalwork", new TechnicalWorkBindingModel { Id = technicalWork });
            Response.Redirect("ListTechnicalWorks");
        }
        public IActionResult UpdateCar()
        {
            if (APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            ViewBag.Cars = APIExecutor.GetRequest<List<CarViewModel>>($"api/main/getcarlist?executorId={APIExecutor.Executor.Id}");
            return View();
        }
        [HttpPost]
        public void UpdateCar(int car, string carNumber, string carBrand)
        {
            if (APIExecutor.Executor == null)
            {
                throw new Exception("Авторизироваться не забыли?");
            }
            if (string.IsNullOrEmpty(carNumber))
            {
                throw new Exception("Введите номер машины");
            }
            if (string.IsNullOrEmpty(carBrand))
            {
                throw new Exception("Введите бренд машины");
            }
            APIExecutor.PostRequest("api/main/updatecar", new CarBindingModel
            {
                Id = car,
                CarNumber = carNumber,
                CarBrand = carBrand,
                ExecutorId = APIExecutor.Executor.Id
            });
            Response.Redirect("ListCars");
        }
        public IActionResult UpdateDefect()
        {
            if (APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            ViewBag.Defects = APIExecutor.GetRequest<List<DefectViewModel>>($"api/main/getdefectlist?executorId={APIExecutor.Executor.Id}");
            return View(APIExecutor.GetRequest<List<CarViewModel>>($"api/main/getcarlist?executorId={APIExecutor.Executor.Id}"));
        }
        [HttpPost]
        public void UpdateDefect(int defect, string DefectType, double DefectPrice, int[] car)
        {
            if (APIExecutor.Executor == null)
            {
                throw new Exception("Авторизироваться не забыли?");
            }
            if (string.IsNullOrEmpty(DefectType))
            {
                throw new Exception("Введите тип неисправности");
            }
            if(DefectPrice < 100)
            {
                throw new Exception("Минимальная цена неисправности 100");
            }
            APIExecutor.PostRequest("api/main/updatedefect", new DefectBindingModel
            {
                ExecutorId = APIExecutor.Executor.Id,
                Id = defect,
                DefectType = DefectType,
                DefectPrice = DefectPrice
            });
            APIExecutor.PostRequest("api/main/addcartodefect", Tuple.Create(
                new DefectSearchModel() { Id = defect },
                car
            ));
            Response.Redirect("ListDefects");
        }
        public IActionResult UpdateTechnicalWork()
        {
            if (APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            ViewBag.TechnicalWorks = APIExecutor.GetRequest<List<TechnicalWorkViewModel>>($"api/main/gettechnicalworklist?executorId={APIExecutor.Executor.Id}");
            return View(APIExecutor.GetRequest<List<CarViewModel>>($"api/main/getcarlist?executorId={APIExecutor.Executor.Id}"));
        }
        [HttpPost]
        public void UpdateTechnicalWork(int technicalWork, string worktype, double workPrice, int[] car)
        {
            if (APIExecutor.Executor == null)
            {
                throw new Exception("Авторизироваться не забыли?");
            }
            if (string.IsNullOrEmpty(worktype))
            {
                throw new Exception("Введите тип ТО");
            }
            if(workPrice < 100)
            {
                throw new Exception("Цена ТО должны быть больше 100");
            }
            APIExecutor.PostRequest("api/main/updatetechnicalwork", new TechnicalWorkBindingModel
            {
                Id = technicalWork,
                WorkType = worktype,
                ExecutorId = APIExecutor.Executor.Id,
                WorkPrice = workPrice,
            });
            APIExecutor.PostRequest("api/main/addcartotechnicalwork", Tuple.Create(
                new TechnicalWorkSearchModel() { Id = technicalWork },
                car
            ));
            Response.Redirect("ListTechnicalWorks");
        }
        public IActionResult CreateCar()
        {
            if (APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            return View();
        }
        [HttpPost]
        public void CreateCar(string CarNumber, string CarBrand)
        {
            if(APIExecutor.Executor == null)
            {
                throw new Exception("Авторизироваться не забыли?");
            }
            APIExecutor.PostRequest("api/main/createcar", new CarBindingModel
            {
                ExecutorId = APIExecutor.Executor.Id,
                CarNumber = CarNumber,
                CarBrand = CarBrand
            });
            Response.Redirect("ListCars");
        }
        public IActionResult CreateDefect()
        {
            if (APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            return View(APIExecutor.GetRequest<List<CarViewModel>>($"api/main/getcarlist?executorId={APIExecutor.Executor.Id}"));
        }
        [HttpPost]
        public void CreateDefect(string defectType, double defectPrice, int[] car)
        {
            if (APIExecutor.Executor == null)
            {
                throw new Exception("Авторизироваться не забыли?");
            }
            APIExecutor.PostRequest("api/main/createdefect", new DefectBindingModel
            {
                ExecutorId = APIExecutor.Executor.Id,
                DefectType = defectType,
                DefectPrice = defectPrice
            });
            APIExecutor.PostRequest("api/main/addcartodefect", Tuple.Create(
                new DefectSearchModel() { DefectType = defectType },
                car
            ));
            Response.Redirect("ListDefects");
        }
        public IActionResult CreateTechnicalWork()
        {
            if (APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            return View(APIExecutor.GetRequest<List<CarViewModel>>($"api/main/getcarlist?executorId={APIExecutor.Executor.Id}"));
        }
        [HttpPost]
        public void CreateTechnicalWork(string WorkType, double WorkPrice, int[] car)
        {
            if (APIExecutor.Executor == null)
            {
                throw new Exception("Авторизироваться не забыли?");
            }
            APIExecutor.PostRequest("api/main/createtechnicalwork", new TechnicalWorkBindingModel
            {
                ExecutorId = APIExecutor.Executor.Id,
                WorkType = WorkType,
                WorkPrice = WorkPrice,
                DateStartWork = DateTime.Now
            });
            APIExecutor.PostRequest("api/main/addcartotechnicalwork", Tuple.Create(
                new TechnicalWorkSearchModel() { WorkType = WorkType },
                car
            ));
            Response.Redirect("ListTechnicalWorks");
        }
        public IActionResult BindingTechnicalWorkToWork()
        {
            if (APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            return View(Tuple.Create(APIExecutor.GetRequest<List<TechnicalWorkViewModel>>($"api/main/gettechnicalworklist?executorId={APIExecutor.Executor.Id}"),
                APIExecutor.GetRequest<List<WorkViewModel>>("api/main/getworks")));
        }
        [HttpPost]
        public void BindingTechnicalWorkToWork(int technicalWork, int work)
        {
            if (APIExecutor.Executor == null)
            {
                throw new Exception("Авторизироваться не забыли?");
            }
            APIExecutor.PostRequest("api/main/updatework", new WorkBindingModel
            {
                Id = work,
                TechnicalWorkId = technicalWork
            });
            Response.Redirect("BindingTechnicalWorkToWork");
        }
        [HttpGet]
        public IActionResult ListWorkToFile()
        {
            if (APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            return View(APIExecutor.GetRequest<List<CarViewModel>>($"api/main/getcarlist?executorId={APIExecutor.Executor.Id}"));
        }

        [HttpPost]
        public IActionResult ListWorkToFile(int[] Ids, string type)
        {
            if (APIExecutor.Executor == null)
            {
                throw new Exception("Авторизироваться не забыли?");
            }
            if (Ids.Length <= 0)
            {
                throw new Exception("Кол-во меньше нуля");
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new Exception("Неопознанный тип");
            }

            List<int> res = new List<int>();

            foreach (var id in Ids)
            {
                res.Add(id);
            }

            if (type == "docx")
            {
                using (var stream = new MemoryStream())
                {
                    _report.SaveWorkByCarsWordFile(new ReportExecutorBindingModel
                    {
                        FileStream = stream,
                        Ids = res
                    });
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "wordfile.docx");
                }
            }
            else
            {
                using (var stream = new MemoryStream())
                {
                    _report.SaveWorkByCarsToExcelFile(new ReportExecutorBindingModel
                    {
                        FileStream = stream,
                        Ids = res
                    });
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "excelfile.xlsx");
                }
            }
        }
        public IActionResult ListCarsToPdf()
        {
            if (APIExecutor.Executor == null)
            {
                return RedirectToAction("Enter");
            }
            return View();
        }
        [HttpPost]
        public void ListCarsToPdf(DateTime dateFrom, DateTime dateTo, string executorEmail)
        {
            if (APIExecutor.Executor == null)
            {
                throw new Exception("Авторизироваться не забыли?");
            }
            if (string.IsNullOrEmpty(executorEmail))
            {
                throw new Exception("Email пуст");
            }
            APIExecutor.PostRequest("api/report/createexecutorreporttopdf", new PdfReportBindingModel
            {
                FileName = Directory.GetCurrentDirectory().Replace("ServiceStationExecutorApp", "ServiceStationRestApi") + "\\Reports\\pdffile.pdf",
                DateFrom = dateFrom,
                DateTo = dateTo,
                ExecutorId = APIExecutor.Executor.Id,
            }) ;
            APIExecutor.PostRequest("api/report/sendpdftomail", new MailSendInfoBindingModel
            {
                MailAddress = executorEmail,
                Subject = "Отчет по машинам",
                Text = "Отчет по машинам с " + dateFrom.ToShortDateString() + " по " + dateTo.ToShortDateString()
            });
            Response.Redirect("ListCarsToPdf");
        }

        [HttpGet]
        public string GetCarsReport(DateTime dateFrom, DateTime dateTo)
        {
            if (APIExecutor.Executor == null)
            {
                throw new Exception("Авторизироваться не забыли?");
            }
            List<ReportCarsViewModel> cars;
            try
            {
                cars = _report.GetCars(new PdfReportBindingModel
                {
                    ExecutorId = APIExecutor.Executor.Id,
                    DateFrom = dateFrom,
                    DateTo = dateTo,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания отчета");
                throw;
            }
            string table = "";
            table += "<h2>Предварительный отчет</h2>";
            table += "<table class=\"table\">";
            table += "<thead class=\"thead-dark\">";
            table += "<tr>";
            table += "<th scope=\"col\">Номер машины</th>";
            table += "<th scope=\"col\">Марка машины</th>";
            table += "<th scope=\"col\">Тип ТО</th>";
            table += "<th scope=\"col\">Дата ТО</th>";
            table += "<th scope=\"col\">Цена ТО</th>";
            table += "<th scope=\"col\">Название ремонта</th>";
            table += "<th scope=\"col\">Цена ремонта</th>";
            table += "</tr>";
            table += "</thead>";
            foreach(var car in cars)
            {
                bool isRepair = true;
                if (car.RepairPrice == 0)
                {
                    isRepair = false;
                }
                table += "<tbody>";
                table += "<tr>";
                table += $"<td>{car.CarNumber}</td>";
                table += $"<td>{car.CarBrand}</td>";
                table += $"<td>{car.WorkType}</td>";
                table += $"<td>{(isRepair ? string.Empty : car.TechnicalWorkDate)}</td>";
                table += $"<td>{(isRepair ? string.Empty : car.TechnicalWorkPrice)}</td>";
                table += $"<td>{(isRepair ? car.RepairName : string.Empty)}</td>";
                table += $"<td>{(isRepair ? car.RepairPrice : string.Empty)}</td>";
                table += "</tr>";
                table += "</tbody>";
            }
            table += "</table>";
            return table;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
