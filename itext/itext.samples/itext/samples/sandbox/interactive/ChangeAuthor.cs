using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;

namespace iText.Samples.Sandbox.Interactive
{
   
    // ChangeAuthor.cs
    //
    // Example showing how to modify annotation author names in a PDF.
    // Demonstrates updating the title property of all page annotations.
 
    public class ChangeAuthor
    {
        public static readonly String DEST = "results/sandbox/interactive/change_author.pdf";

        public static readonly String SRC = "../../../resources/pdfs/page229_annotations.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ChangeAuthor().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));

            IList<PdfAnnotation> pageAnnots = pdfDoc.GetFirstPage().GetAnnotations();
            foreach (PdfAnnotation annot in pageAnnots)
            {
                if (annot.GetTitle() != null)
                {
                    annot.SetTitle(new PdfString("Bruno Lowagie"));
                }
            }

            pdfDoc.Close();
        }
    }
}