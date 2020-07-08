using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.License;

namespace iText.Samples.Sandbox.Typography.Hebrew
{
    public class HebrewBoldText
    {
        public const String DEST = "results/sandbox/typography/HebrewBoldText.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new HebrewBoldText().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSerifHebrew-Regular.ttf",
                    PdfEncodings.IDENTITY_H);
            PdfFont fontBold = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSerifHebrew-Bold.ttf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document.SetFont(font);
            document.SetFontSize(10);

            // רע ומר היה להם ליהודים
            String hebrewText = "\u05E8\u05E2\u0020\u05D5\u05DE\u05E8\u0020\u05D4\u05D9\u05D4\u0020\u05DC\u05D4\u05DD"
                                + "\u0020\u05DC\u05D9\u05D4\u05D5\u05D3\u05D9\u05DD";

            // Add paragraphs with text to the document:
            // Text without thickness
            document.Add(CreateParagraph(hebrewText));

            // Add a paragraph with a set bold font to the paragraph
            document.Add(CreateParagraph(hebrewText).SetFont(fontBold));

            // We don't suggest usage of setBold() method to reach text thickness since the result is written with the usual
            // rather than the bold font: we only emulate "thickness". It's recommended to use an actual bold font instead.
            // For example NotoSerifHebrew-Bold
            document.Add(CreateParagraph(hebrewText).SetBold());

            document.Close();
        }

        // This method creates a paragraph with right text alignment
        private static Paragraph CreateParagraph(String text)
        {
            Paragraph paragraph = new Paragraph(text);

            // In Hebrew text goes from right to left, that's why we need to overwrite the default iText's alignment
            paragraph.SetTextAlignment(TextAlignment.RIGHT);
            return paragraph;
        }
    }
}