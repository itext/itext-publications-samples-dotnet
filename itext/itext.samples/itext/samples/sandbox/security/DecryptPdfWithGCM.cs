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
    // DecryptPdfWithGCM.cs
    //
    // Example showing how to decrypt PDF encrypted with AES-GCM algorithm.
    // Demonstrates decryption using owner password for GCM-encrypted documents.
 
    internal class DecryptPdfWithGCM
    {
        public static readonly String DEST = "results/sandbox/security/decrypt_pdf_with_GCM.pdf";
        public static readonly String SRC = "../../../resources/pdfs/hello_encrypted_with_GCM.pdf";

        public static readonly String OWNER_PASSWORD = "World";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DecryptPdfWithGCM().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument document = new PdfDocument(new PdfReader(SRC, new ReaderProperties()
                .SetPassword(Encoding.UTF8.GetBytes(OWNER_PASSWORD))), new PdfWriter(dest));
            document.Close();
        }
    }
}
