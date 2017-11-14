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
    public class C05E03_CellAlignment {
        public const String DEST = "../../results/chapter05/cell_alignment.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E03_CellAlignment().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1, 1 }));
            table.SetWidth(UnitValue.CreatePercentValue(80));
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.SetTextAlignment(TextAlignment.CENTER);
            table.AddCell(new Cell(1, 3).Add(new Paragraph("Cell with colspan 3")));
            table.AddCell(new Cell(2, 1).Add(new Paragraph("Cell with rowspan 2")).SetTextAlignment(TextAlignment.RIGHT));
            table.AddCell("row 1; cell 1");
            table.AddCell("row 1; cell 2");
            table.AddCell("row 2; cell 1");
            table.AddCell("row 2; cell 2");
            Cell cell = new Cell().Add(new Paragraph("Left").SetTextAlignment(TextAlignment.LEFT)).Add(new Paragraph("Center"
                )).Add(new Paragraph("Right").SetTextAlignment(TextAlignment.RIGHT));
            table.AddCell(cell);
            cell = new Cell().Add(new Paragraph("Middle")).SetVerticalAlignment(VerticalAlignment.MIDDLE);
            table.AddCell(cell);
            cell = new Cell().Add(new Paragraph("Bottom")).SetVerticalAlignment(VerticalAlignment.BOTTOM);
            table.AddCell(cell);
            document.Add(table);
            document.Close();
        }
    }
}
