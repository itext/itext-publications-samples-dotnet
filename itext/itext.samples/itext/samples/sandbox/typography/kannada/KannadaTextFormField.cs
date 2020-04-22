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

namespace iText.Samples.Sandbox.Typography.Kannada
{
    public class KannadaTextFormField
    {
        public const String DEST = "results/sandbox/typography/KannadaTextFormField.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new KannadaTextFormField().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // ಗರುಡನಂದದಿ
            String filedValue = "\u0C97\u0CB0\u0CC1\u0CA1\u0CA8\u0C82\u0CA6\u0CA6\u0CBF";

            String fieldName = "Field name";

            PdfAcroForm form = PdfAcroForm.GetAcroForm(document.GetPdfDocument(), true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansKannada-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // Create a form field and set some of the properties
            PdfFormField formField = PdfTextFormField.CreateText(document.GetPdfDocument(), new Rectangle(50, 750, 80, 25));
            formField
                    .SetValue(filedValue)
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