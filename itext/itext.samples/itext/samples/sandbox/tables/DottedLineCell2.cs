/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class DottedLineCell2
    {
        public static readonly string DEST = "../../results/sandbox/tables/dotted_line_cell2.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DottedLineCell2().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            table.SetMarginBottom(30);
            
            Cell cell = new Cell().Add(new Paragraph("left border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new DottedLineCellRenderer(cell, new[] {false, true, false, false}));
            table.AddCell(cell);

            cell = new Cell().Add(new Paragraph("right border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new DottedLineCellRenderer(cell, new[] {false, false, false, true}));
            table.AddCell(cell);

            cell = new Cell().Add(new Paragraph("top border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new DottedLineCellRenderer(cell, new[] {true, false, false, false}));
            table.AddCell(cell);

            cell = new Cell().Add(new Paragraph("bottom border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new DottedLineCellRenderer(cell, new[] {false, false, true, false}));
            table.AddCell(cell);

            document.Add(table);
            
            table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            table.SetMarginBottom(30);

            cell = new Cell().Add(new Paragraph("left and top border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new DottedLineCellRenderer(cell, new[] {true, true, false, false}));
            table.AddCell(cell);

            cell = new Cell().Add(new Paragraph("right and bottom border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new DottedLineCellRenderer(cell, new[] {false, false, true, true}));
            table.AddCell(cell);

            cell = new Cell().Add(new Paragraph("no border"));
            cell.SetBorder(Border.NO_BORDER);
            table.AddCell(cell);

            cell = new Cell().Add(new Paragraph("full border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new DottedLineCellRenderer(cell, new[] {true, true, true, true}));
            table.AddCell(cell);

            document.Add(table);

            document.Close();
        }

        private class DottedLineCellRenderer : CellRenderer
        {
            private bool[] borders;

            public DottedLineCellRenderer(Cell modelElement, bool[] borders)
                : base(modelElement)
            {
                this.borders = new bool[borders.Length];

                for (int i = 0; i < this.borders.Length; i++)
                {
                    this.borders[i] = borders[i];
                }
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new DottedLineCellRenderer((Cell) modelElement, borders);
            }

            public override void Draw(DrawContext drawContext)
            {
                base.Draw(drawContext);
                PdfCanvas canvas = drawContext.GetCanvas();
                Rectangle position = GetOccupiedAreaBBox();
                canvas.SaveState();
                canvas.SetLineDash(0, 4, 2);

                if (borders[0])
                {
                    canvas.MoveTo(position.GetRight(), position.GetTop());
                    canvas.LineTo(position.GetLeft(), position.GetTop());
                }

                if (borders[2])
                {
                    canvas.MoveTo(position.GetRight(), position.GetBottom());
                    canvas.LineTo(position.GetLeft(), position.GetBottom());
                }

                if (borders[3])
                {
                    canvas.MoveTo(position.GetRight(), position.GetTop());
                    canvas.LineTo(position.GetRight(), position.GetBottom());
                }

                if (borders[1])
                {
                    canvas.MoveTo(position.GetLeft(), position.GetTop());
                    canvas.LineTo(position.GetLeft(), position.GetBottom());
                }

                canvas.Stroke();
                canvas.RestoreState();
            }
        }
    }
}