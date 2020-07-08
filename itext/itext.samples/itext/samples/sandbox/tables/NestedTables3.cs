using System;
using System.IO;
using System.Linq;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class NestedTables3
    {
        public static readonly string DEST = "results/sandbox/tables/nested_tables3.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new NestedTables3().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());

            // Creates outer table
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            
            // Draws header for every nested table.
            // That header will be repeated on every page.
            table.SetNextRenderer(new InnerTableRenderer(table, new Table.RowRange(0, 0)));

            Cell cell = new Cell(1, 2).Add(new Paragraph("This outer header is repeated on every page"));
            table.AddHeaderCell(cell);

            // Creates the first inner table 
            Table inner1 = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();

            // Creates an empty header cell for the header content drawn by outer table renderer
            cell = new Cell();
            cell.SetHeight(20);
            inner1.AddHeaderCell(cell);

            // Creates header cell that will be repeated only on pages, where that table has content
            cell = new Cell().Add(new Paragraph("This inner header won't be repeated on every page"));
            inner1.AddHeaderCell(cell);

            for (int i = 0; i < 10; i++)
            {
                inner1.AddCell(new Cell().Add(new Paragraph("test")));
            }

            cell = new Cell().Add(inner1);
            table.AddCell(cell.SetPadding(0));

            // Creates the second inner table 
            Table inner2 = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();

            // Creates an empty header cell for the header content drawn by outer table renderer
            cell = new Cell();
            cell.SetHeight(20);
            inner2.AddHeaderCell(cell);

            // Creates header cell that will be repeated only on pages, where that table has content
            cell = new Cell().Add(new Paragraph("This inner header may be repeated on every page"));
            inner2.AddHeaderCell(cell);

            for (int i = 0; i < 35; i++)
            {
                inner2.AddCell("test");
            }

            cell = new Cell().Add(inner2);
            table.AddCell(cell.SetPadding(0));

            doc.Add(table);

            doc.Close();
        }

        private class InnerTableRenderer : TableRenderer
        {
            public InnerTableRenderer(Table modelElement, Table.RowRange rowRange)
                : base(modelElement, rowRange)
            {
            }

            protected InnerTableRenderer(Table modelElement)
                : base(modelElement)
            {
            }
            
            public override void DrawChildren(DrawContext drawContext)
            {
                base.DrawChildren(drawContext);

                foreach (IRenderer renderer in childRenderers)
                {
                    PdfCanvas canvas = drawContext.GetCanvas();
                    canvas.BeginText();
                    Rectangle box = ((AbstractRenderer) renderer).GetInnerAreaBBox();
                    UnitValue fontSize = GetPropertyAsUnitValue(Property.FONT_SIZE);
                    canvas.MoveText(box.GetLeft(),
                        box.GetTop() - (fontSize.IsPointValue() ? fontSize.GetValue() : 12f));
                    canvas.SetFontAndSize(GetPropertyAsFont(Property.FONT),
                        fontSize.IsPointValue() ? fontSize.GetValue() : 12f);
                    canvas.ShowText("This inner table header will always be repeated");
                    canvas.EndText();
                    canvas.Stroke();
                }
            }           
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new InnerTableRenderer((Table) modelElement);
            }
        }
    }
}