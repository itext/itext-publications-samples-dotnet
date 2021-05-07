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

namespace iText.Samples.Sandbox.Typography.Hebrew
{
    public class HebrewPushButton
    {
        public const String DEST = "results/sandbox/typography/HebrewPushButton.pdf";
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

            new HebrewPushButton().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSerifHebrew-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // דחוף אותי
            String text = "\u05D3\u05D7\u05D5\u05E3\u0020\u05D0\u05D5\u05EA\u05D9";

            Rectangle rect = new Rectangle(50, 650, 200, 25);

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