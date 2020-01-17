/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
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
    public class MultipleImagesInTable
    {
        public static readonly string DEST = "results/sandbox/tables/multiple_images_in_table.pdf";

        public static readonly string IMG1 = "../../resources/img/brasil.png";

        public static readonly string IMG2 = "../../resources/img/dog.bmp";

        public static readonly string IMG3 = "../../resources/img/fox.bmp";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MultipleImagesInTable().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Image img1 = new Image(ImageDataFactory.Create(IMG1));
            Image img2 = new Image(ImageDataFactory.Create(IMG2));
            Image img3 = new Image(ImageDataFactory.Create(IMG3));

            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            table.SetWidth(UnitValue.CreatePercentValue(20));

            img1.SetAutoScale(true);
            img2.SetAutoScale(true);
            img3.SetAutoScale(true);

            table.AddCell(img1);
            table.AddCell("Brazil");
            table.AddCell(img2);
            table.AddCell("Dog");
            table.AddCell(img3);
            table.AddCell("Fox");

            doc.Add(table);

            doc.Close();
        }
    }
}