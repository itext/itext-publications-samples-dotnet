using iText.Samples.Util;
using System;
using System.IO;
using iText.Commons.Utils;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Tamil
{
    public class TamilCaretAnnotation
    {
        public const String DEST = "results/sandbox/typography/TamilCaretAnnotation.pdf";

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

            new TamilCaretAnnotation().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            // உரிமைகள்
            String line1 = "\u0B89\u0BB0\u0BBF\u0BAE\u0BC8\u0B95\u0BB3\u0BCD";

            // Create a rectangle for an annotation
            Rectangle rectangleAnnot = new Rectangle(55, 750, 35, 35);

            // Create the annotation, set its contents and color
            PdfAnnotation annotation = new PdfCaretAnnotation(rectangleAnnot);
            annotation
                    .SetContents(line1)
                    .SetColor(ColorConstants.MAGENTA);

            // Add an empty page to the document, then add the annotation to the page
            PdfPage page = pdfDocument.AddNewPage();
            page.AddAnnotation(annotation);

            pdfDocument.Close();
        }
    }
}