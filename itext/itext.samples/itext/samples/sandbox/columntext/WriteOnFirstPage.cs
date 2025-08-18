using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Columntext
{
   
    // WriteOnFirstPage.cs
    // 
    // This class demonstrates how to add dynamic content to the first page based on information
    // determined after the document is created. It uses a PdfFormXObject as a placeholder that
    // is populated at the end of document generation with the total page count.
 
    public class WriteOnFirstPage
    {
        public static readonly String DEST = "results/sandbox/columntext/write_on_first_page.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new WriteOnFirstPage().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            PdfFormXObject template = new PdfFormXObject(new Rectangle(0, 0, 523, 50));
            Canvas templateCanvas = new Canvas(template, pdfDoc);

            // In order to add a formXObject to the document, one can wrap it with an image
            doc.Add(new Image(template));
            for (int i = 0; i < 100; i++)
            {
                doc.Add(new Paragraph("test"));
            }

            String textLine = String.Format("There are {0} pages in this document", pdfDoc.GetNumberOfPages());
            templateCanvas.Add(new Paragraph(textLine));
            templateCanvas.Close();

            doc.Close();
        }
    }
}