/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter05 {
    /// <author>Bruno Lowagie (iText Software)</author>
    [WrapToTest]
    public class C05E07_NestedTable {
        public const String DEST = "../../results/chapter05/nested_tables.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E07_NestedTable().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Table table = new Table(UnitValue.CreatePercentArray(2));
            table.SetWidth(UnitValue.CreatePercentValue(80));
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.AddCell(new Cell(1, 2).Add(new Paragraph("Cell with colspan 2")));
            table.AddCell(new Cell().Add(new Paragraph("Cell with rowspan 1")));
            Table inner = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            inner.AddCell("row 1; cell 1");
            inner.AddCell("row 1; cell 2");
            inner.AddCell("row 2; cell 1");
            inner.AddCell("row 2; cell 2");
            table.AddCell(inner);
            document.Add(table);
            table = new Table(UnitValue.CreatePercentArray(2));
            table.SetMarginTop(10);
            table.SetWidth(UnitValue.CreatePercentValue(80));
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.AddCell(new Cell(1, 2).Add(new Paragraph("Cell with colspan 2")));
            table.AddCell(new Cell().Add(new Paragraph("Cell with rowspan 1")));
            inner = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            inner.AddCell("row 1; cell 1");
            inner.AddCell("row 1; cell 2");
            inner.AddCell("row 2; cell 1");
            inner.AddCell("row 2; cell 2");
            table.AddCell(new Cell().Add(inner).SetPadding(0));
            document.Add(table);
            document.Close();
        }
    }
}
