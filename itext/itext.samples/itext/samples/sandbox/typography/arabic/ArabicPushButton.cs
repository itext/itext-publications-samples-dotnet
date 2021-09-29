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

namespace iText.Samples.Sandbox.Typography.Arabic
{
    public class ArabicPushButton
    {
        public const String DEST = "results/sandbox/typography/ArabicPushButton.pdf";
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

            new ArabicPushButton().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoNaskhArabic-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // ادفعني
            String text = "\u0627\u062F\u0641\u0639\u0646\u064A";

            Rectangle rect = new Rectangle(50, 650, 100, 25);

            // Create a button for the form field, set its font and size
            PdfButtonFormField pushButton = PdfFormField.CreatePushButton(pdfDocument, rect, "Name", text);
            pushButton
                    .SetFont(font)
                    .SetFontSize(10);

            // Add the button to the form
            form.AddField(pushButton);

            pdfDocument.Close();
        }
    }
}