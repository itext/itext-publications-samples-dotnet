using System;
using System.IO;
using System.Text;
using iText.Commons.Utils;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Stamper 
{
    public class ReplaceStream 
    {
        public static readonly String DEST = "results/sandbox/stamper/replace_stream.pdf";
        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ReplaceStream().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfPage page = pdfDoc.GetFirstPage();
            PdfDictionary dict = page.GetPdfObject();
            
            PdfObject pdfObject = dict.Get(PdfName.Contents);
            if (pdfObject is PdfStream) 
            {
                PdfStream stream = (PdfStream) pdfObject;
                byte[] data = stream.GetBytes();
                String replacedData = JavaUtil.GetStringForBytes(data).Replace("Hello World", "HELLO WORLD");
                stream.SetData((Encoding.UTF8.GetBytes(replacedData)));
            }
            
            pdfDoc.Close();
        }
    }
}
