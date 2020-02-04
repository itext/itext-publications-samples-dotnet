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
using System.Linq;
using System.Runtime.InteropServices;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using Image = iText.Layout.Element.Image;

namespace iText.Samples.Sandbox.Images
{
    public class ReplaceImage
    {
        public static readonly String DEST = "results/sandbox/images/replace_image.pdf";

        public static readonly String SRC = "../../../resources/pdfs/image.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ReplaceImage().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));

            // Assume that there is a single XObject in the source document
            // and this single object is an image.
            PdfDictionary pageDict = pdfDoc.GetFirstPage().GetPdfObject();
            PdfDictionary resources = pageDict.GetAsDictionary(PdfName.Resources);
            PdfDictionary xObjects = resources.GetAsDictionary(PdfName.XObject);
            PdfName imgRef = xObjects.KeySet().First();
            PdfStream stream = xObjects.GetAsStream(imgRef);
            Image img = ConvertToBlackAndWhitePng(new PdfImageXObject(stream));

            // Replace the original image with the grayscale image
            xObjects.Put(imgRef, img.GetXObject().GetPdfObject());

            pdfDoc.Close();
        }

        private static Image ConvertToBlackAndWhitePng(PdfImageXObject image)
        {
            
            /* We want to convert image to grayscale. In PDF this corresponds
             * to DeviceGray color space. Images in DeviceGray colorspace shall have
             * only 8 bits per pixel (bpp).
             * In C# 8bpp images are not well supported, therefore we would need to perform
             * some tricks on a low level in order to convert RGB 24bpp image to 8 bit image.
             *
             * We will manually set image pixel 8 bit values according to original image
             * RGB pixel values. We know from PDF specification, that DeviceGray color space
             * treats each pixel as the value from 0 to 255 and we know that taking an average
             * of RGB values is a very basic but working approach to get corresponding grayscale
             * value.
             * Note that due to C# restrictions we create image with indexed colorspace
             * (Format8bppIndexed). For now we don't care what are the actual colors in color
             * palette, because we already define pixle values as if they were in grayscale
             * color space. We will explicitly overide color space directly in PDF object later.
             */
            MemoryStream memoryStream = new MemoryStream(image.GetImageBytes());
            Bitmap original = new Bitmap(memoryStream);
            memoryStream.Close();
            Bitmap result = new Bitmap(original.Width, original.Height, PixelFormat.Format8bppIndexed);

            BitmapData data = result.LockBits(new System.Drawing.Rectangle(0, 0, result.Width, result.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            byte[] bytes = new byte[data.Height * data.Stride];
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

            // Convert all pixels to grayscale
            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    var c = original.GetPixel(x, y);
                    var rgb = (byte) ((c.R + c.G + c.B) / 3);
                    bytes[y * data.Stride + x] = rgb;
                }
            }

            Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);
            result.UnlockBits(data);

            using (var stream = new MemoryStream())
            {
                result.Save(stream, ImageFormat.Png);

                /* As discussed above, we want this image to have /DeviceGray colorspace and we have already
                 * ensured that image pixel values are defined correctly for this color space on low level
                 * (by taking average values of red, green and blue components). So, we explicitly override
                 * color space directly in the created image PDF object.
                 */
                ImageData imageData = ImageDataFactory.Create(stream.ToArray());
                PdfImageXObject imageXObject = new PdfImageXObject(imageData);
                PdfStream imageXObjectStream = imageXObject.GetPdfObject();
                imageXObjectStream.Put(PdfName.ColorSpace, PdfName.DeviceGray);

                // Remove a redundant submask
                imageXObjectStream.Remove(PdfName.Mask);

                /* In C# an alpha channel (transparency) is automatically added, however we know that original
                 * image didn't have transparency, that's why we just explicitly throw away any transparency
                 * defined for the PDF object that represents an image.
                 */
                return new Image(imageXObject);
            }
        }
    }
}