using System;
using System.IO;
using iText.Commons.Utils;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Element;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Thai
{
    public class ThaiLink
    {
        public const String DEST = "results/sandbox/typography/ThaiLink.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            using (Stream license = FileUtil.GetInputStreamForFile(
                Environment.GetEnvironmentVariable("ITEXT_LICENSE_FILE_LOCAL_STORAGE") + "/itextkey-typography.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ThaiLink().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            //ผม เรา ฉัน
            String text = "\u0E1C\u0E21\u0020\u0E40\u0E23\u0E32\u0020\u0E09\u0E31\u0E19";

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansThai-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Create an action with an URI. Use the action together with text to create a Link element
            Link link = new Link(text, PdfAction.CreateURI("http://itextpdf.com"));

            // Overwrite some default document properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document
                    .SetFont(font)
                    .SetFontSize(10);

            document
                    .Add(new Paragraph(link))
                    .Add(new Paragraph(text));

            document.Close();
        }
    }
}