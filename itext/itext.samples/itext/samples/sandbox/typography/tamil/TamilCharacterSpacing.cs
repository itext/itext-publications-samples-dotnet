using System;
using System.IO;
using iText.Commons.Utils;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Tamil
{
    public class TamilCharacterSpacing
    {
        public const String DEST = "results/sandbox/typography/TamilCharacterSpacing.pdf";
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

            new TamilCharacterSpacing().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // தமிழ்
            String text = "\u0BA4\u0BAE\u0BBF\u0BB4\u0BCD";

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansTamil-Regular.ttf",
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