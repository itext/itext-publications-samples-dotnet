using iText.Kernel.Pdf;
using iText.Signatures;
using NUnit.Framework;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using System.IO;
using iText.Bouncycastle.Cert;
using iText.Bouncycastle.X509;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;

namespace iText.SigningExamples.Simple
{
    class TestSignBCSimple
    {
        [Test]
        public void TestSignSimpleRsa()
        {
            string testFileName = @"..\..\..\resources\circles.pdf";
            string storePath = @"..\..\..\keystore\test1234.p12";
            char[] storePass = "test1234".ToCharArray();
            string storeAlias = "RSAkey";

            Pkcs12Store pkcs12 = new Pkcs12StoreBuilder().Build();
            pkcs12.Load(new FileStream(storePath, FileMode.Open, FileAccess.Read), storePass);
            AsymmetricKeyParameter key = pkcs12.GetKey(storeAlias).Key;
            X509CertificateEntry[] chainEntries = pkcs12.GetCertificateChain(storeAlias);
            IX509Certificate[] chain = new IX509Certificate[chainEntries.Length];
            for (int i = 0; i < chainEntries.Length; i++)
                chain[i] = new X509CertificateBC(chainEntries[i].Certificate);
            PrivateKeySignature signature = new PrivateKeySignature(new PrivateKeyBC(key), "SHA384");

            using (PdfReader pdfReader = new PdfReader(testFileName))
            using (FileStream result = File.Create("circles-RSA-BC-signed-simple.pdf"))
            {
                PdfSigner pdfSigner = new PdfSigner(pdfReader, result, new StampingProperties().UseAppendMode());
                ITSAClient tsaClient = null;

                pdfSigner.SignDetached(signature, chain, null, null, tsaClient, 0, PdfSigner.CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestSignSimpleDsa()
        {
            string testFileName = @"..\..\..\resources\circles.pdf";
            string storePath = @"..\..\..\keystore\test1234.p12";
            char[] storePass = "test1234".ToCharArray();
            string storeAlias = "DSAkey";

            Pkcs12Store pkcs12 = new Pkcs12StoreBuilder().Build();
            pkcs12.Load(new FileStream(storePath, FileMode.Open, FileAccess.Read), storePass);
            AsymmetricKeyParameter key = pkcs12.GetKey(storeAlias).Key;
            X509CertificateEntry[] chainEntries = pkcs12.GetCertificateChain(storeAlias);
            IX509Certificate[] chain = new IX509Certificate[chainEntries.Length];
            for (int i = 0; i < chainEntries.Length; i++)
                chain[i] = new X509CertificateBC(chainEntries[i].Certificate);
            PrivateKeySignature signature = new PrivateKeySignature(new PrivateKeyBC(key), "SHA1");

            using (PdfReader pdfReader = new PdfReader(testFileName))
            using (FileStream result = File.Create("circles-DSA-BC-signed-simple.pdf"))
            {
                PdfSigner pdfSigner = new PdfSigner(pdfReader, result, new StampingProperties().UseAppendMode());
                ITSAClient tsaClient = null;

                pdfSigner.SignDetached(signature, chain, null, null, tsaClient, 0, PdfSigner.CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestSignSimpleECDsa()
        {
            string testFileName = @"..\..\..\resources\circles.pdf";
            string storePath = @"..\..\..\keystore\test1234.p12";
            char[] storePass = "test1234".ToCharArray();
            string storeAlias = "ECDSAkey";

            Pkcs12Store pkcs12 = new Pkcs12StoreBuilder().Build();
            pkcs12.Load(new FileStream(storePath, FileMode.Open, FileAccess.Read), storePass);
            AsymmetricKeyParameter key = pkcs12.GetKey(storeAlias).Key;
            X509CertificateEntry[] chainEntries = pkcs12.GetCertificateChain(storeAlias);
            IX509Certificate[] chain = new IX509Certificate[chainEntries.Length];
            for (int i = 0; i < chainEntries.Length; i++)
                chain[i] = new X509CertificateBC(chainEntries[i].Certificate);
            PrivateKeySignature signature = new PrivateKeySignature(new PrivateKeyBC(key), "SHA512");

            using (PdfReader pdfReader = new PdfReader(testFileName))
            using (FileStream result = File.Create("circles-ECDSA-BC-signed-simple.pdf"))
            {
                PdfSigner pdfSigner = new PdfSigner(pdfReader, result, new StampingProperties().UseAppendMode());
                ITSAClient tsaClient = null;

                pdfSigner.SignDetached(signature, chain, null, null, tsaClient, 0, PdfSigner.CryptoStandard.CMS);
            }
        }

        [Test]
        public void TestSignSimpleRsaSsaPss()
        {
            string testFileName = @"..\..\..\resources\circles.pdf";
            string storePath = @"..\..\..\keystore\test1234.p12";
            char[] storePass = "test1234".ToCharArray();
            string storeAlias = "RSAkey";

            Pkcs12Store pkcs12 = new Pkcs12StoreBuilder().Build();
            pkcs12.Load(new FileStream(storePath, FileMode.Open, FileAccess.Read), storePass);
            AsymmetricKeyParameter key = pkcs12.GetKey(storeAlias).Key;
            X509CertificateEntry[] chainEntries = pkcs12.GetCertificateChain(storeAlias);
            X509Certificate[] chain = new X509Certificate[chainEntries.Length];
            for (int i = 0; i < chainEntries.Length; i++)
                chain[i] = chainEntries[i].Certificate;
            PrivateKeySignatureContainer signature = new PrivateKeySignatureContainer(key, chain, "SHA384withRSAandMGF1");

            using (PdfReader pdfReader = new PdfReader(testFileName))
            using (FileStream result = File.Create("circles-RSASSAPSS-BC-signed-simple.pdf"))
            {
                PdfSigner pdfSigner = new PdfSigner(pdfReader, result, new StampingProperties().UseAppendMode());

                pdfSigner.SignExternalContainer(signature, 8192);
            }
        }

        [Test]
        public void TestSignSimpleDsaSha256()
        {
            string testFileName = @"..\..\..\resources\circles.pdf";
            string storePath = @"..\..\..\keystore\test1234.p12";
            char[] storePass = "test1234".ToCharArray();
            string storeAlias = "DSAkey";

            Pkcs12Store pkcs12 = new Pkcs12StoreBuilder().Build();
            pkcs12.Load(new FileStream(storePath, FileMode.Open, FileAccess.Read), storePass);
            AsymmetricKeyParameter key = pkcs12.GetKey(storeAlias).Key;
            X509CertificateEntry[] chainEntries = pkcs12.GetCertificateChain(storeAlias);
            X509Certificate[] chain = new X509Certificate[chainEntries.Length];
            for (int i = 0; i < chainEntries.Length; i++)
                chain[i] = chainEntries[i].Certificate;
            PrivateKeySignatureContainer signature = new PrivateKeySignatureContainer(key, chain, "SHA256withDSA");

            using (PdfReader pdfReader = new PdfReader(testFileName))
            using (FileStream result = File.Create("circles-DSASHA256-BC-signed-simple.pdf"))
            {
                PdfSigner pdfSigner = new PdfSigner(pdfReader, result, new StampingProperties().UseAppendMode());

                pdfSigner.SignExternalContainer(signature, 8192);
            }
        }
    }
}
