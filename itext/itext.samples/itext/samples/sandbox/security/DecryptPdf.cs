using System;
using System.IO;
using System.Text;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Security
{
    /**
     * This example shows how to decrypt a pdf document using owner password.
     * To show that decryption is successful, user password is revealed.
     */
    public class DecryptPdf
    {
        public static readonly String DEST = "results/sandbox/security/decrypt_pdf.pdf";
        public static readonly String SRC = "../../../resources/pdfs/hello_encrypted.pdf";
        
        public static readonly String OWNER_PASSWORD = "World";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DecryptPdf().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            using (PdfDocument document = new PdfDocument(
                new PdfReader(SRC, new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(OWNER_PASSWORD))),
                new PdfWriter(dest)
            ))
            {
                byte[] computeUserPassword = document.GetReader().ComputeUserPassword();

                // The result of user password computation logic can be null in case of
                // AES256 password encryption or non password encryption algorithm
                String userPassword = computeUserPassword == null ? null : Encoding.UTF8.GetString(computeUserPassword);
                Console.Out.WriteLine(userPassword);
            }
        }
    }
}