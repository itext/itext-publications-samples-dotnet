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
    public class CustomBorder3
    {
        public static readonly string DEST = "../../results/sandbox/tables/custom_border3.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CustomBorder3().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDoc);

            ILineDash solid = new Solid();
            ILineDash dotted = new Dotted();
            ILineDash dashed = new Dashed();

            // By default column width is calculated automatically for the best fit.
            // useAllAvailableWidth() method set table to use the whole page's width while placing the content.
            Table table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            table.SetMarginBottom(30);
            
            Cell cell = new Cell().Add(new Paragraph("dotted left border"));
            cell.SetBorder(Border.NO_BORDER);
            
            //Draws custom borders to current cell
            cell.SetNextRenderer(new CustomBorder3Renderer(cell,
                new[] {null, dotted, null, null}));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("solid right border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new CustomBorder3Renderer(cell,
                new[] {null, null, null, solid}));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("dashed top border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new CustomBorder3Renderer(cell,
                new[] {dashed, null, null, null}));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("bottom border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new CustomBorder3Renderer(cell,
                new[] {null, null, solid, null}));

            table.AddCell(cell);
            document.Add(table);

            table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            table.SetMarginBottom(30);
            
            cell = new Cell().Add(new Paragraph("dotted left and solid top border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new CustomBorder3Renderer(cell,
                new[] {solid, dotted, null, null}));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("dashed right and dashed bottom border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new CustomBorder3Renderer(cell,
                new[] {null, null, dashed, dashed}));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("no border"));
            cell.SetBorder(Border.NO_BORDER);
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("full solid border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new CustomBorder3Renderer(cell,
                new[] {solid, solid, solid, solid}));
            table.AddCell(cell);

            document.Add(table);
            document.Close();
        }

        interface ILineDash
        {
            void ApplyLineDash(PdfCanvas canvas);
        }


        class Solid : ILineDash
        {
            public void ApplyLineDash(PdfCanvas canvas)
            {
            }
        }


        class Dotted : ILineDash
        {
            public void ApplyLineDash(PdfCanvas canvas)
            {
                canvas.SetLineCapStyle(PdfCanvasConstants.LineCapStyle.ROUND);
                canvas.SetLineDash(0, 4, 2);
            }
        }


        class Dashed : ILineDash
        {
            public void ApplyLineDash(PdfCanvas canvas)
            {
                canvas.SetLineDash(3, 3);
            }
        }

        class CustomBorder3Renderer : CellRenderer
        {
            ILineDash[] borders;

            public CustomBorder3Renderer(Cell modelElement, ILineDash[] borders)
                : base(modelElement)
            {
                this.borders = new ILineDash[borders.Length];
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
                return new CustomBorder3Renderer((Cell) modelElement, borders);
            }

            public override void Draw(DrawContext drawContext)
            {
                base.Draw(drawContext);
                PdfCanvas canvas = drawContext.GetCanvas();
                Rectangle position = GetOccupiedAreaBBox();
                canvas.SaveState();
                
                if (null != borders[0])
                {
                    canvas.SaveState();
                    borders[0].ApplyLineDash(canvas);
                    canvas.MoveTo(position.GetRight(), position.GetTop());
                    canvas.LineTo(position.GetLeft(), position.GetTop());
                    canvas.Stroke();
                    canvas.RestoreState();
                }

                if (null != borders[2])
                {
                    canvas.SaveState();
                    borders[2].ApplyLineDash(canvas);
                    canvas.MoveTo(position.GetRight(), position.GetBottom());
                    canvas.LineTo(position.GetLeft(), position.GetBottom());
                    canvas.Stroke();
                    canvas.RestoreState();
                }

                if (null != borders[3])
                {
                    canvas.SaveState();
                    borders[3].ApplyLineDash(canvas);
                    canvas.MoveTo(position.GetRight(), position.GetTop());
                    canvas.LineTo(position.GetRight(), position.GetBottom());
                    canvas.Stroke();
                    canvas.RestoreState();
                }

                if (null != borders[1])
                {
                    canvas.SaveState();
                    borders[1].ApplyLineDash(canvas);
                    canvas.MoveTo(position.GetLeft(), position.GetTop());
                    canvas.LineTo(position.GetLeft(), position.GetBottom());
                    canvas.Stroke();
                    canvas.RestoreState();
                }

                canvas.Stroke();
                canvas.RestoreState();
            }
        }
    }
}