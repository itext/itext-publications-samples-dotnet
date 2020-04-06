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
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables 
{
    public class DottedLineCell 
    {
        public static readonly string DEST = "results/sandbox/tables/dotted_line_cell.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new DottedLineCell().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            
            doc.Add(new Paragraph("Table event setter approach"));
            
            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            table.AddCell(CreateCellWithBorders("A1"));
            table.AddCell(CreateCellWithBorders("A2"));
            table.AddCell(CreateCellWithBorders("A3"));
            table.AddCell(CreateCellWithBorders("B1"));
            table.AddCell(CreateCellWithBorders("B2"));
            table.AddCell(CreateCellWithBorders("B3"));
            table.AddCell(CreateCellWithBorders("C1"));
            table.AddCell(CreateCellWithBorders("C2"));
            table.AddCell(CreateCellWithBorders("C3"));
            doc.Add(table);
            
            doc.Add(new Paragraph("Cell event setter approach"));
            
            table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            table.AddCell(CreateCellWithBorders("Test"));
            
            doc.Add(table);
            
            doc.Add(new Paragraph("Table event custom render approach"));
         
            table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            table.AddCell(new Cell().Add(new Paragraph("A1")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("A2")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("A3")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("B1")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("B2")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("B3")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("C1")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("C2")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("C3")).SetBorder(Border.NO_BORDER));
            
            // Draws dotted line borders.
            table.SetNextRenderer(new DottedLineTableRenderer(table));
            
            doc.Add(table);
            
            doc.Add(new Paragraph("Cell event custom render approach"));
            
            table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            
            Cell cell = new Cell().Add(new Paragraph("Test"));
            cell.SetNextRenderer(new DottedLineCellRenderer(cell));
            
            // Since we override the border drawing, we do not need the default logic to be triggered, that's why we set
            // the border value as null
            cell.SetBorder(Border.NO_BORDER);
            table.AddCell(cell);
            
            doc.Add(table);
            
            doc.Close();
        }

        private class DottedLineTableRenderer : TableRenderer 
        {
            public DottedLineTableRenderer(Table modelElement)
                : base(modelElement) 
            {
            }

            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer() 
            {
                return new DottedLineTableRenderer((Table)modelElement);
            }

            public override void DrawChildren(DrawContext drawContext) 
            {
                base.DrawChildren(drawContext);
                PdfCanvas canvas = drawContext.GetCanvas();
                canvas.SetLineDash(1f, 3f);
                
                // first horizontal line
                CellRenderer[] cellRenderers = rows[0];
                canvas.MoveTo(cellRenderers[0].GetOccupiedArea().GetBBox().GetLeft(), 
                        cellRenderers[0].GetOccupiedArea().GetBBox().GetTop());
                canvas.LineTo(cellRenderers[cellRenderers.Length - 1].GetOccupiedArea().GetBBox().GetRight(), 
                        cellRenderers[cellRenderers.Length - 1].GetOccupiedArea().GetBBox().GetTop());
                
                foreach (CellRenderer[] renderers in rows) 
                {
                    // horizontal lines
                    canvas.MoveTo(renderers[0].GetOccupiedArea().GetBBox().GetX(), 
                            renderers[0].GetOccupiedArea().GetBBox().GetY());
                    canvas.LineTo(renderers[renderers.Length - 1].GetOccupiedArea().GetBBox().GetRight(), 
                            renderers[renderers.Length - 1].GetOccupiedArea().GetBBox().GetBottom());
                    
                    // first vertical line
                    Rectangle cellRect = renderers[0].GetOccupiedArea().GetBBox();
                    canvas.MoveTo(cellRect.GetLeft(), cellRect.GetBottom());
                    canvas.LineTo(cellRect.GetLeft(), cellRect.GetTop());
                    
                    // vertical lines
                    foreach (CellRenderer renderer in renderers) 
                    {
                        cellRect = renderer.GetOccupiedArea().GetBBox();
                        canvas.MoveTo(cellRect.GetRight(), cellRect.GetBottom());
                        canvas.LineTo(cellRect.GetRight(), cellRect.GetTop());
                    }
                }
                
                canvas.Stroke();
            }
        }

        private class DottedLineCellRenderer : CellRenderer 
        {
            public DottedLineCellRenderer(Cell modelElement)
                : base(modelElement) 
            {
            }

            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer() 
            {
                return new DottedLineCellRenderer((Cell)modelElement);
            }

            public override void Draw(DrawContext drawContext) 
            {
                base.Draw(drawContext);
                drawContext.GetCanvas().SetLineDash(1f, 3f);
                drawContext.GetCanvas().Rectangle(this.GetOccupiedArea().GetBBox());
                drawContext.GetCanvas().Stroke();
            }
        }

        private Cell CreateCellWithBorders(String content) 
        {
            Cell cell = new Cell().Add(new Paragraph(content));
            cell.SetBorder(new DottedBorder(1));
            
            return cell;
        }
    }
}
