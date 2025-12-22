using System.IO;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
   
    // AddPointer.cs
    //
    // Example showing how to add a custom pointer shape over an image.
    // Demonstrates drawing vector graphics on top of a background image.
 
    public class AddPointer
    {
        public static readonly string DEST = "results/sandbox/objects/add_pointer.pdf";
        public static readonly string IMG = "../../../resources/img/map_cic.png";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new AddPointer().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Image img = new Image(ImageDataFactory.Create(IMG));
            Document doc = new Document(pdfDoc, new PageSize(img.GetImageWidth(), img.GetImageHeight()));

            img.SetFixedPosition(0, 0);
            doc.Add(img);

            // Added a custom shape on top of a image
            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetFirstPage());
            canvas.SetStrokeColor(ColorConstants.RED)
                .SetLineWidth(3)
                .MoveTo(220, 330)
                .LineTo(240, 370)
                .Arc(200, 350, 240, 390, 0, 180)
                .LineTo(220, 330)
                .ClosePathStroke()
                .SetFillColor(ColorConstants.RED)
                .Circle(220, 370, 10)
                .Fill();

            doc.Close();
        }
    }
}