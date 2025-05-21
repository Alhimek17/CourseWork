using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using ServiceStationBusinessLogic.OfficePackage.HelperEnums;
using ServiceStationBusinessLogic.OfficePackage.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationBusinessLogic.OfficePackage.Implements
{
    public class SaveToPdfGuarantor : AbstractSaveToPdfGuarantor
    {
        private Document? _document;

        private Section? _section;

        private Table? _table;

        private static ParagraphAlignment GetParagraphAligment(PdfParagraphAlignmentType type)
        {
            return type switch
            {
                PdfParagraphAlignmentType.Center => ParagraphAlignment.Center,
                PdfParagraphAlignmentType.Left => ParagraphAlignment.Left,
                PdfParagraphAlignmentType.Right => ParagraphAlignment.Right,
                _ => ParagraphAlignment.Justify,
            };
        }

        private static void DefineStyles(Document document)
        {
            var style = document.Styles["Normal"];
            style.Font.Name = "Times New Roman";
            style.Font.Size = 14;
            style = document.Styles.AddStyle("NormalTitle", "Normal");
            style.Font.Bold = true;
        }

        protected override void CreatePdf(PdfInfoGuarantor info)
        {
            _document = new Document();
            DefineStyles(_document);

            _section = _document.AddSection();
            _section.PageSetup = _document.DefaultPageSetup.Clone();
            _section.PageSetup.LeftMargin = 22;
        }

        protected override void CreateParagraph(PdfParagraph pdfParagraph)
        {
            if (_section == null)
            {
                return;
            }
            var paragraph = _section.AddParagraph(pdfParagraph.Text);
            paragraph.Format.SpaceAfter = "1cm";
            paragraph.Format.Alignment = GetParagraphAligment(pdfParagraph.ParagraphAligment);
            paragraph.Style = pdfParagraph.Style;
        }

        protected override void CreateTable(List<string> columns)
        {
            if (_document == null) { return; }
            _table = _document.LastSection.AddTable();

            foreach (var elem in columns)
            {
                _table.AddColumn(elem);
            }
        }

        protected override void CreateRow(PdfRowParameters rowParameters)
        {
            if (_table == null)
            {
                return;
            }
            var row = _table.AddRow();
            for (int i = 0; i < rowParameters.Texts.Count; ++i)
            {
                row.Cells[i].AddParagraph(rowParameters.Texts[i]);
                if (!string.IsNullOrEmpty(rowParameters.Style))
                {
                    row.Cells[i].Style = rowParameters.Style;
                }
                Unit borderWidth = 0.5;
                row.Cells[i].Borders.Left.Width = borderWidth;
                row.Cells[i].Borders.Right.Width = borderWidth;
                row.Cells[i].Borders.Top.Width = borderWidth;
                row.Cells[i].Borders.Bottom.Width = borderWidth;
                row.Cells[i].Format.Alignment = GetParagraphAligment(rowParameters.ParagraphAligment);
                row.Cells[i].VerticalAlignment = VerticalAlignment.Center;
            }
        }

        protected override void SavePdf(PdfInfoGuarantor info)
        {
            var renderer = new PdfDocumentRenderer(true);
            renderer.Document = _document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(info.FileName);
        }
    }
}
