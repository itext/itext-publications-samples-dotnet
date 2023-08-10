using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Highlevel.Chapter05 {
    /// <author>Bruno Lowagie (iText Software)</author>

    public class C05E02_ColumnWidths1 {
        public const String DEST = "../../../results/chapter05/column_widths1.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E02_ColumnWidths1().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            
            // Initialize document
            Document document = new Document(pdf);
            Table table = new Table(new float[] { 200, 100, 100 });
            table.AddCell(new Cell(1, 3).Add(new Paragraph("Cell with colspan 3")));
            table.AddCell(new Cell(2, 1).Add(new Paragraph("Cell with rowspan 2")));
            table.AddCell("row 1; cell 1");
            table.AddCell("row 1; cell 2");
            table.AddCell("row 2; cell 1");
            table.AddCell("row 2; cell 2");
            document.Add(table);
            document.Close();
        }
    }
}
