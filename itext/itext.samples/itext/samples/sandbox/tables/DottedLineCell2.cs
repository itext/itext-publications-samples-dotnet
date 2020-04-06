/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

/**
 * Example written by Bruno Lowagie in answer to the following question:
 * http://stackoverflow.com/questions/34555756/one-cell-with-different-border-types
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
        public static readonly string DEST = "results/sandbox/tables/dotted_line_cell2.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new DottedLineCell2().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDoc);
            
            Paragraph paragraph = new Paragraph("Setter approach");
            document.Add(paragraph.SetFontSize(25));
            
            Table table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            table.SetMarginBottom(30);
            
            table.AddCell(CreateCell("left border", new Style().SetBorderLeft(new DottedBorder(1))));
            table.AddCell(CreateCell("right border", new Style().SetBorderRight(new DottedBorder(1))));
            table.AddCell(CreateCell("top border", new Style().SetBorderTop(new DottedBorder(1))));
            table.AddCell(CreateCell("bottom border", new Style().SetBorderBottom(new DottedBorder(1))));
            
            document.Add(table);
            
            table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            table.SetMarginBottom(30);
            
            table.AddCell(CreateCell("left and top border", new Style()
                    .SetBorderLeft(new DottedBorder(1))
                    .SetBorderTop(new DottedBorder(1))));
            table.AddCell(CreateCell("right and bottom border", new Style()
                    .SetBorderRight(new DottedBorder(1))
                    .SetBorderBottom(new DottedBorder(1))));
            table.AddCell(CreateCell("no border", new Style()));
            table.AddCell(CreateCell("full border", new Style()
                    .SetBorderBottom(new DottedBorder(1))
                    .SetBorderTop(new DottedBorder(1))
                    .SetBorderRight(new DottedBorder(1))
                    .SetBorderLeft(new DottedBorder(1))));
            
            document.Add(table);
            
            paragraph = new Paragraph("Custom render approach");
            document.Add(paragraph.SetFontSize(25));
            
            table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            table.SetMarginBottom(30);
            
            Cell cell = new Cell().Add(new Paragraph("left border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new DottedLineCellRenderer(cell, new bool[] { false, true, false, false}));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("right border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new DottedLineCellRenderer(cell, new bool[] { false, false, false, true}));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("top border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new DottedLineCellRenderer(cell, new bool[] { true, false, false, false}));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("bottom border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new DottedLineCellRenderer(cell, new bool[] { false, false, true, false}));
            table.AddCell(cell);
            
            document.Add(table);
            
            table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            table.SetMarginBottom(30);
            
            cell = new Cell().Add(new Paragraph("left and top border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new DottedLineCellRenderer(cell, new bool[] { true, true, false, false}));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("right and bottom border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new DottedLineCellRenderer(cell, new bool[] { false, false, true, true}));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("no border"));
            cell.SetBorder(Border.NO_BORDER);
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("full border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetNextRenderer(new DottedLineCellRenderer(cell, new bool[] { true, true, true, true }));
            table.AddCell(cell);
            
            document.Add(table);
            
            document.Close();
        }

        private class DottedLineCellRenderer : CellRenderer 
        {
            internal bool[] borders;

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
                return new DottedLineCellRenderer((Cell)modelElement, borders);
            }

            public override void Draw(DrawContext drawContext) 
            {
                base.Draw(drawContext);
                PdfCanvas canvas = drawContext.GetCanvas();
                Rectangle position = GetOccupiedAreaBBox();
                canvas.SaveState();
                canvas.SetLineDash(1f, 3f);
                
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

        private static Cell CreateCell(String content, Style style) 
        {
            Cell cell = new Cell()
                    .Add(new Paragraph(content))

                    // By default there is a BORDER property set as SolidBorder. We want to override it
                    // and that's why this property is set to null.
                    // However, if there is a BORDER property in the passed Style instance,
                    // it will be used because it's added afterwards.                
                    .AddStyle(new Style().SetBorder(Border.NO_BORDER));
            
            cell.AddStyle(style);

            return cell;
        }
    }
}
