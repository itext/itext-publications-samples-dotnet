using System;
using System.IO;
using iText.Commons.Utils;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Malayalam
{
    public class MalayalamList
    {
        public const String DEST = "results/sandbox/typography/MalayalamList.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            using (Stream license = FileUtil.GetInputStreamForFile(
                Environment.GetEnvironmentVariable("ITEXT_LICENSE_FILE_LOCAL_STORAGE") + "/itextkey-typography.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MalayalamList().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansMalayalam-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document
                    .SetFont(font)
                    .SetFontSize(10);

            // സാേങ്കതിക പദസൂചികഒരു സ്വതന്ത്ര
            String text = "\u0D38\u0D3E\u0D47\u0D19\u0D4D\u0D15\u0D24\u0D3F\u0D15\u0020\u0D2A\u0D26\u0D38\u0D42\u0D1A"
                          + "\u0D3F\u0D15\u0D12\u0D30\u0D41\u0020\u0D38\u0D4D\u0D35\u0D24\u0D28\u0D4D\u0D24\u0D4D\u0D30";

            List list = new List();
            list
                    .Add(text)
                    .Add(text)
                    .Add(text);

            document.Add(list);

            document.Close();
        }
    }
}