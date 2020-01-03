/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
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
    public class NestedTables
    {
        public static readonly string DEST = "results/sandbox/tables/nested_tables.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new NestedTables().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());

            float[] columnWidths = {150, 40, 90, 51, 35, 25, 35, 35, 35, 32, 32, 33, 35, 60, 46, 26};
            Table table = new Table(columnWidths);
            table.SetWidth(770F);

            BuildNestedTables(table);

            doc.Add(new Paragraph("Add table straight to another table"));
            doc.Add(table);

            doc.Close();
        }

        private static void BuildNestedTables(Table outerTable)
        {
            Table innerTable1 = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            Table innerTable2 = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            
            innerTable1.AddCell("Cell 1");
            innerTable1.AddCell("Cell 2");
            outerTable.AddCell(innerTable1);

            innerTable2.AddCell("Cell 3");
            innerTable2.AddCell("Cell 4");
            outerTable.AddCell(innerTable2);

            Cell cell = new Cell(1, 14);
            outerTable.AddCell(cell);
        }
    }
}