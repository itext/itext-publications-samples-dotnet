using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Layout
{
   
    // TwoColumnParagraphLayout.cs
    //
    // Example showing custom paragraph renderer for two-column layout.
    // Demonstrates splitting a single paragraph across two side-by-side areas.
 
    public class TwoColumnParagraphLayout
    {
        public static readonly string DEST = "results/sandbox/layout/complexElementLayout.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TwoColumnParagraphLayout().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            doc.Add(new Paragraph("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"));

            StringBuilder text = new StringBuilder();
            for (int i = 0; i < 200; i++)
            {
                text.Append("A very long text is here...");
            }

            Paragraph twoColumnParagraph = new Paragraph();

            // Set the custom renderer to create a layout consisted of two columns
            twoColumnParagraph.SetNextRenderer(new TwoColumnParagraphRenderer(twoColumnParagraph));
            twoColumnParagraph.Add(text.ToString());
            doc.Add(twoColumnParagraph);
            doc.Add(new Paragraph("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"));
            doc.Close();
        }

        private class TwoColumnParagraphRenderer : ParagraphRenderer
        {
            public TwoColumnParagraphRenderer(Paragraph modelElement) :
                base(modelElement)
            {
            }

            public override IList<Rectangle> InitElementAreas(LayoutArea area)
            {
                List<Rectangle> areas = new List<Rectangle>();
                Rectangle firstArea = area.GetBBox().Clone();
                Rectangle secondArea = area.GetBBox().Clone();

                firstArea.SetWidth(firstArea.GetWidth() / 2 - 5);
                secondArea.SetX(secondArea.GetX() + secondArea.GetWidth() / 2 + 5);
                secondArea.SetWidth(firstArea.GetWidth());

                areas.Add(firstArea);
                areas.Add(secondArea);
                return areas;
            }

            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new TwoColumnParagraphRenderer((Paragraph) modelElement);
            }
        }
    }
}