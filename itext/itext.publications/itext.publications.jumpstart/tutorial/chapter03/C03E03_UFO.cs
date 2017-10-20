/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.IO;
using iText.IO.Font;
using iText.IO.Util;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Test.Attributes;

namespace Tutorial.Chapter03 {
    /// <summary>Simple event handler example.</summary>
    [WrapToTest]
    public class C03E03_UFO {
        public const String DATA = "../../resources/data/ufo.csv";

        public const String DEST = "../../results/chapter03/ufo.pdf";

        internal static PdfFont helvetica = null;

        internal static PdfFont helveticaBold = null;

        /// <exception cref="System.Exception"/>
        public static void Main(String[] args) {
            helvetica = PdfFontFactory.CreateFont(FontConstants.HELVETICA);
            helveticaBold = PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD);
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C03E03_UFO().CreatePdf(DEST);
        }

        /// <exception cref="System.Exception"/>
        protected internal virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new C03E03_UFO.MyEventHandler(this));
            // Initialize document
            Document document = new Document(pdf);
            Paragraph p = new Paragraph("List of reported UFO sightings in 20th century").SetTextAlignment(TextAlignment
                .CENTER).SetFont(helveticaBold).SetFontSize(14);
            document.Add(p);
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 3, 5, 7, 4 }));
            StreamReader sr = File.OpenText(DATA);
            String line = sr.ReadLine();
            Process(table, line, helveticaBold, true);
            while ((line = sr.ReadLine()) != null) {
                Process(table, line, helvetica, false);
            }
            sr.Close();
            document.Add(table);
            document.Close();
        }

        public virtual void Process(Table table, String line, PdfFont font, bool isHeader) {
            StringTokenizer tokenizer = new StringTokenizer(line, ";");
            while (tokenizer.HasMoreTokens()) {
                if (isHeader) {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(tokenizer.NextToken()).SetFont(font)).SetFontSize(9).SetBorder
                        (new SolidBorder(ColorConstants.BLACK, 0.5f)));
                }
                else {
                    table.AddCell(new Cell().Add(new Paragraph(tokenizer.NextToken()).SetFont(font)).SetFontSize(9).SetBorder(
                        new SolidBorder(ColorConstants.BLACK, 0.5f)));
                }
            }
        }

        protected internal class MyEventHandler : IEventHandler {
            public virtual void HandleEvent(Event @event) {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                int pageNumber = pdfDoc.GetPageNumber(page);
                Rectangle pageSize = page.GetPageSize();
                PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
                //Set background
                Color limeColor = new DeviceCmyk(0.208f, 0, 0.584f, 0);
                Color blueColor = new DeviceCmyk(0.445f, 0.0546f, 0, 0.0667f);
                pdfCanvas.SaveState().SetFillColor(pageNumber % 2 == 1 ? limeColor : blueColor).Rectangle(pageSize.GetLeft
                    (), pageSize.GetBottom(), pageSize.GetWidth(), pageSize.GetHeight()).Fill().RestoreState();
                //Add header and footer
                pdfCanvas.BeginText().SetFontAndSize(C03E03_UFO.helvetica, 9).MoveText(pageSize.GetWidth() / 2 - 60, pageSize
                    .GetTop() - 20).ShowText("THE TRUTH IS OUT THERE").MoveText(60, -pageSize.GetTop() + 30).ShowText(pageNumber
                    .ToString()).EndText();
                //Add watermark
                iText.Layout.Canvas canvas = new iText.Layout.Canvas(pdfCanvas, pdfDoc, page.GetPageSize());
                canvas.SetFontColor(ColorConstants.WHITE);
                canvas.SetProperty(Property.FONT_SIZE, 60);
                canvas.SetProperty(Property.FONT, C03E03_UFO.helveticaBold);
                canvas.ShowTextAligned(new Paragraph("CONFIDENTIAL"), 298, 421, pdfDoc.GetPageNumber(page), TextAlignment.
                    CENTER, VerticalAlignment.MIDDLE, 45);
                pdfCanvas.Release();
            }

            internal MyEventHandler(C03E03_UFO _enclosing) {
                this._enclosing = _enclosing;
            }

            private readonly C03E03_UFO _enclosing;
        }
    }
}
