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
    /**
     * This example shows how to decrypt a pdf document encrypted with AES_GCM using owner password.
     */
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
