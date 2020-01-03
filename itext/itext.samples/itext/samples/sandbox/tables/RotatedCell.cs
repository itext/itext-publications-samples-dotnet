/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
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
    public class RotatedCell
    {
        public static readonly string DEST = "results/sandbox/tables/rotated_cell.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RotatedCell().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(8)).UseAllAvailableWidth();

            for (int i = 0; i < 8; i++)
            {
                Cell cell = new Cell().Add(new Paragraph(String.Format("May {0}, 2016", i + 15)));
                cell.SetRotationAngle(Math.PI / 2);
                cell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                table.AddCell(cell);
            }

            for (int i = 0; i < 16; i++)
            {
                table.AddCell("hi");
            }

            doc.Add(table);

            doc.Close();
        }
    }
}