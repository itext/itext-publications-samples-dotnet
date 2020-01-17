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
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Annotations
{
    public class AddStamp
    {
        public static readonly String DEST = "results/sandbox/annotations/add_stamp.pdf";

        public static readonly String IMG = "../../resources/img/itext.png";
        public static readonly String SRC = "../../resources/pdfs/hello.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddStamp().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            ImageData img = ImageDataFactory.Create(IMG);
            float width = img.GetWidth();
            float height = img.GetHeight();
            PdfFormXObject xObj = new PdfFormXObject(new Rectangle(width, height));

            PdfCanvas canvas = new PdfCanvas(xObj, pdfDoc);
            canvas.AddImage(img, 0, 0, false);

            Rectangle location = new Rectangle(36, 770 - height, width, height);
            PdfStampAnnotation stamp = new PdfStampAnnotation(location);
            stamp.SetStampName(new PdfName("ITEXT"));
            stamp.SetNormalAppearance(xObj.GetPdfObject());

            // Set to print the annotation when the page is printed
            stamp.SetFlags(PdfAnnotation.PRINT);
            pdfDoc.GetFirstPage().AddAnnotation(stamp);

            pdfDoc.Close();
        }
    }
}