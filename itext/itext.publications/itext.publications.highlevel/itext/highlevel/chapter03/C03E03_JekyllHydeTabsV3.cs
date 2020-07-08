using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using iText.Highlevel.Util;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Highlevel.Chapter03 {
    /// <author>iText</author>
    public class C03E03_JekyllHydeTabsV3 {
        public const String SRC = "../../../resources/data/jekyll_hyde.csv";

        public const String DEST = "../../../results/chapter03/jekyll_hyde_tabs3.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C03E03_JekyllHydeTabsV3().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf, PageSize.A4.Rotate());
            float[] stops = new float[] { 80, 120, 580, 590, 720 };
            IList<TabStop> tabstops = new List<TabStop>();
            tabstops.Add(new TabStop(stops[0], TabAlignment.CENTER));
            tabstops.Add(new TabStop(stops[1], TabAlignment.LEFT));
            tabstops.Add(new TabStop(stops[2], TabAlignment.RIGHT));
            tabstops.Add(new TabStop(stops[3], TabAlignment.LEFT));
            TabStop anchor = new TabStop(stops[4], TabAlignment.ANCHOR);
            anchor.SetTabAnchor(' ');
            tabstops.Add(anchor);
            PdfCanvas pdfCanvas = new PdfCanvas(pdf.AddNewPage());
            for (int i = 0; i < stops.Length; i++) {
                pdfCanvas.MoveTo(document.GetLeftMargin() + stops[i], 0);
                pdfCanvas.LineTo(document.GetLeftMargin() + stops[i], 595);
            }
            pdfCanvas.Stroke();
            IList<IList<String>> resultSet = CsvTo2DList.Convert(SRC, "|");
            foreach (IList<String> record in resultSet) {
                Paragraph p = new Paragraph();
                p.AddTabStops(tabstops);
                p.Add(record[0].Trim()).Add(new Tab()).Add(record[1].Trim()).Add(new Tab()).Add(record[2].Trim()).Add(new 
                    Tab()).Add(record[3].Trim()).Add(new Tab()).Add(record[4].Trim()).Add(new Tab()).Add(record[5].Trim() 
                    + " \'");
                document.Add(p);
            }
            document.Close();
        }
    }
}
