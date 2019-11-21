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
    public class SimpleTable4
    {
        public static readonly string DEST = "results/sandbox/tables/simple_table4.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleTable4().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
           
            Paragraph right = new Paragraph(
                "This is right, because we create a paragraph with an indentation to the left and as we are adding" +
                " the paragraph in composite mode, all the properties of the paragraph are preserved.");

            right.SetMarginLeft(20);

            Cell rightCell = new Cell().Add(right);
            table.AddCell(rightCell);

            doc.Add(table);

            doc.Close();
        }
    }
}