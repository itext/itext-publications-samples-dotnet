/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Events
{
    public class TableHeader
    {
        public static readonly String DEST = "results/sandbox/events/table_header.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TableHeader().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            TableHeaderEventHandler handler = new TableHeaderEventHandler(doc);
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, handler);

            // Calculate top margin to be sure that the table will fit the margin.
            float topMargin = 20 + handler.GetTableHeight();
            doc.SetMargins(topMargin, 36, 36, 36);

            for (int i = 0; i < 50; i++)
            {
                doc.Add(new Paragraph("Hello World!"));
            }

            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("Hello World!"));
            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("Hello World!"));

            doc.Close();
        }

        private class TableHeaderEventHandler : IEventHandler
        {
            private Table table;
            private float tableHeight;
            private Document doc;

            public TableHeaderEventHandler(Document doc)
            {
                this.doc = doc;
                InitTable();

                TableRenderer renderer = (TableRenderer) table.CreateRendererSubTree();
                renderer.SetParent(new DocumentRenderer(doc));

                // Simulate the positioning of the renderer to find out how much space the header table will occupy.
                LayoutResult result = renderer.Layout(new LayoutContext(new LayoutArea(0, PageSize.A4)));
                tableHeight = result.GetOccupiedArea().GetBBox().GetHeight();
            }

            public void HandleEvent(Event currentEvent)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) currentEvent;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
                PageSize pageSize = pdfDoc.GetDefaultPageSize();
                float coordX = pageSize.GetX() + doc.GetLeftMargin();
                float coordY = pageSize.GetTop() - doc.GetTopMargin();
                float width = pageSize.GetWidth() - doc.GetRightMargin() - doc.GetLeftMargin();
                float height = GetTableHeight();
                Rectangle rect = new Rectangle(coordX, coordY, width, height);

                new Canvas(canvas, pdfDoc, rect)
                    .Add(table)
                    .Close();
            }

            public float GetTableHeight()
            {
                return tableHeight;
            }

            private void InitTable()
            {
                table = new Table(1).UseAllAvailableWidth();
                table.AddCell("Header row 1");
                table.AddCell("Header row 2");
                table.AddCell("Header row 3");
            }
        }
    }
}