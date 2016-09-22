/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace itext.publications.highlevel.itext.highlevel.chapter05 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C05E01_MyFirstTable {
        public const String DEST = "results/chapter05/my_first_table.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E01_MyFirstTable().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Table table = new Table(3);
            table.AddCell(new Cell(1, 3).Add("Cell with colspan 3"));
            table.AddCell(new Cell(2, 1).Add("Cell with rowspan 2"));
            table.AddCell("row 1; cell 1");
            table.AddCell("row 1; cell 2");
            table.AddCell("row 2; cell 1");
            table.AddCell("row 2; cell 2");
            document.Add(table);
            document.Close();
        }
    }
}
