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
    public class NestedTablesAligned
    {
        public static readonly string DEST = "../../results/sandbox/tables/nested_tables_aligned.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new NestedTablesAligned().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());

            float[] columnWidths = {200f, 200f, 200f};

            Table table = new Table(columnWidths);
            BuildNestedTables(table);

            doc.Add(table);

            doc.Close();
        }

        private void BuildNestedTables(Table outerTable)
        {
            Table innerTable1 = new Table(UnitValue.CreatePercentArray(1));
            innerTable1.SetWidth(100f);
            innerTable1.SetHorizontalAlignment(HorizontalAlignment.LEFT);
            innerTable1.AddCell("Cell 1");
            innerTable1.AddCell("Cell 2");
            outerTable.AddCell(innerTable1);

            Table innerTable2 = new Table(UnitValue.CreatePercentArray(2));
            innerTable2.SetWidth(100f);
            innerTable2.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            innerTable2.AddCell("Cell 3");
            innerTable2.AddCell("Cell 4");
            outerTable.AddCell(innerTable2);

            Table innerTable3 = new Table(UnitValue.CreatePercentArray(2));
            innerTable3.SetWidth(100f);
            innerTable3.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
            innerTable3.AddCell("Cell 5");
            innerTable3.AddCell("Cell 6");
            outerTable.AddCell(innerTable3);
        }
    }
}