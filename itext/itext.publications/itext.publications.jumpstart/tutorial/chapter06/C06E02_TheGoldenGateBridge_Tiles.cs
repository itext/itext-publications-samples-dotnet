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
    public class C06E02_TheGoldenGateBridge_Tiles {
        public const String SRC = "resources/pdf/the_golden_gate_bridge.pdf";

        public const String DEST = "results/chapter06/the_golden_gate_bridge_tiles.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C06E02_TheGoldenGateBridge_Tiles().CreatePdf(SRC, DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String src, String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PdfDocument sourcePdf = new PdfDocument(new PdfReader(src));
            //Original page
            PdfPage origPage = sourcePdf.GetPage(1);
            PdfFormXObject pageCopy = origPage.CopyAsFormXObject(pdf);
            //Original page size
            Rectangle orig = origPage.GetPageSize();
            //Tile size
            Rectangle tileSize = PageSize.A4.Rotate();
            // Transformation matrix
            AffineTransform transformationMatrix = AffineTransform.GetScaleInstance(tileSize.GetWidth() / orig.GetWidth
                () * 2f, tileSize.GetHeight() / orig.GetHeight() * 2f);
            //The first tile
            PdfPage page = pdf.AddNewPage(PageSize.A4.Rotate());
            PdfCanvas canvas = new PdfCanvas(page);
            canvas.ConcatMatrix(transformationMatrix);
            canvas.AddXObject(pageCopy, 0, -orig.GetHeight() / 2f);
            //The second tile
            page = pdf.AddNewPage(PageSize.A4.Rotate());
            canvas = new PdfCanvas(page);
            canvas.ConcatMatrix(transformationMatrix);
            canvas.AddXObject(pageCopy, -orig.GetWidth() / 2f, -orig.GetHeight() / 2f);
            //The third tile
            page = pdf.AddNewPage(PageSize.A4.Rotate());
            canvas = new PdfCanvas(page);
            canvas.ConcatMatrix(transformationMatrix);
            canvas.AddXObject(pageCopy, 0, 0);
            //The fourth tile
            page = pdf.AddNewPage(PageSize.A4.Rotate());
            canvas = new PdfCanvas(page);
            canvas.ConcatMatrix(transformationMatrix);
            canvas.AddXObject(pageCopy, -orig.GetWidth() / 2f, 0);
            pdf.Close();
            sourcePdf.Close();
        }
    }
}
