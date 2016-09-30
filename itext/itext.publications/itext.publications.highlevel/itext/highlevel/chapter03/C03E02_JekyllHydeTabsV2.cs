/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/

using System;
using System.Collections.Generic;
using System.IO;
using iText.Highlevel.Util;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter03 {
    /// <author>iText</author>
    [WrapToTest]
    public class C03E02_JekyllHydeTabsV2 {
        public const String SRC = "../../resources/data/jekyll_hyde.csv";

        public const String DEST = "results/chapter03/jekyll_hyde_tabs2.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C03E02_JekyllHydeTabsV2().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf, PageSize.A4.Rotate());
            float[] stops = new float[] { 80, 120, 430, 640, 720 };
            IList<TabStop> tabstops = new List<TabStop>();
            PdfCanvas pdfCanvas = new PdfCanvas(pdf.AddNewPage());
            for (int i = 0; i < stops.Length; i++) {
                tabstops.Add(new TabStop(stops[i]));
                pdfCanvas.MoveTo(document.GetLeftMargin() + stops[i], 0);
                pdfCanvas.LineTo(document.GetLeftMargin() + stops[i], 595);
            }
            pdfCanvas.Stroke();
            IList<IList<String>> resultSet = CsvTo2DList.Convert(SRC, "|");
            foreach (IList<String> record in resultSet) {
                Paragraph p = new Paragraph();
                p.AddTabStops(tabstops);
                p.Add(record[0].Trim()).Add(new Tab()).Add(record[1].Trim()).Add(new Tab()).Add(record[2].Trim()).Add(new 
                    Tab()).Add(record[3].Trim()).Add(new Tab()).Add(record[4].Trim()).Add(new Tab()).Add(record[5].Trim());
                document.Add(p);
            }
            document.Close();
        }
    }
}
