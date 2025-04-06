using System;
using System.IO;
using System.Text;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Security
{
    /**
     * This example shows how to encrypt a PDF document using AES algorithm without
     * user password, which means password is only required to modify the document.
     */
    public class EncryptPdfWithoutUserPassword
    {
        public static readonly String DEST = "results/sandbox/security/encrypt_pdf_without_user_password.pdf";
        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";
        
        public static readonly String OWNER_PASSWORD = "World";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new EncryptPdfWithoutUserPassword().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument document = new PdfDocument(
                new PdfReader(SRC),
                new PdfWriter(dest,
                    new WriterProperties().SetStandardEncryption(
                        // null user password argument is equal to empty string
                        // this means that no user password required
                        null,
                        Encoding.UTF8.GetBytes(OWNER_PASSWORD),
                        EncryptionConstants.ALLOW_PRINTING,
                        EncryptionConstants.ENCRYPTION_AES_256 | EncryptionConstants.DO_NOT_ENCRYPT_METADATA
                    )));
            document.Close();
        }
    }
}