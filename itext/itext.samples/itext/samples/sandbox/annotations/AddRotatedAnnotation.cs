using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Annot;

namespace iText.Samples.Sandbox.Annotations
{
    public class AddRotatedAnnotation
    {
        public static readonly String DEST = "results/sandbox/annotations/add_rotated_annotation.pdf";

        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddRotatedAnnotation().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfPage firstPage = pdfDoc.GetFirstPage();
            PdfAction linkAction = PdfAction.CreateURI("https://pages.itextpdf.com/ebook-stackoverflow-questions.html");

            Rectangle annotLocation = new Rectangle(30, 770, 90, 30);
            PdfAnnotation link = new PdfLinkAnnotation(annotLocation)

                // Set highlighting type which is enabled after a click on the annotation
                .SetHighlightMode(PdfAnnotation.HIGHLIGHT_INVERT)
                .SetAction(linkAction)
                .SetColor(ColorConstants.RED.GetColorValue());
            firstPage.AddAnnotation(link);

            annotLocation = new Rectangle(30, 670, 30, 90);
            link = new PdfLinkAnnotation(annotLocation)
                .SetHighlightMode(PdfAnnotation.HIGHLIGHT_INVERT)
                .SetAction(linkAction)
                .SetColor(ColorConstants.GREEN.GetColorValue());
            firstPage.AddAnnotation(link);

            annotLocation = new Rectangle(150, 770, 90, 30);
            PdfAnnotation stamp = new PdfStampAnnotation(annotLocation)
                .SetStampName(new PdfName("Confidential"))

                // This method sets the text that will be displayed for the annotation or the alternate description,
                // if this type of annotation does not display text.
                .SetContents("Landscape");
            firstPage.AddAnnotation(stamp);

            annotLocation = new Rectangle(150, 670, 90, 90);
            stamp = new PdfStampAnnotation(annotLocation)
                .SetStampName(new PdfName("Confidential"))
                .SetContents("Portrait")
                .Put(PdfName.Rotate, new PdfNumber(90));
            firstPage.AddAnnotation(stamp);

            annotLocation = new Rectangle(250, 670, 90, 90);
            stamp = new PdfStampAnnotation(annotLocation)
                .SetStampName(new PdfName("Confidential"))
                .SetContents("Portrait")
                .Put(PdfName.Rotate, new PdfNumber(45));
            firstPage.AddAnnotation(stamp);

            pdfDoc.Close();
        }
    }
}