/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class RightCornerTable
    {
        public static readonly string DEST = "results/sandbox/tables/right_corner_table.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RightCornerTable().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(300, 300));
            doc.SetMargins(0, 0, 0, 0);

            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            table.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
            table.SetWidth(90);

            Cell cell = new Cell().Add(new Paragraph(" Date").SetFontColor(ColorConstants.WHITE));
            cell.SetBackgroundColor(ColorConstants.BLACK);
            cell.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            table.AddCell(cell);
            
            Cell cellTwo = new Cell().Add(new Paragraph("10/01/2015"));
            cellTwo.SetBorder(new SolidBorder(2));
            table.AddCell(cellTwo);

            doc.Add(table);

            doc.Close();
        }
    }
}