/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Hyphenation;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Highlevel.Chapter07 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C07E04_ImageWatermark {
        public const String SRC = "../../resources/txt/jekyll_hyde.txt";

        public const String IMG = "../../resources/img/3132614.jpg";

        public const String DEST = "../../results/chapter07/jekyll_hydeV3.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C07E04_ImageWatermark().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            iText.Layout.Element.Image img = new Image(ImageDataFactory.Create(IMG));
            IEventHandler handler = new TransparentImage( img);
            pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, handler);
            // Initialize document
            Document document = new Document(pdf);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            document.SetTextAlignment(TextAlignment.JUSTIFIED).SetHyphenation(new HyphenationConfig("en", "uk", 3, 3));
            StreamReader sr = File.OpenText(SRC);
            String name;
            String line;
            Paragraph p;
            bool title = true;
            int counter = 0;
            IList<Util.Pair<String, Util.Pair<String, int>>> toc = new List<Util.Pair
                <String, Util.Pair<String, int>>>();
            while ((line = sr.ReadLine()) != null) {
                p = new Paragraph(line);
                p.SetKeepTogether(true);
                if (title) {
                    name = String.Format("title{0:00}", counter++);
                    Util.Pair<String, int> titlePage = new Util.Pair<string,int>(line, pdf.GetNumberOfPages());
                    p.SetFont(bold).
                        SetFontSize(12)
                        .SetKeepWithNext(true)
                        .SetDestination(name)
                        .SetNextRenderer(new UpdatePageRenderer(p, titlePage));
                    title = false;
                    document.Add(p);
                    toc.Add(new Util.Pair<string,Util.Pair<string,int>>(name, titlePage));
                }
                else {
                    p.SetFirstLineIndent(36);
                    if (String.IsNullOrEmpty(line)) {
                        p.SetMarginBottom(12);
                        title = true;
                    }
                    else {
                        p.SetMarginBottom(0);
                    }
                    document.Add(p);
                }
            }
            pdf.RemoveEventHandler(PdfDocumentEvent.START_PAGE, handler);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            p = new Paragraph().SetFont(bold).Add("Table of Contents").SetDestination("toc");
            document.Add(p);
            toc.RemoveAt(0);
            IList<TabStop> tabstops = new List<TabStop>();
            tabstops.Add(new TabStop(580, TabAlignment.RIGHT, new DottedLine()));
            foreach (Util.Pair<String, Util.Pair<String, int>> entry in toc) {
                Util.Pair<String, int> text = entry.Value;
                p = new Paragraph().AddTabStops(tabstops).Add(text.Key).Add(new Tab()).Add(text.Value.ToString()).SetAction
                    (PdfAction.CreateGoTo(entry.Key));
                document.Add(p);
            }
            //Close document
            document.Close();
        }

        protected internal class UpdatePageRenderer : ParagraphRenderer {
            protected internal Util.Pair<String, int> entry;

            public UpdatePageRenderer(Paragraph modelElement, Util.Pair
                <String, int> entry)
                : base(modelElement) {
                this.entry = entry;
            }

            public override LayoutResult Layout(LayoutContext layoutContext) {
                LayoutResult result = base.Layout(layoutContext);
                this.entry.Value = layoutContext.GetArea().GetPageNumber();
                return result;
            }
        }

        protected internal class TransparentImage : IEventHandler {
            protected internal PdfExtGState gState;

            protected internal iText.Layout.Element.Image img;

            public TransparentImage(iText.Layout.Element.Image img) {
                this.img = img;
                this.gState = new PdfExtGState().SetFillOpacity(0.2f);
            }

            public virtual void HandleEvent(Event @event) {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdf = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                Rectangle pageSize = page.GetPageSize();
                PdfCanvas pdfCanvas = new PdfCanvas(page.GetLastContentStream(), page.GetResources(), pdf);
                pdfCanvas.SaveState().SetExtGState(this.gState);
                iText.Layout.Canvas canvas = new iText.Layout.Canvas(pdfCanvas, pdf, page.GetPageSize());
                canvas.Add(this.img.ScaleAbsolute(pageSize.GetWidth(), pageSize.GetHeight()));
                pdfCanvas.RestoreState();
                pdfCanvas.Release();
            }
        }
    }
}
