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
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class ClippedCell
    {
        public static readonly string DEST = "../../results/sandbox/tables/clipped_cell.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ClippedCell().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();

            // a long phrase with newlines
            Paragraph p = new Paragraph("Dr. iText or:\nHow I Learned to Stop Worrying\nand Love PDF.");
            Cell cell = new Cell().Add(p);

            // the phrase doesn't fits the height
            cell.SetHeight(50f);
            cell.SetNextRenderer(new ClipContentRenderer(cell));
            
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }
        
        private class ClipContentRenderer : CellRenderer
        {
            public ClipContentRenderer(Cell modelElement)
                : base(modelElement)
            {
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new ClipContentRenderer((Cell) modelElement);
            }

            public override LayoutResult Layout(LayoutContext layoutContext)
            {
                Rectangle area = layoutContext.GetArea().GetBBox();
                
                LayoutContext context = new LayoutContext(new LayoutArea(layoutContext.GetArea().GetPageNumber(),
                    new Rectangle(area.GetLeft(), area.GetTop() - (float) RetrieveHeight(), area.GetWidth(),
                        (float) RetrieveHeight())));

                // If content doesn't fit the size of cell,
                // it returns layout result with cell size optimized for the current clipped context
                LayoutResult result = base.Layout(context);
                if (LayoutResult.FULL != result.GetStatus())
                {
                    return new LayoutResult(LayoutResult.FULL, result.GetOccupiedArea(), null, null);
                }

                return result;
            }
        }
    }
}