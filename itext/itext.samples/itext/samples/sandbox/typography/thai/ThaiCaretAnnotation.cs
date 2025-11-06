using System;
using System.IO;
using iText.Commons.Utils;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Licensing.Base;

namespace iText.Samples.Sandbox.Typography.Thai
{
    public class ThaiCaretAnnotation
    {
        public const String DEST = "results/sandbox/typography/ThaiCaretAnnotation.pdf";

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

            new ThaiCaretAnnotation().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            // ความยุติธรรม
            String line1 = "\u0E04\u0E27\u0E32\u0E21\u0E22\u0E38\u0E15\u0E34\u0E18\u0E23\u0E23\u0E21";

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