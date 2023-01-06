using iText.Kernel.Pdf;
using iText.Signatures;
using NUnit.Framework;
using System;
using System.IO;
using iText.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Cert;
using static iText.Signatures.PdfSigner;

namespace iText.SigningExamples.Pkcs11
{
    class TestSignSimple
    {
        [Test]
        public void TestPkcs11SignSimple()
        {
            string testFileName = @"..\..\..\resources\circles.pdf";

            using (Pkcs11Signature signature = new Pkcs11Signature(@"PKCS11LIBRARY", 1).Select("KEYALIAS", "CERTLABEL", "PIN").SetDigestAlgorithmName("SHA256"))
            // Entrust SAS
//            using (Pkcs11Signature signature = new Pkcs11Signature(@"c:\Program Files\Entrust\SigningClient\P11SigningClient64.dll", 1)
//                .Select(null, "CN=Entrust Limited,OU=ECS,O=Entrust Limited,L=Kanata,ST=Ontario,C=CA", "PIN").SetDigestAlgorithmName("SHA256"))
            // Utimaco HSM
//            using (Pkcs11Signature signature = new Pkcs11Signature(@"C:\Program Files\Utimaco\CryptoServer\Lib\cs_pkcs11_R2.dll", 0)
//                .Select(null, null, "PIN").SetDigestAlgorithmName("SHA256"))
            using (PdfReader pdfReader = new PdfReader(testFileName))
            using (FileStream result = File.Create("circles-pkcs11-signed-simple.pdf"))
            {
                PdfSigner pdfSigner = new PdfSigner(pdfReader, result, new StampingProperties().UseAppendMode());
                ITSAClient tsaClient = new TSAClientBouncyCastle("RFC3161_SERVER_URL");

                IX509Certificate[] certificateWrappers = new IX509Certificate[signature.GetChain().Length];
                for (int i = 0; i < certificateWrappers.Length; ++i) {
                    certificateWrappers[i] = new X509CertificateBC(signature.GetChain()[i]);
                }
                pdfSigner.SignDetached(signature, certificateWrappers, null, null, tsaClient, 0, CryptoStandard.CMS);
            }
        }
    }
}
