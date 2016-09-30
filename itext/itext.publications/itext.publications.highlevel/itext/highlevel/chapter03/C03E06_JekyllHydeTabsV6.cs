/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using iText.Highlevel.Util;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter03 {
    /// <author>iText</author>
    [WrapToTest]
    public class C03E06_JekyllHydeTabsV6 {
        public const String SRC = "../../resources/data/jekyll_hyde.csv";

        public const String DEST = "results/chapter03/jekyll_hyde_tabs6.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C03E06_JekyllHydeTabsV6().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf, PageSize.A4.Rotate());
            float[] stops = new float[] { 40, 580, 590, 720 };
            IList<TabStop> tabstops = new List<TabStop>();
            tabstops.Add(new TabStop(stops[0], TabAlignment.LEFT));
            tabstops.Add(new TabStop(stops[1], TabAlignment.RIGHT));
            tabstops.Add(new TabStop(stops[2], TabAlignment.LEFT));
            TabStop anchor = new TabStop(stops[3], TabAlignment.ANCHOR);
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
                PdfAction uri = PdfAction.CreateURI(String.Format("http://www.imdb.com/title/tt{0}", record[0]));
                Link link = new Link(record[2].Trim(), uri);
                p.Add(record[1].Trim()).Add(new Tab()).Add(link).Add(new Tab()).Add(record[3].Trim()).Add(new Tab()).Add(record
                    [4].Trim()).Add(new Tab()).Add(record[5].Trim() + " \'");
                document.Add(p);
            }
            document.Close();
        }
    }
}
