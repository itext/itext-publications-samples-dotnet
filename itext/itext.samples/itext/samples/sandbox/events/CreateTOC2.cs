using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Navigation;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Events
{
   
    // CreateTOC2.cs
    // 
    // This class demonstrates how to create an interactive table of contents with clickable entries.
    // It enhances the basic TOC example by adding PDF destinations and actions to each entry,
    // allowing readers to navigate directly to specific sections by clicking on TOC items.
 
    public class CreateTOC2
    {
        public static readonly String DEST = "results/sandbox/events/create_toc2.pdf";

        private static int counter = 0;

        private static IList<KeyValuePair<String, KeyValuePair<String, int>>> toc =
            new List<KeyValuePair<String, KeyValuePair<String, int>>>();

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CreateTOC2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            for (int i = 0; i < 10; i++)
            {
                String title = "This is title " + i;
                Text text = new Text(title).SetFontSize(16);
                text.SetNextRenderer(new TOCTextRenderer(text));
                doc.Add(new Paragraph(text));
                for (int j = 0; j < 50; j++)
                {
                    doc.Add(new Paragraph("Line " + j + " of title " + i));
                }
            }

            doc.Add(new AreaBreak());

            // Create a table of contents
            doc.Add(new Paragraph("Table of Contents").SetFontSize(16));
            foreach (KeyValuePair<String, KeyValuePair<String, int>> entry in toc)
            {
                Text text = new Text(entry.Key);
                KeyValuePair<String, int> value = entry.Value;

                text.SetAction(PdfAction.CreateGoTo(value.Key));
                Paragraph p = new Paragraph(text);

                p.AddTabStops(new TabStop(750, TabAlignment.RIGHT, new DottedLine()));
                p.Add(new Tab());

                text = new Text(value.Value.ToString());
                text.SetAction(PdfAction.CreateGoTo(value.Key));
                p.Add(text);

                doc.Add(p);
            }

            doc.Close();
        }

        private class TOCTextRenderer : TextRenderer
        {
            public TOCTextRenderer(Text modelElement) : base(modelElement)
            {
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
                String name = "dest" + (counter++);

                int pageNumber = occupiedArea.GetPageNumber();
                toc.Add(new KeyValuePair<String, KeyValuePair<String, int>>(((Text) modelElement).GetText(),
                    new KeyValuePair<String, int>(name, pageNumber)));

                PdfPage page = drawContext.GetDocument().GetPage(pageNumber);
                drawContext.GetDocument().AddNamedDestination(name,
                    PdfExplicitDestination.CreateFitH(page, page.GetPageSize().GetTop()).GetPdfObject());
            }
        }
    }
}