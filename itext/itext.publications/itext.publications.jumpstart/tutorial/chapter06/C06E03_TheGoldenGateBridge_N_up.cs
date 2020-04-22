using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace Tutorial.Chapter06 {
    public class C06E03_TheGoldenGateBridge_N_up {
        public const String SRC = "../../../resources/pdf/the_golden_gate_bridge.pdf";

        public const String DEST = "../../../results/chapter06/the_golden_gate_bridge_nup.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C06E03_TheGoldenGateBridge_N_up().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PdfDocument sourcePdf = new PdfDocument(new PdfReader(SRC));
            //Original page
            PdfPage origPage = sourcePdf.GetPage(1);
            //Original page size
            Rectangle orig = origPage.GetPageSize();
            PdfFormXObject pageCopy = origPage.CopyAsFormXObject(pdf);
            //N-up page
            PageSize nUpPageSize = PageSize.A4.Rotate();
            PdfPage page = pdf.AddNewPage(nUpPageSize);
            PdfCanvas canvas = new PdfCanvas(page);
            //Scale page
            AffineTransform transformationMatrix = AffineTransform.GetScaleInstance(nUpPageSize.GetWidth() / orig.GetWidth
                () / 2f, nUpPageSize.GetHeight() / orig.GetHeight() / 2f);
            canvas.ConcatMatrix(transformationMatrix);
            //Add pages to N-up page
            canvas.AddXObject(pageCopy, 0, orig.GetHeight());
            canvas.AddXObject(pageCopy, orig.GetWidth(), orig.GetHeight());
            canvas.AddXObject(pageCopy, 0, 0);
            canvas.AddXObject(pageCopy, orig.GetWidth(), 0);
            pdf.Close();
            sourcePdf.Close();
        }
    }
}
