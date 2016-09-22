/*
* To change this license header, choose License Headers in Project Properties.
* To change this template file, choose Tools | Templates
* and open the template in the editor.
*/
using System;
using System.IO;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout.Element;
using iText.Test.Attributes;

namespace itext.publications.highlevel.itext.highlevel.chapter07 {
    /// <author>iText</author>
    [WrapToTest]
    public class C07E05_AddRemovePages {
        public const String SRC = "../../resources/pdfs/jekyll_hyde_bookmarked.pdf";

        public const String DEST = "results/chapter07/jekyll_hyde_updated.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C07E05_AddRemovePages().ManipulatePdf(SRC, DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void ManipulatePdf(String src, String dest) {
            PdfReader reader = new PdfReader(src);
            PdfWriter writer = new PdfWriter(dest);
            PdfDocument pdf = new PdfDocument(reader, writer);
            pdf.AddEventHandler(PdfDocumentEvent.INSERT_PAGE, new C07E05_AddRemovePages.AddPageHandler(this));
            pdf.AddEventHandler(PdfDocumentEvent.REMOVE_PAGE, new C07E05_AddRemovePages.RemovePageHandler(this));
            pdf.AddNewPage(1, PageSize.A4);
            int total = pdf.GetNumberOfPages();
            for (int i = 9; i <= total; i++) {
                pdf.RemovePage(9);
                if (i == 12) {
                    pdf.RemoveAllHandlers();
                }
            }
            pdf.Close();
        }

        protected internal class AddPageHandler : IEventHandler {
            public virtual void HandleEvent(Event @event) {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdf = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas pdfCanvas = new PdfCanvas(page);
                iText.Layout.Canvas canvas = new iText.Layout.Canvas(pdfCanvas, pdf, page.GetPageSize());
                canvas.Add(new Paragraph().Add(docEvent.GetEventType()));
            }

            internal AddPageHandler(C07E05_AddRemovePages _enclosing) {
                this._enclosing = _enclosing;
            }

            private readonly C07E05_AddRemovePages _enclosing;
        }

        protected internal class RemovePageHandler : IEventHandler {
            public virtual void HandleEvent(Event @event) {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                System.Console.Out.WriteLine(docEvent.GetEventType());
            }

            internal RemovePageHandler(C07E05_AddRemovePages _enclosing) {
                this._enclosing = _enclosing;
            }

            private readonly C07E05_AddRemovePages _enclosing;
        }
    }
}
