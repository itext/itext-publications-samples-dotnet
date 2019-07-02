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
    public class CustomBorder2
    {
        public static readonly string DEST = "../../results/sandbox/tables/custom_border2.pdf";

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

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.SetWidth(500);
            table.SetNextRenderer(new CustomBorder2TableRenderer(table, new Table.RowRange(0, 60)));
            
            for (int i = 1; i < 60; i++)
            {
                table.AddCell(new Cell().Add(new Paragraph("Cell " + i)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(TEXT)).SetBorder(Border.NO_BORDER));
            }

            doc.Add(table);

            doc.Close();
        }

        private class CustomBorder2TableRenderer : TableRenderer
        {
            bool wasSplitted;

            public CustomBorder2TableRenderer(Table modelElement)
                : base(modelElement)
            {
            }

            public CustomBorder2TableRenderer(Table modelElement, Table.RowRange rowRange)
                : base(modelElement, rowRange)
            {
            }

            public override IRenderer GetNextRenderer()
            {
                return new CustomBorder2TableRenderer((Table) modelElement);
            }

            protected override TableRenderer[] Split(int row, bool hasContent)
            {
                TableRenderer[] results = base.Split(row, hasContent);
                CustomBorder2TableRenderer splitRenderer = (CustomBorder2TableRenderer) results[0];
                splitRenderer.wasSplitted = true;
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
                
                if (wasSplitted)
                {
                    if (1 == drawContext.GetDocument().GetNumberOfPages())
                    {
                        canvas
                            .MoveTo(area.GetLeft(), area.GetTop())
                            .LineTo(area.GetRight(), area.GetTop());
                    }
                }
                else
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