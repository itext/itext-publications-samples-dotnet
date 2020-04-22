using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Objects
{
    public class ColumnTextParagraphs
    {
        public static readonly string DEST = "results/sandbox/objects/column_text_paragraphs.pdf";
        public static readonly string TEXT = "This is some long paragraph " +
                                             "that will be added over and over again to prove a point.";
        public static readonly Rectangle[] COLUMNS =
        {
            new Rectangle(36, 36, 254, 770),
            new Rectangle(305, 36, 254, 770)
        };

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ColumnTextParagraphs().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            doc.SetRenderer(new CustomDocumentRenderer(doc));

            int pCounter = 0;
            while (pCounter < 30)
            {
                doc.Add(new Paragraph(String.Format("Paragraph {0}: {1}", ++pCounter, TEXT)));
            }

            doc.Close();
        }

        protected class CustomDocumentRenderer : DocumentRenderer
        {
            int nextAreaNumber = 0;
            int currentPageNumber;

            public CustomDocumentRenderer(Document document) : base(document)
            {
            }

            // If renderer overflows on the next area iText will use default getNextRender() method with default renderer
            // parameters. So the method should be overridden with the parameters from the initial renderer
            public override IRenderer GetNextRenderer()
            {
                return new DocumentRenderer(document);
            }

            protected override LayoutArea UpdateCurrentArea(LayoutResult overflowResult)
            {
                if (nextAreaNumber % 2 == 0)
                {
                    currentPageNumber = base.UpdateCurrentArea(overflowResult).GetPageNumber();
                }
                else
                {
                    new PdfCanvas(document.GetPdfDocument(), document.GetPdfDocument().GetNumberOfPages())
                        .MoveTo(297.5f, 36)
                        .LineTo(297.5f, 806)
                        .Stroke();
                }

                currentArea = new RootLayoutArea(currentPageNumber, COLUMNS[nextAreaNumber % 2].Clone());
                nextAreaNumber++;
                return currentArea;
            }
        }
    }
}