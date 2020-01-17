/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Events;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Events
{
    public class PageOrientations
    {
        public static readonly String DEST = "results/sandbox/events/page_orientations.pdf";

        public static readonly PdfNumber PORTRAIT = new PdfNumber(0);
        public static readonly PdfNumber LANDSCAPE = new PdfNumber(90);
        public static readonly PdfNumber INVERTEDPORTRAIT = new PdfNumber(180);
        public static readonly PdfNumber SEASCAPE = new PdfNumber(270);

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PageOrientations().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            // The default page orientation is set to portrait in the custom event handler.
            PageOrientationsEventHandler eventHandler = new PageOrientationsEventHandler();
            pdfDoc.AddEventHandler(PdfDocumentEvent.START_PAGE, eventHandler);
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("A simple page in portrait orientation"));

            eventHandler.SetOrientation(LANDSCAPE);
            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("A simple page in landscape orientation"));

            eventHandler.SetOrientation(INVERTEDPORTRAIT);
            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("A simple page in inverted portrait orientation"));

            eventHandler.SetOrientation(SEASCAPE);
            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("A simple page in seascape orientation"));

            doc.Close();
        }

        private class PageOrientationsEventHandler : IEventHandler
        {
            private PdfNumber orientation = PORTRAIT;

            public void SetOrientation(PdfNumber orientation)
            {
                this.orientation = orientation;
            }

            public void HandleEvent(Event currentEvent)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) currentEvent;
                docEvent.GetPage().Put(PdfName.Rotate, orientation);
            }
        }
    }
}