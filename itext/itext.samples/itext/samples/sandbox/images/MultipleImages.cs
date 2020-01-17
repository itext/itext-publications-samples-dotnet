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
    public class MultipleImages
    {
        public static readonly String DEST = "results/sandbox/images/multiple_images.pdf";

        public static readonly String[] IMAGES =
        {
            "../../resources/img/berlin2013.jpg",
            "../../resources/img/javaone2013.jpg",
            "../../resources/img/map_cic.png"
        };

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MultipleImages().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            Image image = new Image(ImageDataFactory.Create(IMAGES[0]));
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(image.GetImageWidth(), image.GetImageHeight()));

            for (int i = 0; i < IMAGES.Length; i++)
            {
                image = new Image(ImageDataFactory.Create(IMAGES[i]));
                pdfDoc.AddNewPage(new PageSize(image.GetImageWidth(), image.GetImageHeight()));
                image.SetFixedPosition(i + 1, 0, 0);
                doc.Add(image);
            }

            doc.Close();
        }
    }
}