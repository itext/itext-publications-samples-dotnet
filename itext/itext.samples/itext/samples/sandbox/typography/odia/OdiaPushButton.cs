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

namespace iText.Samples.Sandbox.Typography.Odia
{
    public class OdiaPushButton
    {
        public const String DEST = "results/sandbox/typography/OdiaPushButton.pdf";
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

            new OdiaPushButton().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            PdfAcroForm form = PdfFormCreator.GetAcroForm(pdfDocument, true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansOriya-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // ପୁଷ ମେ
            String text = "\u0B2A\u0B41\u0B37\u0020\u0B2E\u0B47";

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);
            Rectangle rect = new Rectangle(50, 650, 100, 25);

            // Create a button for the form field, set its font and size
            PdfButtonFormField pushButton = new PushButtonFormFieldBuilder(pdfDocument, "Name")
                .SetWidgetRectangle(rect).SetCaption(text).CreatePushButton();
            pushButton
                    .SetFont(font)
                    .SetFontSize(10);

            // Add the button to the form
            form.AddField(pushButton);

            pdfDocument.Close();
        }
    }
}