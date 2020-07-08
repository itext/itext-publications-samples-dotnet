using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Objects
{
    public class CenterText
    {
        public static readonly String DEST = "results/sandbox/objects/center_text.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CenterText().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDoc);
            Rectangle pageSize = pdfDoc.GetDefaultPageSize();
            float width = pageSize.GetWidth() - document.GetLeftMargin() - document.GetRightMargin();

            SolidLine line = new SolidLine();
            AddParagraphWithTabs(document, line, width);

            // Draw a custom line to fill both sides, as it is described in iText5 example
            MyLine customLine = new MyLine();
            AddParagraphWithTabs(document, customLine, width);

            document.Close();
        }

        private static void AddParagraphWithTabs(Document document, ILineDrawer line, float width)
        {
            List<TabStop> tabStops = new List<TabStop>();

            // Create a TabStop at the middle of the page
            tabStops.Add(new TabStop(width / 2, TabAlignment.CENTER, line));

            // Create a TabStop at the end of the page
            tabStops.Add(new TabStop(width, TabAlignment.LEFT, line));

            Paragraph p = new Paragraph().AddTabStops(tabStops);
            p
                .Add(new Tab())
                .Add("Text in the middle")
                .Add(new Tab());
            document.Add(p);
        }

        private class MyLine : ILineDrawer
        {
            private float lineWidth = 1;
            private float offset = 2.02f;
            private Color color = ColorConstants.BLACK;

            public void Draw(PdfCanvas canvas, Rectangle drawArea)
            {
                float coordY = drawArea.GetY() + lineWidth / 2 + offset;
                canvas
                    .SaveState()
                    .SetStrokeColor(color)
                    .SetLineWidth(lineWidth)
                    .MoveTo(drawArea.GetX(), coordY)
                    .LineTo(drawArea.GetX() + drawArea.GetWidth(), coordY)
                    .Stroke()
                    .RestoreState();
            }

            public float GetLineWidth()
            {
                return lineWidth;
            }

            public void SetLineWidth(float lineWidth)
            {
                this.lineWidth = lineWidth;
            }

            public Color GetColor()
            {
                return color;
            }

            public void SetColor(Color color)
            {
                this.color = color;
            }

            public float GetOffset()
            {
                return offset;
            }

            public void SetOffset(float offset)
            {
                this.offset = offset;
            }
        }
    }
}