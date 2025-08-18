using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Events
{
   
    // CreateTOC.cs
    // 
    // This class demonstrates how to create a table of contents (TOC) by tracking text elements.
    // It uses a custom renderer to collect title information and page numbers while generating
    // content, then creates a TOC at the end with dotted line leaders to page numbers.
 
    public class CreateTOC
    {
        public static readonly String DEST = "results/sandbox/events/create_toc.pdf";

        private static IList<KeyValuePair<String, int>> toc = new List<KeyValuePair<String, int>>();

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CreateTOC().ManipulatePdf(DEST);
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
            foreach (KeyValuePair<String, int> entry in toc)
            {
                Paragraph p = new Paragraph(entry.Key);
                p.AddTabStops(new TabStop(750, TabAlignment.RIGHT, new DottedLine()));
                p.Add(new Tab());
                p.Add(entry.Value.ToString());
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
                String title = ((Text) modelElement).GetText();

                int pageNumber = GetOccupiedArea().GetPageNumber();
                toc.Add(new KeyValuePair<String, int>(title, pageNumber));
            }
        }
    }
}