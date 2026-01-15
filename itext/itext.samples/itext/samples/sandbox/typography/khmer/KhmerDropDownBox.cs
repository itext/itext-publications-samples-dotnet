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

namespace iText.Samples.Sandbox.Typography.Khmer
{
    public class KhmerDropDownBox
    {
        public const String DEST = "results/sandbox/typography/KhmerDropDownBox.pdf";
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

            new KhmerDropDownBox().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            PdfAcroForm form = PdfFormCreator.GetAcroForm(document.GetPdfDocument(), true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansKhmer-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // ភាសាខ្មែរ
            String line1 = "\u1797\u17B6\u179F\u17B6\u1781\u17D2\u1798\u17C2\u179A";

            // ឆ្នាំ១៩៤៨
            String line2 = "\u1786\u17D2\u1793\u17B6\u17C6\u17E1\u17E9\u17E4\u17E8";

            // បុព្វកថា
            String line3 = "\u1794\u17BB\u1796\u17D2\u179C\u1780\u1790\u17B6";

            // Initialize the array with 3 lines of text. These lines will be used as combo box options
            String[] comboText = new String[] {line1, line2, line3};

            // Create a form field and apply the properties on it
            PdfFormField formField = new ChoiceFormFieldBuilder(document.GetPdfDocument(), "test")
                .SetWidgetRectangle(new Rectangle(50, 750,50, 15)).SetOptions(comboText).CreateComboBox();
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