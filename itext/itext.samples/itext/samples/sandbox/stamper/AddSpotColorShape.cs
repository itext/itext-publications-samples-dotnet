using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Colorspace;
using iText.Kernel.Pdf.Function;

namespace iText.Samples.Sandbox.Stamper 
{
    public class AddSpotColorShape 
    {
        public static readonly String DEST = "results/sandbox/stamper/add_spot_color_shape.pdf";
        public static readonly String SRC = "../../../resources/pdfs/image.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new AddSpotColorShape().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfPage pdfPage = pdfDoc.GetFirstPage();
            
            pdfPage.SetIgnorePageRotationForContent(true);
            PdfCanvas canvas = new PdfCanvas(pdfPage);
            canvas.Arc(0, 0, 842, 595, 0, 360);
            canvas.Arc(25, 25, 817, 570, 0, 360);
            canvas.Arc(50, 50, 792, 545, 0, 360);
            canvas.Arc(75, 75, 767, 520, 0, 360);
            canvas.EoClip();
            canvas.EndPath();
            canvas.SetFillColor(new Separation(CreateCmykColorSpace(0.8f, 0.3f, 0.3f, 0.1f), 0.4f));
            canvas.Rectangle(0, 0, 842, 595);
            canvas.Fill();
            
            pdfDoc.Close();
        }

        private PdfSpecialCs.Separation CreateCmykColorSpace(float c, float m, float y, float k) 
        {
            double[] c0 = new double[] { 0, 0, 0, 0 };
            double[] c1 = new double[] { c, m, y, k };
            IPdfFunction pdfFunction = new PdfType2Function(new double[] { 0, 1 }, 
                    null, c0, c1,1);
            PdfSpecialCs.Separation cs = new PdfSpecialCs.Separation("iTextSpotColorCMYK", 
                    new DeviceCmyk(c, m, y, k).GetColorSpace(), pdfFunction);
            
            return cs;
        }
    }
}
