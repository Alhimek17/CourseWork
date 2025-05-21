using ServiceStationBusinessLogic.OfficePackage.HelperEnums;
using ServiceStationBusinessLogic.OfficePackage.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationBusinessLogic.OfficePackage
{
    public abstract class AbstractSaveToWordGuarantor
    {
        public void CreateDoc(WordInfoGuarantor info)
        {
            CreateWord(info);
            CreateParagraph(new WordParagraph
            {
                Texts = new List<(string, WordTextProperties)> { (info.Title, new WordTextProperties { Bold = true, Size = "24", }) },
                TextProperties = new WordTextProperties
                {
                    Size = "24",
                    JustificationType = WordJustificationType.Center
                }
            });

            foreach (var sparepart in info.DefectsBySparePart)
            {
                CreateParagraph(new WordParagraph
                {
                    Texts = new List<(string, WordTextProperties)> { (sparepart.SparePartName, new WordTextProperties { Bold = true, Size = "24", }) },
                    TextProperties = new WordTextProperties
                    {
                        Size = "24",
                        JustificationType = WordJustificationType.Center
                    }
                });
                foreach (var defect in sparepart.DefectsInfo)
                {
                    CreateParagraph(new WordParagraph
                    {
                        Texts = new List<(string, WordTextProperties)>
                        {
                            (defect.Item1, new WordTextProperties{Size = "24", Bold = true}), (" " + defect.Item2.ToString(), new WordTextProperties{Size = "24"})
                        },
                        TextProperties = new WordTextProperties
                        {
                            Size = "24",
                            JustificationType = WordJustificationType.Both
                        }
                    });
                }
                CreateParagraph(new WordParagraph
                {
                    Texts = new List<(string, WordTextProperties)>
                        {
                            ("Итого: ", new WordTextProperties{Size = "24", Bold = true}), (sparepart.FullPrice.ToString(), new WordTextProperties{Size = "24", Bold = true})
                        },
                    TextProperties = new WordTextProperties
                    {
                        Size = "24",
                        JustificationType = WordJustificationType.Both
                    }
                });
            }
            SaveWord(info);
        }

        protected abstract void CreateWord(WordInfoGuarantor info);

        protected abstract void CreateParagraph(WordParagraph paragraph);

        protected abstract void SaveWord(WordInfoGuarantor info);
    }
}
