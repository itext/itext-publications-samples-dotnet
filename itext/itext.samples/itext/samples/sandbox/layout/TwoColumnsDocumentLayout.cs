using System;
using System.IO;
using System.Text;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Layout
{
   
    // TwoColumnsDocumentLayout.cs
    //
    // Example showing document-wide two-column layout using ColumnRenderer.
    // Demonstrates defining custom column areas for automatic text flow.
 
    public class TwoColumnsDocumentLayout
    {
        public static readonly string DEST = "results/sandbox/layout/complexDocumentLayout.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TwoColumnsDocumentLayout().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Rectangle[] columns =
            {
                new Rectangle(100, 100, 100, 500),
                new Rectangle(400, 100, 100, 500)
            };

            // Set a renderer to create a layout consisted of two vertical rectangles created above
            doc.SetRenderer(new ColumnDocumentRenderer(doc, columns));

            StringBuilder text = new StringBuilder();
            for (int i = 0; i < 200; i++)
            {
                text.Append("A very long text is here...");
            }

            Paragraph paragraph = new Paragraph(text.ToString());
            doc.Add(paragraph);

            doc.Close();
        }
    }
}