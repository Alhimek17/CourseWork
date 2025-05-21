using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using ServiceStationBusinessLogic.OfficePackage.HelperEnums;
using ServiceStationBusinessLogic.OfficePackage.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationBusinessLogic.OfficePackage.Implements
{
    public class SaveToWordGuarantor : AbstractSaveToWordGuarantor
    {
        private WordprocessingDocument? _wordDocument;

        private Body? _docBody;

        private static JustificationValues GetJustificationValues(WordJustificationType type)
        {
            return type switch
            {
                WordJustificationType.Both => JustificationValues.Both,
                WordJustificationType.Center => JustificationValues.Center,
                _ => JustificationValues.Left,
            };
        }

        private static SectionProperties CreateSectionProperties()
        {
            var properties = new SectionProperties();

            var pageSize = new PageSize
            {
                Orient = PageOrientationValues.Portrait
            };

            properties.AppendChild(pageSize);

            return properties;
        }

        private static ParagraphProperties? CreateParagraphProperties(WordTextProperties? paragraphProperties)
        {
            if (paragraphProperties == null)
            {
                return null;
            }

            var properties = new ParagraphProperties();

            properties.AppendChild(new Justification()
            {
                Val = GetJustificationValues(paragraphProperties.JustificationType)
            });

            properties.AppendChild(new SpacingBetweenLines
            {
                LineRule = LineSpacingRuleValues.Auto
            });

            properties.AppendChild(new Indentation());

            var paragraphMarkRunProperties = new ParagraphMarkRunProperties();
            if (!string.IsNullOrEmpty(paragraphProperties.Size))
            {
                paragraphMarkRunProperties.AppendChild(new FontSize { Val = paragraphProperties.Size });
            }
            properties.AppendChild(paragraphMarkRunProperties);

            return properties;
        }

        protected override void CreateWord(WordInfoGuarantor info)
        {
            _wordDocument = WordprocessingDocument.Create(info.FileStream, WordprocessingDocumentType.Document);
            MainDocumentPart mainPart = _wordDocument.AddMainDocumentPart();
            mainPart.Document = new Document();
            _docBody = mainPart.Document.AppendChild(new Body());
        }

        protected override void CreateParagraph(WordParagraph paragraph)
        {
            if (_docBody == null || paragraph == null)
            {
                return;
            }
            var docParagraph = new Paragraph();

            docParagraph.AppendChild(CreateParagraphProperties(paragraph.TextProperties));

            foreach (var run in paragraph.Texts)
            {
                var docRun = new Run();

                var properties = new RunProperties();
                properties.AppendChild(new FontSize { Val = run.Item2.Size });
                if (run.Item2.Bold)
                {
                    properties.AppendChild(new Bold());
                }
                docRun.AppendChild(properties);
                docRun.AppendChild(new Text { Text = run.Item1, Space = SpaceProcessingModeValues.Preserve });

                docParagraph.AppendChild(docRun);
            }
            _docBody.AppendChild(docParagraph);
        }

        protected override void SaveWord(WordInfoGuarantor info)
        {
            if (_docBody == null || _wordDocument == null)
            {
                return;
            }
            _docBody.AppendChild(CreateSectionProperties());
            _wordDocument.MainDocumentPart!.Document.Save();
            _wordDocument.Dispose();
        }
    }
}
