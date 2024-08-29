using System;
using System.IO;
using iText.Bouncycastle.X509;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter02
{
    public class C2_02_SignHelloWorldWithTempFile
    {
        public static readonly string DEST = "results/signatures/chapter02/";

        public static readonly string KEYSTORE = "../../../resources/encryption/certificate.p12";
        public static readonly string SRC = "../../../resources/pdfs/hello.pdf";

        public static readonly char[] PASSWORD = "testpassphrase".ToCharArray();

        public static readonly string[] RESULT_FILES =
        {
            "hello_signed_with_temp.pdf"
        };

        private void Sign(string src, string tmp, string dest, X509Certificate[] chain,
            ICipherParameters pk, string digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            string reason, string location)
        {
            PdfReader reader = new PdfReader(src);

            // Pass the temporary file's path to the PdfSigner constructor
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), tmp,
                new StampingProperties());

            // Create the signature appearance
            Rectangle rect = new Rectangle(36, 648, 200, 100);
            SignerProperties signerProperties = new SignerProperties()
                .SetReason(reason)
                .SetLocation(location)
                .SetPageRect(rect)
                .SetPageNumber(1)
                .SetFieldName("sig");
            signer.SetSignerProperties(signerProperties);

            IExternalSignature pks = new PrivateKeySignature(new PrivateKeyBC(pk), digestAlgorithm);

            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, certificateWrappers, null, null, null, 0, subfilter);
        }

        public static void Main(string[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();

            Pkcs12Store pk12 = new Pkcs12StoreBuilder().Build();
            pk12.Load(new FileStream(KEYSTORE, FileMode.Open, FileAccess.Read), PASSWORD);
            string alias = null;
            foreach (var a in pk12.Aliases)
            {
                alias = ((string) a);
                if (pk12.IsKeyEntry(alias))
                    break;
            }

            ICipherParameters pk = pk12.GetKey(alias).Key;
            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            X509Certificate[] chain = new X509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
            {
                chain[k] = ce[k].Certificate;
            }

            new C2_02_SignHelloWorldWithTempFile().Sign(SRC, DEST, DEST + RESULT_FILES[0], chain, pk,
                DigestAlgorithms.SHA256,
                PdfSigner.CryptoStandard.CMS, "Temp test", "Ghent");
        }
    }
}