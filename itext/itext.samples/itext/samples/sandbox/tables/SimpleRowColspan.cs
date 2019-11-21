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
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class SimpleRowColspan
    {
        public static readonly string DEST = "results/sandbox/tables/simple_row_colspan.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleRowColspan().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(new float[] {1, 2, 2, 2, 1}));

            Cell cell = new Cell(2, 1).Add(new Paragraph("S/N"));
            table.AddCell(cell);

            cell = new Cell(1, 3).Add(new Paragraph("Name"));
            table.AddCell(cell);

            cell = new Cell(2, 1).Add(new Paragraph("Age"));
            table.AddCell(cell);

            table.AddCell("SURNAME");
            table.AddCell("FIRST NAME");
            table.AddCell("MIDDLE NAME");
            table.AddCell("1");
            table.AddCell("James");
            table.AddCell("Fish");
            table.AddCell("Stone");
            table.AddCell("17");

            doc.Add(table);

            doc.Close();
        }
    }
}