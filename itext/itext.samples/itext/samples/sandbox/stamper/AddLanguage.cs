using System;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Stamper 
{
    public class AddLanguage 
    {
        public static readonly String DEST = "results/sandbox/stamper/add_language.pdf";
        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new AddLanguage().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            pdfDoc.GetCatalog().Put(PdfName.Lang, new PdfString("EN"));
            pdfDoc.Close();
        }
    }
}
