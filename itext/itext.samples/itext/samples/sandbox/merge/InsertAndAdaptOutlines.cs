using System;
using System.IO;
using iText.Forms;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Navigation;

namespace iText.Samples.Sandbox.Merge
{
    public class InsertAndAdaptOutlines
    {
        public static readonly String DEST = "results/sandbox/merge/insert_and_adapt_outlines.pdf";

        public static readonly String INSERT = "../../../resources/pdfs/hello.pdf";
        public static readonly String SRC = "../../../resources/pdfs/bookmarks.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new InsertAndAdaptOutlines().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            int insertPageNumber = 4;

            // Copier contains the additional logic to copy acroform fields to a new page.
            // PdfPageFormCopier uses some caching logic which can potentially improve performance
            // in case of the reusing of the same instance.
            PdfPageFormCopier formCopier = new PdfPageFormCopier();
            PdfDocument insertDoc = new PdfDocument(new PdfReader(INSERT));
            insertDoc.CopyPagesTo(1, 1, pdfDoc, insertPageNumber, formCopier);
            insertDoc.Close();

            PdfOutline outlines = pdfDoc.GetOutlines(false);
            PdfOutline outline = outlines.GetAllChildren()[0].AddOutline("Hello", insertPageNumber - 1);
            outline.AddDestination(PdfExplicitDestination.CreateFit(pdfDoc.GetPage(insertPageNumber)));

            pdfDoc.Close();
        }
    }
}