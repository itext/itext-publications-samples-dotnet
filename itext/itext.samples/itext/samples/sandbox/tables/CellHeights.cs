/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class CellHeights
    {
        public static readonly string DEST = "../../results/sandbox/tables/cell_heights.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CellHeights().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, PageSize.A5.Rotate());

            // By default column width is calculated automatically for the best fit.
            // useAllAvailableWidth() method makes table use the whole page's width while placing the content.
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();

            // A long phrase with newlines
            Paragraph p = new Paragraph("Dr. iText or:\nHow I Learned to Stop Worrying\nand Love PDF.");
            Cell cell = new Cell().Add(p);

            // The phrase fits the fixed height
            table.AddCell("set height (more than sufficient)");
            cell.SetHeight(172);

            // In iText7 a cell is meant to be used only once in the table.
            // If you want to reuse it, please clone it (either including the content or not)
            table.AddCell(cell.Clone(true));

            // the phrase doesn't fit the fixed height
            table.AddCell("set height (not sufficient)");
            cell.SetHeight(36);
            table.AddCell(cell.Clone(true));

            // The minimum height is exceeded
            table.AddCell("minimum height");
            cell = new Cell().Add(new Paragraph("Dr. iText"));
            cell.SetMinHeight(70);
            table.AddCell(cell.Clone(true));

            // The last cell that should be extended
            table.AddCell("extend last row");
            cell.DeleteOwnProperty(Property.MIN_HEIGHT);
            table.AddCell(cell.Clone(true));

            table.SetExtendBottomRow(true);

            doc.Add(table);
            
            doc.Close();
        }
    }
}