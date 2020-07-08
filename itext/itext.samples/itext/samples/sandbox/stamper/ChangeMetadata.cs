using System;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Stamper 
{
    public class ChangeMetadata 
    {
        public static readonly String DEST = "results/sandbox/stamper/change_meta_data.pdf";
        public static readonly String SRC = "../../../resources/pdfs/state.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ChangeMetadata().ManipulatePdf();
        }

        protected internal virtual void ManipulatePdf() 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), 
                    new PdfWriter(DEST, new WriterProperties().AddXmpMetadata()));
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.SetTitle("New title");
            info.AddCreationDate();
            
            pdfDoc.Close();
        }
    }
}
