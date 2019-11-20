/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Objects
{
    public class TableInColumn
    {
        public static readonly string DEST = "results/sandbox/objects/table_in_column.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new TableInColumn().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Cell cell = new Cell();
            for (int i = 1; i <= 20; i++)
            {
                cell.Add(new Paragraph("Line " + i));
            }

            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            table.AddCell(cell);
            table.SetNextRenderer(new CustomTableRenderer(table, ResolveTableRect(doc, table)));
            doc.Add(table);

            doc.Add(new AreaBreak());
            table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
            table.AddCell(cell.Clone(true));
            table.AddCell(cell.Clone(true));
            table.SetNextRenderer(new CustomTableRenderer(table, ResolveTableRect(doc, table)));
            doc.Add(table);

            doc.Close();
        }

        private static Rectangle ResolveTableRect(Document doc, Table table)
        {
            Rectangle pageDimension = new Rectangle(36, 36, 523, 770);
            IRenderer tableRenderer = table.CreateRendererSubTree().SetParent(doc.GetRenderer());
            LayoutResult tableLayoutResult = tableRenderer.Layout(new LayoutContext(new LayoutArea(0, pageDimension)));

            Rectangle resultRect;
            if (LayoutResult.PARTIAL == tableLayoutResult.GetStatus())
            {
                resultRect = pageDimension;
            }
            else
            {
                Rectangle tableBBox = tableLayoutResult.GetOccupiedArea().GetBBox();
                resultRect = new Rectangle(pageDimension.GetX(), ((tableBBox.GetBottom() + pageDimension.GetX()) / 2),
                    pageDimension.GetWidth(), tableBBox.GetHeight());
            }

            return resultRect;
        }

        protected class CustomTableRenderer : TableRenderer
        {
            protected Rectangle rect;

            public CustomTableRenderer(Table modelElement, Rectangle rect) : base(modelElement)
            {
                this.rect = rect;
            }

            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overridden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new CustomTableRenderer((Table) modelElement, rect);
            }

            public override LayoutResult Layout(LayoutContext layoutContext)
            {
                return base.Layout(new LayoutContext(new LayoutArea(layoutContext.GetArea().GetPageNumber(), rect)));
            }
        }
    }
}