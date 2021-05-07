using System;
using System.IO;
using iText.Commons.Utils;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Devanagari
{
    public class DevanagariOpacity
    {
        public const String DEST = "results/sandbox/typography/DevanagariOpacity.pdf";
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

            new DevanagariOpacity().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // मैथिली का प्रथम प्रमाण रामायण में मिलता
            Text text = new Text("\u092E\u0948\u0925\u093F\u0932\u0940\u0020\u0915\u093E\u0020\u092A\u094D\u0930\u0925"
                                 + "\u092E\u0020\u092A\u094D\u0930\u092E\u093E\u0923\u0020\u0930\u093E\u092E\u093E\u092F\u0923\u0020"
                                 + "\u092E\u0947\u0902\u0020\u092E\u093F\u0932\u0924\u093E");
            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansDevanagari-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document
                    .SetFont(font)
                    .SetFontSize(10);

            // Wrap the text with paragraphs of different opacity and add them to the document: at first with 0.1, then with
            // 0.5 and then with the default opacity
            document.Add(new Paragraph(text).SetOpacity(0.1f));
            document.Add(new Paragraph(text).SetOpacity(0.5f));
            document.Add(new Paragraph(text));

            document.Close();
        }
    }
}