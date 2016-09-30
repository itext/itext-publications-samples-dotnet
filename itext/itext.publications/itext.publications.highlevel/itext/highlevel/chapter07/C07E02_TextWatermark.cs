/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;

using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Hyphenation;
using iText.Layout.Properties;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter07 {
    /// <author>Bruno Lowagie (iText Software)</author>
    [WrapToTest]
    public class C07E02_TextWatermark {
        public const String SRC = "../../resources/txt/jekyll_hyde.txt";

        public const String DEST = "../../results/chapter07/jekyll_hydeV1.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C07E02_TextWatermark().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new C07E02_TextWatermark.TextWatermark(this));
            // Initialize document
            Document document = new Document(pdf);
            PdfFont font = PdfFontFactory.CreateFont(FontConstants.TIMES_ROMAN);
            PdfFont bold = PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD);
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
            //Close document
            document.Close();
        }

        protected internal class TextWatermark : IEventHandler {
            internal Color lime;

            internal Color blue;

            internal PdfFont helvetica;

            /// <exception cref="System.IO.IOException"/>
            protected internal TextWatermark(C07E02_TextWatermark _enclosing) {
                this._enclosing = _enclosing;
                this.helvetica = PdfFontFactory.CreateFont(FontConstants.HELVETICA);
                this.lime = new DeviceCmyk(0.208f, 0, 0.584f, 0);
                this.blue = new DeviceCmyk(0.445f, 0.0546f, 0, 0.0667f);
            }

            public virtual void HandleEvent(Event @event) {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdf = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                int pageNumber = pdf.GetPageNumber(page);
                Rectangle pageSize = page.GetPageSize();
                PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdf);
                pdfCanvas.SaveState().SetFillColor(pageNumber % 2 == 1 ? this.lime : this.blue).Rectangle(pageSize.GetLeft
                    (), pageSize.GetBottom(), pageSize.GetWidth(), pageSize.GetHeight()).Fill().RestoreState();
                if (pageNumber > 1) {
                    pdfCanvas.BeginText().SetFontAndSize(this.helvetica, 10).MoveText(pageSize.GetWidth() / 2 - 120, pageSize.
                        GetTop() - 20).ShowText("The Strange Case of Dr. Jekyll and Mr. Hyde").MoveText(120, -pageSize.GetTop(
                        ) + 40).ShowText(pageNumber.ToString()).EndText();
                }
                pdfCanvas.Release();
            }

            private readonly C07E02_TextWatermark _enclosing;
        }
    }
}
