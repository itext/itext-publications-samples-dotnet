/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class CellTitle
    {
        public static readonly string DEST = "../../results/sandbox/tables/cell_title.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CellTitle().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            // By default column width is calculated automatically for the best fit.
            // useAllAvailableWidth() method set table to use the whole page's width while placing the content.
            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            Cell cell = GetCell("The title of this cell is title 1", "title 1");
            table.AddCell(cell);
            cell = GetCell("The title of this cell is title 2", "title 2");
            table.AddCell(cell);
            cell = GetCell("The title of this cell is title 3", "title 3");
            table.AddCell(cell);
            doc.Add(table);
            
            doc.Close();
        }

        private class CellTitleRenderer : CellRenderer
        {
            protected string title;

            public CellTitleRenderer(Cell modelElement, String title)
                : base(modelElement)
            {
                this.title = title;
            }

            public override void DrawBorder(DrawContext drawContext)
            {
                PdfPage currentPage = drawContext.GetDocument().GetPage(GetOccupiedArea().GetPageNumber());
                
                // create above canvas in order to draw above borders (notice that iText draws borders using TableRenderer)
                PdfCanvas aboveCanvas = new PdfCanvas(currentPage.NewContentStreamAfter(), currentPage.GetResources(), 
                    drawContext.GetDocument());
                new Canvas(aboveCanvas, drawContext.GetDocument(), GetOccupiedAreaBBox())
                    .Add(new Paragraph(title)
                        .SetMultipliedLeading(1)
                        .SetMargin(0)
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                        .SetFixedPosition(GetOccupiedAreaBBox().GetLeft() + 5, GetOccupiedAreaBBox().GetTop() - 8, 30));
            }
        }

        private Cell GetCell(string content, string title)
        {
            Cell cell = new Cell().Add(new Paragraph(content));
            cell.SetNextRenderer(new CellTitleRenderer(cell, title));
            cell.SetPaddingTop(8).SetPaddingBottom(8);
            return cell;
        }
    }
}