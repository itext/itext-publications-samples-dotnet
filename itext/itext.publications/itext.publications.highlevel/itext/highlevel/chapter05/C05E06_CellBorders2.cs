using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Highlevel.Chapter05 {
    /// <author>Bruno Lowagie (iText Software)</author>

    public class C05E06_CellBorders2 {
        public const String DEST = "../../../results/chapter05/cell_borders2.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E06_CellBorders2().CreatePdf(DEST);
        }
        
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            
            // Initialize document
            Document document = new Document(pdf);
            
            Table table = new Table(UnitValue.CreatePercentArray(new float[]{2, 1, 1}));
            table.SetBorder(new SolidBorder(3))
                .SetWidth(UnitValue.CreatePercentValue(80))
                .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                .SetTextAlignment(TextAlignment.CENTER);
            table.AddCell(new Cell(1, 3)
                .Add(new Paragraph("Cell with colspan 3"))
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(2, 1)
                .Add(new Paragraph("Cell with rowspan 2"))
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("row 1; cell 1")));
            table.AddCell(new Cell().Add(new Paragraph("row 1; cell 2")));
            table.AddCell(new Cell().Add(new Paragraph("row 2; cell 1")));
            table.AddCell(new Cell().Add(new Paragraph("row 2; cell 2")));
            document.Add(table);

            document.Close();
        }
    }
}