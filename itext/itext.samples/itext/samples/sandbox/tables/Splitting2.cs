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
    public class Splitting2
    {
        public static readonly string DEST = "results/sandbox/tables/splitting2.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new Splitting2().ManipulatePdf(DEST);
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

            table.SetKeepTogether(true);

            doc.Add(table);
            
            doc.Close();
        }
    }
}