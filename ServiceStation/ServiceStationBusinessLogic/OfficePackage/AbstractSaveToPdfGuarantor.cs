using ServiceStationBusinessLogic.OfficePackage.HelperEnums;
using ServiceStationBusinessLogic.OfficePackage.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationBusinessLogic.OfficePackage
{
    public abstract class AbstractSaveToPdfGuarantor
    {
        public void CreateDoc(PdfInfoGuarantor info)
        {
            CreatePdf(info);
            CreateParagraph(new PdfParagraph { Text = info.Title, Style = "NormalTitle", ParagraphAligment = PdfParagraphAlignmentType.Center });
            CreateParagraph(new PdfParagraph { Text = $"с {info.DateFrom.ToShortDateString()} по {info.DateTo.ToShortDateString()}", Style = "Normal", ParagraphAligment = PdfParagraphAlignmentType.Center });

            CreateTable(new List<string> { "3cm", "3cm", "3cm", "2cm", "3cm", "3cm", "2cm" });

            CreateRow(new PdfRowParameters
            {
                Texts = new List<string> { "Название запчасти", "Стоимость запчасти", "Название ремонта", "Цена ремонта", "Тип ТО", "Дата ТО", "Цена ТО" },
                Style = "NormalTitle",
                ParagraphAligment = PdfParagraphAlignmentType.Left
            });

            foreach (var sparepart in info.SpareParts)
            {
                bool isRepair = true;
                if (sparepart.RepairPrice.ToString() == "0")
                {
                    isRepair = false;
                }
                CreateRow(new PdfRowParameters
                {
                    Texts = new List<string> { sparepart.SparePartName, sparepart.SparePartPrice.ToString(), isRepair is true ? sparepart.RepairName : "", isRepair is true ? sparepart.RepairPrice.ToString() : "",
                        sparepart.WorkType, isRepair is true ? "" : sparepart.TechnicalWorkDate.Value.ToShortDateString(), isRepair is true ? "" : sparepart.TechnicalWorkPrice.ToString()},
                    Style = "Normal",
                    ParagraphAligment = PdfParagraphAlignmentType.Center
                });
            }
            SavePdf(info);
        }

        protected abstract void CreatePdf(PdfInfoGuarantor info);

        protected abstract void CreateParagraph(PdfParagraph paragraph);

        protected abstract void CreateTable(List<string> columns);

        protected abstract void CreateRow(PdfRowParameters rowParameters);

        protected abstract void SavePdf(PdfInfoGuarantor info);
    }
}
