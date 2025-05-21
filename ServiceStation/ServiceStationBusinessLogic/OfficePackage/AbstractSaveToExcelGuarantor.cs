using ServiceStationBusinessLogic.OfficePackage.HelperEnums;
using ServiceStationBusinessLogic.OfficePackage.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationBusinessLogic.OfficePackage
{
    public abstract class AbstractSaveToExcelGuarantor
    {
        public void CreateReport(ExcelInfoGuarantor info)
        {
            CreateExcel(info);

            InsertCellInWorksheet(new ExcelCellParameters
            {
                ColumnName = "A",
                RowIndex = 1,
                Text = info.Title,
                StyleInfo = ExcelStyleInfoType.Title,
            });

            MergeCells(new ExcelMergeParameters
            {
                CellFromName = "A1",
                CellToName = "B1"
            });

            uint rowIndex = 2;
            foreach (var wc in info.DefectsBySparePart)
            {
                InsertCellInWorksheet(new ExcelCellParameters
                {
                    ColumnName = "A",
                    RowIndex = rowIndex,
                    Text = wc.SparePartName,
                    StyleInfo = ExcelStyleInfoType.Title,
                });

                MergeCells(new ExcelMergeParameters
                {
                    CellFromName = $"A{rowIndex}",
                    CellToName = $"B{rowIndex}"
                });
                rowIndex++;
                foreach (var defect in wc.DefectsInfo)
                {
                    InsertCellInWorksheet(new ExcelCellParameters
                    {
                        ColumnName = "A",
                        RowIndex = rowIndex,
                        Text = defect.Item1,
                        StyleInfo = ExcelStyleInfoType.TextWithBorder,
                    });
                    InsertCellInWorksheet(new ExcelCellParameters
                    {
                        ColumnName = "B",
                        RowIndex = rowIndex,
                        Text = defect.Item2.ToString(),
                        StyleInfo = ExcelStyleInfoType.TextWithBorder
                    });
                    rowIndex++;
                }
                InsertCellInWorksheet(new ExcelCellParameters
                {
                    ColumnName = "A",
                    RowIndex = rowIndex,
                    Text = "Итого:",
                    StyleInfo = ExcelStyleInfoType.TextWithBorder,
                });
                InsertCellInWorksheet(new ExcelCellParameters
                {
                    ColumnName = "B",
                    RowIndex = rowIndex,
                    Text = wc.FullPrice.ToString(),
                    StyleInfo = ExcelStyleInfoType.TextWithBorder
                });
                rowIndex++;
            }
            SaveExcel(info);
        }

        protected abstract void CreateExcel(ExcelInfoGuarantor info);

        protected abstract void InsertCellInWorksheet(ExcelCellParameters excelParams);

        protected abstract void MergeCells(ExcelMergeParameters excelParams);

        protected abstract void SaveExcel(ExcelInfoGuarantor info);
    }
}
