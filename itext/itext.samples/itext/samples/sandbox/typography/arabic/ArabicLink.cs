using System;
using System.IO;
using iText.Commons.Utils;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Licensing.Base;
using iText.Samples.Util;

namespace iText.Samples.Sandbox.Typography.Arabic
{
    public class ArabicLink
    {
        public const String DEST = "results/sandbox/typography/ArabicLink.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            String licensePath = LicenseUtil.GetPathToLicenseFileWithITextCoreAndPdfCalligraphProducts();
            using (Stream license = FileUtil.GetInputStreamForFile(licensePath))
            {
                LicenseKey.LoadLicenseFile(license);
            }

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ArabicLink().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // في القيام بنشاط
            String text = "\u0641\u064A\u0020\u0627\u0644\u0642\u064A\u0627\u0645\u0020\u0628\u0646\u0634\u0627\u0637";

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoNaskhArabic-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Create an action with an URI. Use the action together with text to create a Link element
            Link link = new Link(text, PdfAction.CreateURI("http://itextpdf.com"));

            // Overwrite some default document properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document
                    .SetFont(font)
                    .SetFontSize(10)

                    // In Arabic text goes from right to left, that's why we need to overwrite the default iText's alignment 
                    .SetTextAlignment(TextAlignment.RIGHT);

            document
                    .Add(new Paragraph(link))
                    .Add(new Paragraph(text));

            document.Close();
        }
    }
}