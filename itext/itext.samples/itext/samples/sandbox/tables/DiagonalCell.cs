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
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class DiagonalCell
    {
        public static readonly string DEST = "results/sandbox/tables/diagonal_cell.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DiagonalCell().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            Table table = new Table(UnitValue.CreatePercentArray(6)).UseAllAvailableWidth();
            Cell cell = new Cell();

            // Draws cell content with top right text 'Gravity' and bottom left text 'Occ'
            cell.SetNextRenderer(new DiagonalCellRenderer(cell, "Gravity", "Occ"));

            table.AddCell(cell.SetMinHeight(30));
            table.AddCell(new Cell().Add(new Paragraph("1")).SetMinHeight(30));
            table.AddCell(new Cell().Add(new Paragraph("2")).SetMinHeight(30));
            table.AddCell(new Cell().Add(new Paragraph("3")).SetMinHeight(30));
            table.AddCell(new Cell().Add(new Paragraph("4")).SetMinHeight(30));
            table.AddCell(new Cell().Add(new Paragraph("5")).SetMinHeight(30));

            for (int i = 0; i < 5;)
            {
                table.AddCell(new Cell().Add(new Paragraph((++i).ToString())).SetMinHeight(30));
                table.AddCell(new Cell().SetMinHeight(30));
                table.AddCell(new Cell().SetMinHeight(30));
                table.AddCell(new Cell().SetMinHeight(30));
                table.AddCell(new Cell().SetMinHeight(30));
                table.AddCell(new Cell().SetMinHeight(30));
            }

            doc.Add(table);

            doc.Close();
        }

        private class DiagonalCellRenderer : CellRenderer
        {
            private string textTopRight;

            private string textBottomLeft;

            public DiagonalCellRenderer(Cell modelElement, string textTopRight, string textBottomLeft)
                : base(modelElement)
            {
                this.textTopRight = textTopRight;
                this.textBottomLeft = textBottomLeft;
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new DiagonalCellRenderer((Cell) modelElement, textTopRight, textBottomLeft);
            }

            public override void DrawBorder(DrawContext drawContext)
            {
                PdfCanvas canvas = drawContext.GetCanvas();
                Rectangle rect = GetOccupiedAreaBBox();
                
                canvas
                    .SaveState()
                    .MoveTo(rect.GetLeft(), rect.GetTop())
                    .LineTo(rect.GetRight(), rect.GetBottom())
                    .Stroke()
                    .RestoreState();
                
                new Canvas(canvas, drawContext.GetDocument(), GetOccupiedAreaBBox())
                    .ShowTextAligned(textTopRight,rect.GetRight() - 2, rect.GetTop() - 2, 
                        TextAlignment.RIGHT, VerticalAlignment.TOP, 0)
                    .ShowTextAligned(textBottomLeft, rect.GetLeft() + 2, rect.GetBottom() + 2, TextAlignment.LEFT);
            }
        }
    }
}