using DocumentFormat.OpenXml.Presentation;
using ServiceStationBusinessLogic.OfficePackage.HelperEnums;
using ServiceStationBusinessLogic.OfficePackage.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationBusinessLogic.OfficePackage
{
    public abstract class AbstractSaveToWordExecutor
    {
        public void CreateDoc(WordInfoExecutor info)
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

            foreach(var car in info.WorksByCar)
            {
                CreateParagraph(new WordParagraph
                {
                    Texts = new List<(string, WordTextProperties)> { (car.CarNumber, new WordTextProperties { Bold = true, Size = "24", }) },
                    TextProperties = new WordTextProperties
                    {
                        Size = "24",
                        JustificationType = WordJustificationType.Center
                    }
                });
                foreach(var work in car.WorksInfo)
                {
                    CreateParagraph(new WordParagraph
                    {
                        Texts = new List<(string, WordTextProperties)>
                        {
                            (work.Item1, new WordTextProperties{Size = "24", Bold = true}), (" " +work.Item2.ToString(), new WordTextProperties{Size = "24"})
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
                            ("Итого: ", new WordTextProperties{Size = "24", Bold = true}), (car.FullPrice.ToString(), new WordTextProperties{Size = "24", Bold = true})
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

        protected abstract void CreateWord(WordInfoExecutor info);

        protected abstract void CreateParagraph(WordParagraph paragraph);

        protected abstract void SaveWord(WordInfoExecutor info);
    }
}
