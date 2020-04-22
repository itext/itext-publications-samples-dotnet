using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.License;

namespace iText.Samples.Sandbox.Typography.Arabic
{
    public class ArabicTable
    {
        public const String DEST = "results/sandbox/typography/ArabicTable.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ArabicTable().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // حجزه
            String text = "\u062D\u062C\u0632\u0647";

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoNaskhArabic-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document
                    .SetFont(font)
                    .SetFontSize(10)

                    // In Arabic text goes from right to left, that's why we need to overwrite the default iText's alignment 
                    .SetTextAlignment(TextAlignment.RIGHT);

            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            table
                    .AddCell(text)
                    .AddCell(text)
                    .AddCell(text);

            document.Add(table);

            document.Close();
        }
    }
}