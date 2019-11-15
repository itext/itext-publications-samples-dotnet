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
    public class SimpleTable13
    {
        public static readonly string DEST = "../../results/sandbox/tables/simple_table13.pdf";

        public static readonly String[][] DATA =
        {
            new[]
            {
                "John Edward Jr.", "AAA"
            },
            new[]
            {
                "Pascal Einstein W. Alfi", "BBB"
            },
            new[]
            {
                "St. John", "CCC"
            }
        };

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleTable13().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(new float[] {5, 1}));
            table.SetWidth(UnitValue.CreatePercentValue(50));
            table.SetTextAlignment(TextAlignment.LEFT);
            
            table.AddCell(new Cell().Add(new Paragraph("Name: " + DATA[0][0])).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph(DATA[0][1])).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Surname: " + DATA[1][0])).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph(DATA[1][1])).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("School: " + DATA[2][0])).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph(DATA[1][1])).SetBorder(Border.NO_BORDER));

            doc.Add(table);

            doc.Close();
        }
    }
}