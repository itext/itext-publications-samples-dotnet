/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class RoundedCorners
    {
        public static readonly string DEST = "../../results/sandbox/tables/rounded_corners.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RoundedCorners().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            Cell cell = GetCell("These cells have rounded borders at the top.");
            table.AddCell(cell);

            cell = GetCell("These cells aren't rounded at the bottom.");
            table.AddCell(cell);

            cell = GetCell("A custom cell event was used to achieve this.");
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }

        private Cell GetCell(String content)
        {
            Cell cell = new Cell().Add(new Paragraph(content));
            cell.SetNextRenderer(new RoundedCornersCellRenderer(cell));
            cell.SetPadding(5);
            cell.SetBorder(null);
            return cell;
        }

        private class RoundedCornersCellRenderer : CellRenderer
        {
            public RoundedCornersCellRenderer(Cell modelElement)
                : base(modelElement)
            {
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new RoundedCornersCellRenderer((Cell) modelElement);
            }

            public override void Draw(DrawContext drawContext)
            {
                float llx = GetOccupiedAreaBBox().GetX() + 2;
                float lly = GetOccupiedAreaBBox().GetY() + 2;
                float urx = GetOccupiedAreaBBox().GetX() + GetOccupiedAreaBBox().GetWidth() - 2;
                float ury = GetOccupiedAreaBBox().GetY() + GetOccupiedAreaBBox().GetHeight() - 2;
                
                float r = 4;
                float b = 0.4477f;
                
                PdfCanvas canvas = drawContext.GetCanvas();
                canvas.MoveTo(llx, lly);
                canvas.LineTo(urx, lly);
                canvas.LineTo(urx, ury - r);
                canvas.CurveTo(urx, ury - r * b, urx - r * b, ury, urx - r, ury);
                canvas.LineTo(llx + r, ury);
                canvas.CurveTo(llx + r * b, ury, llx, ury - r * b, llx, ury - r);
                canvas.LineTo(llx, lly);
                canvas.Stroke();
                base.Draw(drawContext);
            }
        }
    }
}