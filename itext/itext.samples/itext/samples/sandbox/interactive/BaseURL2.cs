using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Interactive
{
   
    // BaseURL2.cs
    //
    // Example showing how to set a base URL for relative link resolution.
    // Demonstrates creating links that resolve relative to a document base.
 
    public class BaseURL2
    {
        public static readonly String DEST = "results/sandbox/interactive/base_url2.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new BaseURL2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfDictionary uri = new PdfDictionary();
            uri.Put(PdfName.Type, PdfName.URI);
            uri.Put(new PdfName("Base"), new PdfString("http://itextpdf.com/"));
            pdfDoc.GetCatalog().Put(PdfName.URI, uri);

            PdfAction action = PdfAction.CreateURI("index.php");
            Link link = new Link("Home page", action);
            doc.Add(new Paragraph(link));

            pdfDoc.Close();
        }
    }
}