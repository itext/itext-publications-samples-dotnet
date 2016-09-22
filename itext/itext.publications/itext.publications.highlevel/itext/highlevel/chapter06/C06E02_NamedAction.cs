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
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Test.Attributes;

namespace itext.publications.highlevel.itext.highlevel.chapter06 {
    /// <author>Bruno Lowagie (iText Software)</author>
    [WrapToTest]
    public class C06E02_NamedAction {
        public const String SRC = "../../resources/data/jekyll_hyde.csv";

        public const String DEST = "results/chapter06/jekyll_hyde_named.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C06E02_NamedAction().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            Paragraph p = new Paragraph().Add("Go to last page").SetAction(PdfAction.CreateNamed(PdfName.LastPage));
            document.Add(p);
            IList<IList<String>> resultSet = CsvTo2DList.Convert(SRC, "|");
            resultSet.RemoveAt(0);
            List list = new List(ListNumberingType.DECIMAL);
            foreach (IList<String> record in resultSet) {
                ListItem li = new ListItem();
                li.SetKeepTogether(true);
                li.Add(new Paragraph().SetFontSize(14).Add(record[2])).Add(new Paragraph(String.Format("Directed by {0} ({1}, {2})"
                    , record[3], record[4], record[1])));
                FileInfo file = new FileInfo(String.Format("../../resources/img/{0}.jpg", record[0]));
                if (file.Exists) {
                    iText.Layout.Element.Image img = new Image(ImageDataFactory.Create(file.FullName));
                    img.ScaleToFit(10000, 120);
                    li.Add(img);
                }
                list.Add(li);
            }
            document.Add(list);
            p = new Paragraph().Add("Go to first page").SetAction(PdfAction.CreateNamed(PdfName.FirstPage));
            document.Add(p);
            document.Close();
        }
    }
}
