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

namespace iText.Samples.Sandbox.Typography.Gurmukhi
{
    public class GurmukhiPushButton
    {
        public const String DEST = "results/sandbox/typography/GurmukhiPushButton.pdf";
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

            new GurmukhiPushButton().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            PdfAcroForm form = PdfFormCreator.GetAcroForm(pdfDocument, true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansGurmukhi-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // ਮੈਨੂੰ ਧੱਕੋ
            String line1 = "\u0A2E\u0A48\u0A28\u0A42\u0A70\u0020\u0A27\u0A71\u0A15\u0A4B";

            Rectangle rectangle = new Rectangle(50, 650, 100, 25);

            // Create a button for the form field, set its font and size
            PdfButtonFormField pushButton = new PushButtonFormFieldBuilder(pdfDocument, "Name")
                .SetWidgetRectangle(rectangle).SetCaption(line1).CreatePushButton();
            pushButton
                    .SetFont(font)
                    .SetFontSize(10);

            // Add the button to the form
            form.AddField(pushButton);

            pdfDocument.Close();
        }
    }
}