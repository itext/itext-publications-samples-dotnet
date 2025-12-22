using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Navigation;
using iText.Kernel.Utils;

namespace iText.Samples.Sandbox.Merge
{
   
    // MergeWithOutlines.cs
    //
    // Example showing how to merge PDFs and add bookmarks/outlines.
    // Demonstrates creating hierarchical outline structure for merged docs.
 
    public class MergeWithOutlines
    {
        public static readonly String DEST = "results/sandbox/merge/merge_with_outlines.pdf";

        public static readonly String SRC1 = "../../../resources/pdfs/hello.pdf";
        public static readonly String SRC2 = "../../../resources/pdfs/links1.pdf";
        public static readonly String SRC3 = "../../../resources/pdfs/links2.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MergeWithOutlines().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            PdfDocument srcDoc1 = new PdfDocument(new PdfReader(SRC1));
            PdfDocument srcDoc2 = new PdfDocument(new PdfReader(SRC2));
            PdfDocument srcDoc3 = new PdfDocument(new PdfReader(SRC3));

            int numberOfPages1 = srcDoc1.GetNumberOfPages();
            int numberOfPages2 = srcDoc2.GetNumberOfPages();
            int numberOfPages3 = srcDoc3.GetNumberOfPages();

            PdfMerger merger = new PdfMerger(pdfDoc);
            merger.SetCloseSourceDocuments(true)
                .Merge(srcDoc1, 1, numberOfPages1)
                .Merge(srcDoc2, 1, numberOfPages2)
                .Merge(srcDoc3, 1, numberOfPages3);

            PdfOutline rootOutline = pdfDoc.GetOutlines(false);

            int page = 1;
            PdfOutline helloWorld = rootOutline.AddOutline("Hello World");
            helloWorld.AddDestination(PdfExplicitDestination.CreateFit(pdfDoc.GetPage(page)));
            page += numberOfPages1;

            PdfOutline link1 = helloWorld.AddOutline("link1");
            link1.AddDestination(PdfExplicitDestination.CreateFit(pdfDoc.GetPage(page)));
            page += numberOfPages2;

            PdfOutline link2 = rootOutline.AddOutline("Link 2");
            link2.AddDestination(PdfExplicitDestination.CreateFit(pdfDoc.GetPage(page)));

            pdfDoc.Close();
        }
    }
}