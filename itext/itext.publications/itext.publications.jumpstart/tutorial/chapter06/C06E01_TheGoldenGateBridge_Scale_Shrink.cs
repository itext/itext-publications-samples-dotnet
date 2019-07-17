/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace Tutorial.Chapter06 {
    public class C06E01_TheGoldenGateBridge_Scale_Shrink {
        public const String SRC = "../../resources/pdf/the_golden_gate_bridge.pdf";

        public const String DEST = "../../results/chapter06/the_golden_gate_bridge_scale_shrink.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C06E01_TheGoldenGateBridge_Scale_Shrink().CreatePdf(SRC, DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String src, String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PdfDocument origPdf = new PdfDocument(new PdfReader(src));
            //Original page size
            PdfPage origPage = origPdf.GetPage(1);
            Rectangle orig = origPage.GetPageSizeWithRotation();
            //Add A4 page
            PdfPage page = pdf.AddNewPage(PageSize.A4.Rotate());
            //Shrink original page content using transformation matrix
            PdfCanvas canvas = new PdfCanvas(page);
            AffineTransform transformationMatrix = AffineTransform.GetScaleInstance(page.GetPageSize().GetWidth() / orig
                .GetWidth(), page.GetPageSize().GetHeight() / orig.GetHeight());
            canvas.ConcatMatrix(transformationMatrix);
            PdfFormXObject pageCopy = origPage.CopyAsFormXObject(pdf);
            canvas.AddXObject(pageCopy, 0, 0);
            //Add page with original size
            pdf.AddPage(origPage.CopyTo(pdf));
            //Add A2 page
            page = pdf.AddNewPage(PageSize.A2.Rotate());
            //Scale original page content using transformation matrix
            canvas = new PdfCanvas(page);
            transformationMatrix = AffineTransform.GetScaleInstance(page.GetPageSize().GetWidth() / orig.GetWidth(), page
                .GetPageSize().GetHeight() / orig.GetHeight());
            canvas.ConcatMatrix(transformationMatrix);
            canvas.AddXObject(pageCopy, 0, 0);
            pdf.Close();
            origPdf.Close();
        }
    }
}
