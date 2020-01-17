/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class CountRows
    {
        public static readonly String DEST = "results/sandbox/tables/row_count.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CountRows().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDoc);

            FooterEventHandler footer = new FooterEventHandler(document);
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, footer);

            Table table = new Table(new float[] {50, 100}).SetTextAlignment(TextAlignment.CENTER);
            for (int i = 0; i < 40; i++)
            {
                Cell cell = new Cell();
                cell.SetNextRenderer(new RowNumberCellRenderer(cell, footer));
                table.AddCell(cell);
                table.AddCell("Some text");
            }

            document.Add(table);

            document.Close();
        }

        private class FooterEventHandler : IEventHandler
        {
            private Dictionary<int, int> pageRowsCounts = new Dictionary<int, int>();
            private Document document;

            public FooterEventHandler(Document document)
            {
                this.document = document;
            }

            public int AddRow(int currentPageNumber)
            {
                int rows;
                if (!pageRowsCounts.ContainsKey(currentPageNumber))
                {
                    rows = 0;
                }
                else
                {
                    rows = pageRowsCounts[currentPageNumber];
                }

                rows++;
                pageRowsCounts.Remove(currentPageNumber);
                pageRowsCounts.Add(currentPageNumber, rows);
                return rows;
            }

            public void HandleEvent(Event currentEvent)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) currentEvent;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                int pageNumber = pdfDoc.GetPageNumber(page);

                Paragraph paragraph = new Paragraph(String.Format("There are {0} rows on this page",
                    pageRowsCounts[pageNumber]));
                Canvas canvas = new Canvas(page, page.GetPageSize());
                canvas
                    .ShowTextAligned(paragraph, document.GetRightMargin(),
                        document.GetBottomMargin() / 2, TextAlignment.LEFT)
                    .Close();
            }
        }

        private class RowNumberCellRenderer : CellRenderer
        {
            private FooterEventHandler footer;

            public RowNumberCellRenderer(Cell modelElement, FooterEventHandler footer)
                : base(modelElement)
            {
                this.footer = footer;
            }

            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public CellRenderer GetNextRenderer()
            {
                return new RowNumberCellRenderer((Cell) modelElement, footer);
            }

            public override void Draw(DrawContext drawContext)
            {
                base.Draw(drawContext);
                int pageNumber = GetOccupiedArea().GetPageNumber();
                Rectangle rect = GetOccupiedAreaBBox();

                Paragraph p = new Paragraph(footer.AddRow(pageNumber).ToString());
                float coordX = (rect.GetLeft() + rect.GetRight()) / 2;
                float coordY = (rect.GetBottom() + rect.GetTop()) / 2;
                Canvas canvas = new Canvas(drawContext.GetCanvas(), drawContext.GetDocument(), rect);
                canvas
                    .ShowTextAligned(p, coordX, coordY, TextAlignment.CENTER, VerticalAlignment.MIDDLE)
                    .Close();
            }
        }
    }
}