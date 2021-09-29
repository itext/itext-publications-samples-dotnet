using System;
using System.IO;
using iText.Commons.Utils;
using iText.Kernel.Pdf;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Arabic
{
    public class ArabicBookmark
    {
        public const String DEST = "results/sandbox/typography/ArabicBookmark.pdf";

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

            new ArabicBookmark().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            // Get the complete outline tree of the whole document
            PdfOutline root = pdfDocument.GetOutlines(true);

            // المرجعية
            String bookmarkParent = "\u0627\u0644\u0645\u0631\u062C\u0639\u064A\u0629";

            // دةً انّ
            String bookmarkChild = "\u062F\u0629\u064B\u0020\u0627\u0646\u0651";

            // Add a page to the document
            pdfDocument.AddNewPage();

            // Add some PdfOutline children to the root outline
            PdfOutline bookmarkTree = root.AddOutline(bookmarkParent);
            bookmarkTree
                    .AddOutline(bookmarkChild)
                    .AddOutline(bookmarkChild)
                    .AddOutline(bookmarkChild);

            pdfDocument.Close();
        }
    }
}