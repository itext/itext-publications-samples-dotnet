using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class RowBackground
    {
        public static readonly string DEST = "results/sandbox/tables/row_background.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RowBackground().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            Table table = new Table(UnitValue.CreatePercentArray(7)).UseAllAvailableWidth();
            table.SetNextRenderer(new RowBackgroundTableRenderer(table, new Table.RowRange(0, 9), 2));

            for (int i = 0; i < 10; i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    table.AddCell(new Cell().Add(new Paragraph(j.ToString())).SetBorder(Border.NO_BORDER));
                }
            }

            doc.Add(table);

            doc.Close();
        }

        private class RowBackgroundTableRenderer : TableRenderer
        {
            private int row;

            public RowBackgroundTableRenderer(Table modelElement, Table.RowRange rowRange, int row)
                : base(modelElement, rowRange)
            {
                // the row number of the row that needs a background
                this.row = row;
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new RowBackgroundTableRenderer((Table) modelElement, rowRange, row);
            }

            public override void Draw(DrawContext drawContext)
            {
                PdfCanvas canvas;
                float llx = rows[row][0].GetOccupiedAreaBBox().GetLeft();
                float lly = rows[row][0].GetOccupiedAreaBBox().GetBottom();
                float urx = rows[row][rows[row].Length - 1].GetOccupiedAreaBBox().GetRight();
                float ury = rows[row][0].GetOccupiedAreaBBox().GetTop();
                float h = ury - lly;

                canvas = drawContext.GetCanvas();
                canvas.SaveState();
                canvas.Arc(llx - h / 2, lly, llx + h / 2, ury, 90, 180);
                canvas.LineTo(urx, lly);
                canvas.Arc(urx - h / 2, lly, urx + h / 2, ury, 270, 180);
                canvas.LineTo(llx, ury);
                canvas.SetFillColor(ColorConstants.LIGHT_GRAY);
                canvas.Fill();
                canvas.RestoreState();

                base.Draw(drawContext);
            }
        }
    }
}