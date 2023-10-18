using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Highlevel.Chapter05 {
    /// <author>Bruno Lowagie (iText Software)</author>

    public class C05E06_CellBorders4 {
        
        private class RoundedCornersTableRenderer : TableRenderer
        {
            public RoundedCornersTableRenderer(Table modelElement)
                : base(modelElement) {}
            
            public override IRenderer GetNextRenderer() {
                return new RoundedCornersTableRenderer((Table)GetModelElement());
            }

            protected override void DrawBorders(DrawContext drawContext)
            {
                Rectangle occupiedAreaBBox = this.GetOccupiedAreaBBox();
                UnitValue[] margins = this.GetMargins();
                Rectangle rectangle = this.ApplyMargins(occupiedAreaBBox, margins, false);
                PdfCanvas canvas = drawContext.GetCanvas();
                canvas.RoundRectangle(rectangle.GetX() + 1, rectangle.GetY() + 1, rectangle.GetWidth() - 2, rectangle.GetHeight
                    () - 2, 5).Stroke();
                base.DrawBorder(drawContext);
            }
        }
        
        private class RoundedCornersCellRenderer : CellRenderer {
            public RoundedCornersCellRenderer(Cell modelElement)
                : base(modelElement) {
            }

            public override void DrawBorder(DrawContext drawContext) {
                Rectangle occupiedAreaBBox = this.GetOccupiedAreaBBox();
                UnitValue[] margins = this.GetMargins();
                Rectangle rectangle = this.ApplyMargins(occupiedAreaBBox, margins, false);
                PdfCanvas canvas = drawContext.GetCanvas();
                canvas.RoundRectangle(rectangle.GetX() + 1, rectangle.GetY() + 1,
                    rectangle.GetWidth() - 2, rectangle.GetHeight() - 2, 5).Stroke();
                base.DrawBorder(drawContext);
            }

            public override IRenderer GetNextRenderer() {
                return new RoundedCornersCellRenderer((Cell)GetModelElement());
            }

            protected override Rectangle ApplyMargins(Rectangle rect, UnitValue[] margins, bool reverse) {
                return rect.ApplyMargins(margins[0].GetValue(), margins[1].GetValue(), margins[2].GetValue(),
                    margins[3].GetValue(), reverse);
            }
        }
        
        private class RoundedCornersCell : Cell {
            public RoundedCornersCell()
                : base() {
                SetMargin(2);
            }

            public RoundedCornersCell(int rowspan, int colspan)
                : base(rowspan, colspan) {
                SetMargin(2);
            }

            protected override IRenderer MakeNewRenderer() {
                return new C05E06_CellBorders4.RoundedCornersCellRenderer(this);
            }
        }
        
        public const String DEST = "../../../results/chapter05/cell_borders4.pdf";
        
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E06_CellBorders4().CreatePdf(DEST);

        }
        
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            
            // Initialize document
            Document document = new Document(pdf);
                
            Table table = new Table(UnitValue.CreatePercentArray(new float[]{2, 1, 1}))
                .SetWidth(UnitValue.CreatePercentValue(80))
                .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                .SetTextAlignment(TextAlignment.CENTER);
            Cell cell = new RoundedCornersCell(1, 3)
                .Add(new Paragraph("Cell with colspan 3"));
            table.AddCell(cell);
            cell = new RoundedCornersCell(2, 1)
                .Add(new Paragraph("Cell with rowspan 2"));
            table.AddCell(cell);
            cell = new RoundedCornersCell()
                .Add(new Paragraph("row 1; cell 1"));
            table.AddCell(cell);
            cell = new RoundedCornersCell()
                .Add(new Paragraph("row 1; cell 2"));
            table.AddCell(cell);
            cell = new RoundedCornersCell()
                .Add(new Paragraph("row 2; cell 1"));
            table.AddCell(cell);
            cell = new RoundedCornersCell()
                .Add(new Paragraph("row 2; cell 2"));
            table.AddCell(cell);
            table.SetNextRenderer(new RoundedCornersTableRenderer(table));
            document.Add(table);

            document.Close();
        }
    }
}