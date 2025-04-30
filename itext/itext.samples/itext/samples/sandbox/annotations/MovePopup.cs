using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;

namespace iText.Samples.Sandbox.Annotations
{
   
    // MovePopup.cs
    // 
    // This class demonstrates how to reposition existing annotations in a PDF document.
    // The code opens a PDF file containing a sticky note annotation with an associated
    // popup window, then modifies the position of both by changing their rectangle 
    // coordinates. The sticky note is moved up and left, while its popup window is 
    // repositioned to maintain a logical relationship with the note. This example shows
    // how to manipulate the physical location of annotations within a document.
 
    public class MovePopup
    {
        public static readonly String DEST = "results/sandbox/annotations/move_popup.pdf";

        public static readonly String SRC = "../../../resources/pdfs/hello_sticky_note.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MovePopup().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            PdfDictionary page = pdfDoc.GetFirstPage().GetPdfObject();
            PdfArray annots = page.GetAsArray(PdfName.Annots);

            // Get sticky notes annotation and change the rectangle of that annotation
            PdfDictionary sticky = annots.GetAsDictionary(0);
            PdfArray stickyRect = sticky.GetAsArray(PdfName.Rect);

            PdfArray stickyRectangle = new PdfArray(new float[]
            {
                stickyRect.GetAsNumber(0).FloatValue() - 120, stickyRect.GetAsNumber(1).FloatValue() - 70,
                stickyRect.GetAsNumber(2).FloatValue(), stickyRect.GetAsNumber(3).FloatValue() - 30
            });
            sticky.Put(PdfName.Rect, stickyRectangle);

            // Get pop-up window annotation and change the rectangle of that annotation
            PdfDictionary popup = annots.GetAsDictionary(1);
            PdfArray popupRect = popup.GetAsArray(PdfName.Rect);

            PdfArray popupRectangle = new PdfArray(new float[]
            {
                popupRect.GetAsNumber(0).FloatValue() - 250, popupRect.GetAsNumber(1).FloatValue(),
                popupRect.GetAsNumber(2).FloatValue(), popupRect.GetAsNumber(3).FloatValue() - 250
            });
            popup.Put(PdfName.Rect, popupRectangle);

            doc.Close();
        }
    }
}