using System;
using System.IO;
using iText.Commons.Utils;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Gurmukhi
{
    public class GurmukhiDropDownBox
    {
        public const String DEST = "results/sandbox/typography/GurmukhiDropDownBox.pdf";
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

            new GurmukhiDropDownBox().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(document.GetPdfDocument(), true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansGurmukhi-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // ਸਾਰੇ ਇਨਸਾਨ
            String line1 = "\u0A38\u0A3E\u0A30\u0A47\u0020\u0A07\u0A28\u0A38\u0A3E\u0A28";

            // ਜਦ
            String line2 = "\u0A1C\u0A26";

            // ਪਰਿਵਾਰ
            String line3 = "\u0A2A\u0A30\u0A3F\u0A35\u0A3E\u0A30";

            // Initialize the array with 3 lines of text. These lines will be used as combo box options
            String[] comboText = new String[] {line1, line2, line3};

            // Create a form field and apply the properties on it
            PdfFormField formField = PdfTextFormField.CreateComboBox(document.GetPdfDocument(),
                    new Rectangle(50, 750, 150, 15), "test", line1, comboText);
            formField
                    .SetBorderWidth(1)
                    .SetJustification(1)
                    .SetFont(font)
                    .SetFontSizeAutoScale();

            form.AddField(formField);

            document.Close();
        }
    }
}