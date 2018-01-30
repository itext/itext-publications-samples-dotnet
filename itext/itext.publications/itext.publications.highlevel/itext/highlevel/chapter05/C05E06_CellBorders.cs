/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter05 {
    /// <author>Bruno Lowagie (iText Software)</author>
    [WrapToTest]
    public class C05E06_CellBorders {
        private class RoundedCornersCellRenderer : CellRenderer {
            public RoundedCornersCellRenderer(Cell modelElement)
                : base(modelElement) {
            }

            public override void DrawBorder(DrawContext drawContext) {
                Rectangle occupiedAreaBBox = this.GetOccupiedAreaBBox();
                UnitValue[] margins = this.GetMargins();
                Rectangle rectangle = this.ApplyMargins(occupiedAreaBBox, margins, false);
                PdfCanvas canvas = drawContext.GetCanvas();
                canvas.RoundRectangle(rectangle.GetX() + 1, rectangle.GetY() + 1, rectangle.GetWidth() - 2, rectangle.GetHeight
                    () - 2, 5).Stroke();
                base.DrawBorder(drawContext);
            }

            protected override Rectangle ApplyMargins(Rectangle rect, UnitValue[] margins, bool reverse) {
                return rect.ApplyMargins(margins[0].GetValue(), margins[1].GetValue(), margins[2].GetValue(),
                    margins[3].GetValue(), reverse);
            }
        }

        private class RoundedCornersTableRenderer : TableRenderer
        {
            public RoundedCornersTableRenderer(Table modelElement)
                : base(modelElement) {}

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

        private class RoundedCornersCell : Cell {
            public RoundedCornersCell()
                : base() {
                SetBorder(Border.NO_BORDER);
                SetMargin(2);
            }

            public RoundedCornersCell(int rowspan, int colspan)
                : base(rowspan, colspan) {
                SetBorder(Border.NO_BORDER);
                SetMargin(2);
                SetVerticalAlignment(VerticalAlignment.MIDDLE);
            }

            protected override IRenderer MakeNewRenderer() {
                return new C05E06_CellBorders.RoundedCornersCellRenderer(this);
            }
        }

        public const String DEST = "../../results/chapter05/cell_borders.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E06_CellBorders().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Table table1 = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1, 1 }));
            table1.SetWidth(UnitValue.CreatePercentValue(80));
            table1.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table1.AddCell(new Cell(1, 3).Add(new Paragraph("Cell with colspan 3")).SetPadding(10).SetMargin(5).SetBorder(new DashedBorder
                (0.5f)));
            table1.AddCell(new Cell(2, 1).Add(new Paragraph("Cell with rowspan 2")).SetMarginTop(5).SetMarginBottom(5).SetBorderBottom
                (new DottedBorder(0.5f)).SetBorderLeft(new DottedBorder(0.5f)));
            table1.AddCell(new Cell().Add(new Paragraph("row 1; cell 1")).SetBorder(new DottedBorder(ColorConstants.ORANGE, 0.5f)));
            table1.AddCell(new Cell().Add(new Paragraph("row 1; cell 2")));
            table1.AddCell(new Cell().Add(new Paragraph("row 2; cell 1")).SetMargin(10).SetBorderBottom(new SolidBorder(2)));
            table1.AddCell(new Cell().Add(new Paragraph("row 2; cell 2")).SetPadding(10).SetBorderBottom(new SolidBorder(2)));
            document.Add(table1);
            Table table2 = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1, 1 }));
            table2.SetMarginTop(10);
            table2.SetBorder(new SolidBorder(1));
            table2.SetWidth(UnitValue.CreatePercentValue(80));
            table2.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table2.AddCell(new Cell(1, 3).Add(new Paragraph("Cell with colspan 3")).SetBorder(Border.NO_BORDER));
            table2.AddCell(new Cell(2, 1).Add(new Paragraph("Cell with rowspan 2")).SetBorder(Border.NO_BORDER));
            table2.AddCell(new Cell().Add(new Paragraph("row 1; cell 1")).SetBorder(Border.NO_BORDER));
            table2.AddCell(new Cell().Add(new Paragraph("row 1; cell 2")).SetBorder(Border.NO_BORDER));
            table2.AddCell(new Cell().Add(new Paragraph("row 2; cell 1")).SetBorder(Border.NO_BORDER));
            table2.AddCell(new Cell().Add(new Paragraph("row 2; cell 2")).SetBorder(Border.NO_BORDER));
            document.Add(table2);
            Table table3 = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1, 1 }));
            table3.SetMarginTop(10);
            table3.SetWidth(UnitValue.CreatePercentValue(80));
            table3.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            Cell cell = new C05E06_CellBorders.RoundedCornersCell(1, 3).Add(new Paragraph("Cell with colspan 3"));
            table3.AddCell(cell);
            cell = new C05E06_CellBorders.RoundedCornersCell(2, 1).Add(new Paragraph("Cell with rowspan 2"));
            table3.AddCell(cell);
            cell = new C05E06_CellBorders.RoundedCornersCell().Add(new Paragraph("row 1; cell 1"));
            table3.AddCell(cell);
            cell = new C05E06_CellBorders.RoundedCornersCell().Add(new Paragraph("row 1; cell 2"));
            table3.AddCell(cell);
            cell = new C05E06_CellBorders.RoundedCornersCell().Add(new Paragraph("row 2; cell 1"));
            table3.AddCell(cell);
            cell = new C05E06_CellBorders.RoundedCornersCell().Add(new Paragraph("row 2; cell 2"));
            table3.AddCell(cell);
            table3.SetNextRenderer(new RoundedCornersTableRenderer(table3));
            document.Add(table3);
            document.Close();
        }
    }
}
