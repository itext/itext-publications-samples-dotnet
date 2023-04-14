using System;
using System.IO;
using iText.Bouncycastle.Cert;
using iText.Bouncycastle.X509;
using iText.Kernel.Pdf;
using Org.BouncyCastle.X509;

namespace iText.Samples.Sandbox.Security
{
    public class EncryptWithCertificate
    {
        public static readonly String DEST = "results/sandbox/security/encrypt_with_certificate.pdf";
        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";
        public static readonly String PUBLIC = "../../../resources/encryption/test.cer";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new EncryptWithCertificate().ManipulatePdf(DEST);
        }

        public X509Certificate GetPublicCertificate(String path)
        {
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                X509CertificateParser parser = new X509CertificateParser();
                X509Certificate readCertificate = parser.ReadCertificate(stream);
                return readCertificate;
            }
        }

        protected void ManipulatePdf(String dest)
        {
            // The file created by this example can not be opened, unless
            // you import the private key stored in test.p12 in your certificate store.
            // The password for the p12 file is kspass.
            X509Certificate cert = GetPublicCertificate(PUBLIC);

            PdfDocument document = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest,
                new WriterProperties().SetPublicKeyEncryption(
                    new[] {new X509CertificateBC(cert)},
                    new[] {EncryptionConstants.ALLOW_PRINTING},
                    EncryptionConstants.ENCRYPTION_AES_256)));
            document.Close();
        }
    }
}