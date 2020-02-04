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
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Images
{
    public class BackgroundTransparent
    {
        public static readonly String DEST = "results/sandbox/images/background_transparent.pdf";

        public static readonly String IMAGE = "../../../resources/img/berlin2013.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new BackgroundTransparent().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            PageSize pageSize = PageSize.A4.Rotate();
            Document doc = new Document(pdfDoc, pageSize);

            ImageData image = ImageDataFactory.Create(IMAGE);
            PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
            canvas.SaveState();
            PdfExtGState state = new PdfExtGState().SetFillOpacity(0.6f);
            canvas.SetExtGState(state);
            canvas.AddImage(image, 0, 0, pageSize.GetWidth(), false);
            canvas.RestoreState();

            doc.Add(new Paragraph("Berlin!"));

            doc.Close();
        }
    }
}