using ServiceStationBusinessLogic.OfficePackage.HelperEnums;
using ServiceStationBusinessLogic.OfficePackage.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationBusinessLogic.OfficePackage
{
    public abstract class AbstractSaveToPdfExecutor
    {
        public void CreateDoc(PdfInfoExecutor info)
        {
            CreatePdf(info);
            CreateParagraph(new PdfParagraph { Text = info.Title, Style = "NormalTitle", ParagraphAligment = PdfParagraphAlignmentType.Center });
            CreateParagraph(new PdfParagraph { Text = $"с {info.DateFrom.ToShortDateString()} по {info.DateTo.ToShortDateString()}", Style = "Normal", ParagraphAligment = PdfParagraphAlignmentType.Center });

            CreateTable(new List<string> { "3cm", "4cm", "3cm", "3cm", "2cm", "3cm", "2cm" });

            CreateRow( new PdfRowParameters
            {
                Texts = new List<string> { "Номер машины", "Марка машины", "Тип ТО", "Дата ТО", "Цена ТО", "Название ремонта", "Цена ремонта" },
                Style = "NormalTitle",
                ParagraphAligment = PdfParagraphAlignmentType.Left
            });

            foreach(var car in  info.Cars)
            {
                bool isRepair = true;
                if(car.RepairPrice.ToString() == "0")
                {
                    isRepair = false;
                }
                CreateRow(new PdfRowParameters
                {
                    Texts = new List<string> { car.CarNumber, car.CarBrand, car.WorkType, isRepair is true ? "" : car.TechnicalWorkDate.Value.ToShortDateString(), isRepair is true ? "" : car.TechnicalWorkPrice.ToString(),
                        isRepair is true ? car.RepairName : "", isRepair is true ? car.RepairPrice.ToString() : "" },
                    Style = "Normal",
                    ParagraphAligment = PdfParagraphAlignmentType.Center
                });
            }
            SavePdf(info);
        }

        protected abstract void CreatePdf(PdfInfoExecutor info);

        protected abstract void CreateParagraph(PdfParagraph paragraph);

        protected abstract void CreateTable(List<string> columns);

        protected abstract void CreateRow(PdfRowParameters rowParameters);

        protected abstract void SavePdf(PdfInfoExecutor info);
    }
}
