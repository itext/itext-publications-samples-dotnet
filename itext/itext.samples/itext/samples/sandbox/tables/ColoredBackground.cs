/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class ColoredBackground
    {
        public static readonly string DEST = "results/sandbox/tables/colored_background.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ColoredBackground().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            Table table = new Table(UnitValue.CreatePercentArray(16)).UseAllAvailableWidth();
            
            for (int aw = 0; aw < 16; aw++)
            {
                Cell cell = new Cell().Add(new Paragraph("hi")
                    .SetFont(font)
                    .SetFontColor(ColorConstants.WHITE));
                cell.SetBackgroundColor(ColorConstants.BLUE);
                cell.SetBorder(Border.NO_BORDER);
                cell.SetTextAlignment(TextAlignment.CENTER);
                table.AddCell(cell);
            }

            doc.Add(table);

            doc.Close();
        }
    }
}