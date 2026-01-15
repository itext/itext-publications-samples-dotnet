using iText.Samples.Util;
using System;
using System.IO;
using iText.Commons.Utils;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Gurmukhi
{
    public class GurmukhiWordSpacing
    {
        public const String DEST = "results/sandbox/typography/GurmukhiWordSpacing.pdf";
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

            new GurmukhiWordSpacing().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // ਸਾਰੇ ਇਨਸਾਨ
            String text = "\u0A38\u0A3E\u0A30\u0A47\u0020\u0A07\u0A28\u0A38\u0A3E\u0A28";

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansGurmukhi-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document
                    .SetFont(font)
                    .SetFontSize(10);

            // Create paragraph, add string to it with the default word spacing and add all to the document
            document.Add(new Paragraph(text));

            // Add text with a word spacing of 10 points
            document.Add(new Paragraph(text).SetWordSpacing(10));

            // Set word spacing on the document. It will be applied to the next paragraph
            document.SetWordSpacing(20);
            document.Add(new Paragraph(text));

            document.Close();
        }
    }
}