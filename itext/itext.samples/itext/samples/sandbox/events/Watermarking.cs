using System;
using System.IO;
using iText.Commons.Utils;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Events
{
    public class Watermarking
    {
        public static readonly String DEST = "results/sandbox/events/watermarkings.pdf";

        public static readonly String DATA = "../../../resources/data/united_states.csv";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new Watermarking().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new WatermarkingEventHandler());

            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            Table table = new Table(UnitValue.CreatePercentArray(new float[] {4, 1, 3})).UseAllAvailableWidth();

            using (StreamReader br = new StreamReader(new FileStream(DATA, FileMode.Open)))
            {
                String line = br.ReadLine();
                ParseTextLine(table, line, bold, true);
                while ((line = br.ReadLine()) != null)
                {
                    ParseTextLine(table, line, font, false);
                }
            }

            doc.Add(table);

            doc.Close();
        }

        private static void ParseTextLine(Table table, String line, PdfFont font, bool isHeader)
        {
            StringTokenizer tokenizer = new StringTokenizer(line, ";");
            int c = 0;
            while (tokenizer.HasMoreTokens() && c++ < 3)
            {
                Cell cell = new Cell().Add(new Paragraph(tokenizer.NextToken()).SetFont(font));
                if (isHeader)
                {
                    table.AddHeaderCell(cell);
                }
                else
                {
                    table.AddCell(cell);
                }
            }
        }

        private class WatermarkingEventHandler : IEventHandler
        {
            public void HandleEvent(Event currentEvent)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) currentEvent;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfFont font = null;
                try
                {
                    font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                }
                catch (IOException e)
                {
                    
                    // Such an exception isn't expected to occur,
                    // because helvetica is one of standard fonts
                    Console.Error.WriteLine(e.Message);
                }

                PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
                new Canvas(canvas, page.GetPageSize())
                    .SetFontColor(ColorConstants.LIGHT_GRAY)
                    .SetFontSize(60)
                    
                    // If the exception has been thrown, the font variable is not initialized.
                    // Therefore null will be set and iText will use the default font - Helvetica
                    .SetFont(font)
                    .ShowTextAligned(new Paragraph("WATERMARK"), 298, 421, pdfDoc.GetPageNumber(page),
                        TextAlignment.CENTER, VerticalAlignment.MIDDLE, 45)
                    .Close();
            }
        }
    }
}