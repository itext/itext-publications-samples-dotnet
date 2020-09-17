using System;
using System.IO;
using iText.Kernel.Events;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Events
{
    public class ScaleDown
    {
        public static readonly String DEST = "results/sandbox/events/scale_down.pdf";

        public static readonly String SRC = "../../../resources/pdfs/orientations.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ScaleDown().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument srcDoc = new PdfDocument(new PdfReader(SRC));
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            float scale = 0.5f;
            ScaleDownEventHandler eventHandler = new ScaleDownEventHandler(scale);
            pdfDoc.AddEventHandler(PdfDocumentEvent.START_PAGE, eventHandler);

            int numberOfPages = srcDoc.GetNumberOfPages();
            for (int p = 1; p <= numberOfPages; p++)
            {
                eventHandler.SetPageDict(srcDoc.GetPage(p).GetPdfObject());

                // Copy and paste scaled page content as formXObject
                PdfFormXObject page = srcDoc.GetPage(p).CopyAsFormXObject(pdfDoc);
                PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
                canvas.AddXObjectWithTransformationMatrix(page, scale, 0f, 0f, scale, 0f, 0f);
            }

            pdfDoc.Close();
            srcDoc.Close();
        }

        private class ScaleDownEventHandler : IEventHandler
        {
            protected float scale = 1;
            protected PdfDictionary pageDict;

            public ScaleDownEventHandler(float scale)
            {
                this.scale = scale;
            }

            public void SetPageDict(PdfDictionary pageDict)
            {
                this.pageDict = pageDict;
            }

            public void HandleEvent(Event currentEvent)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) currentEvent;
                PdfPage page = docEvent.GetPage();

                page.Put(PdfName.Rotate, pageDict.GetAsNumber(PdfName.Rotate));

                // The MediaBox value defines the full size of the page.
                ScaleDown(page, pageDict, PdfName.MediaBox, scale);

                // The CropBox value defines the visible size of the page.
                ScaleDown(page, pageDict, PdfName.CropBox, scale);
            }

            protected void ScaleDown(PdfPage destPage, PdfDictionary pageDictSrc, PdfName box, float scale)
            {
                PdfArray original = pageDictSrc.GetAsArray(box);
                if (original != null)
                {
                    float width = original.GetAsNumber(2).FloatValue() - original.GetAsNumber(0).FloatValue();
                    float height = original.GetAsNumber(3).FloatValue() - original.GetAsNumber(1).FloatValue();

                    PdfArray result = new PdfArray();
                    result.Add(new PdfNumber(0));
                    result.Add(new PdfNumber(0));
                    result.Add(new PdfNumber(width * scale));
                    result.Add(new PdfNumber(height * scale));
                    destPage.Put(box, result);
                }
            }
        }
    }
}