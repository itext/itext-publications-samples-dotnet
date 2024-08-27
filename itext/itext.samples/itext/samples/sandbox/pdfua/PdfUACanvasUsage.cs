using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;
using iText.Kernel.Utils;
using iText.Kernel.Validation;
using iText.Pdfua.Checkers;
using iText.Pdfua.Exceptions;

namespace iText.Samples.Sandbox.Pdfua 
{
    public class PdfUACanvasUsage {
        public const String DEST = "results/sandbox/pdfua/pdf_ua_canvas.pdf";

        public const String FONT = "../../../resources/font/FreeSans.ttf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfUACanvasUsage().ManipulatePdf(DEST);
        }

        public virtual void ManipulatePdf(String dest) {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest, new WriterProperties().AddUAXmpMetadata()));
            pdfDoc.SetTagged();
            pdfDoc.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
            pdfDoc.GetCatalog().SetLang(new PdfString("en-US"));
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.SetTitle("English pangram");
            //validation
            ValidationContainer validationContainer = new ValidationContainer();
            validationContainer.AddChecker(new PdfUA1Checker(pdfDoc));
            pdfDoc.GetDiContainer().Register(typeof(ValidationContainer), validationContainer);
            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED
            );
            PdfPage page1 = pdfDoc.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page1);
            canvas.BeginText().SetFontAndSize(font, 12);
            //Pdf/UA conformance exception handling
            try {
                canvas.ShowText("Hello World");
            }
            catch (PdfUAConformanceException) {
            }
            //do handling here
            TagTreePointer tagPointer = new TagTreePointer(pdfDoc).SetPageForTagging(page1).AddTag(StandardRoles.P);
            canvas.OpenTag(tagPointer.GetTagReference()).SaveState().BeginText().SetFontAndSize(font, 12).MoveText(200
                , 200).ShowText("Hello World!").EndText().RestoreState().CloseTag();
            pdfDoc.Close();
        }
    }
}
