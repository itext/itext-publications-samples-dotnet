using System;
using System.IO;
using iText.Commons.Utils;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Kannada
{
    public class KannadaWordSpacing
    {
        public const String DEST = "results/sandbox/typography/KannadaWordSpacing.pdf";
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

            new KannadaWordSpacing().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // ಅಶೋಕನ ಬ್ರಾಹ್ಮೀ ಲಿಪಿ ಉತ್ತರಕ್ಕೆ
            String text = "\u0C85\u0CB6\u0CCB\u0C95\u0CA8\u0020\u0CAC\u0CCD\u0CB0\u0CBE\u0CB9\u0CCD\u0CAE\u0CC0\u0020"
                          + "\u0CB2\u0CBF\u0CAA\u0CBF\u0020\u0C89\u0CA4\u0CCD\u0CA4\u0CB0\u0C95\u0CCD\u0C95\u0CC6";

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansKannada-Regular.ttf",
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