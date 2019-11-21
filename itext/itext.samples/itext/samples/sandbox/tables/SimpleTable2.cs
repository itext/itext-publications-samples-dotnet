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
    public class SimpleTable2
    {
        public static readonly string DEST = "results/sandbox/tables/simple_table2.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleTable2().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(8)).UseAllAvailableWidth();
            
            Cell cell = new Cell(2, 1).Add(new Paragraph("hi"));
            table.AddCell(cell);

            for (int i = 0; i < 14; i++)
            {
                table.AddCell("hi");
            }

            doc.Add(table);

            doc.Close();
        }
    }
}