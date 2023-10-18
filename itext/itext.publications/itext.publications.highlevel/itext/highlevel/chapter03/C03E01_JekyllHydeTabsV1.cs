using System;
using System.Collections.Generic;
using System.IO;
using iText.Highlevel.Util;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Highlevel.Chapter03 {
    public class C03E01_JekyllHydeTabsV1 {
        public const String SRC = "../../../resources/data/jekyll_hyde.csv";

        public const String DEST = "../../../results/chapter03/jekyll_hyde_tabs1.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C03E01_JekyllHydeTabsV1().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf, PageSize.A4.Rotate());
            PdfCanvas pdfCanvas = new PdfCanvas(pdf.AddNewPage());
            for (int i = 1; i <= 10; i++) {
                pdfCanvas.MoveTo(document.GetLeftMargin() + i * 50, 0);
                pdfCanvas.LineTo(document.GetLeftMargin() + i * 50, 595);
            }
            pdfCanvas.Stroke();
            IList<IList<String>> resultSet = CsvTo2DList.Convert(SRC, "|");
            foreach (IList<String> record in resultSet) {
                Paragraph p = new Paragraph();
                p.Add(record[0].Trim()).Add(new Tab()).Add(record[1].Trim()).Add(new Tab()).Add(record[2].Trim()).Add(new 
                    Tab()).Add(record[3].Trim()).Add(new Tab()).Add(record[4].Trim()).Add(new Tab()).Add(record[5].Trim());
                document.Add(p);
            }
            document.Close();
        }
    }
}
