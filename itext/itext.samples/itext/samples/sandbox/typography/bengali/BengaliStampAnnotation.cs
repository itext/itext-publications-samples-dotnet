using System;
using System.IO;
using iText.Commons.Utils;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Bengali
{
    public class BengaliStampAnnotation
    {
        public const String DEST = "results/sandbox/typography/BengaliStampAnnotation.pdf";

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

            new BengaliStampAnnotation().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            // সুরে মিললে সুর হবে
            String line1 =
                    "\u09B8\u09C1\u09B0\u09C7\u0020\u09AE\u09BF\u09B2\u09B2\u09C7\u0020\u09B8\u09C1\u09B0\u0020\u09B9"
                    + "\u09AC\u09C7";

            Rectangle rectangleAnnot = new Rectangle(55, 650, 35, 35);

            // Create a stamp annotation and set some properties
            PdfAnnotation annotation = new PdfStampAnnotation(rectangleAnnot);
            annotation
                    .SetContents(line1)
                    .SetColor(ColorConstants.CYAN);

            // Add an empty page to the document, then add the annotation to the page
            PdfPage page = pdfDocument.AddNewPage();
            page.AddAnnotation(annotation);

            pdfDocument.Close();
        }
    }
}