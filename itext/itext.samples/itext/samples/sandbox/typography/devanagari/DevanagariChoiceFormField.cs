using System;
using System.IO;
using iText.Commons.Utils;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Devanagari
{
    public class DevanagariChoiceFormField
    {
        public const String DEST = "results/sandbox/typography/DevanagariChoiceFormField.pdf";
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

            new DevanagariChoiceFormField().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, true);

            // Embedded parameter indicates whether the font is to be embedded into the target document.
            // We set it to make sure that the resultant document looks the same within different environments
            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansDevanagari-Regular.ttf",
                    PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // पकवान
            String line1 = "\u092A\u0915\u0935\u093E\u0928";

            // मरीया।
            String line2 = "\u092E\u0930\u0940\u092F\u093E\u0964";

            // जलदेव
            String line3 = "\u091C\u0932\u0926\u0947\u0935";

            // Create an array with text lines
            String[] options = new String[] {line1, line2, line3};

            Rectangle rect = new Rectangle(50, 650, 100, 70);

            // Create choice form field with parameters and set values
            PdfChoiceFormField choice = new ChoiceFormFieldBuilder(pdfDocument, "List")
                .SetWidgetRectangle(rect).SetOptions(options).CreateList();
            choice.SetValue("Test");
            choice
                    .SetMultiSelect(true)
                    .SetFont(font)
                    .SetFontSize(10);

            form.AddField(choice);

            pdfDocument.Close();
        }
    }
}