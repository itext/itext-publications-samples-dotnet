/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Images
{
    public class ReduceSize
    {
        public static readonly String DEST = "results/sandbox/images/reduce_size.pdf";

        public static readonly String SRC = "../../../resources/pdfs/single_image.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ReduceSize().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfWriter writer = new PdfWriter(dest, new WriterProperties().SetFullCompressionMode(true));
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), writer);
            float factor = 0.5f;

            foreach (PdfIndirectReference indRef in pdfDoc.ListIndirectReferences())
            {
                // Get a direct object and try to resolve indirects chain.
                // Note: If chain of references has length of more than 32,
                // this method return 31st reference in chain.
                PdfObject pdfObject = indRef.GetRefersTo();
                if (pdfObject == null || !pdfObject.IsStream())
                {
                    continue;
                }

                PdfStream stream = (PdfStream) pdfObject;
                if (!PdfName.Image.Equals(stream.GetAsName(PdfName.Subtype)))
                {
                    continue;
                }

                if (!PdfName.DCTDecode.Equals(stream.GetAsName(PdfName.Filter)))
                {
                    continue;
                }

                PdfImageXObject image = new PdfImageXObject(stream);
                MemoryStream imageStream = new MemoryStream(image.GetImageBytes());
                Bitmap bitmap = new Bitmap(imageStream);
                imageStream.Close();

                int width = (int) (bitmap.Width * factor);
                int height = (int) (bitmap.Height * factor);
                if (width <= 0 || height <= 0)
                {
                    continue;
                }

                // Scale the image
                Bitmap scaledBitmap = new Bitmap(bitmap, new Size(width, height));
                MemoryStream scaledBitmapStream = new MemoryStream();
                scaledBitmap.Save(scaledBitmapStream, ImageFormat.Jpeg);
                
                ResetImageStream(stream, scaledBitmapStream.ToArray(), width, height);
                scaledBitmapStream.Close();
            }

            pdfDoc.Close();
        }

        private static void ResetImageStream(PdfStream stream, byte[] imgBytes, int imgWidth, int imgHeight)
        {
            stream.Clear();
            stream.SetData(imgBytes);
            stream.Put(PdfName.Type, PdfName.XObject);
            stream.Put(PdfName.Subtype, PdfName.Image);
            stream.Put(PdfName.Filter, PdfName.DCTDecode);
            stream.Put(PdfName.Width, new PdfNumber(imgWidth));
            stream.Put(PdfName.Height, new PdfNumber(imgHeight));
            stream.Put(PdfName.BitsPerComponent, new PdfNumber(8));
            stream.Put(PdfName.ColorSpace, PdfName.DeviceRGB);
        }
    }
}