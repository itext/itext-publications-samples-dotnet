using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.License;

namespace iText.Samples.Sandbox.Typography.Khmer
{
    public class KhmerChoiceFormField
    {
        public const String DEST = "results/sandbox/typography/KhmerChoiceFormField.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new KhmerChoiceFormField().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, true);

            // Embedded parameter indicates whether the font is to be embedded into the target document.
            // We set it to make sure that the resultant document looks the same within different environments
            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "KhmerOS.ttf",
                    PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // ភាសាខ្មែរ
            String line1 = "\u1797\u17B6\u179F\u17B6\u1781\u17D2\u1798\u17C2\u179A";

            // ឆ្នាំ១៩៤៨
            String line2 = "\u1786\u17D2\u1793\u17B6\u17C6\u17E1\u17E9\u17E4\u17E8";

            // បុព្វកថា
            String line3 = "\u1794\u17BB\u1796\u17D2\u179C\u1780\u1790\u17B6";

            // Create an array with text lines
            String[] options = new String[] {line1, line2, line3};

            Rectangle rect = new Rectangle(50, 650, 100, 80);

            // Create choice form field with parameters and set values
            PdfChoiceFormField choice = PdfFormField.CreateList(pdfDocument, rect, "List", "Test", options);
            choice
                    .SetMultiSelect(true)
                    .SetFont(font)
                    .SetFontSize(10);

            form.AddField(choice);

            pdfDocument.Close();
        }
    }
}