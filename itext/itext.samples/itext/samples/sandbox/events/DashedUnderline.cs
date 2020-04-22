using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Events
{
    public class DashedUnderline
    {
        public static readonly String DEST = "results/sandbox/events/dashed_underline.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DashedUnderline().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("This text is not underlined"));

            Text text1 = new Text("This text is underlined with a solid line");
            text1.SetUnderline(1, -3);
            doc.Add(new Paragraph(text1));

            Text text2 = new Text("This text is underlined with a dashed line");
            text2.SetNextRenderer(new DashedLineTextRenderer(text2));
            doc.Add(new Paragraph(text2));

            doc.Close();
        }

        private class DashedLineTextRenderer : TextRenderer
        {
            public DashedLineTextRenderer(Text textElement) : base(textElement)
            {
            }

            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new DashedLineTextRenderer((Text) modelElement);
            }

            public override void Draw(DrawContext drawContext)
            {
                base.Draw(drawContext);
                Rectangle rect = this.GetOccupiedAreaBBox();
                PdfCanvas canvas = drawContext.GetCanvas();
                canvas
                    .SaveState()
                    .SetLineDash(3, 3)
                    .MoveTo(rect.GetLeft(), rect.GetBottom() - 3)
                    .LineTo(rect.GetRight(), rect.GetBottom() - 3)
                    .Stroke()
                    .RestoreState();
            }
        }
    }
}