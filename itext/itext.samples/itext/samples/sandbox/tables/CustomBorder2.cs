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
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class CustomBorder2
    {
        public static readonly string DEST = "results/sandbox/tables/custom_border2.pdf";

        public static readonly string TEXT = "This is some long paragraph that will be added over and over " +
                                             "again to prove a point. It should result in rows that are split and rows that aren't.";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CustomBorder2().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            // By default column width is calculated automatically for the best fit.
            // useAllAvailableWidth() method makes table use the whole page's width while placing the content.
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();

            for (int i = 1; i < 60; i++) 
            {
                table.AddCell(new Cell().Add(new Paragraph("Cell " + i)));
                table.AddCell(new Cell().Add(new Paragraph(TEXT)));
            }

            // Use a custom renderer in which borders drawing is overridden
            table.SetNextRenderer(new CustomBorder2TableRenderer(table));

            doc.Add(table);

            doc.Close();
        }

        private class CustomBorder2TableRenderer : TableRenderer
        {
            private bool top = true;
            private bool bottom = true;

            public CustomBorder2TableRenderer(Table modelElement) : base(modelElement)
            {
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new CustomBorder2TableRenderer((Table) modelElement);
            }

            protected override TableRenderer[] Split(int row, bool hasContent, bool cellWithBigRowspanAdded)
            {
                // Being inside this method implies that split has occurred

                TableRenderer[] results = base.Split(row, hasContent, cellWithBigRowspanAdded);

                CustomBorder2TableRenderer splitRenderer = (CustomBorder2TableRenderer)results[0];

                // iText shouldn't draw the bottom split renderer's border,
                // because there is an overflow renderer
                splitRenderer.bottom = false;

                // If top is true, the split renderer is the first renderer of the document.
                // If false, iText has already processed the first renderer
                splitRenderer.top = this.top;

                CustomBorder2TableRenderer overflowRenderer = (CustomBorder2TableRenderer)results[1];

                // iText shouldn't draw the top overflow renderer's border
                overflowRenderer.top = false;

                return results;
            }

            protected override void DrawBorders(DrawContext drawContext)
            {
                Rectangle area = occupiedArea.GetBBox();
                PdfCanvas canvas = drawContext.GetCanvas();

                canvas
                    .SaveState()
                    .MoveTo(area.GetLeft(), area.GetBottom())
                    .LineTo(area.GetLeft(), area.GetTop())
                    .MoveTo(area.GetRight(), area.GetTop())
                    .LineTo(area.GetRight(), area.GetBottom());

                if (top) 
                {
                    canvas
                        .MoveTo(area.GetLeft(), area.GetTop())
                        .LineTo(area.GetRight(), area.GetTop());
                }

                if (bottom) 
                {
                    canvas
                        .MoveTo(area.GetLeft(), area.GetBottom())
                        .LineTo(area.GetRight(), area.GetBottom());
                }

                canvas
                    .Stroke()
                    .RestoreState();
            }
        }
    }
}