/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Drawing;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using iText.Kernel.Pdf.Xobject;
using Image = iText.Layout.Element.Image;

namespace iText.Samples.Sandbox.Images
{
    public class MakeJpgMask
    {
        public static readonly String DEST = "results/sandbox/images/make_jpg_mask.pdf";

        public static readonly String IMAGE = "../../../resources/img/javaone2013.jpg";
        public static readonly String MASK = "../../../resources/img/berlin2013.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MakeJpgMask().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            PageSize pageSize = PageSize.A4.Rotate();
            Document doc = new Document(pdfDoc, pageSize);

            ImageData image = ImageDataFactory.Create(IMAGE);
            ImageData mask = ConvertToBlackAndWhitePng(MASK);
            mask.MakeMask();
            image.SetImageMask(mask);

            /* For soft image masks grayscale 8 bit images are usually used. As described in
             * the ConvertToBlackAndWhitePng, for image masks actual image colorspace doesn't
             * matter, only image pixel values are important. Grayscale images are just
             * convenient for this purpose.
             * Here we adjust SMask image dictionary, in order to accommodate to the hacks
             * that we've performed in ConvertToBlackAndWhitePng method. There we've created
             * an image in indexed color space and by default C# will add transparency to the
             * image. Both these properties are forbidden for images that serve as masks.
             * That's why we explicitly override colorspace to /DeviceGray (which corresponds
             * to grayscale) and we just erase own transparency (/Mask) from the dictionary
             * of mask-image.
             */
            PdfImageXObject imageXObject = new PdfImageXObject(image);
            PdfStream imageXObjectStream = imageXObject.GetPdfObject().GetAsStream(PdfName.SMask);
            imageXObjectStream.Put(PdfName.ColorSpace, PdfName.DeviceGray);

            // Remove a redundant submask
            imageXObjectStream.Remove(PdfName.Mask);

            Image img = new Image(imageXObject);
            img.ScaleAbsolute(pageSize.GetWidth(), pageSize.GetHeight());
            img.SetFixedPosition(0, 0);
            doc.Add(img);

            doc.Close();
        }

        private static ImageData ConvertToBlackAndWhitePng(String image)
        {
            FileStream fileStream = new FileStream(image, FileMode.Open);

            /* Image masks shall have either 8 bit or 1 bit color depth.
             * In this example we create a soft mask, for which 8 bit image is used.
             * For images used as masks, image color space is not relevant, only the
             * values of image pixels are important because they define transparency
             * level.
             * In C#, however, 8 bits per pixel (bpp) images are not well supported,
             * therefore we need to perform some tricks to convert RGB 24bpp image
             * to 8 bit image. We will manually set image pixel 8 bit values according
             * to original image RGB pixel values.
             * Note that even though we create image with indexed colorspace
             * (Format8bppIndexed), we don't care what are the actual colors in color
             * palette, because as mentioned, we don't care about color space for masks.
             */
            Bitmap original = new Bitmap(fileStream);
            fileStream.Close();
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
                return ImageDataFactory.Create(stream.ToArray());
            }
        }
    }
}