using System;
using System.IO;
using iText.Commons.Utils;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Devanagari
{
    public class DevanagariCircleAnnotation
    {
        public const String DEST = "results/sandbox/typography/DevanagariCircleAnnotation.pdf";

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

            new DevanagariCircleAnnotation().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            // मरीया।
            String text = "\u092E\u0930\u0940\u092F\u093E\u0964";

            // Create a rectangle for an annotation
            Rectangle rectangleAnnot = new Rectangle(55, 750, 35, 35);

            // Create the annotation, set its contents and color
            PdfAnnotation annotation = new PdfCircleAnnotation(rectangleAnnot);
            annotation
                    .SetContents(text)
                    .SetColor(ColorConstants.MAGENTA);

            // Add an empty page to the document, then add the annotation to the page
            PdfPage page = pdfDocument.AddNewPage();
            page.AddAnnotation(annotation);

            pdfDocument.Close();
        }
    }
}