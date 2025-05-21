using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using ServiceStationContracts.BindingModels;
using ServiceStationContracts.BusinessLogicsContracts;
using ServiceStationContracts.SearchModels;
using ServiceStationContracts.ViewModels;
using ServiceStationDatabaseImplement.Models;
using ServiceStationGuarantorApp.Models;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;

namespace ServiceStationGuarantorApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGuarantorReportLogic _report;

        public HomeController(ILogger<HomeController> logger, IGuarantorReportLogic guarantorReportLogic)
        {
            _logger = logger;
			_report = guarantorReportLogic;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            if (APIGuarantor.Guarantor == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIGuarantor.Guarantor);
        }

        [HttpPost]
        public void Privacy(string number, string email, string FIO, string password)
        {
            if (APIGuarantor.Guarantor == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            if (string.IsNullOrEmpty(number) || string.IsNullOrEmpty(FIO) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                throw new Exception("Вы ввели не все данные!");
            }
            APIGuarantor.PostRequest("api/guarantor/updatedata", new GuarantorBindingModel
            {
                Id = APIGuarantor.Guarantor.Id,
                GuarantorNumber = number,
                GuarantorEmail = email,
                GuarantorFIO = FIO,
                GuarantorPassword = password,
            });

            APIGuarantor.Guarantor.GuarantorNumber = number;
            APIGuarantor.Guarantor.GuarantorEmail = email;
            APIGuarantor.Guarantor.GuarantorFIO = FIO;
            APIGuarantor.Guarantor.GuarantorPassword = password;
            Response.Redirect("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Enter()
        {
            return View();
        }

		[HttpPost]
		public void Enter(string number, string password)
		{
			if (string.IsNullOrEmpty(number) || string.IsNullOrEmpty(password))
			{
				throw new Exception("Не хватает пароля или номера.");
			}
			APIGuarantor.Guarantor = APIGuarantor.GetRequest<GuarantorViewModel>($"api/guarantor/login?guarantorNumber={number}&password={password}");
			if (APIGuarantor.Guarantor == null)
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
        public void Register(string FIO, string number, string password, string email)
        {
			if (string.IsNullOrEmpty(FIO) || string.IsNullOrEmpty(number) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
				throw new Exception("Введены не все данные");
			}

			APIGuarantor.PostRequest("api/guarantor/register", new GuarantorBindingModel
			{
				GuarantorNumber = number,
				GuarantorEmail = email,
				GuarantorFIO = FIO,
				GuarantorPassword = password
			});
			Response.Redirect("Enter");
			return;
		}
	
		//......................................................... Запчасть..............................................................................

		public IActionResult ListSpareParts()
		{
			if (APIGuarantor.Guarantor == null)
			{
				return RedirectToAction("Enter");
			}
			return View(APIGuarantor.GetRequest<List<SparePartViewModel>>($"api/main/getsparepartlist?guarantorId={APIGuarantor.Guarantor.Id}"));
		}

		public IActionResult CreateSparePart()
		{
			if (APIGuarantor.Guarantor == null)
			{
				return RedirectToAction("Enter");
			}
			return View();
		}

		[HttpPost]
		public void CreateSparePart(string SparePartName, double SparePartPrice)
		{
			if (APIGuarantor.Guarantor == null)
			{
				throw new Exception("Авторизуйтесь");
			}
			APIGuarantor.PostRequest("api/main/createsparepart", new SparePartBindingModel
			{
				GuarantorId = APIGuarantor.Guarantor.Id,
				SparePartName = SparePartName,
				SparePartPrice = SparePartPrice
			});
			Response.Redirect("ListSpareParts");
		}

		public IActionResult UpdateSparePart()
		{
            ViewBag.SpareParts = new List<SparePartViewModel>();
            if (APIGuarantor.Guarantor == null)
			{
				return RedirectToAction("Enter");
			}
			ViewBag.SpareParts = APIGuarantor.GetRequest<List<SparePartViewModel>>($"api/main/getsparepartlist?guarantorId={APIGuarantor.Guarantor.Id}");
			return View();
		}

		[HttpPost]
		public void UpdateSparePart(int sparepart, string sparepartName, double sparepartPrice)
		{
			if (APIGuarantor.Guarantor == null)
			{
				throw new Exception("Авторизуйтесь");
			}
			if (string.IsNullOrEmpty(sparepartName))
			{
				throw new Exception("Введите название запчасти");
			}
			if (sparepartPrice < 0)
			{
				throw new Exception("Цена запчасти не может быть меньше 0");
			}
			APIGuarantor.PostRequest("api/main/updatesparepart", new SparePartBindingModel
			{
				Id = sparepart,
				SparePartName = sparepartName,
				SparePartPrice = sparepartPrice,
				GuarantorId = APIGuarantor.Guarantor.Id
			});
			Response.Redirect("ListSpareParts");
		}

		public IActionResult DeleteSparePart()
		{
			if (APIGuarantor.Guarantor == null)
			{
				return RedirectToAction("Enter");
			}
			ViewBag.SpareParts = APIGuarantor.GetRequest<List<SparePartViewModel>>($"api/main/getsparepartlist?guarantorId={APIGuarantor.Guarantor.Id}");
			return View();
		}

		[HttpPost]
		public void DeleteSparePart(int sparepart)
		{
			if (APIGuarantor.Guarantor == null)
			{
				throw new Exception("Авторизуйтесь");
			}
			APIGuarantor.PostRequest("api/main/deletesparepart", new SparePartBindingModel
			{
				Id = sparepart
			});
			Response.Redirect("ListSpareParts");
		}

        [HttpGet]
        public SparePartViewModel? GetSparePart(int sparepartId)
        {
            if (APIGuarantor.Guarantor == null)
            {
                throw new Exception("Необходима авторизация");
            }
            var result = APIGuarantor.GetRequest<SparePartViewModel>($"api/main/getsparepart?sparepartId={sparepartId}");
            if (result == null) return default;
            
            return result;
        }

        [HttpGet]
        public string GetSparePartsReport(DateTime dateFrom, DateTime dateTo)
        {
            if (APIGuarantor.Guarantor == null)
            {
                throw new Exception("Авторизуйтесь");
            }
            List<ReportSparePartsViewModel> spareparts;
            try
            {
                spareparts = _report.GetSpareParts(new PdfReportBindingModel
                {
                    GuarantorId = APIGuarantor.Guarantor.Id,
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
            table += "<th scope=\"col\">Название запчасти</th>";
            table += "<th scope=\"col\">Стоимость запчасти</th>";
            table += "<th scope=\"col\">Название ремонта</th>";
            table += "<th scope=\"col\">Цена ремонта</th>";
            table += "<th scope=\"col\">Тип ТО</th>";
            table += "<th scope=\"col\">Дата ТО</th>";
            table += "<th scope=\"col\">Цена ТО</th>";
            table += "</tr>";
            table += "</thead>";
            foreach (var sparepart in spareparts)
            {
                bool isRepair = true;
                if (sparepart.RepairPrice == 0)
                {
                    isRepair = false;
                }
                table += "<tbody>";
                table += "<tr>";
                table += $"<td>{sparepart.SparePartName}</td>";
                table += $"<td>{sparepart.SparePartPrice}</td>";
                table += $"<td>{(isRepair ? sparepart.RepairName : string.Empty)}</td>";
                table += $"<td>{(isRepair ? sparepart.RepairPrice : string.Empty)}</td>";
                table += $"<td>{sparepart.WorkType}</td>";
                table += $"<td>{(isRepair ? string.Empty : sparepart.TechnicalWorkDate)}</td>";
                table += $"<td>{(isRepair ? string.Empty : sparepart.TechnicalWorkPrice)}</td>";
                table += "</tr>";
                table += "</tbody>";
            }
            table += "</table>";
            return table;
        }

        //......................................................... Ремонт..............................................................................

        public IActionResult ListRepairs()
        {
            if (APIGuarantor.Guarantor == null)
            {
				return RedirectToAction("Enter");
			}
			return View(APIGuarantor.GetRequest<List<RepairViewModel>>($"api/main/getrepairlist?guarantorId={APIGuarantor.Guarantor.Id}"));
		}

		[HttpGet]
		public Tuple<RepairViewModel, string>? GetRepair(int repairId)
		{
			if (APIGuarantor.Guarantor == null)
			{
				throw new Exception("Необходима авторизация");
			}
			var result = APIGuarantor.GetRequest<Tuple<RepairViewModel, List<Tuple<string, string>>>>($"api/main/getrepair?repairId={repairId}");
			if (result == null) return default;
			string table = "";
			for (int i = 0; i < result.Item2.Count; i++)
			{
				var repairName = result.Item2[i].Item1;
				var repairPrice = result.Item2[i].Item2;
				table += "<tr>";
				table += $"<td>{repairName}</td>";
				table += $"<td>{repairPrice}</td>";
				table += "</tr>";
			}
			return Tuple.Create(result.Item1, table);
		}

		public IActionResult CreateRepair()
		{
			if (APIGuarantor.Guarantor == null)
			{
				return RedirectToAction("Enter");
			}
            return View(APIGuarantor.GetRequest<List<SparePartViewModel>>($"api/main/getsparepartlist?guarantorId={APIGuarantor.Guarantor.Id}"));
        }

		[HttpPost]
		public void CreateRepair(string repairName, double repairPrice, int[] sparepart)
		{
			if (APIGuarantor.Guarantor == null)
			{
				throw new Exception("Авторизуйтесь");
			}
			APIGuarantor.PostRequest("api/main/createrepair", new RepairBindingModel
			{
				GuarantorId = APIGuarantor.Guarantor.Id,
				RepairName = repairName,
				RepairPrice = repairPrice
			});
            APIGuarantor.PostRequest("api/main/addspareparttorepair", Tuple.Create(
                new RepairSearchModel() { RepairName = repairName },
                sparepart
            ));
            Response.Redirect("ListRepairs");
		}

		public IActionResult UpdateRepair()
		{
			if (APIGuarantor.Guarantor == null)
			{
				return RedirectToAction("Enter");
			}
			ViewBag.Repairs = APIGuarantor.GetRequest<List<RepairViewModel>>($"api/main/getrepairlist?guarantorId={APIGuarantor.Guarantor.Id}");
            return View(APIGuarantor.GetRequest<List<SparePartViewModel>>($"api/main/getsparepartlist?guarantorId={APIGuarantor.Guarantor.Id}"));
        }

		[HttpPost]
		public void UpdateRepair(int repair, string RepairName, double RepairPrice, int[] sparepart)
		{
			if (APIGuarantor.Guarantor == null)
			{
				throw new Exception("Авторизуйтесь");
			}
			if (string.IsNullOrEmpty(RepairName))
			{
				throw new Exception("Введите название ремонта");
			}
			if (RepairPrice < 0)
			{
				throw new Exception("Цена ремонта не может быть меньше 0");
			}
			APIGuarantor.PostRequest("api/main/updaterepair", new RepairBindingModel
			{
				GuarantorId = APIGuarantor.Guarantor.Id,
				Id = repair,
				RepairName = RepairName,
				RepairPrice = RepairPrice
			});
            APIGuarantor.PostRequest("api/main/addspareparttorepair", Tuple.Create(
                new RepairSearchModel() { Id = repair }, 
				sparepart
            ));
            Response.Redirect("ListRepairs");
		}

		public IActionResult DeleteRepair()
		{
			if (APIGuarantor.Guarantor == null)
			{
				return RedirectToAction("Enter");
			}
			ViewBag.Repairs = APIGuarantor.GetRequest<List<RepairViewModel>>($"api/main/getrepairlist?guarantorId={APIGuarantor.Guarantor.Id}");
			return View();
		}

		[HttpPost]
		public void DeleteRepair(int repair)
		{
			if (APIGuarantor.Guarantor == null)
			{
				throw new Exception("Авторизуйтесь");
			}
			APIGuarantor.PostRequest("api/main/deleterepair", new RepairBindingModel { Id = repair });
			Response.Redirect("ListRepairs");
		}

		//......................................................... Работа..............................................................................

		[HttpGet]
        public IActionResult ListWorks()
        {
			if (APIGuarantor.Guarantor == null)
			{
				return RedirectToAction("Enter");
			}
			return View(APIGuarantor.GetRequest<List<WorkViewModel>>($"api/main/getworklist?guarantorId={APIGuarantor.Guarantor.Id}"));
		}

		[HttpGet]
		public Tuple<WorkViewModel, string>? GetWork(int workId)
		{
			if (APIGuarantor.Guarantor == null)
			{
				throw new Exception("Необходима авторизация");
			}
			var result = APIGuarantor.GetRequest<Tuple<WorkViewModel, List<Tuple<string, string>>>>($"api/main/getwork?workId={workId}");
			if (result == null) return default;
			string table = "";
			for (int i = 0; i < result.Item2.Count; i++)
			{
				var sparepartName = result.Item2[i].Item1;
				var sparepartPrice = result.Item2[i].Item2;
				table += "<tr>";
				table += $"<td>{sparepartName}</td>";
				table += $"<td>{sparepartPrice}</td>";
				table += "</tr>";
			}
			return Tuple.Create(result.Item1, table);
		}

		public IActionResult CreateWork()
		{
			if (APIGuarantor.Guarantor == null)
			{
				return RedirectToAction("Enter");
			}
            return View(APIGuarantor.GetRequest<List<SparePartViewModel>>($"api/main/getsparepartlist?guarantorId={APIGuarantor.Guarantor.Id}"));
        }

		[HttpPost]
		public void CreateWork(string WorkName, double WorkPrice, int[] sparepart)
		{
			if (APIGuarantor.Guarantor == null)
			{
				throw new Exception("Авторизуйтесь");
			}
			APIGuarantor.PostRequest("api/main/creatework", new WorkBindingModel
			{
				GuarantorId = APIGuarantor.Guarantor.Id,
				WorkName = WorkName,
				WorkPrice = WorkPrice,
				Status = ServiceStationDataModels.Enums.WorkStatus.Принята
			});
            APIGuarantor.PostRequest("api/main/addspareparttowork", Tuple.Create(
                new WorkSearchModel() { WorkName = WorkName },
                sparepart
            ));
            Response.Redirect("ListWorks");
		}

		public IActionResult UpdateWork()
		{
			if (APIGuarantor.Guarantor == null)
			{
				return RedirectToAction("Enter");
			}
			ViewBag.Works = APIGuarantor.GetRequest<List<WorkViewModel>>($"api/main/getworklist?guarantorId={APIGuarantor.Guarantor.Id}");
            return View(APIGuarantor.GetRequest<List<SparePartViewModel>>($"api/main/getsparepartlist?guarantorId={APIGuarantor.Guarantor.Id}"));
        }

		[HttpPost]
		public void UpdateWork(int work, string WorkName, double WorkPrice, int[] sparepart)
		{
			if (APIGuarantor.Guarantor == null)
			{
				throw new Exception("Авторизуйтесь");
			}
			if (string.IsNullOrEmpty(WorkName))
			{
				throw new Exception("Введите название работы");
			}
			if (WorkPrice < 0)
			{
				throw new Exception("Цена работы не может быть меньше 0");
			}
			APIGuarantor.PostRequest("api/main/updatework", new WorkBindingModel
			{
				Id = work,
				WorkName = WorkName,
				GuarantorId = APIGuarantor.Guarantor.Id,
				WorkPrice = WorkPrice,
			});
            APIGuarantor.PostRequest("api/main/addspareparttowork", Tuple.Create(
                new WorkSearchModel() { Id = work },
                sparepart
            ));
            Response.Redirect("ListWorks");
		}

		public IActionResult DeleteWork()
		{
			if (APIGuarantor.Guarantor == null)
			{
				return RedirectToAction("Enter");
			}
			ViewBag.Works = APIGuarantor.GetRequest<List<WorkViewModel>>($"api/main/getworklist?guarantorId={APIGuarantor.Guarantor.Id}");
			return View();
		}

		[HttpPost]
		public void DeleteWork(int work)
		{
			if (APIGuarantor.Guarantor == null)
			{
				throw new Exception("Авторизуйтесь");
			}
			APIGuarantor.PostRequest("api/main/deletework", new WorkBindingModel { Id = work });
			Response.Redirect("ListWorks");
		}

        [HttpPost]
        public void UpdateWorkStatus(int work, string status)
        {
            if (APIGuarantor.Guarantor == null)
            {
                throw new Exception("Авторизуйтесь");
            }
			if (status == "Отдать на выполнение")
			{
				APIGuarantor.PostRequest("api/main/updateworkstatusexecution", new WorkBindingModel { Id = work, Status = ServiceStationDataModels.Enums.WorkStatus.Выполняется});
			}
			if (status == "Готова")
			{
				APIGuarantor.PostRequest("api/main/updateworkstatusexecution", new WorkBindingModel { Id = work, Status = ServiceStationDataModels.Enums.WorkStatus.Готова});
			}
            Response.Redirect("ListWorks");
        }

        public IActionResult UpdateWorkStatus()
        {
            if (APIGuarantor.Guarantor == null)
            {
                return RedirectToAction("Enter");
            }
            ViewBag.Works = APIGuarantor.GetRequest<List<WorkViewModel>>($"api/main/getworklist?guarantorId={APIGuarantor.Guarantor.Id}");
            return View();
        }

        public IActionResult GetWordFile()
        {
            return new PhysicalFileResult(Directory.GetCurrentDirectory() + "\\Reports\\wordfile.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }
        public IActionResult GetExcelFile()
        {
            return new PhysicalFileResult(Directory.GetCurrentDirectory() + "\\Reports\\excelfile.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        public IActionResult GetPdfFile()
        {
            return new PhysicalFileResult(Directory.GetCurrentDirectory() + "\\Reports\\pdffile.pdf", "application/pdf");
        }

        [HttpGet]
		public IActionResult ListDefectSparePartToFile()
		{
            if (APIGuarantor.Guarantor == null)
            {
                return RedirectToAction("Enter");
            }
            return View(APIGuarantor.GetRequest<List<SparePartViewModel>>($"api/main/getsparepartlist?guarantorId={APIGuarantor.Guarantor.Id}"));
        }

        [HttpPost]
        public IActionResult ListDefectSparePartToFile(int[] Ids, string type)
        {
            if (APIGuarantor.Guarantor == null)
            {
                throw new Exception("Авторизуйтесь");
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
					_report.SaveDefectsToWordFile(new ReportGuarantorBindingModel
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
                    _report.SaveDefectsToExcelFile(new ReportGuarantorBindingModel
                    {
                        FileStream = stream,
                        Ids = res
                    });
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "excelfile.xlsx");
                }
            }
        }

        public IActionResult ListSparePartsToPdfFile()
        {
            if (APIGuarantor.Guarantor == null)
            {
                return RedirectToAction("Enter");
            }
            return View();
        }

        [HttpPost]
		public void ListSparePartsToPdfFile(DateTime dateFrom, DateTime dateTo, string guarantorEmail)
		{
            if (APIGuarantor.Guarantor == null)
            {
                throw new Exception("Авторизуйтесь");
            }
            if (string.IsNullOrEmpty(guarantorEmail))
            {
                throw new Exception("Email пуст");
            }
            APIGuarantor.PostRequest("api/report/createguarantorreporttopdf", new PdfReportBindingModel
            {
                FileName = Directory.GetCurrentDirectory().Replace("ServiceStationGuarantorApp", "ServiceStationRestApi") + "\\Reports\\pdffile.pdf",
                DateFrom = dateFrom,
                DateTo = dateTo,
                GuarantorId = APIGuarantor.Guarantor.Id,
            });
            APIGuarantor.PostRequest("api/report/sendpdftomail", new MailSendInfoBindingModel
            {
                MailAddress = guarantorEmail,
                Subject = "Отчет по запчастям",
                Text = "Отчет по запчастям с " + dateFrom.ToShortDateString() + " по " + dateTo.ToShortDateString()
            });
            Response.Redirect("ListSparePartsToPdfFile");
        }

        public IActionResult BindingRepairToDefects()
        {
            if (APIGuarantor.Guarantor == null)
            {
                return RedirectToAction("Enter");
            }
            return View(Tuple.Create(APIGuarantor.GetRequest<List<RepairViewModel>>($"api/main/getrepairlist?guarantorId={APIGuarantor.Guarantor.Id}"),
            APIGuarantor.GetRequest<List<DefectViewModel>>("api/main/getdefects")));
        }

        [HttpPost]
        public void BindingRepairToDefects(int repair, int defect)
        {
            if (APIGuarantor.Guarantor == null)
            {
                throw new Exception("Авторизуйтесь");
            }
            APIGuarantor.PostRequest("api/main/updatedefect", new DefectBindingModel
            {
                Id = defect,
                RepairId = repair
            });
            Response.Redirect("BindingRepairToDefects");
        }
    }
}
