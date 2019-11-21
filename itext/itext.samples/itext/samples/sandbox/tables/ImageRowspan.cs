/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class ImageRowspan
    {
        public static readonly string DEST = "results/sandbox/tables/image_rowspan.pdf";

        public static readonly string IMG = "../../resources/img/bruno.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ImageRowspan().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.AddCell(new Cell(2, 1).Add(new Image(ImageDataFactory.Create(IMG))
                .SetWidth(UnitValue.CreatePercentValue(100))));
            table.AddCell("1");
            table.AddCell("2");

            doc.Add(table);

            doc.Close();
        }
    }
}