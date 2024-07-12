using System;
using System.IO;
using iText.Bouncycastle.Cert;
using iText.Bouncycastle.X509;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter02
{
    public class C2_03_SignEmptyField
    {
        public static readonly string DEST = "results/signatures/chapter02/";

        public static readonly string KEYSTORE = "../../../resources/encryption/certificate.p12";
        public static readonly string SRC = "../../../resources/pdfs/hello_to_sign.pdf";

        public static readonly char[] PASSWORD = "testpassphrase".ToCharArray();

        public static readonly string[] RESULT_FILES =
        {
            "field_signed1.pdf",
            "field_signed2.pdf",
            "field_signed3.pdf",
            "field_signed4.pdf"
        };

        public void Sign(string src, string name, string dest, X509Certificate[] chain,
            ICipherParameters pk, string digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            string reason, string location)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            signer.SetReason(reason);
            signer.SetLocation(location);
            
            // This name corresponds to the name of the field that already exists in the document.
            signer.SetFieldName(name);

            // Specify if the appearance before field is signed will be used
            // as a background for the signed field. The "false" value is the default value.

            signer.GetSignatureField().SetReuseAppearance(false);
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

            C2_03_SignEmptyField app = new C2_03_SignEmptyField();
            app.Sign(SRC, "Signature1", DEST + RESULT_FILES[0], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Test 1", "Ghent");
            app.Sign(SRC, "Signature1", DEST + RESULT_FILES[1], chain, pk,
                DigestAlgorithms.SHA512, PdfSigner.CryptoStandard.CMS,
                "Test 2", "Ghent");
            app.Sign(SRC, "Signature1", DEST + RESULT_FILES[2], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CADES,
                "Test 3", "Ghent");
            app.Sign(SRC, "Signature1", DEST + RESULT_FILES[3], chain, pk,
                DigestAlgorithms.RIPEMD160, PdfSigner.CryptoStandard.CADES,
                "Test 4", "Ghent");
        }
    }
}