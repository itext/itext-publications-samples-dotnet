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

namespace iText.Samples.Sandbox.Typography.Devanagari
{
    public class DevanagariTextFormField
    {
        public const String DEST = "results/sandbox/typography/DevanagariTextFormField.pdf";
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

            new DevanagariTextFormField().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // पकवान
            String fieldValue = "\u092A\u0915\u0935\u093E\u0928";

            String fieldName = "Field name";

            PdfAcroForm form = PdfFormCreator.GetAcroForm(document.GetPdfDocument(), true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansDevanagari-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // Create a form field and set some of the properties
            PdfFormField formField = new TextFormFieldBuilder(document.GetPdfDocument(), fieldName)
                .SetWidgetRectangle(new Rectangle(50, 750, 160, 25)).CreateText();
            formField
                .SetValue(fieldValue)
                .SetJustification(TextAlignment.CENTER)
                .SetFont(font)
                .SetFontSize(10);
            formField.GetFirstFormAnnotation().SetBorderWidth(2);

            form.AddField(formField);

            document.Close();
        }
    }
}