/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.Collections.Generic;
using System.IO;
using iText.IO.Image;
using iText.IO.Source;
using iText.IO.Util;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Highlevel.Chapter03 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C03E19_PagedImages {
        public const String TEST1 = "../../resources/img/test/animated_fox_dog.gif";

        public const String TEST2 = "../../resources/img/test/amb.jb2";

        public const String TEST3 = "../../resources/img/test/marbles.tif";

        public const String DEST = "../../results/chapter03/paged_images.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C03E19_PagedImages().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            Image img;
            // Animated GIF
            Uri url1 = UrlUtil.ToURL(TEST1);
            IList<ImageData> list = ImageDataFactory.CreateGifFrames(url1);
            foreach (ImageData data in list) {
                img = new iText.Layout.Element.Image(data);
                document.Add(img);
            }
            // JBIG2
            Uri url2 = UrlUtil.ToURL(TEST2);
            IRandomAccessSource ras2 = new RandomAccessSourceFactory().CreateSource(url2);
            RandomAccessFileOrArray raf2 = new RandomAccessFileOrArray(ras2);
            int pages2 = Jbig2ImageData.GetNumberOfPages(raf2);
            for (int i = 1; i <= pages2; i++) {
                img = new iText.Layout.Element.Image(ImageDataFactory.CreateJbig2(url2, i));
                document.Add(img);
            }
            // TIFF
            Uri url3 = UrlUtil.ToURL(TEST3);
            IRandomAccessSource ras3 = new RandomAccessSourceFactory().CreateSource(url3);
            RandomAccessFileOrArray raf3 = new RandomAccessFileOrArray(ras3);
            int pages3 = TiffImageData.GetNumberOfPages(raf3);
            for (int i_1 = 1; i_1 <= pages3; i_1++) {
                img = new iText.Layout.Element.Image(ImageDataFactory.CreateTiff(url3, true, i_1, true));
                document.Add(img);
            }
            document.Close();
        }
    }
}
