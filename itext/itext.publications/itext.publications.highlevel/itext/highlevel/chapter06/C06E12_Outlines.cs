/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.Collections.Generic;
using System.IO;
using iText.Highlevel.Util;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter06 {
    /// <author>Bruno Lowagie (iText Software)</author>
    [WrapToTest]
    public class C06E12_Outlines {
        public const String SRC = "../../resources/data/jekyll_hyde.csv";

        public const String DEST = "../../results/chapter06/jekyll_hyde_outlines.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C06E12_Outlines().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            pdf.AddNewPage();
            pdf.GetCatalog().SetPageMode(PdfName.UseOutlines);
            PdfOutline root = pdf.GetOutlines(false);
            IList<IList<String>> resultSet = CsvTo2DList.Convert(SRC, "|");
            resultSet.RemoveAt(0);
            foreach (IList<String> record in resultSet) {
                PdfOutline movie = root.AddOutline(record[2]);
                PdfOutline imdb = movie.AddOutline("Link to IMDB");
                imdb.SetColor(Color.BLUE);
                imdb.SetStyle(PdfOutline.FLAG_BOLD);
                String url = String.Format("http://www.imdb.com/title/tt{0}", record[0]);
                imdb.AddAction(PdfAction.CreateURI(url));
                PdfOutline info = movie.AddOutline("More info:");
                info.SetOpen(false);
                info.SetStyle(PdfOutline.FLAG_ITALIC);
                PdfOutline director = info.AddOutline("Directed by " + record[3]);
                director.SetColor(Color.RED);
                PdfOutline place = info.AddOutline("Produced in " + record[4]);
                place.SetColor(Color.MAGENTA);
                PdfOutline year = info.AddOutline("Released in " + record[1]);
                year.SetColor(Color.DARK_GRAY);
            }
            pdf.Close();
        }
    }
}
