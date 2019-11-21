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
    public class SimpleTable9
    {
        public static readonly string DEST = "results/sandbox/tables/simple_table9.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleTable9().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("With 3 columns:"));

            Table table = new Table(UnitValue.CreatePercentArray(new float[] {10, 10, 80}));
            table.SetMarginTop(5);
            table.AddCell("Col a");
            table.AddCell("Col b");
            table.AddCell("Col c");
            table.AddCell("Value a");
            table.AddCell("Value b");
            table.AddCell("This is a long description for column c. "
                          + "It needs much more space hence we made sure that the third column is wider.");
            doc.Add(table);

            doc.Add(new Paragraph("With 2 columns:"));

            table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.SetMarginTop(5);
            table.AddCell("Col a");
            table.AddCell("Col b");
            table.AddCell("Value a");
            table.AddCell("Value b");
            table.AddCell(new Cell(1, 2).Add(new Paragraph("Value b")));
            table.AddCell(new Cell(1, 2).Add(new Paragraph("This is a long description for column c. "
                                                           + "It needs much more space hence we made sure that the third column is wider.")));
            table.AddCell("Col a");
            table.AddCell("Col b");
            table.AddCell("Value a");
            table.AddCell("Value b");
            table.AddCell(new Cell(1, 2).Add(new Paragraph("Value b")));
            table.AddCell(new Cell(1, 2).Add(new Paragraph("This is a long description for column c. "
                                                           + "It needs much more space hence we made sure that the third column is wider.")));

            doc.Add(table);

            doc.Close();
        }
    }
}