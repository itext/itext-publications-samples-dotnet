/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using System.Text;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Security
{
    public class DecryptPdf
    {
        public static readonly String DEST = "results/sandbox/security/decrypt_pdf.pdf";
        public static readonly String SRC = "../../resources/pdfs/hello_encrypted.pdf";
        
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