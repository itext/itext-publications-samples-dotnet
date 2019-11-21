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
    public class HeaderRowRepeated
    {
        public static readonly string DEST = "results/sandbox/tables/header_row_repeated.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new HeaderRowRepeated().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            // table with 2 columns:
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();

            // header row:
            table.AddHeaderCell("Key");
            table.AddHeaderCell("Value");
            table.SetSkipFirstHeader(true);

            // many data rows:
            for (int i = 1; i < 51; i++)
            {
                table.AddCell("key: " + i);
                table.AddCell("value: " + i);
            }

            doc.Add(table);

            doc.Close();
        }
    }
}