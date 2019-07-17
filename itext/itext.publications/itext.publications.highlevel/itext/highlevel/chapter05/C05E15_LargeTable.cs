/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Highlevel.Chapter05 {
    /// <author>Bruno Lowagie (iText Software)</author>

    public class C05E15_LargeTable {
        public const String DEST = "../../results/chapter05/large_table.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E15_LargeTable().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Table table = new Table(3, true);
            table.AddHeaderCell("Table header 1");
            table.AddHeaderCell("Table header 2");
            table.AddHeaderCell("Table header 3");
            table.AddFooterCell("Table footer 1");
            table.AddFooterCell("Table footer 2");
            table.AddFooterCell("Table footer 3");
            document.Add(table);
            for (int i = 0; i < 1000; i++) {
                table.AddCell(String.Format("Row {0}; column 1", i + 1));
                table.AddCell(String.Format("Row {0}; column 2", i + 1));
                table.AddCell(String.Format("Row {0}; column 3", i + 1));
                if (i % 50 == 0) {
                    table.Flush();
                }
            }
            table.Complete();
            document.Close();
        }
    }
}
