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
    public class ImagesNextToEachOther
    {
        public static readonly string DEST = "../../results/sandbox/tables/image_next_to_each_other.pdf";

        public static readonly string IMG1 = "../../resources/img/javaone2013.jpg";
        public static readonly string IMG2 = "../../resources/img/berlin2013.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ImagesNextToEachOther().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.AddCell(CreateImageCell(IMG1));
            table.AddCell(CreateImageCell(IMG2));

            doc.Add(table);

            doc.Close();
        }

        private static Cell CreateImageCell(string path)
        {
            Image img = new Image(ImageDataFactory.Create(path));
            return new Cell().Add(img.SetAutoScale(true).SetWidth(UnitValue.CreatePercentValue(100)));
        }
    }
}