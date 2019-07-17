/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Tutorial.Chapter01 {
    /// <summary>Simple image example.</summary>
    public class C01E03_QuickBrownFox {
        public const String DOG = "../../resources/img/dog.bmp";

        public const String FOX = "../../resources/img/fox.bmp";

        public const String DEST = "../../results/chapter01/quick_brown_fox.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C01E03_QuickBrownFox().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF writer
            PdfWriter writer = new PdfWriter(dest);
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(writer);
            // Initialize document
            Document document = new Document(pdf);
            // Compose Paragraph
            iText.Layout.Element.Image fox = new Image(ImageDataFactory.Create(FOX));
            iText.Layout.Element.Image dog = new iText.Layout.Element.Image(ImageDataFactory.Create(DOG));
            Paragraph p = new Paragraph("The quick brown ").Add(fox).Add(" jumps over the lazy ").Add(dog);
            // Add Paragraph to document
            document.Add(p);
            //Close document
            document.Close();
        }
    }
}
