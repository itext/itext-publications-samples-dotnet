using System;
using System.IO;
using iText.Commons.Utils;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Arabic
{
    public class ArabicCharacterSpacing
    {
        public const String DEST = "results/sandbox/typography/ArabicCharacterSpacing.pdf";
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

            new ArabicCharacterSpacing().CreatePDF(DEST);
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

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document.SetFont(font);
            document.SetFontSize(10);

            // Create paragraph, add string to it with the default character spacing and add all to the document
            document.Add(CreateParagraph(text));

            // Add text with a character spacing of 5 points
            document.Add(CreateParagraph(text).SetCharacterSpacing(5));

            // Set character spacing on the document. It will be applied to the next paragraph
            document.SetCharacterSpacing(10);
            document.Add(CreateParagraph(text));

            document.Close();
        }

        // This method creates a paragraph with right text alignment
        private static Paragraph CreateParagraph(String text)
        {
            Paragraph paragraph = new Paragraph(text);

            // In Arabic text goes from right to left, that's why we need to overwrite the default iText's alignment
            paragraph.SetTextAlignment(TextAlignment.RIGHT);
            return paragraph;
        }
    }
}