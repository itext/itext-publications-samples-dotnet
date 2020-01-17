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
    public class ColspanRowspan
    {
        public static readonly string DEST = "results/sandbox/tables/colspan_rowspan.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ColspanRowspan().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            Cell cell = new Cell().Add(new Paragraph(" 1,1 "));
            table.AddCell(cell);

            cell = new Cell().Add(new Paragraph(" 1,2 "));
            table.AddCell(cell);

            Cell cell23 = new Cell(2, 2).Add(new Paragraph("multi 1,3 and 1,4"));
            table.AddCell(cell23);

            cell = new Cell().Add(new Paragraph(" 2,1 "));
            table.AddCell(cell);

            cell = new Cell().Add(new Paragraph(" 2,2 "));
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }
    }
}