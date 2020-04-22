using System;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Stamper 
{
    public class Rotate90Degrees 
    {
        public static readonly String DEST = "results/sandbox/stamper/rotate90degrees.pdf";
        public static readonly String SRC = "../../../resources/pdfs/pages.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new Rotate90Degrees().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            for (int p = 1; p <= pdfDoc.GetNumberOfPages(); p++) 
            {
                PdfPage page = pdfDoc.GetPage(p);
                int rotate = page.GetRotation();
                if (rotate == 0) {
                    page.SetRotation(90);
                }
                else 
                {
                    page.SetRotation((rotate + 90) % 360);
                }
            }
            
            pdfDoc.Close();
        }
    }
}
