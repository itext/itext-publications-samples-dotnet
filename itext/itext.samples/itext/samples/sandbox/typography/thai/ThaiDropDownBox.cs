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
using iText.Layout.Properties;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Thai
{
    public class ThaiDropDownBox
    {
        public const String DEST = "results/sandbox/typography/ThaiDropDownBox.pdf";
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

            new ThaiDropDownBox().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            PdfAcroForm form = PdfFormCreator.GetAcroForm(document.GetPdfDocument(), true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansThai-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // คำปรารภ
            String line1 = "\u0E04\u0E33\u0E1B\u0E23\u0E32\u0E23\u0E20";

            // สมาชิก
            String line2 = "\u0E2A\u0E21\u0E32\u0E0A\u0E34\u0E01";

            // ความยุติธรรม
            String line3 = "\u0E04\u0E27\u0E32\u0E21\u0E22\u0E38\u0E15\u0E34\u0E18\u0E23\u0E23\u0E21";

            // Initialize the array with 3 lines of text. These lines will be used as combo box options
            String[] comboText = new String[] {line1, line2, line3};

            // Create a form field and apply the properties on it
            PdfFormField formField = new ChoiceFormFieldBuilder(document.GetPdfDocument(), "test")
                .SetWidgetRectangle(new Rectangle(50, 750, 75, 15)).SetOptions(comboText).CreateComboBox();
            formField.SetValue(line1);
            formField
                    .SetJustification(TextAlignment.CENTER)
                    .SetFont(font)
                    .SetFontSizeAutoScale();
            formField.GetFirstFormAnnotation().SetBorderWidth(1);

            form.AddField(formField);

            document.Close();
        }
    }
}