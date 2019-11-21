/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class CenteredTextInCell
    {
        public static readonly string DEST = "results/sandbox/tables/centered_text_in_cell.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CenteredTextInCell().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            Paragraph para = new Paragraph("Test").SetFont(font);
            para.SetFixedLeading(0);
            para.SetMultipliedLeading(1);

            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            Cell cell = new Cell();
            cell.SetMinHeight(50);
            cell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            cell.Add(para);
            table.AddCell(cell);

            doc.Add(table);
            
            doc.Close();
        }
    }
}