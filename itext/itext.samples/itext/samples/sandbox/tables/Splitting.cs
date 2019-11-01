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
    public class Splitting
    {
        public static readonly string DEST = "../../results/sandbox/tables/splitting.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new Splitting().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Paragraph p = new Paragraph("Test");
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();

            for (int i = 1; i < 6; i++)
            {
                table.AddCell("key " + i);
                table.AddCell("value " + i);
            }

            for (int i = 0; i < 27; i++)
            {
                doc.Add(p);
            }

            doc.Add(table);

            for (int i = 0; i < 24; i++)
            {
                doc.Add(p);
            }

            table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();

            for (int i = 1; i < 6; i++)
            {
                table.AddCell("key " + i);
                table.AddCell("value " + i);
            }

            Table nesting = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();

            Cell cell = new Cell().Add(table);
            cell.SetBorder(Border.NO_BORDER);

            // iText will make its best to process this cell on a single area
            cell.SetKeepTogether(true);

            nesting.AddCell(cell);

            doc.Add(nesting);

            doc.Close();
        }
    }
}