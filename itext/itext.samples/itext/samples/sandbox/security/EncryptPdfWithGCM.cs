using iText.Kernel.Pdf;
using iText.Samples.Sandbox.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace itext.samples.itext.samples.sandbox.security
{

    // EncryptPdfWithGCM.cs
    //
    // Example showing how to encrypt PDF using AES-GCM mode with PDF 2.0.
    // Demonstrates modern encryption requiring PDF 2.0 specification.
    
    internal class EncryptPdfWithGCM
    {
        public static readonly String DEST = "results/sandbox/security/encrypt_pdf_with_GCM.pdf";
        public static readonly String SRC = "../../../resources/pdfs/hello_pdf2.pdf";

        public static readonly String OWNER_PASSWORD = "World";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new EncryptPdfWithGCM().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument document = new PdfDocument(new PdfReader(SRC),
                new PdfWriter(dest, new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0).SetStandardEncryption(
                    null,
                    Encoding.UTF8.GetBytes(OWNER_PASSWORD),
                    EncryptionConstants.ALLOW_PRINTING,
                    EncryptionConstants.ENCRYPTION_AES_GCM)));
            document.Close();
        }
    }
}
