using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Navigation;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Events
{
    public class CreateTOCinColumn
    {
        public static readonly String DEST = "results/sandbox/events/create_toc_in_column.pdf";

        private static IList<KeyValuePair<String, PdfDestination>> list =
            new List<KeyValuePair<String, PdfDestination>>();

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CreateTOCinColumn().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            Rectangle[] columns =
            {
                new Rectangle(36, 36, 173, 770),
                new Rectangle(213, 36, 173, 770),
                new Rectangle(389, 36, 173, 770)
            };

            doc.SetRenderer(new ColumnDocumentRenderer(doc, columns));
            PdfOutline root = pdfDoc.GetOutlines(false);
            for (int i = 0; i <= 20; i++)
            {
                int start = (i * 10) + 1;
                int end = (i + 1) * 10;
                String title = String.Format("Numbers from {0} to {1}", start, end);
                Text c = new Text(title);
                TOCTextRenderer renderer = new TOCTextRenderer(c);
                renderer.setRoot(root);
                c.SetNextRenderer(renderer);
                doc.Add(new Paragraph(c));
                doc.Add(CreateTable(start, end));
            }

            doc.Add(new AreaBreak());
            foreach (KeyValuePair<String, PdfDestination> entry in list)
            {
                Link c = new Link(entry.Key, entry.Value);
                doc.Add(new Paragraph(c));
            }

            doc.Close();
        }

        private static Table CreateTable(int start, int end)
        {
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            for (int i = start; i <= end; i++)
            {
                table.AddCell(new Cell().Add(new Paragraph(i.ToString())));
                table.AddCell(new Cell().Add(new Paragraph("Test")));
            }

            return table;
        }

        private class TOCTextRenderer : TextRenderer
        {
            protected PdfOutline root;

            public TOCTextRenderer(Text modelElement) : base(modelElement)
            {
            }

            public void setRoot(PdfOutline root)
            {
                this.root = root;
            }

            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new TOCTextRenderer((Text) modelElement);
            }

            public override void Draw(DrawContext drawContext)
            {
                base.Draw(drawContext);
                Rectangle rect = GetOccupiedAreaBBox();
                PdfPage page = drawContext.GetDocument().GetPage(GetOccupiedArea().GetPageNumber());
                PdfDestination dest = PdfExplicitDestination.CreateXYZ(page, rect.GetLeft(), rect.GetTop(), 0);

                list.Add(new KeyValuePair<String, PdfDestination>(((Text) modelElement).GetText(), dest));

                PdfOutline curOutline = root.AddOutline(((Text) modelElement).GetText());
                curOutline.AddDestination(dest);
            }
        }
    }
}