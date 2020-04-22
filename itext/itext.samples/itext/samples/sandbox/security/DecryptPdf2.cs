using System;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Security
{
    public class DecryptPdf2
    {
        public static readonly String DEST = "results/sandbox/security/decrypt_pdf2.pdf";
        public static readonly String SRC = "../../../resources/pdfs/encrypt_pdf_without_user_password.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DecryptPdf2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            // This is not actually a decrypt example.
            // The old iText5 test shows how to open an encrypted pdf document
            // without user password for modifying with preserving an old owner password
            PdfReader reader = new PdfReader(SRC);
            reader.SetUnethicalReading(true);

            using (PdfDocument document = new PdfDocument(
                new PdfReader(SRC).SetUnethicalReading(true), 
                new PdfWriter(dest),
                new StampingProperties().PreserveEncryption()
            ))
            {
                // here we can modify the document
            }
        }
    }
}