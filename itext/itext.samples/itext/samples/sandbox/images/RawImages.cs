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
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Images
{
    public class RawImages
    {
        public static readonly String DEST = "results/sandbox/images/raw_images.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RawImages().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());
            int fitWidth = 30;
            int fitHeight = 30;

            // Add the gray square
            Image img = new Image(ImageDataFactory.Create(1, 1, 1, 8,
                new byte[] {(byte) 0x80}, null));
            img.ScaleToFit(fitWidth, fitHeight);
            doc.Add(img);

            // Add the red square
            img = new Image(ImageDataFactory.Create(1, 1, 3, 8,
                new byte[] {(byte) 255, (byte) 0, (byte) 0}, null));
            img.ScaleToFit(fitWidth, fitHeight);
            doc.Add(img);

            // Add the green square
            img = new Image(ImageDataFactory.Create(1, 1, 3, 8,
                new byte[] {(byte) 0, (byte) 255, (byte) 0}, null));
            img.ScaleToFit(fitWidth, fitHeight);
            doc.Add(img);

            // Add the blue square
            img = new Image(ImageDataFactory.Create(1, 1, 3, 8,
                new byte[] {(byte) 0, (byte) 0, (byte) 255}, null));
            img.ScaleToFit(fitWidth, fitHeight);
            doc.Add(img);

            // Add the cyan square
            img = new Image(ImageDataFactory.Create(1, 1, 4, 8,
                new byte[] {(byte) 255, (byte) 0, (byte) 0, (byte) 0}, null));
            img.ScaleToFit(fitWidth, fitHeight);
            doc.Add(img);

            // Add the magenta square
            img = new Image(ImageDataFactory.Create(1, 1, 4, 8,
                new byte[] {(byte) 0, (byte) 255, (byte) 0, (byte) 0}, null));
            img.ScaleToFit(fitWidth, fitHeight);
            doc.Add(img);

            // Add the yellow square
            img = new Image(ImageDataFactory.Create(1, 1, 4, 8,
                new byte[] {(byte) 0, (byte) 0, (byte) 255, (byte) 0}, null));
            img.ScaleToFit(fitWidth, fitHeight);
            doc.Add(img);

            // Add the black square
            img = new Image(ImageDataFactory.Create(1, 1, 4, 8,
                new byte[] {(byte) 0, (byte) 0, (byte) 0, (byte) 255}, null));
            img.ScaleToFit(fitWidth, fitHeight);
            doc.Add(img);

            doc.Close();
        }
    }
}