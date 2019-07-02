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
    public class MultipleImagesInCell
    {
        public static readonly string DEST = "../../results/sandbox/tables/multiple_images_in_cell.pdf";

        public static readonly string IMG1 = "../../resources/img/brasil.png";

        public static readonly string IMG2 = "../../resources/img/dog.bmp";

        public static readonly string IMG3 = "../../resources/img/fox.bmp";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MultipleImagesInCell().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Image img1 = new Image(ImageDataFactory.Create(IMG1));
            Image img2 = new Image(ImageDataFactory.Create(IMG2));
            Image img3 = new Image(ImageDataFactory.Create(IMG3));

            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            table.SetWidth(UnitValue.CreatePercentValue(50));
            table.AddCell("Different images, one after the other vertically:");

            Cell cell = new Cell();

            // There's no image autoscaling by default
            cell.Add(img1.SetAutoScale(true));
            cell.Add(img2.SetAutoScale(true));
            cell.Add(img3.SetAutoScale(true));
            table.AddCell(cell);
            doc.Add(table);
            doc.Add(new AreaBreak());

            // In the snippet after this autoscaling is not needed
            // Notice that we do not need to create new Image instances since the images had been already flushed
            img1.SetAutoScale(false);
            img2.SetAutoScale(false);
            img3.SetAutoScale(false);
            table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            table.AddCell("Different images, one after the other vertically, but scaled:");

            cell = new Cell();
            img1.SetWidth(UnitValue.CreatePercentValue(20));
            cell.Add(img1);
            img2.SetWidth(UnitValue.CreatePercentValue(20));
            cell.Add(img2);
            img3.SetWidth(UnitValue.CreatePercentValue(20));
            cell.Add(img3);
            table.AddCell(cell);

            table.AddCell("Different images, one after the other horizontally:");

            // Notice that the table is not flushed yet so it's strictly forbidden to change image properties yet
            img1 = new Image(ImageDataFactory.Create(IMG1));
            img2 = new Image(ImageDataFactory.Create(IMG2));
            img3 = new Image(ImageDataFactory.Create(IMG3));
            Paragraph p = new Paragraph();
            img1.Scale(0.3f, 0.3f);
            p.Add(img1);
            p.Add(img2);
            p.Add(img3);
            table.AddCell(p);
            table.AddCell("Text and images (mixed):");

            img2 = new Image(ImageDataFactory.Create(IMG2));
            img3 = new Image(ImageDataFactory.Create(IMG3));
            p = new Paragraph("The quick brown ");
            p.Add(img3);
            p.Add(" jumps over the lazy ");
            p.Add(img2);
            cell = new Cell();
            cell.Add(p);
            table.AddCell(cell);

            doc.Add(table);
            
            doc.Close();
        }
    }
}