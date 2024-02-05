using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Element;
using iText.Pdfua.Checkers;

namespace iText.Samples.Sandbox.Pdfua 
{
    public class PdfUAGraphicsUsage {
        public const String DEST = "results/sandbox/pdfua/pdf_ua_graphics.pdf";

        public static readonly String DOG = "../../../resources/img/dog.bmp";

        public const String FONT = "../../../resources/font/FreeSans.ttf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfUAGraphicsUsage().ManipulatePdf(DEST);
        }

        public virtual void ManipulatePdf(String dest) {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest, new WriterProperties().AddUAXmpMetadata().SetPdfVersion
                (PdfVersion.PDF_1_7)));
            pdfDoc.SetTagged();
            pdfDoc.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
            pdfDoc.GetCatalog().SetLang(new PdfString("en-US"));
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.SetTitle("English pangram");
            //validation
            ValidationContainer validationContainer = new ValidationContainer();
            validationContainer.AddChecker(new PdfUA1Checker(pdfDoc));
            pdfDoc.GetDiContainer().Register(typeof(ValidationContainer), validationContainer);
            Document document = new Document(pdfDoc);
            iText.Layout.Element.Image img = new Image(ImageDataFactory.Create(DOG));
            img.GetAccessibilityProperties().SetAlternateDescription("Alternative description");
            document.Add(img);
            document.Close();
        }
    }
}
