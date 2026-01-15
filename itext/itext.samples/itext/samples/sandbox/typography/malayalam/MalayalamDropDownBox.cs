using iText.Samples.Util;
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

namespace iText.Samples.Sandbox.Typography.Malayalam
{
    public class MalayalamDropDownBox
    {
        public const String DEST = "results/sandbox/typography/MalayalamDropDownBox.pdf";
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

            new MalayalamDropDownBox().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            PdfAcroForm form = PdfFormCreator.GetAcroForm(document.GetPdfDocument(), true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansMalayalam-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // സാേങ്കതിക
            String line1 = "\u0D38\u0D3E\u0D47\u0D19\u0D4D\u0D15\u0D24\u0D3F\u0D15";

            // പദസൂചികഒരു
            String line2 = "\u0D2A\u0D26\u0D38\u0D42\u0D1A\u0D3F\u0D15\u0D12\u0D30\u0D41";

            // സ്വതന്ത്ര
            String line3 = "\u0D38\u0D4D\u0D35\u0D24\u0D28\u0D4D\u0D24\u0D4D\u0D30";

            // Initialize the array with 3 lines of text. These lines will be used as combo box options
            String[] comboText = new String[] {line1, line2, line3};

            // Create a form field and apply the properties on it
            PdfFormField formField = new ChoiceFormFieldBuilder(document.GetPdfDocument(), "test")
                .SetWidgetRectangle(new Rectangle(50, 750, 150, 15)).SetOptions(comboText).CreateComboBox();
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