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

namespace iText.Samples.Sandbox.Typography.Malayalam
{
    public class MalayalamPushButton
    {
        public const String DEST = "results/sandbox/typography/MalayalamPushButton.pdf";
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

            new MalayalamPushButton().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, true);

            // Embedded parameter indicates whether the font is to be embedded into the target document.
            // We set it to make sure that the resultant document looks the same within different environments.
            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansMalayalam-Regular.ttf",
                    PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

            // എന്നെ തള്ളൂ
            String text = "\u0D0E\u0D28\u0D4D\u0D28\u0D46\u0020\u0D24\u0D33\u0D4D\u0D33\u0D42";

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);
            Rectangle rectangle = new Rectangle(50, 650, 100, 25);

            // Create a button for the form field, set its font and size
            PdfButtonFormField pushButton = PdfFormField.CreatePushButton(pdfDocument, rectangle, "Name", text);
            pushButton
                    .SetFont(font)
                    .SetFontSize(10);

            // Add the button to the form
            form.AddField(pushButton);

            pdfDocument.Close();
        }
    }
}