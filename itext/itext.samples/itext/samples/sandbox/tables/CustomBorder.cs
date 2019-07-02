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
    public class CustomBorder
    {
        public static readonly string DEST = "../../results/sandbox/tables/custom_border.pdf";

        public static readonly string TEXT = "This is some long paragraph\n" +
                                             "that will be added over and over\n" +
                                             "again to prove a point.\n" +
                                             "It should result in rows\n" +
                                             "that are split\n" +
                                             " and rows that aren't.";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CustomBorder().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.SetWidth(500);

            for (int i = 1; i < 60; i++)
            {
                table.AddCell(new Cell().Add(new Paragraph("Cell " + i)).SetBorderBottom(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(TEXT)).SetBorderBottom(Border.NO_BORDER));
            }

            // the last row
            table.AddCell(new Cell().Add(new Paragraph("Cell " + 60)));
            table.AddCell(new Cell().Add(new Paragraph(TEXT)));

            doc.Add(table);

            doc.Close();
        }
    }
}