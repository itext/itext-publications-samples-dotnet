using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.License;

namespace iText.Samples.Sandbox.Typography.Tamil
{
    public class TamilTextFormField
    {
        public const String DEST = "results/sandbox/typography/TamilTextFormField.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TamilTextFormField().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // தமிழ்
            String fieldValue = "\u0BA4\u0BAE\u0BBF\u0BB4\u0BCD";

            String fieldName = "Field name";

            PdfAcroForm form = PdfAcroForm.GetAcroForm(document.GetPdfDocument(), true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansTamil-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // Create a form field and set some of the properties
            PdfFormField formField = PdfTextFormField.CreateText(document.GetPdfDocument(), 
                    new Rectangle(50, 750, 100, 25));
            formField
                    .SetValue(fieldValue)
                    .SetBorderWidth(2)
                    .SetFont(font)
                    .SetFontSize(10)
                    .SetJustification(1)
                    .SetFieldName(fieldName);

            form.AddField(formField);

            document.Close();
        }
    }
}