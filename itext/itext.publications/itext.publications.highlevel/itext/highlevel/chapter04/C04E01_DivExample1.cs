/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.Collections.Generic;
using System.IO;
using iText.Highlevel.Util;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter04 {
    /// <author>Bruno Lowagie (iText Software)</author>
    [WrapToTest]
    public class C04E01_DivExample1 {
        public const String SRC = "../../resources/data/jekyll_hyde.csv";

        public const String DEST = "results/chapter04/jekyll_hyde_overviewV1.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C04E01_DivExample1().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            IList<IList<String>> resultSet = CsvTo2DList.Convert(SRC, "|");
            resultSet.RemoveAt(0);
            foreach (IList<String> record in resultSet) {
                Div div = new Div().SetBorderLeft(new SolidBorder(2)).SetPaddingLeft(3).SetMarginBottom(10);
                String url = String.Format("http://www.imdb.com/title/tt{0}", record[0]);
                Link movie = new Link(record[2], PdfAction.CreateURI(url));
                div.Add(new Paragraph(movie.SetFontSize(14))).Add(new Paragraph(String.Format("Directed by {0} ({1}, {2})", record
                    [3], record[4], record[1])));
                FileInfo file = new FileInfo(String.Format("../../resources/img/{0}.jpg", record[0]));
                if (file.Exists) {
                    iText.Layout.Element.Image img = new Image(ImageDataFactory.Create(file.FullName));
                    img.ScaleToFit(10000, 120);
                    div.Add(img);
                }
                document.Add(div);
            }
            document.Close();
        }
    }
}
