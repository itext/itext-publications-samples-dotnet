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
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Images
{
    public class WatermarkedImages4
    {
        public static readonly String DEST = "results/sandbox/images/watermarked_images4.pdf";

        public static readonly String IMAGE1 = "../../../resources/img/bruno.jpg";
        public static readonly String IMAGE2 = "../../../resources/img/dog.bmp";
        public static readonly String IMAGE3 = "../../../resources/img/fox.bmp";
        public static readonly String IMAGE4 = "../../../resources/img/bruno_ingeborg.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new WatermarkedImages4().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Image img = GetWatermarkedImage(pdfDoc, new Image(ImageDataFactory.Create(IMAGE1)));
            doc.Add(img);

            img = GetWatermarkedImage(pdfDoc, new Image(ImageDataFactory.Create(IMAGE2)));
            doc.Add(img);

            img = GetWatermarkedImage(pdfDoc, new Image(ImageDataFactory.Create(IMAGE3)));
            doc.Add(img);

            Image srcImage = new Image(ImageDataFactory.Create(IMAGE4));
            srcImage.ScaleToFit(400, 700);
            img = GetWatermarkedImage(pdfDoc, srcImage);
            doc.Add(img);

            doc.Close();
        }

        private static Image GetWatermarkedImage(PdfDocument pdfDocument, Image img)
        {
            float width = img.GetImageScaledWidth();
            float height = img.GetImageScaledHeight();
            PdfFormXObject template = new PdfFormXObject(new Rectangle(width, height));

            // Use a highlevel Canvas to add the image because the scaling properties were set to the
            // highlevel Image object.
            new Canvas(template, pdfDocument)
                .Add(img)
                .Close();

            new PdfCanvas(template, pdfDocument)
                .SaveState()
                .SetStrokeColor(ColorConstants.GREEN)
                .SetLineWidth(3)
                .MoveTo(width * 0.25f, height * 0.25f)
                .LineTo(width * 0.75f, height * 0.75f)
                .MoveTo(width * 0.25f, height * 0.75f)
                .LineTo(width * 0.25f, height * 0.25f)
                .Stroke()
                .SetStrokeColor(ColorConstants.WHITE)
                .Ellipse(0, 0, width, height)
                .Stroke()
                .RestoreState();

            return new Image(template);
        }
    }
}