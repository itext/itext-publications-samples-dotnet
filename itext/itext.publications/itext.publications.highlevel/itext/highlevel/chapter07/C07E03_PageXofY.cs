using System;
using System.IO;

using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Event;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Hyphenation;
using iText.Layout.Properties;

namespace iText.Highlevel.Chapter07 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C07E03_PageXofY {
        public const String SRC = "../../../resources/txt/jekyll_hyde.txt";

        public const String DEST = "../../../results/chapter07/jekyll_hydeV2.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C07E03_PageXofY().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, new C07E03_PageXofY.Header("The Strange Case of Dr. Jekyll and Mr. Hyde"));
            C07E03_PageXofY.PageXofY @event = new C07E03_PageXofY.PageXofY();
            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, @event);
            // Initialize document
            Document document = new Document(pdf);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            document.SetTextAlignment(TextAlignment.JUSTIFIED).SetHyphenation(new HyphenationConfig("en", "uk", 3, 3));
            StreamReader sr = File.OpenText(SRC);
            String line;
            Div div = new Div();
            while ((line = sr.ReadLine()) != null) {
                document.Add(new Paragraph(line).SetFont(bold).SetFontSize(12).SetMarginBottom(0).SetKeepWithNext(true));
                div = new Div().SetFont(font).SetFontSize(11).SetMarginBottom(18);
                while ((line = sr.ReadLine()) != null) {
                    div.Add(new Paragraph(line).SetMarginBottom(0).SetFirstLineIndent(36).SetMultipliedLeading(1.2f));
                    if (String.IsNullOrEmpty(line)) {
                        document.Add(div);
                        break;
                    }
                }
            }
            document.Add(div);
            @event.WriteTotal(pdf);
            //Close document
            document.Close();
        }

        protected internal class Header : AbstractPdfDocumentEventHandler {
            internal String header;

            public Header(String header) {
                this.header = header;
            }

            protected override void OnAcceptedEvent(AbstractPdfDocumentEvent @event) {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdf = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                if (pdf.GetPageNumber(page) == 1) {
                    return;
                }
                Rectangle pageSize = page.GetPageSize();
                PdfCanvas pdfCanvas = new PdfCanvas(page.GetLastContentStream(), page.GetResources(), pdf);
                iText.Layout.Canvas canvas = new iText.Layout.Canvas(pdfCanvas, pageSize);
                canvas.ShowTextAligned(this.header, pageSize.GetWidth() / 2, pageSize.GetTop() - 30, TextAlignment.CENTER);
            }
        }

        protected internal class PageXofY : AbstractPdfDocumentEventHandler {
            protected internal PdfFormXObject placeholder;

            protected internal float side = 20;

            protected internal float x = 300;

            protected internal float y = 25;

            protected internal float space = 4.5f;

            protected internal float descent = 3;

            public PageXofY() {
                this.placeholder = new PdfFormXObject(new Rectangle(0, 0, this.side, this.side));
            }

            protected override void OnAcceptedEvent(AbstractPdfDocumentEvent @event) {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdf = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                int pageNumber = pdf.GetPageNumber(page);
                Rectangle pageSize = page.GetPageSize();
                PdfCanvas pdfCanvas = new PdfCanvas(page.GetLastContentStream(), page.GetResources(), pdf);
                iText.Layout.Canvas canvas = new iText.Layout.Canvas(pdfCanvas, pageSize);
                Paragraph p = new Paragraph().Add("Page ").Add(pageNumber.ToString()).Add(" of");
                canvas.ShowTextAligned(p, this.x, this.y, TextAlignment.RIGHT);
                pdfCanvas.AddXObjectAt(this.placeholder, this.x + this.space, this.y - this.descent);
                pdfCanvas.Release();
            }

            public virtual void WriteTotal(PdfDocument pdf) {
                iText.Layout.Canvas canvas = new iText.Layout.Canvas(this.placeholder, pdf);
                canvas.ShowTextAligned(pdf.GetNumberOfPages().ToString(), 0, this.descent, TextAlignment.LEFT);
            }
        }
    }
}
