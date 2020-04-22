using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Stamper 
{
    public class AddRotatedTemplate 
    {
        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";
        public static readonly String DEST = "results/sandbox/stamper/add_rotated_template.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new AddRotatedTemplate().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            // Add content to the template without rotation
            PdfFormXObject formXObject = new PdfFormXObject(new Rectangle(80, 120));
            new Canvas(formXObject, pdfDoc)
                    .Add(new Paragraph("Some long text that needs to be distributed over several lines."));
            
            // Add template to the pdf document page applying rotation
            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetFirstPage());
            canvas.AddXObject(formXObject, 36, 600);
            double angle = Math.PI / 4;
            canvas.AddXObject(formXObject, (float)Math.Cos(angle), 
                    -(float)Math.Sin(angle), (float)Math.Cos(angle), (float)Math.Sin(angle), 150, 600);
            
            pdfDoc.Close();
        }
    }
}
