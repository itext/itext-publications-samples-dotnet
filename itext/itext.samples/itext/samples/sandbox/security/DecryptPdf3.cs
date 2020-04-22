using System;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Security
{
    public class DecryptPdf3
    {
        public static readonly String DEST = "results/sandbox/security/decrypt_pdf3.pdf";
        public static readonly String SRC = "../../../resources/pdfs/encrypt_pdf_without_user_password.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DecryptPdf3().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument document = new PdfDocument(new PdfReader(SRC).SetUnethicalReading(true), new PdfWriter(dest));
            document.Close();
        }
    }
}