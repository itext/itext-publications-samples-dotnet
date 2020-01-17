/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class ClipCenterCellContent
    {
        public static readonly string DEST = "results/sandbox/tables/clip_center_cell_content.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ClipCenterCellContent().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
            
            for (int r = 'A'; r <= 'Z'; r++)
            {
                for (int c = 1; c <= 5; c++)
                {
                    Cell cell = new Cell();
                    if (r == 'D' && c == 2)
                    {
                        // Draw a content that will be clipped in the cell 
                        cell.SetNextRenderer(new ClipCenterCellContentCellRenderer(cell,
                            new Paragraph("D2 is a cell with more content than we can fit into the cell.")));
                    }
                    else
                    {
                        cell.Add(new Paragraph(((char) r).ToString() + c));
                    }

                    table.AddCell(cell);
                }
            }

            doc.Add(table);

            doc.Close();
        }

        private class ClipCenterCellContentCellRenderer : CellRenderer
        {
            private Paragraph content;

            public ClipCenterCellContentCellRenderer(Cell modelElement, Paragraph content)
                : base(modelElement)
            {
                this.content = content;
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new ClipCenterCellContentCellRenderer((Cell) modelElement, content);
            }

            public override void Draw(DrawContext drawContext)
            {

                // Fictitiously layout the renderer and find out, how much space does it require
                IRenderer pr = content.CreateRendererSubTree().SetParent(this);

                LayoutResult textArea = pr.Layout(new LayoutContext(
                    new LayoutArea(0, new Rectangle(GetOccupiedAreaBBox().GetWidth(), 1000))));

                float spaceNeeded = textArea.GetOccupiedArea().GetBBox().GetHeight();
                Console.WriteLine("The content requires {0} pt whereas the height is {1} pt.",
                    spaceNeeded, GetOccupiedAreaBBox().GetHeight());

                float offset = (GetOccupiedAreaBBox().GetHeight() - textArea.GetOccupiedArea()
                                    .GetBBox().GetHeight()) / 2;
                Console.WriteLine("The difference is {0} pt; we'll need an offset of {1} pt.",
                    -2f * offset, offset);

                PdfFormXObject xObject = new PdfFormXObject(new Rectangle(GetOccupiedAreaBBox().GetWidth(),
                    GetOccupiedAreaBBox().GetHeight()));

                Canvas layoutCanvas = new Canvas(new PdfCanvas(xObject, drawContext.GetDocument()),
                    drawContext.GetDocument(),
                    new Rectangle(0, offset, GetOccupiedAreaBBox().GetWidth(), spaceNeeded));
                layoutCanvas.Add(content);

                drawContext.GetCanvas().AddXObject(xObject, occupiedArea.GetBBox().GetLeft(),
                    occupiedArea.GetBBox().GetBottom());
            }
        }
    }
}