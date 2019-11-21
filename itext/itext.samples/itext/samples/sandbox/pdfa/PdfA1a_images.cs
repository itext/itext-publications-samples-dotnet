/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Pdfa;

namespace iText.Samples.Sandbox.Pdfa
{
    public class PdfA1a_images
    {
        public const float MARGIN_OF_ONE_CM = 28.8f;

        public static readonly string DEST = "results/sandbox/pdfa/pdf_a1a_images.pdf";

        public static readonly String FONT = "../../resources/font/OpenSans-Regular.ttf";

        public static readonly String LOGO = "../../resources/img/hero.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PdfA1a_images().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);

            Stream fileStream =
                new FileStream("../../resources/data/sRGB_CS_profile.icm", FileMode.Open, FileAccess.Read);
            PdfADocument pdfDoc = new PdfADocument(new PdfWriter(dest), PdfAConformanceLevel.PDF_A_1A,
                new PdfOutputIntent("Custom", "",
                    null, "sRGB IEC61966-2.1", fileStream));
            pdfDoc.GetCatalog().SetLang(new PdfString("nl-nl"));
            
            pdfDoc.SetTagged();

            Document doc = new Document(pdfDoc);
            doc.SetMargins(MARGIN_OF_ONE_CM, MARGIN_OF_ONE_CM, MARGIN_OF_ONE_CM, MARGIN_OF_ONE_CM);

            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info
                .SetTitle("title")
                .SetAuthor("Author")
                .SetSubject("Subject")
                .SetCreator("Creator")
                .SetKeywords("Metadata, iText, PDF")
                .SetCreator("My program using iText")
                .AddCreationDate();

            Paragraph element = new Paragraph("Hello World").SetFont(font).SetFontSize(10);
            doc.Add(element);

            Image logoImage = new Image(ImageDataFactory.Create(LOGO));
            logoImage.GetAccessibilityProperties().SetAlternateDescription("Logo");
            doc.Add(logoImage);

            doc.Close();
        }
    }
}