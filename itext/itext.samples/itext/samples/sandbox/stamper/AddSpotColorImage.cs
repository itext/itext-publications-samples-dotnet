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
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Colorspace;
using iText.Kernel.Pdf.Function;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Stamper 
{
    public class AddSpotColorImage 
    {
        public static readonly String DEST = "results/sandbox/stamper/add_spot_color_image.pdf";
        public static readonly String SRC = "../../../resources/pdfs/image.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new AddSpotColorImage().ManipulatePdf(DEST);
        }

        public PdfSpecialCs GetSeparationColorspace(PdfWriter writer, DeviceCmyk cmyk) 
        {
            PdfDictionary pdfDictionary = new PdfDictionary();
            pdfDictionary.Put(PdfName.FunctionType, new PdfNumber(2));
            pdfDictionary.Put(PdfName.Domain, new PdfArray(new float[] { 0, 1 }));
            pdfDictionary.Put(PdfName.C0, new PdfArray(new float[] { 0, 0, 0, 0 }));
            pdfDictionary.Put(PdfName.C1, new PdfArray(cmyk.GetColorValue()));
            pdfDictionary.Put(PdfName.N, new PdfNumber(1));
            
            PdfFunction pdfFunction = new PdfFunction.Type2(pdfDictionary);
            
            return new PdfSpecialCs.Separation("mySpotColor", cmyk.GetColorSpace(), pdfFunction);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            // Suppose that this is our image data
            byte[] circleData = new byte[] 
                    { (byte)0x3c, (byte)0x7e, (byte)0xff, (byte)0xff, (byte)0xff, (byte)0xff, (byte)0x7e, (byte)0x3c };
            
            PdfSpecialCs colorspace = GetSeparationColorspace(pdfDoc.GetWriter(), 
                    new DeviceCmyk(0.8f, 0.3f, 0.3f, 0.1f));
            
            // Specifying a single component colorspace in the image
            ImageData image = ImageDataFactory.Create(8, 8, 1, 1, circleData, 
                    new int[] { 0, 0 });
            PdfImageXObject imageXObject = new PdfImageXObject(image);
            imageXObject.Put(PdfName.ColorSpace, colorspace.GetPdfObject());
            imageXObject.MakeIndirect(pdfDoc);
            
            // Now we add the image to the existing PDF document
            PdfPage pdfPage = pdfDoc.GetFirstPage();
            pdfPage.SetIgnorePageRotationForContent(true);
            PdfCanvas canvas = new PdfCanvas(pdfPage);
            canvas.AddXObject(imageXObject, 100, 200, 100);
            
            pdfDoc.Close();
        }
    }
}
