using ServiceStationBusinessLogic.OfficePackage.HelperEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationBusinessLogic.OfficePackage.HelperModels
{
    public class PdfRowParameters
    {
        public List<string> Texts { get; set; } = new();

        public string Style { get; set; } = string.Empty;

        public PdfParagraphAlignmentType ParagraphAligment { get; set; }
    }
}
