﻿using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Highlevel.Chapter05 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C05E05_CellPadding {
        
        public const String DEST = "../../../results/chapter05/cell_padding.pdf";
        
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E05_CellPadding().CreatePdf(DEST);
        }
        
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            
            // Initialize document
            Document document = new Document(pdf);
            Table table = new Table(UnitValue.CreatePercentArray(new float[]{2, 1, 1}));
            table.SetBackgroundColor(ColorConstants.ORANGE);
            table.SetWidth(UnitValue.CreatePercentValue(80));
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.AddCell(
                new Cell(1, 3).Add(new Paragraph("Cell with colspan 3"))
                    .SetPadding(10).SetBackgroundColor(ColorConstants.GREEN));
            table.AddCell(new Cell(2, 1).Add(new Paragraph("Cell with rowspan 2"))
                .SetPaddingLeft(30)
                .SetFontColor(ColorConstants.WHITE).SetBackgroundColor(ColorConstants.BLUE));
            table.AddCell(new Cell().Add(new Paragraph("row 1; cell 1"))
                .SetFontColor(ColorConstants.WHITE).SetBackgroundColor(ColorConstants.RED));
            table.AddCell(new Cell().Add(new Paragraph("row 1; cell 2")));
            table.AddCell(new Cell().Add(new Paragraph("row 2; cell 1"))
                .SetFontColor(ColorConstants.WHITE).SetBackgroundColor(ColorConstants.RED));
            table.AddCell(new Cell().Add(new Paragraph("row 2; cell 2")).SetPadding(10)
                .SetFontColor(ColorConstants.WHITE).SetBackgroundColor(ColorConstants.RED));
            document.Add(table);
            document.Close();
        }
    }
}
