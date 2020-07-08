using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.License;

namespace iText.Samples.Sandbox.Typography.Bengali
{
    public class BengaliBoldText
    {
        public const String DEST = "results/sandbox/typography/BengaliBoldText.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new BengaliBoldText().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansBengali-Regular.ttf",
                    PdfEncodings.IDENTITY_H);
            PdfFont fontBold = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansBengali-Bold.ttf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document.SetFont(font);
            document.SetFontSize(10);

            // আমার কানে লাগে
            Text bengaliText =
                    new Text("\u0986\u09AE\u09BE\u09B0\u0020\u0995\u09BE\u09A8\u09C7\u0020\u09B2\u09BE\u0997\u09C7");

            // Add paragraphs with text to the document:
            // Text without thickness
            document.Add(new Paragraph(bengaliText));

            // Add a paragraph with a set bold font to the paragraph
            document.Add(new Paragraph(bengaliText).SetFont(fontBold));

            // We don't suggest usage of setBold() method to reach text thickness since the result is written with the usual
            // rather than the bold font: we only emulate "thickness". It's recommended to use an actual bold font instead.
            // For example NotoSansBengali-Bold
            document.Add(new Paragraph(bengaliText).SetBold());

            document.Close();
        }
    }
}