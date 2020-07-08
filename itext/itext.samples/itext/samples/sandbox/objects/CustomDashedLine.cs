using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class CustomDashedLine
    {
        public static readonly string DEST = "results/sandbox/objects/custom_dashed_line.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new CustomDashedLine().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("Before dashed line"));

            CustomDashedLineSeparator dashedLine = new CustomDashedLineSeparator();
            dashedLine.SetDash(10f);
            dashedLine.SetPhase(2.5f);
            dashedLine.SetGap(7f);
            dashedLine.SetLineWidth(3f);
            doc.Add(new LineSeparator(dashedLine));
            doc.Add(new Paragraph("After dashed line"));

            doc.Close();
        }

        protected class CustomDashedLineSeparator : DottedLine
        {
            private float dash;
            private float phase;

            public void SetDash(float dash)
            {
                this.dash = dash;
            }

            public void SetPhase(float phase)
            {
                this.phase = phase;
            }

            public override void Draw(PdfCanvas canvas, Rectangle drawArea)
            {
                canvas.SaveState()
                    .SetLineWidth(GetLineWidth())
                    .SetLineDash(dash, gap, phase)
                    .MoveTo(drawArea.GetX(), drawArea.GetY())
                    .LineTo(drawArea.GetX() + drawArea.GetWidth(), drawArea.GetY())
                    .Stroke()
                    .RestoreState();
            }
        }
    }
}