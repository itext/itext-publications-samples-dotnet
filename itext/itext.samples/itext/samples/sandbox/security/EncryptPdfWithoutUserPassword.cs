/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
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
    public class EncryptPdfWithoutUserPassword
    {
        public static readonly String DEST = "results/sandbox/security/encrypt_pdf_without_user_password.pdf";
        public static readonly String SRC = "../../resources/pdfs/hello.pdf";
        
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
                        EncryptionConstants.ENCRYPTION_AES_128 | EncryptionConstants.DO_NOT_ENCRYPT_METADATA
                    )));
            document.Close();
        }
    }
}