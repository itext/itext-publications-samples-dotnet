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
    public class PageRotation
    {
        public static readonly String DEST = "results/sandbox/events/page_rotation.pdf";

        public static readonly PdfNumber PORTRAIT = new PdfNumber(0);
        public static readonly PdfNumber LANDSCAPE = new PdfNumber(90);
        public static readonly PdfNumber INVERTEDPORTRAIT = new PdfNumber(180);
        public static readonly PdfNumber SEASCAPE = new PdfNumber(270);

        private static readonly Paragraph HELLO_WORLD = new Paragraph("Hello World!");

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PageRotation().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            // The default page rotation is set to portrait in the custom event handler.
            PageRotationEventHandler eventHandler = new PageRotationEventHandler();
            pdfDoc.AddEventHandler(PdfDocumentEvent.START_PAGE, eventHandler);
            Document doc = new Document(pdfDoc);

            doc.Add(HELLO_WORLD);

            eventHandler.SetRotation(LANDSCAPE);
            doc.Add(new AreaBreak());
            doc.Add(HELLO_WORLD);

            eventHandler.SetRotation(INVERTEDPORTRAIT);
            doc.Add(new AreaBreak());
            doc.Add(HELLO_WORLD);

            eventHandler.SetRotation(SEASCAPE);
            doc.Add(new AreaBreak());
            doc.Add(HELLO_WORLD);

            eventHandler.SetRotation(PORTRAIT);
            doc.Add(new AreaBreak());
            doc.Add(HELLO_WORLD);

            doc.Close();
        }

        private class PageRotationEventHandler : IEventHandler
        {
            private PdfNumber rotation = PORTRAIT;

            public void SetRotation(PdfNumber orientation)
            {
                this.rotation = orientation;
            }

            public void HandleEvent(Event currentEvent)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) currentEvent;
                docEvent.GetPage().Put(PdfName.Rotate, rotation);
            }
        }
    }
}