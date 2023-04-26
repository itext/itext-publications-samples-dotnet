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

namespace iText.Samples.Sandbox.Typography.Tamil
{
    public class TamilPushButton
    {
        public const String DEST = "results/sandbox/typography/TamilPushButton.pdf";
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

            new TamilPushButton().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, true);

            // Embedded parameter indicates whether the font is to be embedded into the target document.
            // We set it to make sure that the resultant document looks the same within different environments
            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansTamil-Regular.ttf",
                    PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

            // என்னை தள்ளி விடு
            String text =
                    "\u0B8E\u0BA9\u0BCD\u0BA9\u0BC8\u0020\u0BA4\u0BB3\u0BCD\u0BB3\u0BBF\u0020\u0BB5\u0BBF\u0B9F\u0BC1";

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);
            Rectangle rect = new Rectangle(50, 650, 140, 30);

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