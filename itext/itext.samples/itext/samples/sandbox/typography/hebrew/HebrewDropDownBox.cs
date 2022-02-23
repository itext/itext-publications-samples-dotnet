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

namespace iText.Samples.Sandbox.Typography.Hebrew
{
    public class HebrewDropDownBox
    {
        public const String DEST = "results/sandbox/typography/HebrewDropDownBox.pdf";
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

            new HebrewDropDownBox().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(document.GetPdfDocument(), true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSerifHebrew-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // רע ומר היה להם ליהודים
            String line1 = "\u05E8\u05E2\u0020\u05D5\u05DE\u05E8\u0020\u05D4\u05D9\u05D4\u0020\u05DC\u05D4\u05DD" +
                           "\u0020\u05DC\u05D9\u05D4\u05D5\u05D3\u05D9\u05DD";

            // כל
            String line2 = "\u05DB\u05DC";

            // אדם
            String line3 = "\u05D0\u05D3\u05DD";

            // Initialize the array with 3 lines of text. These lines will be used as combo box options
            String[] comboText = new String[] {line1, line2, line3};

            // Create a form field and apply the properties on it
            PdfFormField formField = new ChoiceFormFieldBuilder(document.GetPdfDocument(), "test")
                .SetWidgetRectangle(new Rectangle(50, 750, 125, 15)).SetOptions(comboText).CreateComboBox();
            formField.SetValue(line1);
            formField
                    .SetBorderWidth(1)
                    .SetJustification(2)
                    .SetFont(font)
                    .SetFontSizeAutoScale();

            form.AddField(formField);

            document.Close();
        }
    }
}