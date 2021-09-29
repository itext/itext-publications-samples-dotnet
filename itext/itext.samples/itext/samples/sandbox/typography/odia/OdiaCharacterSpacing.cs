using System;
using System.IO;
using iText.Commons.Utils;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Odia
{
    public class OdiaCharacterSpacing
    {
        public const String DEST = "results/sandbox/typography/OdiaCharacterSpacing.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            using (Stream license = FileUtil.GetInputStreamForFile(
                Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/itextkey-typography.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new OdiaCharacterSpacing().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // ସବୁ ମନୁଷ୍ଯ ଜନ୍ମକାଳରୁ
            String text =
                    "\u0B38\u0B2C\u0B41\u0020\u0B2E\u0B28\u0B41\u0B37\u0B4D\u0B2F\u0020\u0B1C\u0B28\u0B4D\u0B2E\u0B15"
                    + "\u0B3E\u0B33\u0B30\u0B41";

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansOriya-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document.SetFont(font);
            document.SetFontSize(10);

            // Create paragraph, add string to it with the default character spacing and add all to the document
            document.Add(new Paragraph(text));

            // Add text with a character spacing of 5 points
            document.Add(new Paragraph(text).SetCharacterSpacing(5));

            // Set character spacing on the document. It will be applied to the next paragraph
            document.SetCharacterSpacing(10);
            document.Add(new Paragraph(text));

            document.Close();
        }
    }
}