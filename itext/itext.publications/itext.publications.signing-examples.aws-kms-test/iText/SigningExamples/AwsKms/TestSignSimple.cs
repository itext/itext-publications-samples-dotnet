using iText.Kernel.Pdf;
using iText.Signatures;
using NUnit.Framework;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using System;
using System.IO;
using iText.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Cert;
using static iText.Signatures.PdfSigner;

namespace iText.SigningExamples.AwsKms
{
    public class TestSignSimple
    {
        [Test]
        public void TestSignSimpleRsa()
        {
            string testFileName = @"..\..\..\resources\circles.pdf";

            string keyId = "alias/SigningExamples-RSA_2048";
            Func<System.Collections.Generic.List<string>, string> selector = list => list.Find(name => name.StartsWith("RSASSA_PKCS1_V1_5"));
            AwsKmsSignature signature = new AwsKmsSignature(keyId, selector);
            System.Security.Cryptography.X509Certificates.X509Certificate2 certificate2 = CertificateUtils.GenerateSelfSignedCertificate(
                keyId,
                "CN=AWS KMS PDF Signing Test RSA,OU=signing tests,O=iText",
                selector
            );
            X509Certificate certificate = new X509Certificate(X509CertificateStructure.GetInstance(certificate2.RawData));

            using (PdfReader pdfReader = new PdfReader(testFileName))
            using (FileStream result = File.Create("circles-aws-kms-signed-simple-RSA.pdf"))
            {
                PdfSigner pdfSigner = new PdfSigner(pdfReader, result, new StampingProperties().UseAppendMode());

                pdfSigner.SignDetached(signature, new IX509Certificate[] { new X509CertificateBC(certificate) }, null, null, null, 0, CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestSignSimpleEcdsa()
        {
            string testFileName = @"..\..\..\resources\circles.pdf";

            string keyId = "alias/SigningExamples-ECC_NIST_P256";
            Func<System.Collections.Generic.List<string>, string> selector = list => list.Find(name => name.StartsWith("ECDSA_SHA_256"));
            AwsKmsSignature signature = new AwsKmsSignature(keyId, selector);
            System.Security.Cryptography.X509Certificates.X509Certificate2 certificate2 = CertificateUtils.GenerateSelfSignedCertificate(
                keyId,
                "CN=AWS KMS PDF Signing Test ECDSA,OU=signing tests,O=iText",
                selector
            );
            X509Certificate certificate = new X509Certificate(X509CertificateStructure.GetInstance(certificate2.RawData));

            using (PdfReader pdfReader = new PdfReader(testFileName))
            using (FileStream result = File.Create("circles-aws-kms-signed-simple-ECDSA.pdf"))
            {
                PdfSigner pdfSigner = new PdfSigner(pdfReader, result, new StampingProperties().UseAppendMode());

                pdfSigner.SignDetached(signature, new IX509Certificate[] { new X509CertificateBC(certificate) }, null, null, null, 0, CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestSignSimpleRsaSsaPss()
        {
            string testFileName = @"..\..\..\resources\circles.pdf";

            string keyId = "alias/SigningExamples-RSA_2048";
            Func<System.Collections.Generic.List<string>, string> selector = list => list.Find(name => name.StartsWith("RSASSA_PSS"));
            System.Security.Cryptography.X509Certificates.X509Certificate2 certificate2 = CertificateUtils.GenerateSelfSignedCertificate(
                keyId,
                "CN=AWS KMS PDF Signing Test RSAwithMGF1,OU=signing tests,O=iText",
                selector
            );
            X509Certificate certificate = new X509Certificate(X509CertificateStructure.GetInstance(certificate2.RawData));
            AwsKmsSignatureContainer signature = new AwsKmsSignatureContainer(certificate, keyId, selector);

            using (PdfReader pdfReader = new PdfReader(testFileName))
            using (FileStream result = File.Create("circles-aws-kms-signed-simple-RSAwithMGF1.pdf"))
            {
                PdfSigner pdfSigner = new PdfSigner(pdfReader, result, new StampingProperties().UseAppendMode());

                pdfSigner.SignExternalContainer(signature, 8192);
            }
        }

        [Test]
        public void TestSignSimpleEcdsaExternal()
        {
            string testFileName = @"..\..\..\resources\circles.pdf";

            string keyId = "alias/SigningExamples-ECC_NIST_P256";
            Func<System.Collections.Generic.List<string>, string> selector = list => list.Find(name => name.StartsWith("ECDSA_SHA_256"));
            System.Security.Cryptography.X509Certificates.X509Certificate2 certificate2 = CertificateUtils.GenerateSelfSignedCertificate(
                keyId,
                "CN=AWS KMS PDF Signing Test ECDSA,OU=signing tests,O=iText",
                selector
            );
            X509Certificate certificate = new X509Certificate(X509CertificateStructure.GetInstance(certificate2.RawData));
            AwsKmsSignatureContainer signature = new AwsKmsSignatureContainer(certificate, keyId, selector);

            using (PdfReader pdfReader = new PdfReader(testFileName))
            using (FileStream result = File.Create("circles-aws-kms-signed-simple-ECDSA-External.pdf"))
            {
                PdfSigner pdfSigner = new PdfSigner(pdfReader, result, new StampingProperties().UseAppendMode());

                pdfSigner.SignExternalContainer(signature, 8192);
            }
        }
    }
}