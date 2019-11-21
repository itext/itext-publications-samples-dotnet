/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class CellWithGlue
    {
        public static readonly string DEST = "results/sandbox/tables/cell_with_glue.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CellWithGlue().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            // By default column width is calculated automatically for the best fit.
            // useAllAvailableWidth() method makes table use the whole page's width while placing the content.
            Table table = new Table(UnitValue.CreatePercentArray(2));
            table.SetHorizontalAlignment(HorizontalAlignment.LEFT);
            table.SetWidth(UnitValue.CreatePercentValue(60));
            table.SetMarginBottom(20);

            Cell cell = new Cell().Add(new Paragraph("Received Rs (in Words):"));

            // Set all the cell's borders except for the right one to have black color and width of 1 point
            cell.SetBorder(new SolidBorder(1));
            cell.SetBorderRight(Border.NO_BORDER);
            table.AddCell(cell);

            cell = new Cell().Add(new Paragraph("Priceless"));
            cell.SetTextAlignment(TextAlignment.RIGHT);
            cell.SetBorder(new SolidBorder(1));
            cell.SetBorderLeft(Border.NO_BORDER);
            table.AddCell(cell);

            doc.Add(table);

            table.SetWidth(UnitValue.CreatePercentValue(50));

            doc.Add(table);

            table = new Table(UnitValue.CreatePercentArray(1));
            table.SetHorizontalAlignment(HorizontalAlignment.LEFT);
            table.SetWidth(UnitValue.CreatePercentValue(50));

            Paragraph p = new Paragraph();
            p.Add(new Text("Received Rs (In Words):"));
            p.AddTabStops(new TabStop(1000, TabAlignment.RIGHT));
            p.Add(new Tab());
            p.Add(new Text("Priceless"));
            table.AddCell(new Cell().Add(p));

            doc.Add(table);
            
            doc.Close();
        }
    }
}