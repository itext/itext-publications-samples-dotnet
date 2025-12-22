using System;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Interactive
{
   
    // BaseURL3.cs
    //
    // Example showing how to add a base URL to an existing PDF document.
    // Demonstrates modifying the URI dictionary in the document catalog.
 
    public class BaseURL3
    {
        public static readonly String DEST = "results/sandbox/interactive/base_url3.pdf";

        public static readonly String SRC = "../../../resources/pdfs/base_url.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new BaseURL3().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));

            PdfDictionary uri = new PdfDictionary();
            uri.Put(PdfName.Type, PdfName.URI);
            uri.Put(new PdfName("Base"), new PdfString("http://itextpdf.com/"));
            pdfDoc.GetCatalog().Put(PdfName.URI, uri);

            pdfDoc.Close();
        }
    }
}