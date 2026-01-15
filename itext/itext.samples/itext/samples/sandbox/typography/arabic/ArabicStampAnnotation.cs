using System;
using System.IO;
using iText.Commons.Utils;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Licensing.Base;
using iText.Samples.Util;

namespace iText.Samples.Sandbox.Typography.Arabic
{
    public class ArabicStampAnnotation
    {
        public const String DEST = "results/sandbox/typography/ArabicStampAnnotation.pdf";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            String licensePath = LicenseUtil.GetPathToLicenseFileWithITextCoreAndPdfCalligraphProducts();
            using (Stream license = FileUtil.GetInputStreamForFile(licensePath))
            {
                LicenseKey.LoadLicenseFile(license);
            }

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ArabicStampAnnotation().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            // في القيام بنشاط
            String line1 = "\u0641\u064A\u0020\u0627\u0644\u0642\u064A\u0627\u0645\u0020\u0628\u0646\u0634\u0627\u0637";

            // Create a rectangle for an annotation
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